using Swoc2024;
using Swoc2024.Planning;
using System.Numerics;

namespace BasicTester;

public static class PlanningHelpers
{
    public record SplitAction(string OldName, Position NextPosition);
    public record MoveAction(string Name, Position NextPosition);

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

    public static void AssertPlanNextMoves(this IEnumerable<PlanSnakeAction> planned, World world, params MoveAction[] moves)
    {
        var nextMoves = planned.Where(i => i is MoveSnakeAction).Select(i => (i as MoveSnakeAction)!).ToList();

        Assert.That(nextMoves, Has.Count.EqualTo(moves.Length));

        foreach(var move in nextMoves)
        {
            var next = moves.ToList().Find(i => i.Name == move.Snake.Name)?.NextPosition;
            Assert.That(move.Plan.NextPosition, Is.EqualTo(next));
        }
    }

    public static void AssertPlanNextSplits(this IEnumerable<PlanSnakeAction> planned, World world, params SplitAction[] moves)
    {
        var nextMoves = planned.Where(i => i is SplitSnakeAction).Select(i => (i as SplitSnakeAction)!).ToList();

        Assert.That(nextMoves, Has.Count.EqualTo(moves.Length));

        for (int i = 0; i < moves.Length; i++)
        {
            Assert.Multiple(() => {
                Assert.That(nextMoves[i], Is.TypeOf<SplitSnakeAction>());

                Assert.That(nextMoves[i].Snake.Name, Is.EqualTo(moves[i].OldName));
                Assert.That(nextMoves[i].Plan.NextPosition, Is.EqualTo(moves[i].NextPosition));
            });
        }
    }
}
