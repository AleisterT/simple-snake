using TMPro;
using UniRx;
using UnityEngine;

namespace SnakeGame.Gameplay.View
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreLabel;

        public void Initialize(IReadOnlyReactiveProperty<int> scoreProperty)
        {
            scoreProperty.Subscribe(OnScoreChanged);
        }

        private void OnScoreChanged(int newScore)
        {
            scoreLabel.SetText($"Score: {newScore}");
        }
    }
}