using System;
using SmileProject.Generic.Audio;
using SmileProject.SpaceInvader.Sounds;
using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay
{
    public class Shield : SpaceEntity
    {
        public event Action<Shield> Destroyed;
        public int Durability { get { return _durability; } }

        [SerializeField]
        private int _durability = 5;

        private AudioManager _audioManager;
        private SoundKeys _getHitSound, _destroyedSound;


        /// <summary>
        /// Set shield durability. When reach zero shield will be destroy
        /// </summary>
        /// <param name="durability"></param>
        /// <returns></returns>
        public Shield SetDurability(int durability)
        {
            _durability = durability;
            return this;
        }

        /// <summary>
        /// Set flip x for shield sprite
        /// </summary>
        /// <param name="isFlip"></param>
        /// <returns></returns>
        public Shield SetSpriteFlipX(bool isFlip)
        {
            _spriteRenderer.flipX = isFlip;
            return this;
        }

        /// <summary>
        /// Inject sound to shield object
        /// </summary>
        /// <param name="audioManager"></param>
        /// <param name="getHitSound"></param>
        /// <param name="destroyedSound"></param>
        public virtual void SetSounds(AudioManager audioManager, SoundKeys getHitSound, SoundKeys destroyedSound)
        {
            _audioManager = audioManager;
            _getHitSound = getHitSound;
            _destroyedSound = destroyedSound;
        }

        private void GetHit()
        {
            _durability--;
            if (_durability == 0)
            {
                Destroy();
                return;
            }
            else
            {
                var _ = SoundHelper.PlaySound(_getHitSound, _audioManager);
                if (_durability < 3)
                {
                    AnimateSprite();
                }
            }
        }

        private void Destroy()
        {
            var _ = SoundHelper.PlaySound(_destroyedSound, _audioManager);
            Destroyed?.Invoke(this);
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            Bullet bullet = other.transform.GetComponent<Bullet>();
            string otherTag = other.tag;
            if (otherTag == Tags.Bullet)
            {
                GetHit();
            }
            else if (otherTag == Tags.Invader)
            {
                Destroy();
            }
        }

        #region Pooling
        public override void OnSpawn()
        {
        }

        public override void OnDespawn()
        {
        }
        #endregion
    }
}
