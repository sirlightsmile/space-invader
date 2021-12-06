using System.Threading.Tasks;
using SmileProject.Generic;
using SmileProject.SpaceInvader.GameData;
using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay
{
    /// <summary>
    /// Common laser gun. Infinity bullet.
    /// </summary>
    public class LaserGun : Weapon
    {
        private LaserGunModel _model;
        private PoolManager _poolManager;
        private AudioManager _audioManager;
        private SoundKeys _shootSound;

        public LaserGun(LaserGunModel model, PoolManager poolManager)
        {
            _model = model;
            _poolManager = poolManager;
            SetLevel(WEAPON_INITIAL_LEVEL);
            SetMaxLevel(model.MaxLevel);
            SetDamage(model.BaseDamage);
            SetAttackSpeed(model.BaseSpeed);
        }

        public override void Attack(SpaceWarrior attacker)
        {
            Shoot(attacker);
        }

        public override async Task Setup()
        {
            await Reload();
        }

        private void Shoot(SpaceWarrior attacker)
        {
            Bullet bullet = _poolManager.GetItem<Bullet>(_model.BulletType.ToString());
            Transform attackPoint = _attackPointTransform.transform;
            bullet.SetPosition(attackPoint.position).SetRotation(attackPoint.rotation).SetDamage(_damage).SetOwner(attacker);
            // bullet.SetActive(true);
            var _ = PlayShootSound();
        }

        private async Task Reload()
        {
            string poolName = _model.BulletType.ToString();
            if (!_poolManager.HasPool(poolName))
            {
                PoolOptions options = new PoolOptions
                {
                    //TODO: adjust size
                    AssetKey = _model.BulletAsset,
                    PoolName = poolName,
                    InitialSize = 10,
                    CanExtend = true,
                    ExtendAmount = 10
                };
                await _poolManager.CreatePool<Bullet>(options);
            }
        }



        public void LevelUp(int addLevel = 1)
        {
            int addedLevel = _level + addLevel;
            int targetLevel = Mathf.Clamp(addedLevel, _level, _maxLevel);
            if (_level != targetLevel)
            {
                _level = targetLevel;
                UpdateStatus();
            }
        }

        // public void SetSounds(AudioManager audioManager, SoundKeys shootSound)
        // {
        //     _audioManager = audioManager;
        //     _shootSound = shootSound;
        // }


        private async Task PlayShootSound()
        {
            // TODO: shot sound
            // if (_audioManager != null)
            // {
            //     await _audioManager.PlaySound(_shootSound);
            // }
        }

        private void UpdateStatus()
        {
            int currentLevel = _level;
            int newDamage = _model.BaseDamage + (currentLevel * _model.DamageIncrement);
            int newAttackSpeed = _model.BaseSpeed + (currentLevel * _model.SpeedIncrement);
            _damage = newDamage;
            _attackSpeed = newAttackSpeed;
            SetDamage(newDamage);
            SetAttackSpeed(newAttackSpeed);
        }
    }
}