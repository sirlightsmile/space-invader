using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay
{
    public class Bullet : SpaceObject
    {
        public Creature Owner
        {
            get
            {
                return _owner;
            }
            private set
            {
                _owner = value;
            }
        }

        // TODO: make config for these
        private float _bulletSpeed = 5f;
        private int _damage = 1;
        private float _yBorder;
        private Creature _owner;

        /// <summary>
        /// Reference of owner tag. In case owner was destroyed before bullet reached target
        /// </summary>
        private string _ownerTag;

        private void Start()
        {
            SetBorder();
        }

        /// <summary>
        /// Set bullet damage
        /// </summary>
        /// <param name="damage">damage</param>
        /// <returns>Bullet</returns>
        public Bullet SetDamage(int damage)
        {
            _damage = damage;
            return this;
        }

        /// <summary>
        /// Set bullet owner
        /// </summary>
        /// <param name="owner">owner</param>
        /// <returns>Bullet</returns>
        public Bullet SetOwner(Creature owner)
        {
            Owner = owner;
            _ownerTag = owner.tag;
            return this;
        }

        /// <summary>
        /// Set bullet position in world space
        /// </summary>
        /// <param name="position">world position</param>
        /// <returns>Bullet</returns>
        public Bullet SetPosition(Vector2 position)
        {
            transform.position = position;
            return this;
        }

        /// <summary>
        /// Set bullet rotation
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public Bullet SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
            return this;
        }

        private void Update()
        {
            Vector2 moveVector = Vector2.Dot(transform.up, Vector2.down) > 0 ? -transform.up : transform.up;
            transform.Translate(moveVector * Time.deltaTime * _bulletSpeed);
            if (IsVisible())
            {
                // TODO: destroy bullet
            }
        }

        /// <summary>
        /// Is visible in vertical
        /// </summary>
        /// <returns></returns>
        private bool IsVisible()
        {
            return transform.position.y > _yBorder || transform.position.y < -_yBorder;
        }

        /// <summary>
        /// Setup world vertical border for check visible
        /// </summary>
        private void SetBorder()
        {
            float borderY = Screen.height;
            float borderWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(0, borderY, 0)).y;
            _yBorder = borderWorldPoint;
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag != _ownerTag)
            {
                Destroy(this.gameObject);
            }
        }
    }
}