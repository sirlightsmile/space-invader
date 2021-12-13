using Newtonsoft.Json;

namespace SmileProject.SpaceInvader.GameData
{
    public class PlayerSpaceshipModel : SpaceshipModel
    {
        [JsonProperty("speed")]
        /// <summary>
        /// Spaceship movement speed
        /// </summary>
        public float Speed { get; private set; }
    }
}