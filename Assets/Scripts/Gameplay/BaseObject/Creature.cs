using System;
using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay
{
    public abstract class Creature : SpaceObject
    {
        /// <summary>
        /// Invoke when got hit <Attacker, Defender>
        /// </summary>
        public event Action<Creature, Creature> GotHit;

        /// <summary>
        /// Invoke when creature hp reached zero
        /// </summary>
        public event Action<Creature> Dead;

        public int HP { get; private set; }

        public Creature SetHP(int hp)
        {
            HP = hp;
            return this;
        }

        public virtual bool IsDead()
        {
            return HP <= 0;
        }

        public virtual void GetHit(int damage, Creature attacker)
        {
            int result = HP - damage;
            HP = Mathf.Clamp(result, 0, HP);
            GotHit?.Invoke(attacker, this);

            if (IsDead())
            {
                ShipDestroy();
            }
            else
            {
                // TODO: play get hit sound
            }
        }

        protected virtual void ShipDestroy()
        {
            Dead?.Invoke(this);
        }

    }
}
