using System;
using System.Threading.Tasks;
using SmileProject.Generic.Audio;
using SmileProject.SpaceInvader.GameData;
using SmileProject.SpaceInvader.Weapon;
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

        private const string NormalAnimStateName = "Normal";
        private const string GetHitAnimStateName = "GetHit";

        public int HP { get; private set; }

        [SerializeField]
        protected Transform _attackPointTransform;

        [SerializeField]
        protected Animator _animator;

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
            Debug.Log("Got hit!!");
            int result = HP - damage;
            HP = Mathf.Clamp(result, 0, HP);
            GotHit?.Invoke(attacker, this);
            Debug.Log("Play get hit animation");
            PlayGetHitAnimation();

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
        public override void OnSpawn()
        {
        }

        public override void OnDespawn()
        {
            SetSprite(null);
            _weapon = null;
        }
        #endregion

        public virtual void Destroy()
        {
            HP = 0;
            Destroyed?.Invoke(this);
            var _ = PlaySound(_destroyedSound);
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == Tags.Bullet)
            {
                Bullet bullet = other.GetComponent<Bullet>();
                if (bullet != null && bullet.Owner.tag != transform.tag)
                {
                    GetHit(bullet.Damage, bullet.Owner);
                }
            }
        }

        private void OnEnable()
        {
            PlayIdleAnimation();
        }

        private void PlayGetHitAnimation()
        {
            Debug.Log("Play get hit animation");
            _animator.Play(GetHitAnimStateName);
        }

        private void PlayIdleAnimation()
        {
            _animator.Play(NormalAnimStateName);
        }
    }
}
