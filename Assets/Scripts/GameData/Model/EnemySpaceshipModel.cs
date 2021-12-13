using Newtonsoft.Json;
using SmileProject.SpaceInvader.Gameplay.Enemy;

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
        public EnemyType Type { get; private set; }
    }
}