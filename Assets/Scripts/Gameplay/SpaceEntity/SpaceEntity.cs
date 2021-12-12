using System.Collections;
using System.Collections.Generic;
using SmileProject.Generic.Pooling;
using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    /// <summary>
    /// Base object of every displayed object in gameplay scene
    /// </summary>
    public abstract class SpaceEntity : PoolObject
    {
        /// <summary>
        /// Get sprite width
        /// </summary>
        /// <value>sprite width</value>
        public float Width
        {
            get
            {
                return _spriteRenderer?.bounds.size.x * _spriteRenderer?.sprite?.pixelsPerUnit ?? 0;
            }
        }

        /// <summary>
        /// Get sprite height
        /// </summary>
        /// <value>sprite height</value>
        public float Height
        {
            get
            {
                return _spriteRenderer?.bounds.size.y * _spriteRenderer?.sprite.pixelsPerUnit ?? 0;
            }
        }

        [SerializeField]
        protected SpriteRenderer _spriteRenderer;

        protected IList<Sprite> _animateSprites = null;

        private int _currentSpriteFrame = 0;

        public virtual void Awake()
        {
            Debug.Assert(_spriteRenderer != null, "Missing sprite renderer reference in space object. Should assign reference in prefab.");
        }

        /// <summary>
        /// Set position on world space
        /// </summary>
        /// <param name="pos">vector 2 position</param>
        /// <returns></returns>
        public SpaceEntity SetPosition(Vector2 pos)
        {
            this.transform.position = pos;
            return this;
        }

        /// <summary>
        /// Set sprite to sprite renderer of the object
        /// </summary>
        /// <param name="sprite">sprite</param>
        /// <returns></returns>
        public void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        /// <summary>
        /// Set sprites for animate. First sprite will set as index of start frame.
        /// </summary>
        /// <param name="sprites">array of animate sprite</param>
        /// <param name="startFrame">index of sprite</param>
        /// <returns></returns>
        public SpaceEntity SetSprite(IList<Sprite> sprites, int startFrame = 0)
        {
            if (startFrame > sprites.Count)
            {
                Debug.LogAssertion("Start frame out of length.");
                startFrame = 0;
            }
            _spriteRenderer.sprite = sprites[startFrame];
            _currentSpriteFrame = startFrame;
            _animateSprites = sprites;
            return this;
        }

        /// <summary>
        /// Animate sprite to next frame
        /// </summary>
        public void AnimateSprite(bool ignoreInterval = false)
        {
            if (_animateSprites == null || _animateSprites.Count < 2)
            {
                Debug.LogAssertion("Unable to animate sprite. Should have more than one sprites");
                return;
            }
            _currentSpriteFrame = _currentSpriteFrame++ % _animateSprites.Count;
            Sprite currentFrameSprite = _animateSprites[_currentSpriteFrame];
            SetSprite(currentFrameSprite);
        }

        /// <summary>
        /// On entity trigger enter. Manage every event that affects this object
        /// </summary>
        /// <param name="other"></param>
        protected abstract void OnTriggerEnter2D(Collider2D other);
    }
}
