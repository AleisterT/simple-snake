using UnityEngine;

namespace SnakeGame.Gameplay.Edibles
{
    [CreateAssetMenu(fileName = "IncreaseLengthEdible", menuName = "Game Configs/Edibles/Increase Length")]
    public class IncreaseLengthEdible : EdibleElement
    {
        public override void GrantReward(EdibleRewardContext edibleRewardContext)
        {
            edibleRewardContext.SnakeController.IncreaseLength();
        }
    }
}