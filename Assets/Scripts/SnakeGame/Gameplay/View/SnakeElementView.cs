using System;
using SnakeGame.Gameplay.Snake;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SnakeGame.Gameplay.View
{
    public class SnakeElementView : MonoBehaviour
    {
        [SerializeField] private float borderSize = 0.1f;
        [SerializeField] private new Collider collider;
        private SnakeElementType _elementType;
        public SnakeElementType ElementType => _elementType;

        public float Length => collider.bounds.size.z + borderSize;

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

        public IObservable<SnakeElementView> OnHitSnakeAsObservable()
        {
            return gameObject.OnTriggerEnterAsObservable()
                .Select(element => element.gameObject.GetComponent<SnakeElementView>())
                .Where(element => element != null);
        }
    }
}