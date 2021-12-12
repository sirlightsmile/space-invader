using Configs;
using System.Threading.Tasks;
using SmileProject.Generic.GameState;
using UnityEngine;
using SmileProject.SpaceInvader.Gameplay.Player;

namespace SmileProject.SpaceInvader.Gameplay
{
    /// <summary>
    /// This class controll game state life time
    /// </summary>
    public class GameController
    {
        private GameStateManager _gameStateManager;
        private PlayerController _playerController;
        private GameplayController _gameplayController;

        public GameController(GameStateManager gameStateManager, PlayerController playerController, GameplayController gameplayController)
        {
            _gameStateManager = gameStateManager;
            _gameplayController = gameplayController;
            _playerController = playerController;
        }

        public void StartBattle()
        {
            _gameplayController.StandBy();
        }

        public async Task PauseBattle()
        {
            await _gameStateManager.ChangeStateAsync(GameStates.Pause);
        }

        public async Task GoToMainMenu()
        {
            await _gameStateManager.ChangeStateAsync(GameStates.MainMenu);
        }
    }
}
