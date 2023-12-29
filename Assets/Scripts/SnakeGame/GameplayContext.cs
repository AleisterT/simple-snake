using System;
using SnakeGame.Edibles;
using SnakeGame.View;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace SnakeGame
{
    public class GameplayContext : MonoBehaviour
    {
        [SerializeField] private GameConfigs configsCollection;
        [SerializeField] private EdgeView topEdge;
        [SerializeField] private EdgeView bottomEdge;
        [SerializeField] private EdgeView leftEdge;
        [SerializeField] private EdgeView rightEdge;
        [SerializeField] private SnakeController snakeController;
        [SerializeField] private EdibleElementsController edibleElementsController;

        private GameplayInputHandler _gameplayInputHandler;

        void Awake()
        {
            configsCollection.Initialize();

            snakeController.Initialize(3, 2, Vector2.up);

            var bounds = Rect.MinMaxRect(
                leftEdge.InnerBorderCenter.X, bottomEdge.InnerBorderCenter.Y,
                rightEdge.InnerBorderCenter.X, topEdge.InnerBorderCenter.Y
            );

            edibleElementsController.Initialize(bounds, snakeController);

            _gameplayInputHandler = new GameplayInputHandler(snakeController);

            Observable.NextFrame()
                .Subscribe(_ =>
                    edibleElementsController.SpawnRandomElement());
        }

        private void OnDestroy()
        {
            _gameplayInputHandler.Dispose();
        }
    }
}