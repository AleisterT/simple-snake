using SnakeGame.Configs;
using SnakeGame.Gameplay.View;
using SnakeGame.Utils;
using UnityEngine;

namespace SnakeGame.Gameplay.Snake
{
    [CreateAssetMenu(fileName = "SnakeConfig", menuName = "Game Configs/Snake Config")]
    public class SnakeConfig : GameConfig
    {
        [field: SerializeField] public SnakeElementView ElementPrefab { get; private set; }
        [field: SerializeField] public float InitialSpeed { get; private set; }
        [field: SerializeField] public Direction InitialDirection { get; private set; }
        [field: SerializeField] public int InitialLength { get; private set; }
    }
}