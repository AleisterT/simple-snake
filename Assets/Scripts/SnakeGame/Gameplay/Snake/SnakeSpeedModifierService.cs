namespace SnakeGame.Gameplay.Snake
{
    //Keeping modifier in a separate controller with its own data will allow us to e.g. stack modifiers if we need so.
    public class SnakeSpeedModifierService
    {
        private readonly GameStateService _gameStateService;
        private readonly SnakeController _snakeController;
        private readonly SnakeConfig _snakeConfig;

        private float _speedMultiplier;
        private float? _multiplierEndTime;

        public SnakeSpeedModifierService(GameStateService gameStateService, SnakeController snakeController,
            SnakeConfig snakeConfig)
        {
            _gameStateService = gameStateService;
            _snakeController = snakeController;
            _snakeConfig = snakeConfig;
        }

        public void SetSpeedMultiplier(float multiplier, float bonusTime)
        {
            _multiplierEndTime = _gameStateService.GameplayTime + bonusTime;
            _snakeController.SetSpeed(_snakeConfig.InitialSpeed * multiplier);
        }

        public void UpdateGameplay(float time)
        {
            var hasActiveMultiplayerToReset = time > _multiplierEndTime;
            if (!hasActiveMultiplayerToReset)
            {
                return;
            }

            ResetMultiplier();
        }

        private void ResetMultiplier()
        {
            _snakeController.SetSpeed(_snakeConfig.InitialSpeed);
            _multiplierEndTime = null;
        }
    }
}