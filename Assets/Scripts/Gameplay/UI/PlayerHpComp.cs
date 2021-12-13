using System;
using SmileProject.SpaceInvader.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SmileProject.SpaceInvader.Gameplay.UI
{
    public class PlayerHpComp : BaseUIComponent
    {
        [SerializeField]
        private Text _playerHpText;

        public void SetPlayerHp(int hp)
        {
            _playerHpText.text = $"HP : {hp.ToString()}";
        }
    }
}