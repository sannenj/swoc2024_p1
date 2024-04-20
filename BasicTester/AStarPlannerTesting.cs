using Swoc2024;

namespace BasicTester;

public class AStarPlannerTesting
{
    AStarPlanner planner;

    [SetUp]
    public void SetUp()
    {
        planner = new();
    }

    [Test]
    public void SimpleTwoDimensionTest0_0_to_5_5()
    {
        IPlanner.PlanResult? result = planner.PlanNextMove([], new Position([0, 0]), new Position([5, 5]));
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
        IPlanner.PlanResult? result = planner.PlanNextMove([], new Position([5, 0]), new Position([5, 5]));
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
        IPlanner.PlanResult? result = planner.PlanNextMove([], new Position([9, 4, 2]), new Position([10, 4, 2]));
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
        IPlanner.PlanResult? result = planner.PlanNextMove([], new Position([9, 4, 2]), new Position([2, 5, 80]));
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
        IPlanner.PlanResult? result = planner.PlanNextMove([new Position([9, 5])], new Position([9, 4]), new Position([9, 8]));
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
        Position current = new([9, 4]);
        Position target = new([9, 8]);
        Position[] blocked = [
            new([9, 5]),
        ];
        List<IPlanner.PlanResult> result = PlanResult(blocked, planner, current, target).ToList();
        Assert.That(result, Has.Count.EqualTo(6));
        Assert.Multiple(() =>
        {
            AssertMoveCountDown(result, 6);
            AssertPositions(result, [
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
            AssertMoveCountDown(result, 8);
            AssertPositions(result, [
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

        List<IPlanner.PlanResult> results = PlanResult(blocked, planner, current, target).ToList();
        Assert.That(results, Has.Count.EqualTo(8));
        Assert.Multiple(() =>
        {
            AssertMoveCountDown(results, 8);
            AssertPositions(results, [
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

        List<IPlanner.PlanResult> results = PlanResult(blocked, planner, current, target).ToList();
        Assert.That(results, Has.Count.EqualTo(10));
        Assert.Multiple(() =>
        {
            AssertMoveCountDown(results, 10);
            AssertPositions(results, [
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

    private IEnumerable<IPlanner.PlanResult> PlanResult(Position[] blocked, IPlanner planner, Position current, Position target)
    {
        const int maxCount = 20;
        int count = 0;
        IPlanner.PlanResult? result = planner.PlanNextMove(blocked, current, target);
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

    private void AssertMoveCountDown(IEnumerable<IPlanner.PlanResult> plans, int startCount)
    {
        int count = startCount;
        int indexCount = 0;
        foreach(var plan in plans)
        {
            Assert.That(plan.MoveCount, Is.EqualTo(count), $"The MoveCount left for step ${indexCount} wasn't {count} but was {plan.MoveCount}.");
            count--;
            indexCount++;
        }
    }

    private void AssertPositions(IEnumerable<IPlanner.PlanResult> plans, params Position[] positions)
    {
        int count = 0;
        foreach (var plan in plans)
        {
            Assert.That(plan.NextPosition, Is.EqualTo(positions[count]), $"The Position for step ${count} wasn't {positions[count]} but was {plan.NextPosition}.");
            count++;
        }
    }
}
