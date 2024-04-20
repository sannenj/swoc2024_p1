namespace Swoc2024;

public class Cell(Position position, string player, bool hasFood)
{
    public Position Position { get; set; } = position;

    public string Player { get; set; } = player;

    public bool HasFood { get; set; } = hasFood;
}
