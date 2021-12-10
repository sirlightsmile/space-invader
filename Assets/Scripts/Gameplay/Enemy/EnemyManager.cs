using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmileProject.Generic.Utilities;
using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay.Enemy
{
    public class EnemyManager
    {
        /// <summary>
        /// Invoke when enemy destroyed with enemy destroy score
        /// </summary>
        public event Action<int> EnemyDestroyed;

        /// <summary>
        /// Invoke when all enemies spaceship in wave had destroyed
        /// </summary>
        public event Action AllSpaceshipDestroyed;

        /// <summary>
        /// Invoke when is enemy ready status changed
        /// </summary>
        public event Action<bool> EnemyReadyStatusChanged;

        /// <summary>
        /// Whather enemy ready to fight or not
        /// </summary>
        /// <value></value>
        public bool IsEnemiesReady { get; private set; }

        private EnemyFormationController _formationController;
        private List<EnemySpaceship> _enemySpaceships = new List<EnemySpaceship>();

        /// <summary>
        /// Shoot chance in percent (max is 1)
        /// </summary>
        private float _randomShootChance = 0.3f;

        /// <summary>
        /// Reference of time that trigger spaceships shoot
        /// </summary>
        private float _lastShootTimestamp = 0;

        /// <summary>
        /// Time interval for trigger spaceships shoot (seconds)
        /// </summary>
        private float _triggerShootInterval = 2f;

        /// <summary>
        /// Max random time for shoot async (seconds)
        /// </summary>
        private float _shootAsyncInterval = 1f;

        public EnemyManager(EnemyFormationController formationController)
        {
            _formationController = formationController;
            formationController.SpaceshipAdded += OnEnemySpaceshipAdded;
        }

        /// <summary>
        /// Enemy manager update loop
        /// Should manual update by gameplay controller
        /// </summary>
        public void Update()
        {
            if (!IsEnemiesReady)
            {
                return;
            }

            float currentTime = Time.time;
            RandomEnemiesShoot(currentTime);
        }

        private void RandomEnemiesShoot(float currentTime)
        {
            if (currentTime - _lastShootTimestamp < _triggerShootInterval)
            {
                return;
            }

            _lastShootTimestamp = currentTime;
            foreach (var spaceship in _enemySpaceships)
            {
                // random for shoot percent
                float random = UnityEngine.Random.Range(0f, 1f);
                bool isShoot = random <= _randomShootChance;
                if (isShoot)
                {
                    SafeInvoke.Invoke(async () => { await TriggerShootAsync(spaceship); });
                }
            }
        }

        private EnemySpaceship GetRandomSpaceship()
        {
            int index = UnityEngine.Random.Range(0, _enemySpaceships.Count);
            return _enemySpaceships[index];
        }

        private async Task TriggerShootAsync(EnemySpaceship spaceship)
        {
            float randomDelay = UnityEngine.Random.Range(0f, _shootAsyncInterval);
            // from second to millisecond
            int delayTimeMillisecond = (int)(randomDelay * 1000);
            await Task.Delay(delayTimeMillisecond);
            if (spaceship != null && spaceship.IsActive)
            {
                spaceship?.Shoot();
            }
        }

        private void OnFormationReady()
        {
            IsEnemiesReady = true;
            EnemyReadyStatusChanged?.Invoke(IsEnemiesReady);
        }

        private void OnEnemySpaceshipAdded(Spaceship spaceship)
        {
            spaceship.Destroyed += OnEnemyDestroyed;
            _enemySpaceships.Add(spaceship.GetComponent<EnemySpaceship>());
        }

        private void OnEnemyDestroyed(Spaceship spaceship)
        {
            spaceship.Destroyed -= OnEnemyDestroyed;
            EnemySpaceship enemy = spaceship.GetComponent<EnemySpaceship>();
            _enemySpaceships.Remove(enemy);
            EnemyDestroyed?.Invoke(enemy.Score);
            if (_enemySpaceships.Count == 0)
            {
                AllSpaceshipDestroyed?.Invoke();
            }
        }
    }
}