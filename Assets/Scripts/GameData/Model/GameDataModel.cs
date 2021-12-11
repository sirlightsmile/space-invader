using System;
using Newtonsoft.Json;

namespace SmileProject.SpaceInvader.GameData
{
    public class GameDataModel
    {
        [JsonProperty("spaceship_gun_model")]
        public SpaceshipGunModel[] SpaceshipGuns { get; private set; }

        [JsonProperty("player_spaceship_model")]
        public PlayerSpaceshipModel[] PlayerSpaceships { get; private set; }

        [JsonProperty("enemy_spaceship_model")]
        public EnemySpaceshipModel[] EnemySpaceships { get; private set; }
    }
}