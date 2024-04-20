using Swoc2024;
using Swoc2024.Planning;
using System.Numerics;

namespace BasicTester;

public class BasicPlannerTesting
{
    [Test]
    public void TargetFirstFood()
    {
        Planner planner = new(new AStarPlanner([20, 20]), new([0, 0]));
        planner.SetMySnake("me", Planner.Target.Food);
        World world = new();
        world.StartWorld([
            new Cell(new([0, 0]), "me", false),
            new Cell(new([0, 5]), "", true),
            new Cell(new([8, 2]), "", true),
        ]);
        var planned = planner.PlanSnakes(world).ToList();
        planned.AssertPlanNextMoves(world, [new("me", new ([0, 1]))]);
    }

    [Test]
    public void ContinueAfterFirstFood()
    {
        Planner planner = new(new AStarPlanner([20, 20]), new([0, 0]));
        World world = new();
        planner.SetMySnake("me", Planner.Target.Food);
        world.StartWorld([
            new Cell(new([0, 4]), "me", false),
            new Cell(new([0, 5]), "", true),
            new Cell(new([1, 3]), "", true),
        ]);
        var planned = planner.PlanSnakes(world).ToList();
        planned.AssertPlanNextMoves(world, [
            new("me", new([0, 5]))
        ]);

        Assert.That(world.GetSnakes().First().Positions, Has.Count.EqualTo(1));
        world.QueueUpdate(new Cell(new([0, 5]), "me", false));
        world.QueueUpdate(new Cell(new([0, 4]), "", false));
        Assert.That(world.GetSnakes().First().Positions, Has.Count.EqualTo(1));
        planned = planner.PlanSnakes(world).ToList();

        planned.AssertPlanNextMoves(world, [
            new("me", new([1, 5]))
        ]);

        world.QueueUpdate(new Cell(new([1, 5]), "me", false));
        Assert.That(world.GetSnakes().First().Positions, Has.Count.EqualTo(2));
        planned = planner.PlanSnakes(world).ToList();

        planned.AssertPlanNextMoves(world, [
            new("me", new([1, 4]))
        ]);
    }
}
