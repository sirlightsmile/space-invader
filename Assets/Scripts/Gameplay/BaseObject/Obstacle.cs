using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay
{
    public class Obstacle : SpaceObject
    {
        public int Durability { get; private set; }

        public Obstacle SetDurability(int durability)
        {
            Durability = durability;
            return this;
        }

        public override void OnTriggerEnter(Collider other)
        {
            // TODO: on get hit with enemy/bullet
            throw new System.NotImplementedException();
        }
    }
}
