﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Swoc2024
{
    public class Planner
    {
        IPlanner planner;
        public enum Target
        {
            Food
        }

        private record SnakeTarget(string Snake, Target Target);

        private readonly List<SnakeTarget> mySnakes;

        public Planner(IPlanner plan)
        {
            planner = plan;
            mySnakes = [];
        }

        public void SetMySnake(string snake, Target target) => mySnakes.Add(new SnakeTarget(snake, target));

        public void RemoveMySnake(string snake) => mySnakes.RemoveAll(i => i.Snake == snake);

        public IEnumerable<PlannedSnakeMove> PlanSnakes(World world)
        {
            var snakes = world.GetSnakes().Where(i => mySnakes.Where(j => j.Snake == i.Name).Any()).ToList();
            var food = world.GetFood().ToList();

            var otherBlocks = world.GetSnakes().Where(i => !mySnakes.Where(j => j.Snake == i.Name).Any()).SelectMany(i => i.Positions).ToList();

            List<(PlannedSnakeMove?, int)> plans = [];

            foreach(var snake in snakes)
            {
                if (snake.Head is null)
                    continue;
                var foods = Finder.SortToDistance(snake.Head, food);
                foreach(var foodtarget in foods)
                {
                    IPlanner.PlanResult? result = planner.PlanNextMove(otherBlocks.ToArray(), snake.Head, foodtarget);
                    if (result is null)
                        continue;

                    if (snake.Positions.Contains(result.NextPosition))
                        continue;

                    yield return new PlannedSnakeMove(snake, result.NextPosition, foodtarget);
                    otherBlocks.Add(result.NextPosition);
                    break;
                }
            }
        }
    }
}
