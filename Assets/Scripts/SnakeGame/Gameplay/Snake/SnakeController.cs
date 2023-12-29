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
    public class SnakeController : MonoBehaviour
    {
        private readonly List<SnakeElementView> _elements = new();

        private ObjectPool<SnakeElementView> _elementsPool;

        public float Speed { get; private set; }
        private float _nextUpdateTime = 0;
        private bool _addLastElementInNextUpdate = false;

        private GameSpaceVector _direction;
        private SnakeConfig _snakeConfig;

        public float ElementSize => _elements[0].Length;

        public void Initialize()
        {
            _snakeConfig = GameConfigs.GetConfig<SnakeConfig>();

            InitializePool();
            _direction = _snakeConfig.InitialDirection.ToGameVector();
            SetSpeed(_snakeConfig.InitialSpeed);
            CreateBody(_snakeConfig.InitialLength);
        }

        private void CreateBody(int length)
        {
            SnakeElementView head = _elementsPool.Get();
            head.Initialize(SnakeElementType.Head, Vector3.zero);

            head.OnHitEdgeAsObservable().Subscribe(OnHeadHitEdge);

            _elements.Add(head);

            for (int i = 0; i < length; i++)
            {
                SnakeElementView previousElement = _elements[i];
                Vector3 initialPosition = previousElement.transform.position -
                                          _direction.ToWorldSpace() * previousElement.Length;
                SpawnNewElement(initialPosition);
            }
        }

        private void OnHeadHitEdge(EdgeView edgeView)
        {
            var boardSize = edgeView.InnerBorderCenter.ToWorldSpace() -
                            edgeView.OppositeEdge.InnerBorderCenter.ToWorldSpace() - edgeView.EdgeNormal * ElementSize;
            _elements[0].transform.position -= boardSize;
        }

        private void SpawnNewElement(Vector3 position)
        {
            SnakeElementView elementInstance = _elementsPool.Get();
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
        }

        public void SetSpeed(float speed)
        {
            Speed = speed;
        }

        public void TrySetDirection(Direction direction)
        {
            bool isDirectionInSameAxis = Math.Abs(_direction.X - direction.ToGameVector().X) < float.Epsilon ||
                                         Math.Abs(_direction.Y - direction.ToGameVector().Y) < float.Epsilon;
            if (isDirectionInSameAxis)
            {
                return;
            }

            _direction = direction.ToGameVector();
        }

        private void Update()
        {
            if (_nextUpdateTime > Time.time)
            {
                return;
            }

            _nextUpdateTime = Time.time + 1 / Speed;

            if (_addLastElementInNextUpdate)
            {
                SpawnNewElement(_elements[^1].transform.position);
                _addLastElementInNextUpdate = false;
            }


            for (var index = _elements.Count - 1; index >= 0; index--)
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

        public void SetPause(bool isPaused)
        {
            _nextUpdateTime = isPaused ? float.MaxValue : Time.time;
        }
    }
}