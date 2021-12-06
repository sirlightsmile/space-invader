﻿using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public abstract class SpaceObject : MonoBehaviour
    {
        /// <summary>
        /// Get sprite width
        /// </summary>
        /// <value>sprite width</value>
        protected float Width
        {
            get
            {
                return _spriteRenderer?.bounds.size.x * _spriteRenderer?.sprite.pixelsPerUnit ?? 0;
            }
        }

        /// <summary>
        /// Get sprite height
        /// </summary>
        /// <value>sprite height</value>
        protected float Height
        {
            get
            {
                return _spriteRenderer?.bounds.size.y * _spriteRenderer?.sprite.pixelsPerUnit ?? 0;
            }
        }

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private Sprite[] _animateSprites = null;

        private int _currentSpriteFrame = 0;

        public virtual void Awake()
        {
            if (_spriteRenderer == null)
            {
                Debug.Assert(_spriteRenderer != null, "Missing sprite renderer reference in space object. Should assign reference in prefab.");
                _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }
        }

        /// <summary>
        /// Set sprite to sprite renderer of the object
        /// </summary>
        /// <param name="sprite">sprite</param>
        /// <returns></returns>
        public SpaceObject SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
            return this;
        }

        /// <summary>
        /// Set sprites for animate. First sprite will set as index of start frame.
        /// </summary>
        /// <param name="sprites">array of animate sprite</param>
        /// <param name="startFrame">index of sprite</param>
        /// <returns></returns>
        public SpaceObject SetSprite(Sprite[] sprites, int startFrame = 0)
        {
            if (startFrame > sprites.Length)
            {
                Debug.LogAssertion("Start frame out of length.");
                startFrame = 0;
            }
            _spriteRenderer.sprite = sprites[startFrame];
            _animateSprites = sprites;
            return this;
        }

        /// <summary>
        /// Animate sprite to next frame
        /// </summary>
        public void AnimateSprite(bool ignoreInterval = false)
        {
            //TODO: animate interval
            if (_animateSprites.Length > 1)
            {
                Debug.LogAssertion("Unable to animate sprite. Should have more than one sprites");
                return;
            }
            _currentSpriteFrame = _currentSpriteFrame++ % _animateSprites.Length;
            Sprite currentFrameSprite = _animateSprites[_currentSpriteFrame];
            SetSprite(currentFrameSprite);
        }

        protected abstract void OnTriggerEnter2D(Collider2D other);
    }
}
