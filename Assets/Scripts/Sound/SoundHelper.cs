
using System;
using System.Threading.Tasks;
using SmileProject.Generic.Audio;
using UnityEngine;

namespace SmileProject.SpaceInvader.Sounds
{
    public static class SoundHelper
    {
        /// <summary>
        /// Try play sound without exception even sound key is not assigned
        /// </summary>
        /// <param name="soundKey"></param>
        /// <param name="audioManager"></param>
        /// <returns></returns>
        public static async Task<int> PlaySound(SoundKeys soundKey, AudioManager audioManager)
        {
            try
            {
                if (audioManager != null && soundKey != null)
                {
                    return await audioManager.PlaySound(soundKey);
                }
                Debug.Assert(audioManager != null, "Audio manager should not be null when trying to play sound");
                Debug.Assert(soundKey != null, "Sound key should not be null when trying to play sound");
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
            return -1;
        }
    }
}