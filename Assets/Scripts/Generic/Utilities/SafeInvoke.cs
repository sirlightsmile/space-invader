using System;
using UnityEngine;

namespace SmileProject.Generic.Utilities
{
    public static class SafeInvoke
    {
        /// <summary>
        /// Invoke specific action without any returning result
        /// any exception will log to error tracker without crashing the game
        /// </summary>
        /// <param name="action"></param>
        public static void Invoke(Action action)
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
        }
    }
}
