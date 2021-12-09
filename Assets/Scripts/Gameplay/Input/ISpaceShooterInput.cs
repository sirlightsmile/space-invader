using System;
using SmileProject.SpaceInvader.Constant;

namespace SmileProject.SpaceInvader.Gameplay.Input
{
    public interface ISpaceShooterInput
    {
        /// <summary>
        /// Invoke when got horizontal input
        /// </summary>
        event Action<MoveDirection> HorizontalInput;

        /// <summary>
        /// Invoke when got attack input
        /// </summary>
        event Action AttackInput;

        /// <summary>
        /// Invoke when got menu call input
        /// </summary>
        event Action MenuInput;

        /// <summary>
        /// Invoke when got confirm action input
        /// </summary>
        event Action ConfirmInput;

        void SetAllowInput(bool isAllowInput);

        void SetAllowAttack(bool isAllowAttack);
    }
}
