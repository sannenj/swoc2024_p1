using Swoc2024;
using Swoc2024.Planning;

namespace BasicTester;

public static class PlanningHelpers
{
    public static IEnumerable<PlanResult> PlanResult(Position[] blocked, IPlanner planner, Position current, Position target)
    {
        const int maxCount = 20;
        int count = 0;
        PlanResult? result = planner.PlanNextMove(blocked, current, target);
        if (result is not null)
        {
            while (result is not null && result.NextPosition != target && count < maxCount)
            {
                yield return result;
                result = planner.PlanNextMove(blocked, result.NextPosition, target);
                count++;
            }
            if (result is not null && result.MoveCount != 0)
            {
                yield return result;
            }
        }
    }

    public static void AssertPlannedMoveCountDown(IEnumerable<PlanResult> plans, int startCount)
    {
        int count = startCount;
        int indexCount = 0;
        foreach (var plan in plans)
        {
            Assert.That(plan.MoveCount, Is.EqualTo(count), $"The MoveCount left for step ${indexCount} wasn't {count} but was {plan.MoveCount}.");
            count--;
            indexCount++;
        }
    }

    public static void AssertPlannedPositions(IEnumerable<PlanResult> plans, params Position[] positions)
    {
        int count = 0;
        foreach (var plan in plans)
        {
            Assert.That(plan.NextPosition, Is.EqualTo(positions[count]), $"The Position for step {count} wasn't {positions[count]} but was {plan.NextPosition}.");
            count++;
        }
    }

    public static void AssertPlanNextMoves(this Planner planner, World world, params Position[] moves)
    {
        var nextMoves = planner.PlanSnakes(world).ToList();

        Assert.That(nextMoves, Has.Count.EqualTo(moves.Length));

        for(int i = 0; i < moves.Length; i++)
        {
            Assert.That(nextMoves[i].Planned.NextPosition, Is.EqualTo(moves[i]));
        }

    }
}
