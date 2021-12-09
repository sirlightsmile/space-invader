using System.IO;
using UnityEngine;

namespace Configs
{
    public static class ConfigsHelper
    {
        /// <summary>
        /// Base config path
        /// </summary>
        private const string baseConfigPath = "Config/Regions";

        /// <summary>
        /// Base config path for default region
        /// </summary>
        private const string defaultRegionPath = "Config/Regions/Default";

        /// <summary>
        /// Get config of current environment by config's name
        /// </summary>
        /// <typeparam name="T">Type of config</typeparam>
        /// <param name="region">regional server name</param>
        /// <param name="configName">config's name</param>
        /// <returns></returns>
        public static T GetEnvironmentConfig<T>(string region, string configName) where T : ScriptableObject
        {
            // var path = Path.Combine(baseConfigPath, region, GameEnvironment.Value, configName);
            // Debug.Assert(!string.IsNullOrEmpty(path), "Config path cannot be null.");

            // var config = (T)Resources.Load(path, typeof(T));

            // // fallback to default region config
            // if (config == null)
            // {
            //     var defaultRegion = Path.Combine(defaultRegionPath, GameEnvironment.Value, configName);
            //     Debug.Assert(!string.IsNullOrEmpty(defaultRegion), "Config path cannot be null.");

            //     config = (T)Resources.Load(defaultRegion, typeof(T));
            //     Debug.Assert(config != null, $"{configName} doesn't exist!");
            // }

            // return config;
            return null;
        }
    }
}
