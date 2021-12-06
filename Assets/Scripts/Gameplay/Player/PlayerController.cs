using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay.Player
{
    public class PlayerController
    {       /// <summary>
            /// Invoke when player get hit with current hp
            /// </summary>
        public Action<int> PlayerGetHit;

        /// <summary>
        /// Invoke when player destroyed
        /// </summary>
        public Action PlayerDestroyed;

        public int PlayerScore { get; private set; } = 0;
        // public PlayerSpaceship PlayerSpaceship { get; private set; }
        // private PlayerSpaceshipBuilder _builder;

        // public PlayerController(ISpaceShooterInput inputManager, PlayerSpaceshipBuilder builder)
        // {
        //     _builder = builder;
        //     inputManager.AttackInput += PlayerShoot;
        //     inputManager.HorizontalInput += PlayerMove;
        // }

        // public async Task<PlayerSpaceship> CreatePlayer(Vector2 spawnPoint)
        // {
        //     PlayerSpaceship player = await _builder.BuildRandomSpaceship();
        //     player.SetPosition(spawnPoint);
        //     SetPlayer(player);
        //     return player;
        // }

        // public void AddScore(int score)
        // {
        //     PlayerScore += score;
        // }

        // private void SetPlayer(PlayerSpaceship player)
        // {
        //     this.PlayerSpaceship = player;
        //     player.GotHit += OnPlayerGotHit;
        //     player.Destroyed += OnPlayerDestroyed;
        // }

        // private void PlayerShoot()
        // {
        //     if (PlayerSpaceship != null && PlayerSpaceship.IsActive)
        //     {
        //         PlayerSpaceship.Shoot();
        //     }
        // }


        // private void PlayerMove(MoveDirection moveDirection)
        // {
        //     if (PlayerSpaceship != null && PlayerSpaceship.IsActive)
        //     {
        //         PlayerSpaceship.MoveToDirection(moveDirection);
        //     }
        // }

        // private void OnPlayerGotHit(Spaceship other, Spaceship player)
        // {
        //     int currentHp = player != null ? player.HP : 0;
        //     PlayerGetHit?.Invoke(currentHp);
        // }

        // private void OnPlayerDestroyed(Spaceship spaceship)
        // {
        //     PlayerDestroyed?.Invoke();
        // }
    }
}