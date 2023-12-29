using UnityEngine;

namespace SnakeGame.Gameplay.View
{
    public class PauseView : MonoBehaviour
    {
        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}