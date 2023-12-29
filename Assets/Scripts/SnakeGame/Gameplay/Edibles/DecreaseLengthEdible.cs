using UnityEngine;

namespace SnakeGame.Gameplay.Edibles
{
    [CreateAssetMenu(fileName = "DecreaseLengthEdible", menuName = "Game Configs/Edibles/Decrease Length")]
    public class DecreaseLengthEdible : EdibleElement
    {
        public override void GrantReward(EdibleRewardContext edibleRewardContext)
        {
            edibleRewardContext.SnakeController.DecreaseLength();
        }
    }
}