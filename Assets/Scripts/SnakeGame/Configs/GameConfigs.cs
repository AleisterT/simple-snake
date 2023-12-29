using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SnakeGame.Configs
{
    /// <summary>
    /// Easily accessible read-only global configs collection.
    /// It assumes only one config of each type is present.
    /// </summary>
    [CreateAssetMenu(fileName = "GameConfigs", menuName = "Game Configs/Game Configs")]
    public class GameConfigs : ScriptableObject
    {
        [SerializeField] private List<GameConfig> gameConfigs;

        private static Dictionary<Type, GameConfig> _typeToConfig;


        public static TConfig GetConfig<TConfig>() where TConfig : GameConfig
        {
            return (TConfig)_typeToConfig[typeof(TConfig)];
        }

        public void Initialize()
        {
            if (_typeToConfig != null)
            {
                return;
            }

            _typeToConfig = gameConfigs.ToDictionary(config => config.GetType(), config => config);
        }
    }
}