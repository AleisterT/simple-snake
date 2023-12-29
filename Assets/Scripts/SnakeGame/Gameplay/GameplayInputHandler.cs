using System;
using SnakeGame.Gameplay.Snake;
using SnakeGame.Utils;
using UniRx;
using UnityEngine.InputSystem;

namespace SnakeGame.Gameplay
{
    public class GameplayInputHandler : IDisposable
    {
        private readonly PauseController _pauseController;
        private readonly SnakeController _snakeController;
        private readonly CompositeDisposable _disposables = new();
        private readonly GameplayInputActions _gameplayInputActions;

        public GameplayInputHandler(SnakeController snakeController, PauseController pauseController)
        {
            _snakeController = snakeController;
            _pauseController = pauseController;

            _gameplayInputActions = new();
            _gameplayInputActions.AddTo(_disposables);

            SubscribeInputEvents();

            _gameplayInputActions.Enable();
        }

        private void SubscribeInputEvents()
        {
            Observable.FromEvent<InputAction.CallbackContext>(
                    h => _gameplayInputActions.Gameplay.Up.performed += h,
                    h => _gameplayInputActions.Gameplay.Up.performed -= h)
                .Subscribe(_ => _snakeController.TrySetDirection(Direction.Up))
                .AddTo(_disposables);

            Observable.FromEvent<InputAction.CallbackContext>(
                    h => _gameplayInputActions.Gameplay.Down.performed += h,
                    h => _gameplayInputActions.Gameplay.Down.performed -= h)
                .Subscribe(_ => _snakeController.TrySetDirection(Direction.Down))
                .AddTo(_disposables);

            Observable.FromEvent<InputAction.CallbackContext>(
                    h => _gameplayInputActions.Gameplay.Left.performed += h,
                    h => _gameplayInputActions.Gameplay.Left.performed -= h)
                .Subscribe(_ => _snakeController.TrySetDirection(Direction.Left))
                .AddTo(_disposables);

            Observable.FromEvent<InputAction.CallbackContext>(
                    h => _gameplayInputActions.Gameplay.Right.performed += h,
                    h => _gameplayInputActions.Gameplay.Right.performed -= h)
                .Subscribe(_ => _snakeController.TrySetDirection(Direction.Right))
                .AddTo(_disposables);

            Observable.FromEvent<InputAction.CallbackContext>(
                    h => _gameplayInputActions.Gameplay.Pause.performed += h,
                    h => _gameplayInputActions.Gameplay.Pause.performed -= h)
                .Subscribe(_ => _pauseController.TogglePause())
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}