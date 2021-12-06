using System;
using Newtonsoft.Json;

namespace SmileProject.SpaceInvader.GameData
{
    [Serializable]
    public class LaserGunModel : WeaponModel
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