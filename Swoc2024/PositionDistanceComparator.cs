namespace Swoc2024;

/// <summary>
/// Position based on closest to target.
/// </summary>
/// <param name="target">The coordinate to be closest too.</param>
public class PositionDistanceComparator(Position target) : IComparer<Position>
{
    public int Compare(Position? x, Position? y)
    {
        if (x is null || y is null) return 0;

        int dx = x.DistanceTo(target);
        int dy = y.DistanceTo(target);
        if (dx > dy) return 1;
        if (dx < dy) return -1;
        return 0;
    }
}
