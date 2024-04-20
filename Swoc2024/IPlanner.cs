namespace Swoc2024;

public interface IPlanner
{
    public record PlanResult (int MoveCount, Position NextPosition);

    PlanResult? PlanNextMove(Position[] blocked, Position current, Position target);
}
