using System;
using System.Collections;
using System.Threading.Tasks;
using SmileProject.Generic.Utilities;
using SmileProject.SpaceInvader.Constant;
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
		public event Action<EnemySpaceship[,]> FormationReady;

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

        [SerializeField]
        private float _xBorder = 0.5f;

        private float _moveSpeed = 0.5f;
        private Transform[] _rowContainers;
        private EnemySpaceshipBuilder _enemyBuilder;

        public void Initialize(EnemySpaceshipBuilder enemyBuilder)
        {
            _enemyBuilder = enemyBuilder;
            _enemyBuilder.SpaceshipBuilded += OnSpaceshipBuilded;
        }

        public void SetMoveSpeed(float speed)
        {
            _moveSpeed = speed;
        }

        public async Task SpawnEnemies()
        {
            Debug.Log("Spawn Enemies");
            _rowContainers = new Transform[_columns];
            Vector2 startPos = Vector2.zero;
            EnemySpaceship[,] spaceshipGrid = new EnemySpaceship[_columns, _rows];
            for (int y = _columns - 1; y >= 0; y--)
            {
                Transform currentRow = CreateRowContainer();
                _rowContainers[y] = currentRow;
                for (int x = 0; x < _rows; x++)
                {
                    EnemySpaceship enemy = await _enemyBuilder.BuildRandomEnemySpaceship();
                    enemy.transform.SetParent(currentRow);
                    enemy.transform.localPosition = new Vector2(startPos.x + (x * _xInterval), -(startPos.y + (y * _yInterval)));
                    enemy.SetGridIndex(x, y);
                    spaceshipGrid[y, x] = enemy;
                }
                StartCoroutine(MoveEnemyRowSide(currentRow));
                await Task.Delay(_spawnInterval);
            }
            Debug.Log("Enemies Formation Ready");
            FormationReady?.Invoke(spaceshipGrid);
        }

        public void OnWaveChanged()
        {
            SafeInvoke.InvokeAsync(async () => { await SpawnEnemies(); });
        }

        private Transform CreateRowContainer()
        {
            Transform row = new GameObject("Row_Container").transform;
            row.SetParent(_container);
            row.transform.localPosition = new Vector2(0, 0);
            return row;
        }

        /// <summary>
        /// Coroutine to move enemy row left or right
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        private IEnumerator MoveEnemyRowSide(Transform container)
        {
            MoveDirection moveDirection = MoveDirection.Right;
            int direction = (int)moveDirection;
            while (container.childCount > 0)
            {
                float posX = container.transform.localPosition.x;
                container.Translate((Vector2.right * direction) * _moveSpeed * Time.deltaTime);
                if ((posX > _xBorder && direction > 0) || (posX < -_xBorder && direction < 0))
                {
                    direction = -direction;
                    yield return StartCoroutine(MoveDown(container));
                }
                yield return null;
            }

            Destroy(container.gameObject);
        }

        /// <summary>
        /// Coroutine to move enemy row down
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        private IEnumerator MoveDown(Transform container)
        {
            float startPosY = container.transform.position.y;
            float targetPosY = startPosY - _yInterval;

            while (container.transform.position.y > targetPosY)
            {
                container.transform.Translate(Vector2.down * _moveSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            yield return null;
        }

        private void OnSpaceshipBuilded(Spaceship spaceship)
        {
            SpaceshipAdded?.Invoke(spaceship);
        }
    }
}
