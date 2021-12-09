using System;
using Newtonsoft.Json;

namespace SmileProject.SpaceInvader.GameData
{
    public class WeaponModel
    {
        [JsonProperty("id")]
        /// <summary>
        /// Id of gun
        /// </summary>
        public string ID { get; private set; }

        [JsonProperty("base_damage")]
        /// <summary>
        /// Base Damage
        /// </summary>
        public int BaseDamage;

        [JsonProperty("damage_increment")]
        /// <summary>
        /// Damage increase per level
        /// </summary>
        public int DamageIncrement { get; private set; }

        [JsonProperty("base_speed")]
        /// <summary>
        /// Base shooting speed
        /// </summary>
        public int BaseSpeed { get; private set; }

        [JsonProperty("speed_increment")]
        /// <summary>
        /// Speed increase per level
        /// </summary>
        public int SpeedIncrement { get; private set; }

        [JsonProperty("max_level")]
        /// <summary>
        /// Max Level
        /// </summary>
        public int MaxLevel { get; private set; }
    }
}