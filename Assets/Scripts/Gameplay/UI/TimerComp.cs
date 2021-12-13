using SmileProject.SpaceInvader.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SmileProject.SpaceInvader.Gameplay.UI
{
    public class TimerComp : BaseUIComponent
    {
        [SerializeField]
        private Text timeText;

        /// <summary>
        /// Set time left text
        /// </summary>
        /// <param name="timeLeft"></param>
        public void SetTimeText(int timeLeft)
        {
            timeText.text = $"Time Left : {timeLeft}";
        }
    }
}