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
        public event Action<Spaceship> Dead;

        public int HP { get; private set; }

        [SerializeField]
        protected Transform _attackPointTransform;

        protected SpaceshipGun _weapon;
        protected AudioManager _audioManager;
        protected SoundKeys _getHitSound, _destroyedSound;

        public virtual void Setup<T>(T spaceshipModel) where T : SpaceshipModel
        {
            HP = spaceshipModel.HP;
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
                var _ = PlaySound(_getHitSound);
            }
        }

        public virtual async Task SetWeapon(SpaceshipGun newWeapon)
        {
            await newWeapon.Setup();
            _weapon = newWeapon;
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
            Dead?.Invoke(this);
        }
    }
}
