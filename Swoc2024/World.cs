using System.Collections.Concurrent;
using System.Xml.Linq;

namespace Swoc2024;

public class World
{
    public event EventHandler<Snake> SnakeDied;

    readonly ConcurrentQueue<Cell> cellQueue;
    readonly Dictionary<string, Snake> snakes;
    readonly List<Position> food;
    bool worldStarted;

    public World()
    {
        worldStarted = false;
        food = [];
        snakes = [];
        cellQueue = new();
    }

    public void QueueUpdate(Cell cell)
    {
        cellQueue.Enqueue(cell);
        ProcessQueue();
    }

    public IEnumerable<Snake> GetSnakes() => [.. snakes.Values];

    public IEnumerable<Position> GetFood() => [.. food];

    public void StartWorld(IEnumerable<Cell> cells)
    {
        worldStarted = true;

        foreach (var cell in cells)
        {
            ProcessCell(cell);
        }

        ProcessQueue();
    }

    private void ProcessQueue()
    {
        if (worldStarted == false)
        {
            return;
        }
        while (cellQueue.TryDequeue(out Cell? result))
        {
            ProcessCell(result);
        }
    }

    private void ProcessCell(Cell cell)
    {
        if (string.IsNullOrWhiteSpace(cell.Player) && !cell.HasFood)
        {
            string? toRemoveKey = default;
            foreach (var snake in snakes)
            {
                if (snake.Value.Positions.RemoveAll(i => i == cell.Position) > 0)
                {
                    if (snake.Value.Positions.Count == 0)
                    {
                        toRemoveKey = snake.Key;
                    }
                }
            }
            if (toRemoveKey != null)
            {
                SnakeDied?.Invoke(this, snakes[toRemoveKey]);
                snakes.Remove(toRemoveKey);
            }
        }
        else if (!cell.HasFood)
        {
            if (snakes.ContainsKey(cell.Player) == false)
            {
                snakes.Add(cell.Player, new Snake(cell.Player, cell.Position));
            }
            if (snakes[cell.Player].Positions.Contains(cell.Position) == false)
            {
                snakes[cell.Player].Positions.Add(cell.Position);
            }
            if (food.Contains(cell.Position))
            {
                food.Remove(cell.Position);
            }
        }
        else if (cell.HasFood)
        {
            if (!food.Contains(cell.Position))
            {
                food.Add(cell.Position);
            }
        }
    }
}
