using System;
using SmileProject.SpaceInvader.Constant;
using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay.Input
{
    public class InputManager : MonoBehaviour, ISpaceShooterInput
    {
        public event Action<MoveDirection> HorizontalInput;

        public event Action AttackInput;

        public event Action MenuInput;

        public event Action ConfirmInput;

        private bool _allowInput = true;
        private bool _allowAttack = true;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// Set permission for every input.
        /// </summary>
        /// <param name="allowInput">whether allow input or not</param>
        public void SetAllowInput(bool allowInput)
        {
            _allowInput = allowInput;
        }

        /// <summary>
        /// Set permission for attack input
        /// </summary>
        /// <param name="allowAttack">whether allow attack input or not</param>
        public void SetAllowAttack(bool allowAttack)
        {
            _allowAttack = allowAttack;
        }

        private void Update()
        {
            if (!_allowInput)
            {
                return;
            }

            float axisRaw = UnityEngine.Input.GetAxisRaw("Horizontal");
            if (axisRaw != 0)
            {
                MoveDirection direction = axisRaw < 0 ? MoveDirection.Left : MoveDirection.Right;
                HorizontalInput?.Invoke(direction);
            }

            if (_allowAttack)
            {
                if (UnityEngine.Input.GetButton("Fire1"))
                {
                    AttackInput?.Invoke();
                }
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                MenuInput?.Invoke();
            }

            if (UnityEngine.Input.GetButtonDown("Submit"))
            {
                ConfirmInput?.Invoke();
            }
        }
    }
}
