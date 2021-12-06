using System.Threading.Tasks;
using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay
{
    public abstract class Weapon
    {
        public const int WEAPON_INITIAL_LEVEL = 1;

        protected int _level;
        protected int _maxLevel;
        protected int _damage;
        protected float _attackSpeed;
        protected Transform _attackPointTransform;

        /// <summary>
        /// Set attack initiate point
        /// /// </summary>
        /// <param name="transform">Transform which represent to attack initiate point</param>
        public void SetAttackPointTransform(Transform transform)
        {
            _attackPointTransform = transform;
        }

        public abstract void Attack(SpaceWarrior attacker);

        /// <summary>
        /// Prepare weapon for use. Setting up or loading required assets
        /// </summary>
        public abstract Task Setup();

        protected Weapon SetDamage(int damage)
        {
            _damage = damage;
            return this;
        }

        protected Weapon SetLevel(int level)
        {
            _level = level;
            return this;
        }

        protected Weapon SetMaxLevel(int maxLevel)
        {
            _maxLevel = maxLevel;
            return this;
        }

        protected Weapon SetAttackSpeed(int speed)
        {
            _attackSpeed = speed;
            return this;
        }
    }
}