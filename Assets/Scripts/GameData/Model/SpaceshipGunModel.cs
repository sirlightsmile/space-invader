using System;
using Newtonsoft.Json;

namespace SmileProject.SpaceInvader.GameData
{
    public class SpaceshipGunModel : WeaponModel
    {
        [JsonProperty("bullet_type")]
        /// <summary>
        /// Type of bullet
        /// </summary>
        public BulletType BulletType { get; private set; }

        [JsonProperty("bullet_asset")]
        /// <summary>
        /// Bullet's asset name
        /// </summary>
        public string BulletAsset { get; private set; }
    }
}