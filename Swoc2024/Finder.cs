namespace Swoc2024
{
    public static class Finder
    {
        public record DistancePosition(Position Position, double Distance);

        public static IEnumerable<Position> SortToDistance(Position current, IEnumerable<Position> targets)
        {
            var li = targets.ToList();
            li.Sort(new PositionDistanceComparator(current));
            return li;
        }
    }
}
