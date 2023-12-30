using SnakeGame.Gameplay.Snake;

namespace SnakeGame.Gameplay.Edibles
{
    public readonly struct EdibleRewardContext
    {
        public EdibleRewardContext(SnakeController snakeController,
            SnakeSpeedModifierService speedModifierService)
        {
            SnakeController = snakeController;
            SpeedModifierService = speedModifierService;
        }

        public SnakeController SnakeController { get; }
        public SnakeSpeedModifierService SpeedModifierService { get; }
    }
}