namespace Swoc2024;

public class PlannedSnakeMove : IEquatable<PlannedSnakeMove?>
{
    public Snake Snake { get; set; }
    public Position NextPosition { get; set; }

    public PlannedSnakeMove(Snake snake, Position nextPosition)
    {
        Snake = snake;
        NextPosition = nextPosition;
    }

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
        return HashCode.Combine(Snake, NextPosition);
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

        return left.Snake == right.Snake && left.NextPosition == right.NextPosition;
    }
}
