using System.Xml.Linq;

namespace Swoc2024.Planning;

public class Planner(IPlanner plan, Position homePosition)
{
    readonly IPlanner planner = plan;
    readonly Position home = homePosition;

    public enum Target
    {
        Food,
        Home,
        Kamikaze,
    }

    private struct SnakeTarget(string snakeName, Target target)
    {
        public string SnakeName { get; set; } = snakeName;
        public Target Target { get; set; } = target;
    }

    private List<SnakeTarget> mySnakes = [];

    public void SetMySnake(string snake, Target target) => mySnakes.Add(new SnakeTarget(snake, target));

    public void RemoveMySnake(string snake) => mySnakes.RemoveAll(i => i.SnakeName == snake);

    public string[] GetMySnakes(World world) => GetSnakes(world).Select(i => i.Name).ToArray();

    private List<Snake> GetSnakes(World world)
    {
        return world.GetSnakes().Where(i => mySnakes.Where(j => j.SnakeName == i.Name).Any()).ToList();
    }

    public IEnumerable<PlanSnakeAction> PlanSnakes(World world)
    {
        var snakes = GetSnakes(world);
        var food = world.GetFood().ToList();

        var otherBlocks = world.GetSnakes().Where(i => !mySnakes.Where(j => j.SnakeName == i.Name).Any()).SelectMany(i => i.Positions).ToList();

        List<string> justAdded = [];
        snakes.Where(i => i is not null).ToList().ForEach(i => Console.WriteLine($"{i.Name}, {GetTarget(i.Name).Target}"));
        foreach (Snake snake in snakes.Where(i => i is not null && GetTarget(i.Name).Target != Target.Home))
        {
            if (snake.Positions.Count > 5)
            {
                int i = mySnakes.FindIndex(i => i.SnakeName == snake.Name);
                var foundSnake = mySnakes[i];
                foundSnake.Target = Target.Home;
                mySnakes[i] = foundSnake;

                // Don't plan remainder to home just yet. That is the third loop below. Plan new snake first for SplitAction
                (PlanResult? result, Position destination) = PlanToFood([.. otherBlocks], snake.Positions.First(), [.. snake.Positions.Skip(1)], world);
                if (result is null)
                    continue;

                otherBlocks.Add(result.NextPosition);
                string name = GetName(snake.Name);
                mySnakes.Add(new SnakeTarget(name, Target.Food));
                justAdded.Add(name); // Make sure we don't send two moves for one snake when created
                yield return new SplitSnakeAction(snake, name, result);
            }
        }
        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(snakes.Select(i => GetTarget(i.Name))));
        foreach (var snake in snakes.Where(i => GetTarget(i.Name).Target == Target.Food && !justAdded.Contains(i.Name)))
        {
            if (snake is null || snake.Positions.Count == 0 || snake.Head is null)
                continue;

            (PlanResult? result, Position destination) = PlanToFood([.. otherBlocks], snake.Head, [.. snake.Positions], world);
            if (result is not null)
            {
                Console.WriteLine("Goto food");
                yield return new MoveSnakeAction(snake, destination, result);
                otherBlocks.Add(result.NextPosition);
            }
        }

        foreach (var snake in snakes.Where(i => GetTarget(i.Name).Target == Target.Home && !justAdded.Contains(i.Name)))
        {
            if (snake is null || snake.Positions.Count == 0 || snake.Head is null)
                continue;

            PlanResult? result = planner.PlanNextMove(otherBlocks.ToArray(), snake.Head, home);
            if (result is not null)
            {
                Console.WriteLine("Goto home");
                yield return new MoveSnakeAction(snake, home, result);
                otherBlocks.Add(result.NextPosition);
            }
            else
            {
                Console.WriteLine($"Failed to plan snake {snake.Name} a path home. It is stuck.");
            }
        }
    }

    private SnakeTarget GetTarget(string name) => mySnakes.Where(i => i.SnakeName == name).First();

    private (PlanResult? result, Position destination) PlanToFood(Position[] blocked, Position current, Position[] currentSnake, World world)
    {
        foreach (var food in Finder.SortToDistance(current, world.GetFood()))
        {
            PlanResult? result = planner.PlanNextMove(blocked, current, food);
            if (result is null)
                continue;

            if (currentSnake.Contains(result.NextPosition))
                continue;

            return (result, food);
        }
        return default;
    }

    private string GetName(string lastName)
    {
        return lastName + lastName[^1];
    }
}
