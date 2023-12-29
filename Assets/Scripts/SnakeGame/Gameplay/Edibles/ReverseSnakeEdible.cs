using UnityEngine;

namespace SnakeGame.Gameplay.Edibles
{
    [CreateAssetMenu(fileName = "ReverseSnakeEdible", menuName = "Game Configs/Edibles/Reverse Snake")]
    public class ReverseSnakeEdible : EdibleElement
    {
        public override void GrantReward(EdibleRewardContext edibleRewardContext)
        {
            edibleRewardContext.SnakeController.Reverse();
        }
    }
}