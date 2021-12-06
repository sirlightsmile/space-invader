using System;
using System.Threading.Tasks;
using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay
{
    /// <summary>
    /// Base object of warrior in space
    /// </summary>
    public abstract class SpaceWarrior : SpaceObject
    {
        /// <summary>
        /// Invoke when got hit <Attacker, Defender>
        /// </summary>
        public event Action<SpaceWarrior, SpaceWarrior> GotHit;

        /// <summary>
        /// Invoke when SpaceWarrior hp reached zero
        /// </summary>
        public event Action<SpaceWarrior> Dead;

        public int HP { get; private set; }

        [SerializeField]
        protected Transform _attackPointTransform;

        protected Weapon _weapon;

        public SpaceWarrior SetHP(int hp)
        {
            HP = hp;
            return this;
        }

        public virtual bool IsDead()
        {
            return HP <= 0;
        }

        public virtual void GetHit(int damage, SpaceWarrior attacker)
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

        public virtual async Task SetWeapon(Weapon newWeapon)
        {
            await newWeapon.Setup();
            _weapon = newWeapon;
            _weapon.SetAttackPointTransform(_attackPointTransform);
        }

        protected virtual void Destroy()
        {
            Dead?.Invoke(this);
        }
    }
}
