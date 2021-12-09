using System.Threading.Tasks;
using SmileProject.SpaceInvader.GameData;
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

        private EnemySpaceshipBuilder _enemyBuilder;
        private GameDataManager _gameDataManager;

        public EnemySpawner(EnemySpaceshipBuilder enemyBuilder, GameDataManager gameDataManager)
        {
            _enemyBuilder = enemyBuilder;
            _gameDataManager = gameDataManager;
        }

        public async Task SpawnEnemies()
        {
            // TODO: config later
            int rows = 10;
            int columns = 10;
            for (int y = 0; y < columns; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    Vector2 startPos = Vector2.zero;
                    _gameDataManager.GetEnemySpaceshipModels();
                    EnemySpaceship enemy = await _enemyBuilder.BuildRandomEnemySpaceship();
                    enemy.transform.SetParent(_container);
                    float xInterval = ((enemy.Width / 2) + _widthInterval);
                    float yInterval = ((enemy.Height / 2) + _heightInterval);
                    enemy.transform.localPosition = new Vector2(startPos.x + (x * xInterval), -(startPos.y + (y * _heightInterval)));
                }
            }
        }
    }
}
