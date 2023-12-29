using System;
using UnityEngine;

namespace SnakeGame.Utils
{
    public static class DirectionExtensions
    {
        public static GameSpaceVector ToGameVector(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => new GameSpaceVector(Vector2.up),
                Direction.Down => new GameSpaceVector(Vector2.down),
                Direction.Left => new GameSpaceVector(Vector2.left),
                Direction.Right => new GameSpaceVector(Vector2.right),
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };
        }
    }
}