
namespace Swoc2024;

public class Snake(string name, params Position[] positions) : IEquatable<Snake?>
{
    public string Name { get; set; } = name;
    public List<Position> Positions { get; set; } = [.. positions];

    public override string ToString()
    {
        return $"{{{Name}, {Positions.Count}}}";
    }

    public override bool Equals(object? obj)
    {
        return obj is Snake snk && Equals(this, snk);
    }

    public bool Equals(Snake? other)
    {
        return Equals(this, other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Positions);
    }

    public static bool operator ==(Snake left, Snake right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Snake left, Snake right)
    {
        return !Equals(left, right);
    }

    private static bool Equals(Snake? snake1, Snake? snake2)
    {
        if (ReferenceEquals(snake1, snake2))
        {
            return true;
        }

        if (snake1 is null || snake2 is null)
        {
            return false;
        }

        return snake1.Name == snake2.Name && snake1.Positions.SequenceEqual(snake2.Positions);
    }
}
