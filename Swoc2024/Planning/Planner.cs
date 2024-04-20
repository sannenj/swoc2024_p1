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
        foreach (Snake snake in snakes.Where(i => i is not null))
        {
            if (snake.Positions.Count > 5)
            {
                int i = mySnakes.FindIndex(i => i.SnakeName == snake.Name);
                var foundSnake = mySnakes[i];
                foundSnake.Target = Target.Home;
                mySnakes[i] = foundSnake;
                (PlanResult? result, Position destination) = PlanToFood(otherBlocks.ToArray(), snake.Positions.First(), snake.Positions.ToArray(), world);
                if (result is null)
                    continue;
                string name = GetName(snake.Name);
                mySnakes.Add(new SnakeTarget(name, Target.Food));
                justAdded.Add(name);
                yield return new SplitSnakeAction(snake, destination, name, result);
            }
        }

        foreach (var snake in snakes.Where(i => GetTarget(i.Name).Target == Target.Food && !justAdded.Contains(i.Name)))
        {
            if (snake.Head is null)
                continue;

            (PlanResult? result, Position destination) = PlanToFood([.. otherBlocks], snake.Head, [.. snake.Positions], world);
            if (result is not null)
            {
                yield return new MoveSnakeAction(snake, destination, result);
                otherBlocks.Add(result.NextPosition);
            }
        }

        foreach (var snake in snakes.Where(i => GetTarget(i.Name).Target == Target.Home && !justAdded.Contains(i.Name)))
        {
            if (snake.Head is null)
                continue;

            PlanResult? result = planner.PlanNextMove([.. otherBlocks], snake.Head, home);
            if (result is null)
                continue;

            if (snake.Positions.Contains(result.NextPosition))
                continue;

            
            yield return new MoveSnakeAction(snake, home, result);
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
