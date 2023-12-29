using UnityEngine;

namespace SnakeGame.View
{
    public class EdgeView : MonoBehaviour
    {
        [field: SerializeField] public EdgeView OppositeEdge { get; private set; }
        [SerializeField] private Transform innerBorderCenter;
        public GameSpaceVector InnerBorderCenter => GameSpaceVector.FromWorldSpaceVector(innerBorderCenter.position);
    }
}