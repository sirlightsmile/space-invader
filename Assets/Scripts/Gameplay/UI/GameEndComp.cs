using System;
using UnityEngine;
using UnityEngine.UI;

namespace SmileProject.SpaceInvader.Gameplay.UI
{
    public class GameEndComp : BaseUIComponent
    {
        [SerializeField]
        private Text _endText;

        /// <summary>
        /// Show end game ui with end title
        /// </summary>
        /// <param name="end title"></param>
        public void Show(string endTitle)
        {
            _endText.text = endTitle;
            Show();
        }
    }
}