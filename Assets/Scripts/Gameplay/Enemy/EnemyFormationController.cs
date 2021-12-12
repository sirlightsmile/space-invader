using System;
using System.Threading.Tasks;
using SmileProject.Generic.Utilities;
using SmileProject.SpaceInvader.GameData;
using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay.Enemy
{
    public class EnemyFormationController : MonoBehaviour
    {
        /// <summary>
        /// Invoke when add new spaceship to formation
        /// </summary>
        public event Action<Spaceship> SpaceshipAdded;

        /// <summary>
		/// Invoke when all spaceship reached all formation point
		/// </summary>
		public event Action FormationReady;

        [SerializeField]
        private Transform _container;

        [SerializeField]
        private float _widthInterval = 0.2f;

        [SerializeField]
        private float _heightInterval = 0.2f;

        [SerializeField]
        private int _columns = 10;

        [SerializeField]
        private int _rows = 5;

        [SerializeField]
        private int _spawnInterval = 200;

        private Transform[] _rowContainers;
        private EnemySpaceshipBuilder _enemyBuilder;
        private GameDataManager _gameDataManager;

        public void Initialize(EnemySpaceshipBuilder enemyBuilder, GameDataManager gameDataManager)
        {
            _enemyBuilder = enemyBuilder;
            _gameDataManager = gameDataManager;
            _enemyBuilder.SpaceshipBuilded += OnSpaceshipBuilded;
        }

        public async Task SpawnEnemies()
        {
            Debug.Log("Spawn Enemies");
            _rowContainers = new Transform[_columns];
            for (int y = _columns - 1; y >= 0; y--)
            {
                Transform currentRow = new GameObject($"Row_Container_${y}").transform;
                _rowContainers[y] = currentRow;
                currentRow.SetParent(_container);
                currentRow.transform.localPosition = new Vector2(0, 0);
                for (int x = 0; x < _rows; x++)
                {
                    Vector2 startPos = Vector2.zero;
                    _gameDataManager.GetEnemySpaceshipModels();
                    EnemySpaceship enemy = await _enemyBuilder.BuildRandomEnemySpaceship();
                    enemy.transform.SetParent(currentRow);
                    float xInterval = enemy.transform.localScale.x + _widthInterval;
                    float yInterval = enemy.transform.localScale.y + _heightInterval;
                    enemy.transform.localPosition = new Vector2(startPos.x + (x * xInterval), -(startPos.y + (y * yInterval)));
                }
                await Task.Delay(_spawnInterval);
            }
            Debug.Log("Enemies Formation Ready");
            FormationReady?.Invoke();
        }

        public void OnWaveChanged()
        {
            SafeInvoke.InvokeAsync(async () => { await SpawnEnemies(); });
        }

        private void Update()
        {
            //Move step * Move speed
            // float moveStep = 0.2f;
            // float moveSpeed = 1f;
            // foreach (Transform container in _rowContainers)
            // {
            //     container?.transform.localPosition = new Vector2();
            // }
        }

        private void OnSpaceshipBuilded(Spaceship spaceship)
        {
            SpaceshipAdded?.Invoke(spaceship);
        }

#if UNITY_EDITOR

        void OnDrawGizmos()
        {
            // draw game grid
            int gridScale = 100;
            int width = Screen.width;
            int height = Screen.height;
            int widthOffset = width % gridScale;
            int heightOffset = height % gridScale;
            int column = Mathf.FloorToInt(width / gridScale);
            int row = Mathf.FloorToInt(height / gridScale);

            Vector2 startPos = _container.transform.position;

            // Debug.Log($"Width : {width} Height : {height}");
            // Debug.Log($"Width Offset: {widthOffset} Height Offset: {heightOffset}");
            // Debug.Log($"Column: {column} Row: {row}");
            // Debug.Log("Start point : " + startPos.x + " " + startPos.y);

            for (int y = 0; y < column; y++)
            {
                for (int x = 0; x < row; x++)
                {
                    float baseEnemySize = 0.5f;
                    float xInterval = baseEnemySize + _widthInterval;
                    float yInterval = baseEnemySize + _heightInterval;
                    var pos = new Vector2(startPos.x + (x * xInterval), -(startPos.y + (y * yInterval)));
                    Gizmos.DrawWireCube(pos, new Vector2(0.5f, 0.5f));
                }
            }

        }
#endif
    }
}
