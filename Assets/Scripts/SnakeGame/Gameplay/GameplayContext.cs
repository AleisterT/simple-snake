using SnakeGame.Configs;
using SnakeGame.Gameplay.Edibles;
using SnakeGame.Gameplay.Snake;
using SnakeGame.Gameplay.View;
using UnityEngine;

namespace SnakeGame.Gameplay
{
    public class GameplayContext : MonoBehaviour
    {
        [SerializeField] private GameConfigs configsCollection;
        [SerializeField] private EdgeView topEdge;
        [SerializeField] private EdgeView bottomEdge;
        [SerializeField] private EdgeView leftEdge;
        [SerializeField] private EdgeView rightEdge;
        [SerializeField] private ScoreView scoreView;
        [SerializeField] private PauseView pauseView;
        [SerializeField] private SnakeController snakeController;
        [SerializeField] private EdibleElementsController edibleElementsController;

        private GameplayInputHandler _gameplayInputHandler;
        private SnakeSpeedModifierController _snakeSpeedModifierController;
        private ScoreController _scoreController;

        void Awake()
        {
            configsCollection.Initialize();

            snakeController.Initialize();

            _scoreController = new ScoreController();

            SnakeConfig snakeConfig = GameConfigs.GetConfig<SnakeConfig>();
            _snakeSpeedModifierController = new SnakeSpeedModifierController(snakeController, snakeConfig);

            scoreView.Initialize(_scoreController.Score);

            topEdge.Initialize(snakeController.ElementSize, Vector2.up);
            bottomEdge.Initialize(snakeController.ElementSize, Vector2.down);
            leftEdge.Initialize(snakeController.ElementSize, Vector2.left);
            rightEdge.Initialize(snakeController.ElementSize, Vector2.right);

            var bounds = Rect.MinMaxRect(
                leftEdge.InnerBorderCenter.X, bottomEdge.InnerBorderCenter.Y,
                rightEdge.InnerBorderCenter.X, topEdge.InnerBorderCenter.Y
            );

            edibleElementsController.Initialize(bounds, snakeController, _snakeSpeedModifierController,
                _scoreController);

            PauseController pauseController = new PauseController(snakeController, pauseView);
            pauseController.TogglePause();

            _gameplayInputHandler = new GameplayInputHandler(snakeController, pauseController);
        }

        private void OnDestroy()
        {
            _gameplayInputHandler.Dispose();
            _snakeSpeedModifierController.Dispose();
            _scoreController.Dispose();
        }
    }
}