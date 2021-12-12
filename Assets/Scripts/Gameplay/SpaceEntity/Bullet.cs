using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay
{
    public class Bullet : SpaceEntity
    {
        /// <summary>
        /// Owner of this bullet. Return null if owner already dead or inactive
        /// </summary>
        /// <value></value>
        public Spaceship Owner
        {
            get
            {
                return _owner.IsActive && !_owner.IsDead() ? _owner : null;
            }
            private set
            {
                _owner = value;
            }
        }

        public int Damage { get; private set; } = 1;

        private float _bulletSpeed;
        private float _yBorder;
        private Spaceship _owner;

        /// <summary>
        /// Reference of owner tag. In case owner was destroyed before bullet reached target
        /// </summary>
        private string _ownerTag;

        private void Start()
        {
            SetBorder();
        }

        public override void OnSpawn()
        {
        }

        public override void OnDespawn()
        {
        }

        /// <summary>
        /// Set bullet damage
        /// </summary>
        /// <param name="damage">damage</param>
        /// <returns>Bullet</returns>
        public Bullet SetDamage(int damage)
        {
            Damage = damage;
            return this;
        }

        /// <summary>
        /// Set bullet owner
        /// </summary>
        /// <param name="owner">owner</param>
        /// <returns>Bullet</returns>
        public Bullet SetSpeed(float speed)
        {
            _bulletSpeed = speed;
            return this;
        }

        /// <summary>
        /// Set bullet owner
        /// </summary>
        /// <param name="owner">owner</param>
        /// <returns>Bullet</returns>
        public Bullet SetOwner(Spaceship owner)
        {
            Owner = owner;
            _ownerTag = owner.tag;
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
                Destroy();
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

        private void Destroy()
        {
            ReturnToPool();
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag != _ownerTag)
            {
                Destroy();
            }
        }
    }
}