using System;
using System.Threading.Tasks;
using SmileProject.Generic.Audio;
using SmileProject.SpaceInvader.GameData;
using SmileProject.SpaceInvader.Gameplay.Weapon;
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
        public event Action<Spaceship> Destroyed;

        public int HP { get; private set; }

        [SerializeField]
        protected Transform _attackPointTransform;

        protected SpaceshipGun _weapon;
        protected AudioManager _audioManager;
        protected SoundKeys _getHitSound, _destroyedSound;

        /// <summary>
        /// Setup spaceship from spaceship model
        /// </summary>
        /// <param name="spaceshipModel"></param>
        /// <typeparam name="T"></typeparam>
        public virtual void Setup<T>(T spaceshipModel) where T : SpaceshipModel
        {
            HP = spaceshipModel.HP;
        }

        /// <summary>
        /// Check weather spaceship hp reached 0 or not.
        /// </summary>
        /// <returns>true if hp reached 0</returns>
        public virtual bool IsDead()
        {
            return HP <= 0;
        }

        /// <summary>
        /// Invoke attack from spaceship's weapon
        /// </summary>
        public virtual void Shoot()
        {
            if (_weapon == null)
            {
                Debug.LogAssertion("Spaceship weapon should not be null.");
                return;
            }
            _weapon.Attack(this);
        }

        /// <summary>
        /// Call when spaceship take damage
        /// </summary>
        /// <param name="damage">damage received</param>
        /// <param name="attacker">attacker spaceship reference</param>
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
                var _ = PlaySound(_getHitSound);
            }
        }

        /// <summary>
        /// Attach weapon to spaceship
        /// </summary>
        /// <param name="weapon"></param>
        /// <returns></returns>
        public virtual async Task SetWeapon(SpaceshipGun weapon)
        {
            await weapon.Setup();
            _weapon = weapon;
            _weapon.SetAttackPointTransform(_attackPointTransform);
        }

        public virtual void SetSounds(AudioManager audioManager, SoundKeys getHitSound, SoundKeys destroyedSound)
        {
            _audioManager = audioManager;
            _getHitSound = getHitSound;
            _destroyedSound = destroyedSound;
        }

        public async Task<int> PlaySound(SoundKeys soundKey)
        {
            try
            {
                if (_audioManager != null && soundKey != null)
                {
                    return await _audioManager.PlaySound(soundKey);
                }
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
            return -1;
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
            Destroyed?.Invoke(this);
        }
    }
}
