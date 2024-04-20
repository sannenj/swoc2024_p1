namespace Swoc2024.Planning;

public enum SnakeAction
{
    Move,
    Split
}

public record PlanSnakeAction(SnakeAction SnakeAction, Snake Snake);

public record MoveSnakeAction(Snake Snake, Position Destination, PlanResult Plan) : PlanSnakeAction(SnakeAction.Move, Snake);

public record SplitSnakeAction(Snake Snake, Position Destination, string NewName, PlanResult Plan) : PlanSnakeAction(SnakeAction.Split, Snake);
