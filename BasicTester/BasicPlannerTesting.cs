﻿using Swoc2024;
using Swoc2024.Planning;

namespace BasicTester;

public class BasicPlannerTesting
{
    [Test]
    public void TargetFirstFood()
    {
        Planner planner = new(new AStarPlanner());
        planner.SetMySnake("me", Planner.Target.Food);
        World world = new();
        world.StartWorld([
            new Cell(new([0, 0]), "me", false),
            new Cell(new([0, 5]), "", true),
            new Cell(new([8, 2]), "", true),
        ]);
        planner.AssertPlanNextMoves(world, [new ([0, 1])]);
    }

    [Test]
    public void ContinueAfterFirstFood()
    {
        Planner planner = new(new AStarPlanner());
        World world = new();
        planner.SetMySnake("me", Planner.Target.Food);
        world.StartWorld([
            new Cell(new([0, 4]), "me", false),
            new Cell(new([0, 5]), "", true),
            new Cell(new([1, 3]), "", true),
        ]);
        planner.AssertPlanNextMoves(world, [
            new([0, 5])
        ]);

        Assert.That(world.GetSnakes().First().Positions, Has.Count.EqualTo(1));
        world.QueueUpdate(new Cell(new([0, 5]), "me", false));
        world.QueueUpdate(new Cell(new([0, 4]), "", false));
        Assert.That(world.GetSnakes().First().Positions, Has.Count.EqualTo(1));

        planner.AssertPlanNextMoves(world, [
            new([1, 5])
        ]);

        world.QueueUpdate(new Cell(new([1, 5]), "me", false));
        Assert.That(world.GetSnakes().First().Positions, Has.Count.EqualTo(2));

        planner.AssertPlanNextMoves(world, [
            new([1, 4])
        ]);
    }
}