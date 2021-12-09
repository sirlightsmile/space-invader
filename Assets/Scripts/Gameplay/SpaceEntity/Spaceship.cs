using System;
using System.Threading.Tasks;
using SmileProject.SpaceInvader.GameData;
using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay
{
    /// <summary>
    /// Base object of warrior in space
    /// </summary>
    public abstract class Spaceship : SpaceEntity
    {
        /// <summary>
        /// Invoke when got hit <Attacker, Defender>
        /// </summary>
        public event Action<Spaceship, Spaceship> GotHit;

        /// <summary>
        /// Invoke when SpaceWarrior hp reached zero
        /// </summary>
        public event Action<Spaceship> Dead;

        public int HP { get; private set; }

        [SerializeField]
        protected Transform _attackPointTransform;

        protected Weapon<WeaponModel> _weapon;

        public Spaceship SetHP(int hp)
        {
            HP = hp;
            return this;
        }

        public virtual bool IsDead()
        {
            return HP <= 0;
        }

        public virtual void GetHit(int damage, Spaceship attacker)
        {
            int result = HP - damage;
            HP = Mathf.Clamp(result, 0, HP);
            GotHit?.Invoke(attacker, this);

            if (IsDead())
            {
                Destroy();
            }
            else
            {
                // TODO: play get hit sound
            }
        }

        public virtual async Task SetWeapon(Weapon<WeaponModel> newWeapon)
        {
            await newWeapon.Setup();
            _weapon = newWeapon;
            _weapon.SetAttackPointTransform(_attackPointTransform);
        }

        #region Pooling
        // TODO: implement
        public override void OnSpawn()
        {
            throw new System.NotImplementedException();
        }

        public override void OnDespawn()
        {
            throw new System.NotImplementedException();
        }
        #endregion

        protected virtual void Destroy()
        {
            Dead?.Invoke(this);
        }
    }
}
