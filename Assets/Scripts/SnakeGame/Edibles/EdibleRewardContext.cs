namespace SnakeGame.Edibles
{
    public readonly struct EdibleRewardContext
    {
        public EdibleRewardContext(SnakeController snakeController)
        {
            SnakeController = snakeController;
        }

        public SnakeController SnakeController { get; }
    }
}