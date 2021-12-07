using System.Threading.Tasks;
using SmileProject.Generic.Pooling;
using SmileProject.SpaceInvader.GameData;
using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay
{
    /// <summary>
    /// Common laser gun. Infinity bullet.
    /// </summary>
    public class LaserGun : Weapon<LaserGunModel>
    {
        private PoolManager _poolManager;

        public LaserGun(LaserGunModel model, PoolManager poolManager) : base(model)
        {
            _poolManager = poolManager;
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
            Bullet bullet = _poolManager.GetItem<Bullet>(Model.BulletType.ToString());
            Transform attackPoint = AttackPoint.transform;
            bullet.SetPosition(attackPoint.position).SetRotation(attackPoint.rotation).SetDamage(Damage).SetOwner(attacker);
            bullet.SetActive(true);
            var _ = PlayAttackSound();
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