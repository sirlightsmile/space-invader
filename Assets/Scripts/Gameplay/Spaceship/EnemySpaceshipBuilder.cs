
using System;
using System.Threading.Tasks;
using SmileProject.Generic.Audio;
using SmileProject.Generic.Pooling;
using SmileProject.Generic.ResourceManagement;
using SmileProject.SpaceInvader.GameData;
using SmileProject.SpaceInvader.Gameplay.Enemy;
using SmileProject.SpaceInvader.Gameplay.Weapon;
using SmileProject.SpaceInvader.Sounds;

namespace SmileProject.SpaceInvader.Gameplay
{
    public class EnemySpaceshipBuilder : BaseSpaceshipBuilder
    {
        private const string PREFAB_KEY = "EnemyPrefab";
        private const int ENEMY_INITIAL_POOL_SIZE = 10;

        public EnemySpaceshipBuilder(IResourceLoader resourceLoader, GameDataManager gameDataManager, WeaponFactory weaponFactory, AudioManager audioManager) : base(resourceLoader, gameDataManager, weaponFactory, audioManager) { }

        public async Task SetupSpaceshipPool(PoolManager poolManager)
        {
            await SetupPool(poolManager, PREFAB_KEY, ENEMY_INITIAL_POOL_SIZE);
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