using System;
using System.Threading.Tasks;
using SmileProject.SpaceInvader.Config;
using SmileProject.SpaceInvader.Constant;
using SmileProject.SpaceInvader.Gameplay.Input;
using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay.Player
{
    public class PlayerController
    {
        /// <summary>
        /// Invoke when player get hit with current hp
        /// </summary>
        public Action<int> PlayerGetHit;

        /// <summary>
        /// Invoke when player destroyed
        /// </summary>
        public Action PlayerDestroyed;

        public int KillScore { get; private set; } = 0;
        public PlayerSpaceship PlayerSpaceship { get; private set; }
        private PlayerSpaceshipBuilder _builder;
        private PlayerConfig _config;
        private float _invokeAttackInterval = 0.3f;
        private float _attackTimestamp = 0;

        public PlayerController(ISpaceShooterInput inputManager, PlayerSpaceshipBuilder builder)
        {
            _builder = builder;
            inputManager.AttackInput += InvokeAttackInterval;
            inputManager.HorizontalInput += PlayerMove;
        }

        public void ApplyPlayerConfig(PlayerConfig config)
        {
            _config = config;
            SetFireRatePerSecond(config.PlayerFireRate);
        }

        private void SetFireRatePerSecond(int fireRate)
        {
            _invokeAttackInterval = 1f / fireRate;
        }

        public async Task<PlayerSpaceship> CreatePlayer(Vector2 spawnPoint)
        {
            PlayerSpaceship player = await _builder.BuildSpaceshipById(_config.PlayerSpaceshipID);
            player.SetPosition(spawnPoint);
            SetPlayer(player);
            return player;
        }

        public void AddKillScore(int score)
        {
            KillScore += score;
        }

        private void SetPlayer(PlayerSpaceship player)
        {
            this.PlayerSpaceship = player;
            player.GotHit += OnPlayerGotHit;
            player.Destroyed += OnPlayerDestroyed;
        }

        private void InvokeAttackInterval()
        {
            if (Time.time - _attackTimestamp > _invokeAttackInterval)
            {
                PlayerShoot();
            }
        }

        private void PlayerShoot()
        {
            if (PlayerSpaceship != null && PlayerSpaceship.IsActive)
            {
                _attackTimestamp = Time.time;
                PlayerSpaceship.Shoot();
            }
        }


        private void PlayerMove(MoveDirection moveDirection)
        {
            if (PlayerSpaceship != null && PlayerSpaceship.IsActive)
            {
                PlayerSpaceship.MoveToDirection(moveDirection);
            }
        }

        private void OnPlayerGotHit(Spaceship other, Spaceship player)
        {
            int currentHp = player != null ? player.HP : 0;
            PlayerGetHit?.Invoke(currentHp);
        }

        private void OnPlayerDestroyed(Spaceship spaceship)
        {
            PlayerDestroyed?.Invoke();
        }
    }
}