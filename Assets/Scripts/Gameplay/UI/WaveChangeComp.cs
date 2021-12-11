using UnityEngine;
using UnityEngine.UI;

namespace SmileProject.SpaceInvader.Gameplay.UI
{
    public class WaveChangeComp : BaseUIComponent
    {
        [SerializeField]
        private Text _waveNumberText;

        /// <summary>
        /// Show wave change UI then auto hide
        /// </summary>
        /// <param name="waveNumber">number of wave</param>
        /// <param name="showTime">show time in milliseconds</param>
        /// <returns></returns>
        public void ShowWave(int waveNumber, int showTime)
        {
            _waveNumberText.text = $"Wave : {waveNumber}";
            Show(showTime);
        }
    }
}