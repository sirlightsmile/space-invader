using System;
using System.Threading.Tasks;
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
        public int TotalScore { get { return TimerScore + KillScore; } }
        public int TimerScore { get; private set; } = 0;
        public int KillScore { get; private set; } = 0;
        public PlayerSpaceship PlayerSpaceship { get; private set; }
        private PlayerSpaceshipBuilder _builder;

        public PlayerController(ISpaceShooterInput inputManager, PlayerSpaceshipBuilder builder)
        {
            _builder = builder;
            inputManager.AttackInput += PlayerShoot;
            inputManager.HorizontalInput += PlayerMove;
        }

        public async Task<PlayerSpaceship> CreatePlayer(Vector2 spawnPoint)
        {
            //TODO: config player spaceship id
            PlayerSpaceship player = await _builder.BuildSpaceshipById("ps01");
            player.SetPosition(spawnPoint);
            SetPlayer(player);
            return player;
        }

        public void AddKillScore(int score)
        {
            KillScore += score;
        }

        public void SetTimerScore(int score)
        {
            TimerScore = score;
        }

        private void SetPlayer(PlayerSpaceship player)
        {
            this.PlayerSpaceship = player;
            player.GotHit += OnPlayerGotHit;
            player.Destroyed += OnPlayerDestroyed;
        }

        private void PlayerShoot()
        {
            if (PlayerSpaceship != null && PlayerSpaceship.IsActive)
            {
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