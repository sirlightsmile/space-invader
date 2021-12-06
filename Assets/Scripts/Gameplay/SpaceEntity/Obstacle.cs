using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay
{
    public class Obstacle : SpaceEntity
    {
        public int Durability { get; private set; }

        public Obstacle SetDurability(int durability)
        {
            Durability = durability;
            return this;
        }

        private void GetHit()
        {
            Durability--;
            if (Durability == 0)
            {
                OnDestroy();
            }
        }

        private void OnDestroy()
        {
            Destroy(this.gameObject);
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
                OnDestroy();
            }
        }
    }
}
