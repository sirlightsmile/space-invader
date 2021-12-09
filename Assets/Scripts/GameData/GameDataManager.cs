using UnityEngine;
using System;
using System.Threading.Tasks;
using SmileProject.Generic.ResourceManagement;

namespace SmileProject.SpaceInvader.GameData
{
    public class GameDataManager
    {
        /// <summary>
        /// Key saved in addressable assets for load game data
        /// </summary>
        private const string GAME_DATA_KEY = "GameData";
        private GameDataModel _gameData;

        /// <summary>
        /// Load game data to game data manager. Call only once per session.
        /// </summary>
        /// <returns></returns>
        public async Task Initialize(IResourceLoader resourceLoader)
        {
            _gameData = await resourceLoader.LoadJsonAsModel<GameDataModel>(GAME_DATA_KEY);
            Debug.Log("Game Data Initialized.");
        }

        public SpaceshipGunModel GetSpaceshipGunModelById(string id)
        {
            SpaceshipGunModel model = Array.Find(GetSpaceshipGunModels(), (obj) => obj.ID == id);
            Debug.Assert(model != null, $"SpaceShipGun game data id : [{id}] not found.");
            return model;
        }

        public SpaceshipGunModel[] GetSpaceshipGunModels()
        {
            Debug.Assert(_gameData.SpaceshipGuns != null, "SpaceShipGun game data should not be null.");
            return _gameData.SpaceshipGuns;
        }

        public EnemySpaceshipModel[] GetEnemySpaceshipModels()
        {
            Debug.Assert(_gameData.EnemySpaceships != null, "Enemy spaceship game data should not be null.");
            return _gameData.EnemySpaceships;
        }

        public EnemySpaceshipModel GetEnemySpaceshipModelById(string id)
        {
            EnemySpaceshipModel model = Array.Find(GetEnemySpaceshipModels(), (obj) => obj.ID == id);
            Debug.Assert(model != null, $"EnemySpaceshipModel game data number : [{id}] not found.");
            return model;
        }

        public PlayerSpaceshipModel[] GetPlayerSpaceshipModels()
        {
            Debug.Assert(_gameData.PlayerSpaceships != null, "Player spaceship game data should not be null.");
            return _gameData.PlayerSpaceships;
        }

        public PlayerSpaceshipModel GetPlayerSpaceshipModelById(string id)
        {
            PlayerSpaceshipModel model = Array.Find(GetPlayerSpaceshipModels(), (obj) => obj.ID == id);
            Debug.Assert(model != null, $"PlayerSpaceshipModel game data number : [{id}] not found.");
            return model;
        }
    }
}