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

        [JsonProperty("color")]
        /// <summary>
        /// Color tint for enemy spaceship
        /// </summary>
        public Color Color { get; private set; }
    }
}