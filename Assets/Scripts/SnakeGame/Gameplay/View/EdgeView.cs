using SnakeGame.Utils;
using UnityEngine;

namespace SnakeGame.Gameplay.View
{
    public class EdgeView : MonoBehaviour
    {
        [field: SerializeField] public EdgeView OppositeEdge { get; private set; }
        [SerializeField] private Transform innerBorderCenter;
        public GameSpaceVector InnerBorderCenter => GameSpaceVector.FromWorldSpaceVector(innerBorderCenter.position);
        public Vector3 EdgeNormal { get; private set; }

        public void Initialize(float snakeElementSize, Vector2 edgeNormal)
        {
            EdgeNormal = new GameSpaceVector(edgeNormal).ToWorldSpace();

            AlignBorderToMultipleOfSnakeSize(snakeElementSize);
        }

        private void AlignBorderToMultipleOfSnakeSize(float snakeElementSize)
        {
            GameSpaceVector nearestMultiplicationOfSnakeSize = new GameSpaceVector(
                Mathf.Round(InnerBorderCenter.X / snakeElementSize) * snakeElementSize,
                Mathf.Round(InnerBorderCenter.Y / snakeElementSize) * snakeElementSize
            );

            Vector3 pivotOffset = transform.position - innerBorderCenter.transform.position;

            transform.position = nearestMultiplicationOfSnakeSize.ToWorldSpace() + pivotOffset;
        }
    }
}