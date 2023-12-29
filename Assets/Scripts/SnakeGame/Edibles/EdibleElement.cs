using UnityEngine;

namespace SnakeGame.Edibles
{
    public abstract class EdibleElement : ScriptableObject
    {
        [field: SerializeField] public float Weight { get; set; }

        public abstract void GrantReward(EdibleRewardContext edibleRewardContext);
    }
}