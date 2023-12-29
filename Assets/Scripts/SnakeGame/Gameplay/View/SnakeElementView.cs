using System;
using SnakeGame.Gameplay.Snake;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SnakeGame.Gameplay.View
{
    public class SnakeElementView : MonoBehaviour
    {
        [SerializeField] private Collider collider;
        private SnakeElementType _elementType;
        public SnakeElementType ElementType => _elementType;

        public float Length => collider.bounds.size.z;

        public void Initialize(SnakeElementType elementType, Vector3 position)
        {
            _elementType = elementType;
            transform.position = position;
        }

        public IObservable<EdgeView> OnHitEdgeAsObservable()
        {
            return gameObject.OnTriggerEnterAsObservable()
                .Select(element => element.gameObject.GetComponent<EdgeView>())
                .Where(edgeView => edgeView != null);
        }
    }
}