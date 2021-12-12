using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay.Enemy
{
    public class EnemySpaceship : Spaceship
    {
        public int Score { get; private set; }

        public EnemyType Type { get; private set; }

        public int GridX { get; private set; }

        public int GridY { get; private set; }

        /// <summary>
        /// Set grid index for reference when find adjacent spaceship
        /// </summary>
        /// <param name="x">index in row</param>
        /// <param name="y">index in column</param>
        /// <returns></returns>
        public EnemySpaceship SetGridIndex(int x, int y)
        {
            GridX = x;
            GridY = y;
            return this;
        }

        /// <summary>
        /// Set enemy type
        /// Will effect when find adjacent spaceship
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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

        public override void Destroy()
        {
            base.Destroy();
            ReturnToPool();
        }
    }
}
