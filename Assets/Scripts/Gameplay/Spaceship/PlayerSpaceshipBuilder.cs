
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
        private const string PrefabKey = "PlayerPrefab";

        public PlayerSpaceshipBuilder(IResourceLoader resourceLoader, GameDataManager gameDataManager, WeaponFactory weaponFactory, AudioManager audioManager) : base(resourceLoader, gameDataManager, weaponFactory, audioManager) { }

        public async Task<PlayerSpaceship> BuildPlayerSpaceship(PlayerSpaceshipModel model)
        {
            var spaceship = await BuildSpaceship<PlayerSpaceship, PlayerSpaceshipModel>(PrefabKey, model);
            string weaponId = model.BasicWeaponId;
            SpaceshipGun weapon = !String.IsNullOrEmpty(weaponId) ? _weaponFactory.CreateSpaceshipGunById(weaponId) : _weaponFactory.CreateRandomSpaceshipGun();
            await spaceship.SetWeapon(weapon);
            spaceship.SetSounds(_audioManager, GameSoundKeys.Impact, GameSoundKeys.PlayerExplosion);
            spaceship.SetBorder();
            return spaceship;
        }

        public async Task<PlayerSpaceship> BuildSpaceshipById(string id)
        {
            PlayerSpaceshipModel model = _gameDataManager.GetPlayerSpaceshipModelById(id);
            return await BuildPlayerSpaceship(model);
        }
    }
}