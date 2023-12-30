using System;
using UniRx;

namespace SnakeGame.Gameplay
{
    public class ScoreService : IDisposable
    {
        private readonly ReactiveProperty<int> _score = new(0);
        public IReadOnlyReactiveProperty<int> Score => _score;

        public void IncreaseScore()
        {
            _score.Value += 1;
        }

        public void Dispose()
        {
            _score?.Dispose();
        }
    }
}