using System;
using TMPro;
using UniRx;
using UnityEngine;

namespace SnakeGame.Gameplay.View
{
    public class ScoreView : MonoBehaviour, IDisposable
    {
        [SerializeField] private TMP_Text scoreLabel;

        private IDisposable _scoreListener;

        public void Initialize(IReadOnlyReactiveProperty<int> scoreProperty)
        {
            _scoreListener = scoreProperty.Subscribe(OnScoreChanged);
        }

        private void OnScoreChanged(int newScore)
        {
            scoreLabel.SetText($"Score: {newScore}");
        }

        public void Dispose()
        {
            _scoreListener?.Dispose();
        }
    }
}