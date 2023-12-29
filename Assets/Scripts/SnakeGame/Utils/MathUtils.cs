using UnityEngine;

namespace SnakeGame.Utils
{
    public static class MathUtils
    {
        public static bool IsMultiple(float number, float divisor)
        {
            float result = number / divisor;

            // Check if the result is very close to an integer
            // You can adjust the epsilon value based on your precision needs
            float epsilon = 0.0001f; // Adjust as needed
            return Mathf.Abs(result - Mathf.Round(result)) < epsilon;
        }
    }
}