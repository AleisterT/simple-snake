using UnityEngine;

namespace SnakeGame.Gameplay.Edibles
{
    [CreateAssetMenu(fileName = "ChangeSpeedEdible", menuName = "Game Configs/Edibles/Change Speed")]
    public class ChangeSpeedEdible : EdibleElement
    {
        [field: SerializeField] public float SpeedMultiplier { get; private set; }
        [field: SerializeField] public float BonusTime { get; private set; }

        public override void GrantReward(EdibleRewardContext edibleRewardContext)
        {
            edibleRewardContext.SpeedModifierController.SetSpeedMultiplier(SpeedMultiplier, BonusTime);
        }
    }
}