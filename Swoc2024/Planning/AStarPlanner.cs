namespace Swoc2024.Planning;

public class AStarPlanner(int[] dimensionSizes) : IPlanner
{
    public int[] dimensions = dimensionSizes;

    public PlanResult? PlanNextMove(Position[] blocked, Position current, Position target)
    {
        (Position? pos, int distance) = GetNextToTarget(current, target, blocked, current.Dimensions);
        return pos is null ? null : new PlanResult(distance, pos);
    }

    private (Position?, int) GetNextToTarget(Position current, Position target, Position[] blocked, int k)
    {
        // A* Pathfinding Algorithm
        var openSet = new List<Position>([current]);
        var cameFrom = new Dictionary<Position, Position>();
        var gScore = new Dictionary<Position, int> { { current, 0 } };
        var fScore = new Dictionary<Position, int> { { current, current.DistanceTo(target) } };

        while (openSet.Any())
        {
            var currentPos = openSet.OrderBy(pos => fScore[pos]).First();

            if (currentPos == target)
            {
                return ReconstructPath(cameFrom, currentPos);
            }

            openSet.Remove(currentPos);

            foreach (var neighbor in GetNeighbors(currentPos, blocked, k))
            {
                var tentativeGScore = gScore[currentPos] + 1;

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = currentPos;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + neighbor.DistanceTo(target);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        // No path found
        return default;
    }

    private List<Position> GetNeighbors(Position position, Position[] blocked, int k)
    {
        var neighbors = new List<Position>();

        for (int i = 0; i < k; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                var coordinates = position.Positions.ToArray();
                coordinates[i] += j;

                var neighbor = new Position(coordinates);
                if (!blocked.Contains(neighbor) && neighbor.InDimension(dimensions))
                {
                    neighbors.Add(neighbor);
                }
            }
        }

        return neighbors;
    }

    private static (Position?, int) ReconstructPath(Dictionary<Position, Position> cameFrom, Position current)
    {
        if (cameFrom is null)
        {
            return default;
        }

        var path = new List<Position>([current]);

        int count = 0;
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
            count++;
        }
        if (path is null || path.Count == 0)
        {
            return default;
        }

        return (path[1], count);
    }
}
