using SnakeGame.Configs;
using SnakeGame.Gameplay.Edibles;
using SnakeGame.Gameplay.Snake;
using SnakeGame.Gameplay.View;
using UniRx;
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

        private readonly CompositeDisposable _disposables = new();

        private GameStateService _gameStateService;
        private SnakeSpeedModifierService _snakeSpeedModifierService;

        private void Awake()
        {
            configsCollection.Initialize();

            _gameStateService = new GameStateService(pauseView);
            var scoreService = new ScoreService();

            InitializeSnakeController(_gameStateService);

            _snakeSpeedModifierService = CreateSnakeSpeedModifierService(_gameStateService);

            InitializeScoreView(scoreService);

            InitializeEdgeViews();

            InitializeEdibleElementsController(_snakeSpeedModifierService, scoreService);

            _gameStateService.TogglePause();

            CreateGameplayInputService(_gameStateService);
        }

        private void CreateGameplayInputService(GameStateService gameStateService)
        {
            var gameplayInputService = new GameplayInputService(snakeController, gameStateService);
            gameplayInputService.AddTo(_disposables);
        }

        private void InitializeSnakeController(GameStateService gameStateService)
        {
            snakeController.Initialize(gameStateService);
            snakeController.AddTo(_disposables);
        }

        private void InitializeEdibleElementsController(SnakeSpeedModifierService snakeSpeedModifierService,
            ScoreService scoreService)
        {
            var bounds = Rect.MinMaxRect(
                leftEdge.InnerBorderCenter.X, bottomEdge.InnerBorderCenter.Y,
                rightEdge.InnerBorderCenter.X, topEdge.InnerBorderCenter.Y
            );

            edibleElementsController.Initialize(bounds, snakeController, snakeSpeedModifierService, scoreService);
            edibleElementsController.AddTo(_disposables);
        }

        private void InitializeEdgeViews()
        {
            topEdge.Initialize(snakeController.ElementSize, Vector2.up);
            bottomEdge.Initialize(snakeController.ElementSize, Vector2.down);
            leftEdge.Initialize(snakeController.ElementSize, Vector2.left);
            rightEdge.Initialize(snakeController.ElementSize, Vector2.right);
        }

        private void InitializeScoreView(ScoreService scoreService)
        {
            scoreView.Initialize(scoreService.Score);
            scoreView.AddTo(_disposables);
        }

        private SnakeSpeedModifierService CreateSnakeSpeedModifierService(GameStateService gameStateService)
        {
            var snakeConfig = GameConfigs.GetConfig<SnakeConfig>();
            return new SnakeSpeedModifierService(gameStateService, snakeController, snakeConfig);
        }

        private void Update()
        {
            _gameStateService.Tick(Time.deltaTime);

            snakeController.UpdateGameplay(_gameStateService.GameplayTime);
            _snakeSpeedModifierService.UpdateGameplay(_gameStateService.GameplayTime);
        }

        private void OnDestroy()
        {
            _disposables.Clear();
        }
    }
}