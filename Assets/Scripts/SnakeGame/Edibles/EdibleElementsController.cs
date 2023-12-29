using SnakeGame.View;
using UniRx;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace SnakeGame.Edibles
{
    public class EdibleElementsController : MonoBehaviour
    {
        private ObjectPool<EdibleElementView> _elementsPool;
        private EdibleElementsConfig _edibleElementsConfig;
        private SnakeController _snakeController;
        private SnakeConfig _snakeConfig;
        private Rect _boardRect;

        private readonly Collider[] _physicsOverlapResults = new Collider[100];

        public void Initialize(Rect boardRect, SnakeController snakeController)
        {
            _snakeController = snakeController;
            _boardRect = boardRect;
            _edibleElementsConfig = GameConfigs.GetConfig<EdibleElementsConfig>();
            _snakeConfig = GameConfigs.GetConfig<SnakeConfig>();

            InitializePool();
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
        }

        public void SpawnRandomElement()
        {
            var randomPositionInBounds = GetValidRandomPositionInBounds();

            var randomElement = _edibleElementsConfig.GetRandomEdibleElement();
            var elementInstance = _elementsPool.Get();
            elementInstance.Initialize(randomElement);
            elementInstance.transform.position = randomPositionInBounds.ToWorldSpace();

            elementInstance.OnEatenAsObservable().Subscribe(OnElementEaten);
        }

        private void OnElementEaten(EdibleElementView edibleElementView)
        {
            _elementsPool.Release(edibleElementView);
            edibleElementView.EdibleElement.GrantReward(new EdibleRewardContext(_snakeController));
        }

        private GameSpaceVector GetValidRandomPositionInBounds()
        {
            bool isColliding;
            GameSpaceVector randomPositionInBounds;
            do
            {
                randomPositionInBounds = new GameSpaceVector(
                    Random.Range(_boardRect.xMin, _boardRect.xMax),
                    Random.Range(_boardRect.yMin, _boardRect.yMax));

                var collisionCount = Physics.OverlapSphereNonAlloc(randomPositionInBounds.ToWorldSpace(),
                    _snakeConfig.ElementPrefab.Length, _physicsOverlapResults,
                    1 << GameLayers.Snake | 1 << GameLayers.Border);

                isColliding = collisionCount > 0;
            } while (isColliding);

            return randomPositionInBounds;
        }
    }
}