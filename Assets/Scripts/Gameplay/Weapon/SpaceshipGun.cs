using System.Threading.Tasks;
using SmileProject.Generic.Audio;
using SmileProject.Generic.Pooling;
using SmileProject.Generic.Utilities;
using SmileProject.SpaceInvader.GameData;
using SmileProject.SpaceInvader.Gameplay;
using UnityEngine;

namespace SmileProject.SpaceInvader.Weapon
{
    /// <summary>
    /// Common spaceship gun. Infinity bullet.
    /// </summary>
    public class SpaceshipGun : Weapon<SpaceshipGunModel>
    {
        private PoolManager _poolManager;
        private AudioManager _audioManager;
        private SoundKeys _shootSound;

        public SpaceshipGun(SpaceshipGunModel model, PoolManager poolManager) : base(model)
        {
            _poolManager = poolManager;
        }

        public override void Attack(Spaceship attacker)
        {
            Shoot(attacker);
        }

        public override async Task Setup()
        {
            await Reload();
        }

        private void Shoot(Spaceship attacker)
        {
            Bullet bullet = _poolManager.GetItem<Bullet>(Model.BulletType.ToString());
            Transform attackPoint = AttackPoint.transform;
            bullet.SetRotation(attackPoint.rotation).SetDamage(Damage).SetOwner(attacker).SetPosition(attackPoint.position);
            bullet.SetActive(true);
            SafeInvoke.InvokeAsync(async () => { await PlayAttackSound(); });
        }

        private async Task Reload()
        {
            string poolName = Model.BulletType.ToString();
            if (!_poolManager.HasPool(poolName))
            {
                PoolOptions options = new PoolOptions
                {
                    //TODO: adjust size
                    AssetKey = Model.BulletAsset,
                    PoolName = poolName,
                    InitialSize = 10,
                    CanExtend = true,
                    ExtendAmount = 10
                };
                await _poolManager.CreatePool<Bullet>(options);
            }
        }
    }
}