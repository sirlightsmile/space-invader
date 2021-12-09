using SmileProject.Generic.Pooling;
using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay.Enemy
{
    public class EnemySpawner
    {
        [SerializeField]
        private Transform _container;

        [SerializeField]
        private float _widthInterval = 1f;

        [SerializeField]
        private float _heightInterval = 1f;

        private AlienFactory _factory;
        private PoolManager _pooling;

        public EnemySpawner()
        {
            // _pooling.CreatePool<Alien>();
        }

        public void SpawnEnemies()
        {
            // TODO: config later
            int rows = 10;
            int columns = 10;
            for (int y = 0; y < columns; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    Vector2 startPos = Vector2.zero;
                    EnemySpaceship alien = _factory.GetRandomAlien();
                    alien.transform.SetParent(_container);
                    float xInterval = ((alien.Width / 2) + _widthInterval);
                    float yInterval = ((alien.Height / 2) + _heightInterval);
                    alien.transform.localPosition = new Vector2(startPos.x + (x * xInterval), -(startPos.y + (y * _heightInterval)));
                }
            }
        }
    }
}
