using SmileProject.SpaceInvader.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SmileProject.SpaceInvader.MainMenu.UI
{
    public class ScoreItem : BaseUIComponent
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private Text _scoreText;

        public void Setup(Sprite icon, int score)
        {
            _icon.sprite = icon;
            _scoreText.text = string.Format("= {0} Score", score);
        }
    }
}