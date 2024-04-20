using Swoc2024;
using Swoc2024.Planning;
using System.Numerics;

namespace BasicTester;

public class PlanningSplitTesting
{
    [Test]
    public void TestBasicSplit()
    {
        var plan = new Planner(new AStarPlanner(), new([0, 0]));
        World world = new();
        world.StartWorld([
            new Cell(new([8, 8]), "", true),
            new Cell(new([5, 8]), "me", false),
            new Cell(new([5, 7]), "me", false),
            new Cell(new([5, 6]), "me", false),
            new Cell(new([5, 5]), "me", false),
            new Cell(new([5, 4]), "me", false),
            new Cell(new([5, 3]), "me", false),
        ]);

        plan.SetMySnake("me", Planner.Target.Food);
        var planned = plan.PlanSnakes(world).ToList();

        planned.AssertPlanNextMoves(world, [
            new("me", new([4, 3]))
        ]);

        planned.AssertPlanNextSplits(world, [
            new PlanningHelpers.SplitAction("me", new([6, 8])),
        ]);

        world.QueueUpdate(new Cell(new([4, 3]), "me", false));
        world.QueueUpdate(new Cell(new([6, 8]), "mee", false));
        world.QueueUpdate(new Cell(new([5, 8]), "", false));
        world.QueueUpdate(new Cell(new([5, 7]), "", false));


        planned = plan.PlanSnakes(world).ToList();

        planned.AssertPlanNextMoves(world, [
            new("mee", new([7, 8])),
            new("me", new([3, 3])),
        ]);

        planned.AssertPlanNextSplits(world, []);
    }
}
