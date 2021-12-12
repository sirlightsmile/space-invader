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

        [SerializeField]
        protected List<Sprite> _animateSprites;

        private int _currentSpriteFrame = 0;
        private Coroutine _animateSpriteCoroutine = null;

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
            _animateSprites = new List<Sprite>(sprites);
            Debug.Log("Set animate sprite length : " + _animateSprites.Count);
            return this;
        }

        /// <summary>
        /// Animate sprite to next sprite frame
        /// </summary>
        public void AnimateSprite(int? frameCount = null)
        {
            if (_animateSprites == null || _animateSprites.Count < 1)
            {
                Debug.LogAssertion("Unable to animate sprite. Should have more than one sprites");
                return;
            }
            int spritesCount = frameCount.HasValue ? frameCount.Value : _animateSprites.Count;
            _currentSpriteFrame = ++_currentSpriteFrame % spritesCount;
            Sprite currentFrameSprite = _animateSprites[_currentSpriteFrame];
            SetSprite(currentFrameSprite);
        }

        public void AnimateSpriteLoop(float interval, int? frameCount = null)
        {
            _animateSpriteCoroutine = StartCoroutine(AnimateSpriteCoroutine(interval, frameCount));
        }

        public void StopAnimateSprite()
        {
            if (_animateSpriteCoroutine != null)
            {
                StopCoroutine(_animateSpriteCoroutine);
                _animateSpriteCoroutine = null;
            }
        }

        private IEnumerator AnimateSpriteCoroutine(float animateInterval, int? frameCount = null)
        {
            while (IsActive)
            {
                AnimateSprite(frameCount);
                yield return new WaitForSeconds(animateInterval);
            }
        }

        /// <summary>
        /// On entity trigger enter. Manage every event that affects this object
        /// </summary>
        /// <param name="other"></param>
        protected abstract void OnTriggerEnter2D(Collider2D other);
    }
}
