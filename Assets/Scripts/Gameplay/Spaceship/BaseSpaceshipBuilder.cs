
using System;
using System.Threading.Tasks;
using SmileProject.Generic.Audio;
using SmileProject.Generic.Pooling;
using SmileProject.Generic.ResourceManagement;
using SmileProject.SpaceInvader.GameData;
using SmileProject.SpaceInvader.Gameplay.Weapon;

namespace SmileProject.SpaceInvader.Gameplay
{
    public abstract class BaseSpaceshipBuilder
    {
        public event Action<Spaceship> SpaceshipBuilded;

        protected const string ASSET_PREFIX = "SpaceshipSprites/";

        private IResourceLoader _resourceLoader;
        private PoolManager _poolManager;
        protected GameDataManager _gameDataManager;
        protected AudioManager _audioManager;
        protected WeaponFactory _weaponFactory;

        public BaseSpaceshipBuilder(IResourceLoader resourceLoader, GameDataManager gameDataManager, WeaponFactory weaponFactory, AudioManager audioManager)
        {
            _resourceLoader = resourceLoader;
            _gameDataManager = gameDataManager;
            _audioManager = audioManager;
            _weaponFactory = weaponFactory;
        }

        /// <summary>
        /// (Optional) Setup pool to generate spaceship
        /// </summary>
        /// <param name="poolManager">pool manager</param>
        /// <param name="templateKey">pool asset name</param>
        /// <param name="size">pool size</param>
        /// <returns></returns>
        public virtual async Task SetupPool(PoolManager poolManager, string templateKey, int size)
        {
            _poolManager = poolManager;
            if (!poolManager.HasPool(templateKey))
            {
                PoolOptions options = new PoolOptions
                {
                    AssetKey = templateKey,
                    PoolName = templateKey,
                    InitialSize = size,
                    CanExtend = true,
                    ExtendAmount = size
                };
                await poolManager.CreatePool<Spaceship>(options);
            }
        }

        /// <summary>
        /// Build spaceship from id. If pool manager has assigned this will build from pool
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract Task<Spaceship> BuildSpaceshipById(string id);

        /// <summary>
        /// Build spaceship from model. If pool manager has assigned this will build from pool
        /// </summary>
        /// <param name="templateKey">asset name</param>
        /// <param name="model">spaceship data model</param>
        /// <typeparam name="T">T extends Spaceship</typeparam>
        /// <typeparam name="T2">T2 extends SpaceshipModel</typeparam>
        /// <returns></returns>
        public async virtual Task<T> BuildSpaceship<T, T2>(string templateKey, T2 model) where T : Spaceship where T2 : SpaceshipModel
        {
            T spaceship = null;
            if (_poolManager != null && _poolManager.HasPool(templateKey))
            {
                spaceship = _poolManager.GetItem<T>(templateKey);
            }
            else
            {
                spaceship = await _resourceLoader.InstantiateAsync<T>(templateKey, null, true);
            }
            // spaceship.Setup(model);
            spaceship.SetActive(true);
            string spriteName = GetAssetPrefix() + model.AssetName;
            await _resourceLoader.SetSpriteAsync(spriteName, spaceship.SetSprite);
            SpaceshipBuilded?.Invoke(spaceship);
            return spaceship;
        }

        protected virtual string GetAssetPrefix()
        {
            // default no prefix
            return "";
        }
    }
}