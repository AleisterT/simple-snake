using SnakeGame.View;
using UnityEngine;

namespace SnakeGame
{
    [CreateAssetMenu(fileName = "SnakeConfig", menuName = "Game Configs/Snake Config")]
    public class SnakeConfig : GameConfig
    {
        [field: SerializeField] public SnakeElementView ElementPrefab { get; private set; }
    }
}