using System;
using System.Collections.Generic;
using System.Linq;
using SnakeGame.Configs;
using SnakeGame.Gameplay.View;
using SnakeGame.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

namespace SnakeGame.Gameplay.Snake
{
    public class SnakeController : MonoBehaviour, IDisposable
    {
        public float ElementSize => _elements[0].Length;

        private readonly CompositeDisposable _disposables = new();
        private readonly List<SnakeElementView> _elements = new();
        private ObjectPool<SnakeElementView> _elementsPool;

        private SnakeConfig _snakeConfig;
        private GameStateService _gameStateService;

        private float _speed;
        private float _nextUpdateTime;
        private bool _addLastElementInNextUpdate;
        private GameSpaceVector _direction;
        private float _lastDirectionChangeUpdateTime;

        public void Initialize(GameStateService gameStateService)
        {
            _gameStateService = gameStateService;
            _snakeConfig = GameConfigs.GetConfig<SnakeConfig>();

            InitializePool();
            _direction = _snakeConfig.InitialDirection.ToGameVector();
            SetSpeed(_snakeConfig.InitialSpeed);
            CreateBody(_snakeConfig.InitialLength);
        }

        private void CreateBody(int length)
        {
            var head = _elementsPool.Get();
            head.Initialize(SnakeElementType.Head, Vector3.zero);

            head.OnHitEdgeAsObservable().Subscribe(OnHeadHitEdge).AddTo(_disposables);
            head.OnHitSnakeAsObservable().Subscribe(OnHeadHitSnake).AddTo(_disposables);

            _elements.Add(head);

            for (var i = 0; i < length; i++)
            {
                var previousElement = _elements[i];
                var initialPosition = previousElement.transform.position -
                                      _direction.ToWorldSpace() * previousElement.Length;
                SpawnNewElement(initialPosition);
            }
        }

        private void OnHeadHitSnake(SnakeElementView snakeElementView)
        {
            _gameStateService.Restart();
        }

        private void OnHeadHitEdge(EdgeView edgeView)
        {
            var boardSize = edgeView.InnerBorderCenter.ToWorldSpace() -
                            edgeView.OppositeEdge.InnerBorderCenter.ToWorldSpace() - edgeView.EdgeNormal * ElementSize;
            _elements[0].transform.position -= boardSize;
        }

        private void SpawnNewElement(Vector3 position)
        {
            var elementInstance = _elementsPool.Get();
            elementInstance.Initialize(SnakeElementType.Regular, position);
            _elements.Add(elementInstance);
        }

        private void InitializePool()
        {
            _elementsPool = new ObjectPool<SnakeElementView>(
                () => Instantiate(_snakeConfig.ElementPrefab, transform),
                element => element.gameObject.SetActive(true),
                element => element.gameObject.SetActive(false),
                Destroy
            );
            _elementsPool.AddTo(_disposables);
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public void TrySetDirection(Direction direction)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var alreadyChangedDirectionInThisUpdate = _lastDirectionChangeUpdateTime == _nextUpdateTime;
            if (alreadyChangedDirectionInThisUpdate)
            {
                return;
            }

            var isDirectionInSameAxis = Math.Abs(_direction.X - direction.ToGameVector().X) < float.Epsilon ||
                                        Math.Abs(_direction.Y - direction.ToGameVector().Y) < float.Epsilon;
            if (isDirectionInSameAxis)
            {
                return;
            }

            _direction = direction.ToGameVector();
            _lastDirectionChangeUpdateTime = _nextUpdateTime;
        }

        public void UpdateGameplay(float time)
        {
            if (_nextUpdateTime > time)
            {
                return;
            }

            _nextUpdateTime = time + 1 / _speed;

            TryAddNextElement();

            UpdateSnake();
        }

        private void UpdateSnake()
        {
            for (var index = _elements.Count - 1; index >= 0; index--) UpdateElement(index);
        }

        private void UpdateElement(int index)
        {
            var snakeElementView = _elements[index];

            switch (snakeElementView.ElementType)
            {
                case SnakeElementType.Head:
                    snakeElementView.transform.position += _direction.ToWorldSpace() * snakeElementView.Length;
                    Assert.AreEqual(index, 0);
                    break;
                case SnakeElementType.Regular:
                    Assert.AreNotEqual(index, 0);
                    var previousElement = _elements[index - 1];
                    snakeElementView.transform.position = previousElement.transform.position;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void TryAddNextElement()
        {
            if (!_addLastElementInNextUpdate)
            {
                return;
            }

            SpawnNewElement(_elements[^1].transform.position);
            _addLastElementInNextUpdate = false;
        }

        public void IncreaseLength()
        {
            _addLastElementInNextUpdate = true;
        }

        public void DecreaseLength()
        {
            if (_elements.Count == 1)
            {
                return;
            }

            var lastElement = _elements[^1];
            _elementsPool.Release(lastElement);
            _elements.RemoveAt(_elements.Count - 1);
        }

        public void Reverse()
        {
            _direction = new GameSpaceVector(-_direction.X, -_direction.Y);

            var currentPositions = _elements.Select(e => e.transform.position).ToList();

            for (var i = 0; i < _elements.Count; i++)
            {
                var elementView = _elements[i];
                elementView.transform.position = currentPositions[currentPositions.Count - i - 1];
            }
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}