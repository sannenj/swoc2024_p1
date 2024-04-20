using Swoc2024;

namespace BasicTester
{
    public class WorldTesting
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void JustOneSnake()
        {
            World world = new();

            var positions = new Position[]
            {
                new([0, 0, 2]),
                new([0, 0, 1]),
                new([0, 0, 3])
            };

            world.StartWorld(
            [
                new(positions[0], "Hello", false),
                new(positions[1], "Hello", false),
                new(positions[2], "Hello", false),
            ]);

            var snakes = world.GetSnakes();
            Assert.Multiple(() =>
            {
                Assert.That(snakes.Count(), Is.EqualTo(1));
                Snake expectedSnake = new("Hello", positions);
                Assert.That(snakes.First(), Is.EqualTo(expectedSnake));
            });
        }

        [Test]
        public void JustOneSnakeMoving()
        {
            World world = new();

            var positions = new Position[]
            {
                new([0, 0, 1]),
                new([0, 0, 2]),
                new([0, 0, 3]),
                new([0, 0, 4])
            };

            world.StartWorld(
            [
                new(positions[0], "Hello", false),
                new(positions[1], "Hello", false),
                new(positions[2], "Hello", false),
            ]);

            world.QueueUpdate(new Cell(positions[3], "Hello", false));
            world.QueueUpdate(new Cell(positions[0], "", false));

            var snakes = world.GetSnakes();
            Assert.Multiple(() =>
            {
                Assert.That(snakes.Count(), Is.EqualTo(1));
                var pos = positions.Skip(1).Take(3).ToArray();
                Snake expectedSnake = new("Hello", pos);
                Assert.That(snakes.First(), Is.EqualTo(expectedSnake));
            });
        }


        [Test]
        public void TwoBasicSnakes()
        {
            World world = new();

            var positions = new Position[]
            {
                new([0, 0, 1]),
                new([0, 0, 2]),
                new([0, 0, 3]),
                new([0, 1, 3]),
                new([0, 1, 2]),
                new([0, 1, 1])
            };

            world.StartWorld(
            [
                new(positions[0], "Snake1", false),
                new(positions[1], "Snake1", false),
                new(positions[2], "Snake1", false),
                new(positions[3], "Snake2", false),
                new(positions[4], "Snake2", false),
            ]);

            var snakes = world.GetSnakes().ToList();
            Assert.Multiple(() =>
            {
                Assert.That(snakes, Has.Count.EqualTo(2));
                Snake expectedSnake1 = new("Snake1", positions.Take(3).ToArray());
                Snake expectedSnake2 = new("Snake2", positions.Skip(3).Take(2).ToArray());
                Assert.That(snakes.ElementAt(0), Is.EqualTo(expectedSnake1));
                Assert.That(snakes.ElementAt(1), Is.EqualTo(expectedSnake2));
            });
        }

        [Test]
        public void GetWithFoodOnly()
        {
            World world = new();

            var positions = new Position[]
            {
                new([0, 0, 2]),
                new([0, 0, 1]),
                new([0, 0, 3])
            };

            world.StartWorld(
            [
                new(positions[0], "", true),
                new(positions[1], "", true),
                new(positions[2], "", true),
            ]);

            var snakes = world.GetSnakes().ToList();
            var food = world.GetFood().ToList();
            Assert.Multiple(() =>
            {
                Assert.That(snakes, Has.Count.EqualTo(0));
                Assert.That(food, Has.Count.EqualTo(3));
                Assert.That(food, Is.EquivalentTo(positions));
            });
        }
    }
}