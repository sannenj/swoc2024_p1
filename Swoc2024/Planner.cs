namespace Swoc2024;

public class Planner
{
    public enum Target
    {
        Food
    }

    private record SnakeTarget(string Snake, Target Target);

    
    private List<SnakeTarget> mySnakes;
    
    public Planner()
    {
        mySnakes = [];
    }

    public void SetMySnake(string snake, Target target) => mySnakes.Add(new SnakeTarget(snake, target));

    public void RemoveMySnake(string snake) => mySnakes.RemoveAll(i => i.Snake == snake);

    public IEnumerable<PlannedSnakeMove> PlanSnakes(World world)
    {
        var snakes = world.GetSnakes().Where(i => mySnakes.Where(j => j.Snake == i.Name).Any());

        return new List<PlannedSnakeMove>();
    }
}
