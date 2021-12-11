using System;
using System.Threading.Tasks;
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

        /// <summary>
        /// Invoke specific async function without any returning result
        /// any exception will log to error tracker without crashing the game
        /// </summary>
        /// <param name="function"></param>
        public static void InvokeAsync(Func<Task> function)
        {
            try
            {
                function();
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
        }
    }
}
