using UnityEngine;

namespace SnakeGame.Gameplay.Edibles
{
    public class EdibleElement : ScriptableObject
    {
        [field: SerializeField] public float Weight { get; set; }

        public virtual void GrantReward(EdibleRewardContext edibleRewardContext)
        {
        }
    }
}