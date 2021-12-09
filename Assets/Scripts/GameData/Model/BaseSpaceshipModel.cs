using Newtonsoft.Json;

namespace SmileProject.SpaceInvader.GameData
{
    public class BaseSpaceshipModel
    {
        [JsonProperty("id")]
        /// <summary>
        /// Spaceship ID
        /// </summary>
        public string ID { get; private set; }

        [JsonProperty("asset_name")]
        /// <summary>
        /// Spaceship sprite asset id from addressable assets
        /// </summary>
        public string AssetName { get; private set; }

        [JsonProperty("hp")]
        /// <summary>
        /// Spaceship HP
        /// </summary>
        public int HP { get; private set; }

        [JsonProperty("basic_weapon_id")]
        /// <summary>
        /// Weapon id which will be attached when building
        /// </summary>
        public string BasicWeaponId { get; private set; }
    }
}