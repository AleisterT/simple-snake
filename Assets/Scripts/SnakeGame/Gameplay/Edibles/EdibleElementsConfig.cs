using System.Collections.Generic;
using System.Linq;
using SnakeGame.Configs;
using SnakeGame.Gameplay.View;
using UnityEngine;

namespace SnakeGame.Gameplay.Edibles
{
    [CreateAssetMenu(fileName = "EdibleElementsConfig", menuName = "Game Configs/Edible Elements Config")]
    public class EdibleElementsConfig : GameConfig
    {
        [SerializeField] private List<EdibleElement> edibleElements;

        [field: SerializeField] public EdibleElementView EdibleElementPrefab { get; private set; }

        public EdibleElement GetRandomEdibleElement()
        {
            float weight = edibleElements.Sum(edibleElement => edibleElement.Weight);

            if (weight == 0)
            {
                Debug.LogWarning($"Total weight of random elements is 0 in {name}");
            }

            float randomWeight = Random.Range(0, weight);

            weight = 0;
            foreach (var edibleElement in edibleElements)
            {
                weight += edibleElement.Weight;
                if (randomWeight < weight)
                {
                    return edibleElement;
                }
            }

            return edibleElements[^1];
        }
    }
}