using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay.Enemy
{
    public class EnemySpaceship : Spaceship
    {
        public int Score { get; private set; }

        public EnemyType Type { get; private set; }

        public EnemySpaceship SetType(EnemyType type)
        {
            Type = type;
            return this;
        }

        /// <summary>
        ///  Set score that player will get after killing this alien
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public EnemySpaceship SetScore(int score)
        {
            Debug.Assert(score > 0, "Kill score should be more than zero.");
            Score = score;
            return this;
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
        }
    }
}
