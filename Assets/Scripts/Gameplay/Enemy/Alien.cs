using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay.Enemy
{
    public class Alien : SpaceWarrior
    {
        public int Score { get; private set; }

        public Alien SetColor(Color color)
        {
            _spriteRenderer.color = color;
            return this;
        }

        /// <summary>
        ///  Set score that player will get after killing this alien
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public Alien SetScore(int score)
        {
            Debug.Assert(score > 0, "Kill score should be more than zero.");
            Score = score;
            return this;
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            // TODO: on trigger enter bullet
            throw new System.NotImplementedException();
        }
    }
}
