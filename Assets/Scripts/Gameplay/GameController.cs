using Configs;
using System.Threading.Tasks;
using SmileProject.Generic.GameState;
using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay
{
    /// <summary>
    /// This class controll game state life time
    /// </summary>
    public class GameController : MonoBehaviour
    {
        private GameStateManager _gameStateManager;
        private bool isStateChanging = false;
        public GameController(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
        }

        public async Task StartBattle()
        {
            await _gameStateManager.ChangeStateAsync(GameStates.Battle);
        }
    }
}
