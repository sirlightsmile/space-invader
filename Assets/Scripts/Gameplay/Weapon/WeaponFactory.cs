using SmileProject.Generic.Audio;
using SmileProject.Generic.Pooling;
using SmileProject.SpaceInvader.GameData;
using SmileProject.SpaceInvader.Sounds;
using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay.Weapon
{
    public class WeaponFactory
    {
        private GameDataManager _gameDataManager;
        private PoolManager _poolManager;
        private AudioManager _audioManager;

        public WeaponFactory(GameDataManager gameDataManager, PoolManager poolManager, AudioManager audioManager)
        {
            _gameDataManager = gameDataManager;
            _poolManager = poolManager;
            _audioManager = audioManager;
        }

        public SpaceshipGun CreateSpaceshipGun(SpaceshipGunModel model)
        {
            SpaceshipGun gun = new SpaceshipGun(model, _poolManager);
            gun.SetSounds(_audioManager, GameSoundKeys.Shoot);
            return gun;
        }

        public SpaceshipGun CreateRandomSpaceshipGun()
        {
            var spaceshipGuns = _gameDataManager.GetSpaceshipGunModels();
            int randomIndex = Random.Range(0, spaceshipGuns.Length);
            SpaceshipGunModel randomModel = spaceshipGuns[randomIndex];
            SpaceshipGun randomGun = CreateSpaceshipGun(randomModel);
            return randomGun;
        }

        public SpaceshipGun CreateSpaceshipGunById(string id)
        {
            SpaceshipGunModel model = _gameDataManager.GetSpaceshipGunModelById(id);
            SpaceshipGun spaceshipGun = CreateSpaceshipGun(model);
            return spaceshipGun;
        }
    }
}