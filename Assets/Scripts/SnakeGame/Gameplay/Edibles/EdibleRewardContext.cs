using SnakeGame.Gameplay.Snake;

namespace SnakeGame.Gameplay.Edibles
{
    public readonly struct EdibleRewardContext
    {
        public EdibleRewardContext(SnakeController snakeController,
            SnakeSpeedModifierController speedModifierController)
        {
            SnakeController = snakeController;
            SpeedModifierController = speedModifierController;
        }

        public SnakeController SnakeController { get; }
        public SnakeSpeedModifierController SpeedModifierController { get; }
    }
}