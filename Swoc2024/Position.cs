
namespace Swoc2024;

public class Position(int[] positions) : IEquatable<Position?>
{
    public int[] Positions { get; set; } = positions;

    public static bool operator ==(Position pos1, Position pos2)
    {
        return Equals(pos1, pos2);
    }

    public static bool operator !=(Position pos1, Position pos2)
    {
        return !Equals(pos1, pos2);
    }

    public override bool Equals(object? obj)
    {
        return obj is Position pos && Equals(this, pos);
    }

    public bool Equals(Position? other)
    {
        return Equals(this, other);
    }

    public static bool Equals(Position? pos1, Position? pos2)
    {
        if (pos1 is null || pos2 is null)
            return false;

        if (pos1.Positions.Length != pos2.Positions.Length)
            return false;

        for (int i = 0; i < pos1.Positions.Length; i++)
        {
            if (pos1.Positions[i] != pos2.Positions[i])
                return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Positions);
    }

    public override string ToString()
    {
        return $"{string.Join(',', Positions.Select(i => i.ToString()))}";
    }
}
