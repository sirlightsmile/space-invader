using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SmileProject.Generic.GameState
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
    }
}
