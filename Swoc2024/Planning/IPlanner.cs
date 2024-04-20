namespace Swoc2024.Planning;

public interface IPlanner
{

    PlanResult? PlanNextMove(Position[] blocked, Position current, Position target);
}
