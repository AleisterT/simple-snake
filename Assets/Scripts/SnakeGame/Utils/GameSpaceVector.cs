using UnityEngine;

namespace SnakeGame.Utils
{
    public readonly struct GameSpaceVector
    {
        private readonly Vector2 _vector;

        public float X => _vector.x;
        public float Y => _vector.y;

        public GameSpaceVector(float x, float y)
        {
            _vector = new(x, y);
        }

        public GameSpaceVector(Vector2 vector)
        {
            _vector = vector;
        }

        public static GameSpaceVector FromWorldSpaceVector(Vector3 worldSpaceVector)
        {
            return new(worldSpaceVector.x, worldSpaceVector.z);
        }

        public Vector3 ToWorldSpace() => new(_vector.x, 0, _vector.y);
    }
}