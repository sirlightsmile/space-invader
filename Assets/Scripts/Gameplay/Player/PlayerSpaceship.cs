using SmileProject.SpaceInvader.Constant;
using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay.Player
{
    public class PlayerSpaceship : Spaceship
    {
        [SerializeField]
        private float _moveSpeed;

        private float _moveBorder;

        /// <summary>
        /// Set speed to player movement
        /// </summary>
        /// <param name="speed">speed in float</param>
        /// <returns></returns>
        public PlayerSpaceship SetSpeed(float speed)
        {
            _moveSpeed = speed;
            return this;
        }

        /// <summary>
        /// Move spaceship in horizontal direction
        /// </summary>
        /// <param name="direction">move direction</param>
        public void MoveToDirection(MoveDirection direction)
        {
            float directionValue = (float)direction;
            float posX = this.transform.position.x + (directionValue * (_moveSpeed * Time.deltaTime));
            posX = Mathf.Clamp(posX, -_moveBorder, _moveBorder);
            this.transform.position = new Vector3(posX, this.transform.position.y, this.transform.position.z);
        }

        /// <summary>
        /// Setup world border for clamp movement
        /// </summary>
        public void SetBorder()
        {
            float halfSpriteSize = Width / 2;
            float borderRight = Screen.width - halfSpriteSize;
            float borderWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(borderRight, 0, 0)).x;
            _moveBorder = borderWorldPoint;
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == Tags.Bullet)
            {
                Bullet bullet = other.GetComponent<Bullet>();
                GetHit(bullet.Damage, bullet.Owner);
            }
            else if (other.tag == Tags.Invader)
            {
                // instant dead
                GetHit(HP, other.GetComponent<Spaceship>());
            }
        }
    }
}