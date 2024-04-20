using Swoc2024;
using Swoc2024.Planning;
using static BasicTester.PlanningHelpers;


namespace BasicTester;

public class AStarPlannerTesting
{
    [Test]
    public void SimpleTwoDimensionTest0_0_to_5_5()
    {
        AStarPlanner planner = new([10, 10]);
        PlanResult? result = planner.PlanNextMove([], new Position([0, 0]), new Position([5, 5]));
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.MoveCount, Is.EqualTo(10));
            Assert.That(result.NextPosition, Is.EqualTo(new Position([1, 0])));
        });
    }

    [Test]
    public void SimpleTwoDimensionTest5_0_to_5_5()
    {
        AStarPlanner planner = new([10, 10]);
        PlanResult? result = planner.PlanNextMove([], new Position([5, 0]), new Position([5, 5]));
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.MoveCount, Is.EqualTo(5));
            Assert.That(result.NextPosition, Is.EqualTo(new Position([5, 1])));
        });
    }

    [Test]
    public void SimpleThreeDimensionTest9_4_2_to_10_4_2()
    {
        AStarPlanner planner = new([20, 20, 20]);
        PlanResult? result = planner.PlanNextMove([], new Position([9, 4, 2]), new Position([10, 4, 2]));
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.MoveCount, Is.EqualTo(1));
            Assert.That(result.NextPosition, Is.EqualTo(new Position([10, 4, 2])));
        });
    }

    [Test]
    public void SimpleThreeDimensionTest9_4_2_to_2_5_80()
    {
        AStarPlanner planner = new([200, 200, 200]);
        PlanResult? result = planner.PlanNextMove([], new Position([9, 4, 2]), new Position([2, 5, 80]));
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.MoveCount, Is.EqualTo(86));
            Assert.That(result.NextPosition, Is.EqualTo(new Position([8, 4, 2])));
        });
    }

    [Test]
    public void PlanAroundInTwoDimension()
    {
        AStarPlanner planner = new([200, 200]);
        PlanResult? result = planner.PlanNextMove([new Position([9, 5])], new Position([9, 4]), new Position([9, 8]));
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.MoveCount, Is.EqualTo(6));
            Assert.That(result.NextPosition, Is.EqualTo(new Position([8, 4])));
        });
    }


    [Test]
    public void PlanAroundInTwoDimensionFullPath()
    {
        AStarPlanner planner = new([20, 20]);
        Position current = new([9, 4]);
        Position target = new([9, 8]);
        Position[] blocked = [
            new([9, 5]),
        ];
        List<PlanResult> result = PlanResult(blocked, planner, current, target).ToList();
        Assert.That(result, Has.Count.EqualTo(6));
        Assert.Multiple(() =>
        {
            AssertPlannedMoveCountDown(result, 6);
            AssertPlannedPositions(result, [
                    new Position([8, 4]),
                    new Position([8, 5]),
                    new Position([8, 6]),
                    new Position([9, 6]),
                    new Position([9, 7]),
                    new Position([9, 8]),
            ]);
        });
    }

    [Test]
    public void PlanAroundInTwoDimensionWallFullPath()
    {
        AStarPlanner planner = new([20, 20]);
        Position[] blocked = [
            new ([10, 5]),
            new ([9, 5]),
            new([8, 5]),
        ];
        Position target = new([9, 8]);
        Position current = new([9, 4]);
        var result = PlanResult(blocked, planner, current, target).ToList();
        Assert.That(result, Has.Count.EqualTo(8));
        Assert.Multiple(() =>
        {
            AssertPlannedMoveCountDown(result, 8);
            AssertPlannedPositions(result, [
                new Position([8, 4]),
                new Position([7, 4]),
                new Position([7, 5]),
                new Position([7, 6]),
                new Position([8, 6]),
                new Position([9, 6]),
                new Position([9, 7]),
                new Position([9, 8])
            ]);
        });
    }


    [Test]
    public void PlanAroundInTwoDimensionWallAroundTargetFullPathOrigin9_4()
    {
        Position current = new([9, 4]);
        Position target = new([9, 8]);
        Position[] blocked = [
            new ([target.Positions[0] - 1,  target.Positions[1]]),
            new ([target.Positions[0] - 1,  target.Positions[1] + 1]),
            new ([target.Positions[0] - 1,  target.Positions[1] - 1]),
            new ([target.Positions[0],      target.Positions[1] + 1]),
            new ([target.Positions[0],      target.Positions[1] - 1]),
            new ([target.Positions[0] + 1,  target.Positions[1] - 1]),
            new ([target.Positions[0] + 1,  target.Positions[1] + 1]),
          //new ([target.Positions[0] + 1,  target.Positions[1]]),
        ];

        AStarPlanner planner = new([20, 20]);
        List<PlanResult> results = PlanResult(blocked, planner, current, target).ToList();
        Assert.That(results, Has.Count.EqualTo(8));
        Assert.Multiple(() =>
        {
            AssertPlannedMoveCountDown(results, 8);
            AssertPlannedPositions(results, [
                new([9,  5]),
                new([9,  6]),
                new([10,  6]),
                new([11,  6]),
                new([11,  7]),
                new([11,  8]),
                new([10, 8]),
                new([9, 8]),
            ]);
        });
    }


    [Test]
    public void PlanAroundInTwoDimensionWallAroundTargetFullPathOrigin6_9()
    {
        Position current = new([6, 9]);
        Position target = new([9, 8]);
        Position[] blocked = [
            new ([target.Positions[0] - 1,  target.Positions[1]]),
            new ([target.Positions[0] - 1,  target.Positions[1] + 1]),
            new ([target.Positions[0] - 1,  target.Positions[1] - 1]),
            new ([target.Positions[0],      target.Positions[1] + 1]),
            new ([target.Positions[0],      target.Positions[1] - 1]),
            new ([target.Positions[0] + 1,  target.Positions[1] - 1]),
            new ([target.Positions[0] + 1,  target.Positions[1] + 1]),
          //new ([target.Positions[0] + 1,  target.Positions[1]]),
        ];

        AStarPlanner planner = new([20, 20]);
        List<PlanResult> results = PlanResult(blocked, planner, current, target).ToList();
        Assert.That(results, Has.Count.EqualTo(10));
        Assert.Multiple(() =>
        {
            AssertPlannedMoveCountDown(results, 10);
            AssertPlannedPositions(results, [
                new([7,  9]),
                new([7, 10]),
                new([8, 10]),
                new([9, 10]),
                new([10, 10]),
                new([11, 10]),
                new([11, 9]),
                new([11, 8]),
                new([10, 8]),
                new([9, 8]),
            ]);
        });
    }


    [Test]
    public void PlanAroundInTwoDimensionFullPathButDimensionRangeLimited()
    {
        AStarPlanner planner = new([20, 20]);
        Position current = new([10, 18]);
        Position target = new([19, 18]);
        Position[] blocked = [
            new([15, 19]),
            new([15, 19]),
            new([15, 18]),
            new([15, 17]),
            new([15, 16]),
            new([15, 15]),
        ];
        List<PlanResult> result = PlanResult(blocked, planner, current, target).ToList();
        Assert.That(result, Has.Count.EqualTo(17));
        Assert.Multiple(() =>
        {
            AssertPlannedMoveCountDown(result, 17);
            AssertPlannedPositions(result, [
                    new Position([11, 18]),
                    new Position([12, 18]),
                    new Position([13, 18]),
                    new Position([14, 18]),
                    new Position([14, 17]),
                    new Position([14, 16]),
                    new Position([14, 15]),
                    new Position([14, 14]),
                    new Position([15, 14]),
                    new Position([16, 14]),
                    new Position([17, 14]),
                    new Position([18, 14]),
                    new Position([19, 14]),
                    new Position([19, 15]),
                    new Position([19, 16]),
                    new Position([19, 17]),
                    new Position([19, 18])
            ]);
        });
    }
}
