using SnakeGame.Gameplay.Snake;
using SnakeGame.Gameplay.View;

namespace SnakeGame.Gameplay
{
    public class PauseController
    {
        private readonly SnakeController _snakeController;
        private readonly PauseView _pauseView;
        private bool _isPaused;

        public PauseController(SnakeController snakeController, PauseView pauseView)
        {
            _snakeController = snakeController;
            _pauseView = pauseView;
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;
            _pauseView.SetVisible(_isPaused);
            _snakeController.SetPause(_isPaused);
        }
    }
}