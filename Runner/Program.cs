using Google.Protobuf.Collections;
using Grpc.Core;
using Grpc.Net.Client;
using PlayerInterface;
using Swoc2024;
using Swoc2024.Planning;
using System.Text.Json;
using Client = PlayerInterface.PlayerHost.PlayerHostClient;

start:

Console.WriteLine("Connecting...");
var client = GetClient();
Console.WriteLine("Connected");

Console.WriteLine("Registering...");
(GameSettings settings, string name) = await RegisterAsync(client);
Console.WriteLine("Registered");

Planner planner = new(new AStarPlanner(settings.Dimensions.ToArray()), new(settings.StartAddress.ToArray()));
planner.SetMySnake(name, Planner.Target.Food);

World world = new();

world.SnakeDied += (s, e) => Console.WriteLine($"Snake: {e.Name} died.");

_ = SubscribeServerEvents(client, world);

_ = SubscribeDelta(client, settings.PlayerIdentifier, world, async () => await UpdateWithPlanner(client, settings.PlayerIdentifier, world, planner));

await SetupWorldAsync(client, world);

while(true)
{
    await Task.Delay(TimeSpan.FromSeconds(2));
    Console.WriteLine($"My snake count is {planner.GetMySnakes(world).Length}.");
    if (planner.GetMySnakes(world).Length == 0)
    {
        goto start; // Dirty, I know, but it works :)
    }
    var snakes = world.GetSnakes().ToList(); //.Where(i => i.Name == "Tommie").ToList();
    JsonSerializerOptions jsonOpts = new()
    {
        WriteIndented = true,
    };

    //Console.WriteLine(JsonSerializer.Serialize(snakes.First().Positions.Take(2)));
    //Console.WriteLine(JsonSerializer.Serialize(snakes, new JsonSerializerOptions()
    //{
    //    WriteIndented = true,
    //}));
    Console.WriteLine($"World food count: {world.GetFood().Count()}");
}

Client GetClient()
{
    var channel = GrpcChannel.ForAddress("http://192.168.178.62:5168");
    return new Client(channel);
}

async Task<(GameSettings settings, string name)> RegisterAsync(Client client)
{
    byte[] buf = new byte[10];
    Random.Shared.NextBytes(buf);

    string playerName = args.Length > 0 ? args[0] : Convert.ToBase64String(buf);

    var register = new RegisterRequest
    {
        PlayerName = playerName,
    };
    Console.WriteLine($"Registering as: {playerName}.");
    var settings = await client.RegisterAsync(register);
    Console.WriteLine($"Registered, got ID: {settings.PlayerIdentifier}.");
    return (settings, playerName);
}

Task SubscribeDelta(Client client, string playerid, World world, Action onUpdate, CancellationToken cancellation = default)
{
    var req = new SubsribeRequest
    {
        PlayerIdentifier = settings.PlayerIdentifier,
    };
    var deltaStream = client.Subscribe(req, cancellationToken: cancellation);
    Console.WriteLine("Subscribed");

    return Task.Factory.StartNew(async () =>
    {
        while(await deltaStream.ResponseStream.MoveNext())
        {
            var message = deltaStream.ResponseStream.Current;
            foreach(var cell in message.UpdatedCells)
            {
                world.QueueUpdate(new Cell(new Position([.. cell.Address]), cell.Player, cell.FoodValue > 0));
            }
            onUpdate?.Invoke();
        }
    });
}

Task SubscribeServerEvents(Client client, World world, CancellationToken cancellation = default)
{
    var stateChanges = client.SubscribeToServerEvents(new EmptyRequest { }, cancellationToken: cancellation);

    return Task.Factory.StartNew(async () =>
    {
        while (await stateChanges.ResponseStream.MoveNext())
        {
            var message = stateChanges.ResponseStream.Current;
            if (message.MessageType == MessageType.GameStateChange)
            {
                await Console.Out.WriteLineAsync($"Update: {message.Message}");

            }
            else
            {
                await Console.Out.WriteLineAsync($"PlayerJoined: {message.Message}");
            }
        }
    });
}

async Task SetupWorldAsync(Client client, World world, CancellationToken cancellation = default)
{
    var gameWorld = await client.GetGameStateAsync(new EmptyRequest(), cancellationToken: cancellation);

    world.StartWorld(
        gameWorld.UpdatedCells.Select(i => 
            new Cell(
                new Position([.. i.Address]),
                i.Player, 
                i.FoodValue > 0
            )
        )
    );
}

async Task UpdateWithPlanner(Client client, string id, World world, Planner planner)
{
    foreach(var plan in planner.PlanSnakes(world))
    {
        if (plan is MoveSnakeAction move)
        {
            Console.WriteLine($"Moving snake: {move.Snake.Name}:{move.Snake.Positions.Count}:{move.Snake.Score}, from {move.Snake.Head} to {move.Plan.NextPosition} towards {move.Destination}:{move.Plan.MoveCount}.");
            Move newMove = new();
            newMove.NextLocation.AddRange(move.Plan.NextPosition.Positions);
            newMove.SnakeName = move.Snake.Name;
            newMove.PlayerIdentifier = id;
            await client.MakeMoveAsync(newMove);
        }
        else if (plan is SplitSnakeAction split)
        {
            Console.WriteLine($"Requesting to split snake {plan.Snake.Name} into {split.NewName}.");
            var request = new SplitRequest()
            {
                NewSnakeName = split.NewName,
                OldSnakeName = split.Snake.Name,
                PlayerIdentifier = id,
                SnakeSegment = 1
            };
            request.NextLocation.AddRange(split.Plan.NextPosition.Positions);
            await client.SplitSnakeAsync(request);
        }
    }
}
