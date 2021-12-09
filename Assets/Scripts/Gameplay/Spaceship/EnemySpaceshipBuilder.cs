
using System;
using System.Threading.Tasks;
using SmileProject.Generic.Audio;
using SmileProject.Generic.Pooling;
using SmileProject.Generic.ResourceManagement;
using SmileProject.SpaceInvader.GameData;
using SmileProject.SpaceInvader.Weapon;
using SmileProject.SpaceInvader.Sounds;
using SmileProject.SpaceInvader.Gameplay.Enemy;

namespace SmileProject.SpaceInvader.Gameplay
{
    public class EnemySpaceshipBuilder : BaseSpaceshipBuilder
    {
        private const string PREFAB_KEY = "EnemyPrefab";

        public EnemySpaceshipBuilder(IResourceLoader resourceLoader, GameDataManager gameDataManager, WeaponFactory weaponFactory, AudioManager audioManager) : base(resourceLoader, gameDataManager, weaponFactory, audioManager) { }

        public async Task SetupSpaceshipPool(PoolManager poolManager, int size)
        {
            await SetupPool(poolManager, PREFAB_KEY, size);
        }

        public async Task<EnemySpaceship> BuildRandomEnemySpaceship()
        {
            EnemySpaceshipModel[] models = _gameDataManager.GetEnemySpaceshipModels();
            int randomIndex = UnityEngine.Random.Range(0, models.Length);
            EnemySpaceshipModel randomModel = models[randomIndex];
            return await BuildEnemySpaceship(randomModel); ;
        }

        public async Task<EnemySpaceship> BuildEnemySpaceship(EnemySpaceshipModel model)
        {
            var spaceship = await BuildSpaceship<EnemySpaceship, EnemySpaceshipModel>(PREFAB_KEY, model);
            string weaponId = model.BasicWeaponId;
            if (!String.IsNullOrEmpty(weaponId))
            {
                SpaceshipGun weapon = _weaponFactory.CreateSpaceshipGunById(weaponId);
                await spaceship.SetWeapon(weapon);
            }
            spaceship.SetScore(model.Score);
            //TODO: set color
            spaceship.SetSounds(_audioManager, GameSoundKeys.Hit, GameSoundKeys.Explosion);
            return spaceship;
        }

        public async override Task<Spaceship> BuildSpaceshipById(string id)
        {
            EnemySpaceshipModel model = _gameDataManager.GetEnemySpaceshipModelById(id);
            return await BuildEnemySpaceship(model);
        }
    }
}