using System;
using System.Collections.Generic;
using SnakeGame.View;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

namespace SnakeGame
{
    public class SnakeController : MonoBehaviour
    {
        private readonly List<SnakeElementView> _elements = new();

        private ObjectPool<SnakeElementView> _elementsPool;

        private float _speed;
        private float _nextUpdateTime = 0;
        private bool _addLastElementInNextUpdate = false;

        private GameSpaceVector _direction;
        private SnakeConfig _snakeConfig;

        public void Initialize(int length, int speed, Vector2 direction)
        {
            _snakeConfig = GameConfigs.GetConfig<SnakeConfig>();

            InitializePool();
            _direction = new GameSpaceVector(direction);
            SetSpeed(speed);
            CreateBody(length);
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
            _speed = speed;
        }

        public void TrySetDirection(Vector2 direction)
        {
            bool isDirectionInSameAxis = Math.Abs(_direction.X - direction.x) < float.Epsilon ||
                                         Math.Abs(_direction.Y - direction.y) < float.Epsilon;
            if (isDirectionInSameAxis)
            {
                return;
            }

            _direction = new(direction);
        }

        private void Update()
        {
            if (_nextUpdateTime > Time.time)
            {
                return;
            }

            _nextUpdateTime = Time.time + 1 / _speed;

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
                    case SnakeElementType.PostTransition:
                        break;
                    case SnakeElementType.BeforeTransition:
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
    }
}