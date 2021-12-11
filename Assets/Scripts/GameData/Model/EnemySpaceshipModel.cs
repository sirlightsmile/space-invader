using Newtonsoft.Json;
using UnityEngine;

namespace SmileProject.SpaceInvader.GameData
{
    public class EnemySpaceshipModel : SpaceshipModel
    {
        [JsonProperty("score")]
        /// <summary>
        /// Score player will get when destroyed
        /// </summary>
        public int Score { get; private set; }

        [JsonProperty("type")]
        /// <summary>
        /// Enemy type will apply custom color to enemy prefab
        /// </summary>
        public string Type { get; private set; }
    }
}