using System.Diagnostics;

namespace Swoc2024;

[DebuggerStepThrough]
public class Position(int[] positions) : IEquatable<Position?>
{
    public int[] Positions { get; set; } = positions;

    public int Dimensions => Positions.Length;

    public int DistanceTo(Position target)
    {
        // Calculate the Manhattan distance as the heuristic
        int distance = 0;
        for (int i = 0; i < Positions.Length; i++)
        {
            distance += Math.Abs(Positions[i] - target.Positions[i]);
        }
        return distance;
    }

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
