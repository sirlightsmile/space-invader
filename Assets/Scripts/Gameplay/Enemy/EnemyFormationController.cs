using System;
using System.Threading.Tasks;
using SmileProject.Generic.Utilities;
using SmileProject.SpaceInvader.Constant;
using SmileProject.SpaceInvader.GameData;
using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay.Enemy
{
    public class EnemiesRow
    {
        public Transform Container { get; set; }
        public MoveDirection MoveDirection { get; set; } = MoveDirection.Right;
    }

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

        [Header("Spawn")]
        [SerializeField]
        private Transform _container;

        [Tooltip("inteval x between each enemies")]
        [SerializeField]
        private float _xInterval = 0.7f;

        [Tooltip("interval y between each enemies")]
        [SerializeField]
        private float _yInterval = 0.7f;

        [Tooltip("total enemies spawn in column")]
        [SerializeField]
        private int _columns = 5;

        [Tooltip("total enemies spawn in row")]
        [SerializeField]
        private int _rows = 8;

        [Tooltip("Delay time between spawn enemies each rows")]
        [SerializeField]
        private int _spawnInterval = 200;

        [Header("Movement")]
        [SerializeField]
        private float _moveSpeed = 0.5f;

        [SerializeField]
        private float _xBorder = 0.5f;

        private EnemiesRow[] _rowContainers;
        private EnemySpaceshipBuilder _enemyBuilder;
        private GameDataManager _gameDataManager;
        private float borderWorldPoint;
        private MoveDirection _moveDirection = MoveDirection.Right;

        public void Initialize(EnemySpaceshipBuilder enemyBuilder, GameDataManager gameDataManager)
        {
            _enemyBuilder = enemyBuilder;
            _gameDataManager = gameDataManager;
            _enemyBuilder.SpaceshipBuilded += OnSpaceshipBuilded;
        }

        public async Task SpawnEnemies()
        {
            Debug.Log("Spawn Enemies");
            _rowContainers = new EnemiesRow[_columns];
            Vector2 startPos = Vector2.zero;
            for (int y = _columns - 1; y >= 0; y--)
            {
                Transform currentRow = new GameObject($"Row_Container_{y}").transform;
                currentRow.SetParent(_container);
                currentRow.transform.localPosition = new Vector2(0, 0);
                _rowContainers[y] = new EnemiesRow() { Container = currentRow };
                for (int x = 0; x < _rows; x++)
                {
                    _gameDataManager.GetEnemySpaceshipModels();
                    EnemySpaceship enemy = await _enemyBuilder.BuildRandomEnemySpaceship();
                    enemy.transform.SetParent(currentRow);
                    enemy.transform.localPosition = new Vector2(startPos.x + (x * _xInterval), -(startPos.y + (y * _yInterval)));
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

        private void CalculateBorder()
        {
            // use half size due to pivot center
            var rowSize = _xInterval * _rows;
            var worldMaxBorder = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
            borderWorldPoint = worldMaxBorder.x - rowSize;
            Debug.Log("rowSize " + rowSize);
            Debug.Log("Border max x " + worldMaxBorder.x);
            Debug.Log("Border World Point : " + borderWorldPoint);
        }

        private void Update()
        {
            //Move step * Move speed
            if (_rowContainers == null)
            {
                return;
            }
            foreach (EnemiesRow enemyRow in _rowContainers)
            {
                if (enemyRow?.Container == null)
                {
                    continue;
                }
                Transform container = enemyRow.Container;
                float posX = container.transform.localPosition.x;
                MoveDirection moveDirection = enemyRow.MoveDirection;
                int direction = (int)moveDirection;
                container.Translate((Vector2.right * direction) * _moveSpeed * Time.deltaTime);
                if ((posX > _xBorder && moveDirection == MoveDirection.Right) || (posX < -_xBorder && moveDirection == MoveDirection.Left))
                {
                    enemyRow.MoveDirection = enemyRow.MoveDirection == MoveDirection.Right ? MoveDirection.Left : MoveDirection.Right;
                }
            }
        }

        private void ChangeDirection(EnemiesRow row)
        {
            row.MoveDirection = row.MoveDirection == MoveDirection.Right ? MoveDirection.Left : MoveDirection.Right;
        }

        private void OnSpaceshipBuilded(Spaceship spaceship)
        {
            SpaceshipAdded?.Invoke(spaceship);
        }
    }
}
