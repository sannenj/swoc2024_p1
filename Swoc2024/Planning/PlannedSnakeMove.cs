namespace Swoc2024.Planning;

public class PlannedSnakeMove(Snake snake, Position destination, PlanResult plan) : IEquatable<PlannedSnakeMove?>
{
    public Snake Snake { get; set; } = snake;
    public Position Destination { get; set; } = destination;
    public PlanResult Planned { get; set; } = plan;

    public override bool Equals(object? obj)
    {
        return Equals(this, obj as PlannedSnakeMove);
    }

    public bool Equals(PlannedSnakeMove? other)
    {
        return Equals(this, other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Snake, Planned);
    }

    public static bool operator ==(PlannedSnakeMove? left, PlannedSnakeMove? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(PlannedSnakeMove? left, PlannedSnakeMove? right)
    {
        return !Equals(left, right);
    }

    private static bool Equals(PlannedSnakeMove? left, PlannedSnakeMove? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null)
        {
            return false;
        }

        return left.Snake == right.Snake && left.Planned == right.Planned;
    }
}
