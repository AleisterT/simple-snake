using System;
using UniRx;

namespace SnakeGame.Gameplay.Snake
{
    //Keeping modifier in a separate controller with its own data will allow us to e.g. stack modifiers if we need so.
    public class SnakeSpeedModifierController : IDisposable
    {
        private readonly SnakeController _snakeController;
        private readonly SnakeConfig _snakeConfig;

        private float _speedMultiplier;
        private float _bonusTime;

        private IDisposable _currentSpeedMultiplierDisposable;

        public SnakeSpeedModifierController(SnakeController snakeController, SnakeConfig snakeConfig)
        {
            _snakeController = snakeController;
            _snakeConfig = snakeConfig;
        }

        public void SetSpeedMultiplier(float multiplier, float bonusTime)
        {
            _currentSpeedMultiplierDisposable?.Dispose();
            _snakeController.SetSpeed(_snakeConfig.InitialSpeed * multiplier);

            _currentSpeedMultiplierDisposable = Observable.Timer(TimeSpan.FromSeconds(bonusTime))
                .Subscribe(ResetSpeed);
        }

        private void ResetSpeed(long _)
        {
            _snakeController.SetSpeed(_snakeConfig.InitialSpeed);
        }

        public void Dispose()
        {
            _currentSpeedMultiplierDisposable?.Dispose();
        }
    }
}