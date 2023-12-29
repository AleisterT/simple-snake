using System;
using SnakeGame.View;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SnakeGame
{
    public class GameplayInputHandler : IDisposable
    {
        private readonly SnakeController _snakeController;

        private CompositeDisposable _disposables = new();
        private readonly GameplayInputActions _gameplayInputActions;

        public GameplayInputHandler(SnakeController snakeController)
        {
            _snakeController = snakeController;

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
                .Subscribe(_ => _snakeController.TrySetDirection(Vector2.up))
                .AddTo(_disposables);

            Observable.FromEvent<InputAction.CallbackContext>(
                    h => _gameplayInputActions.Gameplay.Down.performed += h,
                    h => _gameplayInputActions.Gameplay.Down.performed -= h)
                .Subscribe(_ => _snakeController.TrySetDirection(Vector2.down))
                .AddTo(_disposables);

            Observable.FromEvent<InputAction.CallbackContext>(
                    h => _gameplayInputActions.Gameplay.Left.performed += h,
                    h => _gameplayInputActions.Gameplay.Left.performed -= h)
                .Subscribe(_ => _snakeController.TrySetDirection(Vector2.left))
                .AddTo(_disposables);

            Observable.FromEvent<InputAction.CallbackContext>(
                    h => _gameplayInputActions.Gameplay.Right.performed += h,
                    h => _gameplayInputActions.Gameplay.Right.performed -= h)
                .Subscribe(_ => _snakeController.TrySetDirection(Vector2.right))
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}