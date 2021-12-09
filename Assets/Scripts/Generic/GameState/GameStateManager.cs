using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SmileProject.Generic.GameState
{
    public class GameStateManager : MonoBehaviour
    {
        public delegate void StateChangedHandler(BaseGameState oldState, BaseGameState newState);
        public event StateChangedHandler StateChanged;
        public BaseGameState CurrentState { get; private set; }

        public int CurrentStateID
        {
            get
            {
                Debug.Assert(CurrentState != null, "Empty state is invalid");
                return CurrentState.ID;
            }
        }

        public bool IsInitialized { get; private set; }
        private Dictionary<int, BaseGameState> _states;
        private bool isStateChanging = false;

        public async Task Init(IEnumerable<BaseGameState> states, int initialStateID)
        {
            Debug.Assert(!IsInitialized, "Re-initialization not allowed");
            Debug.Assert(states != null, "Missing states");

            InitStates(states);
            await ChangeStateAsync(initialStateID);
            IsInitialized = true;

            Debug.Log("GameStateManager Initialized");
        }

        /// <summary>
        /// Transition to target state by state ID
        /// </summary>
        /// <param name="state"></param>
        /// <param name="isInitialState"></param>
        public async Task ChangeStateAsync(int stateID)
        {
            Debug.Assert(IsInitialized, "GameStateManager not initialized");
            Debug.Assert(_states.ContainsKey(stateID), "Invalid state: " + stateID);

            try
            {
                await ChangeStateAsync(_states[stateID]);
            }
            catch (Exception exception)
            {
                Debug.LogError(string.Format("StateChangedException : {0}", exception));
            }
        }

        private async Task ChangeStateAsync(BaseGameState newState)
        {
            Debug.Assert(newState != null, "Invalid target state");
            BaseGameState oldState = CurrentState;

            Debug.Log(string.Format("State Changing from {0} to {1}", oldState.Name, newState.Name));
            isStateChanging = true;
            await oldState.OnStateExit();
            CurrentState = newState;
            await newState.OnStateEnter();
            isStateChanging = false;
            Debug.Log(string.Format("State Changed from {0} to {1}", oldState.Name, newState.Name));

            StateChanged?.Invoke(oldState, CurrentState);
        }

        private void Update()
        {
            if (!isStateChanging)
            {
                CurrentState?.OnStateUpdate();
            }
        }

        private void InitStates(IEnumerable<BaseGameState> states)
        {
            _states = new Dictionary<int, BaseGameState>();

            foreach (BaseGameState state in states)
            {
                _states.Add(state.ID, state);
            }
            Debug.Log("States Initialized");
        }
    }
}
