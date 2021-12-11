
using System;
using System.Threading.Tasks;
using SmileProject.Generic.Audio;
using SmileProject.Generic.ResourceManagement;
using SmileProject.SpaceInvader.GameData;
using SmileProject.SpaceInvader.Gameplay.Player;
using SmileProject.SpaceInvader.Sounds;
using SmileProject.SpaceInvader.Weapon;

namespace SmileProject.SpaceInvader.Gameplay
{
    public class PlayerSpaceshipBuilder : BaseSpaceshipBuilder
    {
        private const string PREFAB_KEY = "PlayerPrefab";

        public PlayerSpaceshipBuilder(IResourceLoader resourceLoader, GameDataManager gameDataManager, WeaponFactory weaponFactory, AudioManager audioManager) : base(resourceLoader, gameDataManager, weaponFactory, audioManager) { }

        public async Task<PlayerSpaceship> BuildPlayerSpaceship(SpaceshipModel model)
        {
            var spaceship = await BuildSpaceship<PlayerSpaceship, SpaceshipModel>(PREFAB_KEY, model);
            string weaponId = model.BasicWeaponId;
            SpaceshipGun weapon = !String.IsNullOrEmpty(weaponId) ? _weaponFactory.CreateSpaceshipGunById(weaponId) : _weaponFactory.CreateRandomSpaceshipGun();
            await spaceship.SetWeapon(weapon);
            spaceship.SetBorder();
            spaceship.SetSounds(_audioManager, GameSoundKeys.Impact, GameSoundKeys.PlayerExplosion);
            return spaceship;
        }

        public async override Task<Spaceship> BuildSpaceshipById(string id)
        {
            SpaceshipModel model = _gameDataManager.GetPlayerSpaceshipModelById(id);
            return await BuildPlayerSpaceship(model);
        }

        public async Task<PlayerSpaceship> BuildRandomSpaceship()
        {
            SpaceshipModel[] models = _gameDataManager.GetPlayerSpaceshipModels();
            int randomIndex = UnityEngine.Random.Range(0, models.Length);
            SpaceshipModel randomModel = models[randomIndex];
            return await BuildPlayerSpaceship(randomModel);
        }
    }
}