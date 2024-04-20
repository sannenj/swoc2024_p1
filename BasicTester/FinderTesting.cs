using NUnit.Framework.Internal;
using Swoc2024;

namespace BasicTester;

internal class FinderTesting
{
    [Test]
    public void TestDistanceSortingTwoDimensions()
    {
        Position[] positions =
        [
            new([2, 2]),
            new([0, 1]),
            new([1, 1]),
        ];
        var sorted = Finder.SortToDistance(new Position([0, 0]), positions).ToList();
        Assert.Multiple(() =>
        {
            Assert.That(sorted, Has.Count.EqualTo(3));
            Assert.That(sorted[0], Is.EqualTo(positions[1]));
            Assert.That(sorted[1], Is.EqualTo(positions[2]));
            Assert.That(sorted[2], Is.EqualTo(positions[0]));
        });
    }

    [Test]
    public void TestDistanceSortingThreeDimensions()
    {
        Position[] positions =
        [
            new([2, 2, 0]),
            new([1, 1, 2]),
            new([0, 1, 5]),
            new([1, 0, 2]),
        ];
        var sorted = Finder.SortToDistance(new Position([0, 0, 0]), positions).ToList();
        Assert.Multiple(() =>
        {
            Assert.That(sorted, Has.Count.EqualTo(4));
            Assert.That(sorted[0], Is.EqualTo(positions[3]));
            Assert.That(sorted[1], Is.EqualTo(positions[0]));
            Assert.That(sorted[2], Is.EqualTo(positions[1]));
            Assert.That(sorted[3], Is.EqualTo(positions[2]));
        });
    }
}
