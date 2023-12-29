using System;
using SnakeGame.Edibles;
using UniRx;
using UnityEngine;

namespace SnakeGame.View
{
    public class EdibleElementView : MonoBehaviour, IDisposable
    {
        private Subject<EdibleElementView> _onEatenSubject;
        public EdibleElement EdibleElement { get; private set; }

        public void Initialize(EdibleElement edible)
        {
            EdibleElement = edible;
            _onEatenSubject = new Subject<EdibleElementView>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<SnakeElementView>() == null)
            {
                return;
            }

            _onEatenSubject.OnNext(this);
        }

        public IObservable<EdibleElementView> OnEatenAsObservable()
        {
            return _onEatenSubject;
        }

        public void Dispose()
        {
            _onEatenSubject?.Dispose();
        }
    }
}