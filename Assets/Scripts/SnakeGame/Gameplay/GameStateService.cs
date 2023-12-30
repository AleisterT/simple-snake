using SnakeGame.Gameplay.View;
using SnakeGame.Utils;
using UnityEngine.SceneManagement;

namespace SnakeGame.Gameplay
{
    public class GameStateService
    {
        public float GameplayTime { get; private set; }

        private readonly PauseView _pauseView;
        private bool _isPaused;

        public GameStateService(PauseView pauseView)
        {
            _pauseView = pauseView;
        }

        public void Restart()
        {
            SceneManager.LoadScene(Scenes.GameplayScene);
        }

        public void Tick(float deltaTime)
        {
            if (_isPaused)
            {
                return;
            }

            GameplayTime += deltaTime;
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;
            _pauseView.SetVisible(_isPaused);
        }
    }
}