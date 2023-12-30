using System;
using SnakeGame.Gameplay.Snake;
using SnakeGame.Utils;
using UniRx;
using UnityEngine.InputSystem;

namespace SnakeGame.Gameplay
{
    public class GameplayInputService : IDisposable
    {
        private readonly GameStateService _gameStateService;
        private readonly SnakeController _snakeController;
        private readonly CompositeDisposable _disposables = new();
        private readonly GameplayInputActions _gameplayInputActions;

        public GameplayInputService(SnakeController snakeController, GameStateService gameStateService)
        {
            _snakeController = snakeController;
            _gameStateService = gameStateService;

            _gameplayInputActions = new GameplayInputActions();
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
                .Subscribe(_ => _gameStateService.TogglePause())
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}