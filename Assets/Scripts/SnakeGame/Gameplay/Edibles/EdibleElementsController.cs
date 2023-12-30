using System;
using SnakeGame.Configs;
using SnakeGame.Gameplay.Snake;
using SnakeGame.Gameplay.View;
using SnakeGame.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace SnakeGame.Gameplay.Edibles
{
    public class EdibleElementsController : MonoBehaviour, IDisposable
    {
        private ObjectPool<EdibleElementView> _elementsPool;
        private EdibleElementsConfig _edibleElementsConfig;
        private SnakeSpeedModifierService _speedModifierService;
        private SnakeController _snakeController;
        private ScoreService _scoreService;
        private Rect _boardRect;

        private readonly Collider[] _physicsOverlapResults = new Collider[100];
        private readonly CompositeDisposable _disposables = new();

        public void Initialize(Rect boardRect, SnakeController snakeController,
            SnakeSpeedModifierService speedModifierService,
            ScoreService scoreService)
        {
            _scoreService = scoreService;
            _snakeController = snakeController;
            _speedModifierService = speedModifierService;
            _boardRect = boardRect;
            _edibleElementsConfig = GameConfigs.GetConfig<EdibleElementsConfig>();

            InitializePool();

            SpawnRandomElement();
        }

        private void InitializePool()
        {
            var elementPrefab = _edibleElementsConfig.EdibleElementPrefab;

            _elementsPool = new ObjectPool<EdibleElementView>(
                () => Instantiate(elementPrefab, transform),
                element => element.gameObject.SetActive(true),
                element =>
                {
                    element.Dispose();
                    element.gameObject.SetActive(false);
                },
                Destroy
            );
            _elementsPool.AddTo(_disposables);
        }

        private void SpawnRandomElement()
        {
            var randomPositionInBounds = GetValidRandomPositionInBounds();

            var randomElement = _edibleElementsConfig.GetRandomEdibleElement();
            var elementInstance = _elementsPool.Get();
            elementInstance.Initialize(randomElement);
            elementInstance.transform.position = randomPositionInBounds.ToWorldSpace();

            elementInstance.OnEatenAsObservable().Subscribe(OnElementEaten).AddTo(_disposables);
        }

        private void OnElementEaten(EdibleElementView edibleElementView)
        {
            _scoreService.IncreaseScore();
            _elementsPool.Release(edibleElementView);
            edibleElementView.EdibleElement.GrantReward(
                new EdibleRewardContext(_snakeController, _speedModifierService)
            );
            SpawnRandomElement();
        }

        private GameSpaceVector GetValidRandomPositionInBounds()
        {
            bool isColliding;
            GameSpaceVector randomPositionInBounds;
            do
            {
                randomPositionInBounds = new GameSpaceVector(
                    GetRandomFloat(_boardRect.xMin, _boardRect.xMax, _snakeController.ElementSize),
                    GetRandomFloat(_boardRect.yMin, _boardRect.yMax, _snakeController.ElementSize));

                var collisionCount = Physics.OverlapSphereNonAlloc(randomPositionInBounds.ToWorldSpace(),
                    _snakeController.ElementSize, _physicsOverlapResults,
                    (1 << GameLayers.Snake) | (1 << GameLayers.Border));

                isColliding = collisionCount > 0;
            } while (isColliding);

            return randomPositionInBounds;
        }

        private float GetRandomFloat(float min, float max, float multiplicationOf)
        {
            var range = max - min;
            var randomInt = Random.Range(0, (int)range);
            return min + randomInt * multiplicationOf;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}