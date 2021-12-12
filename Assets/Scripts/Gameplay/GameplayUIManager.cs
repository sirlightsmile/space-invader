using SmileProject.SpaceInvader.Gameplay;
using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay.UI
{
    public class GameplayUIManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerScoreComp _playerScoreComp;

        [SerializeField]
        private PlayerHpComp _playerHpComp;

        [SerializeField]
        private WaveChangeComp _waveChangComp;

        [SerializeField]
        private GameStartComp _gameStartComp;

        [SerializeField]
        private GameEndComp _gameEndComp;

        [SerializeField]
        private GameplayMenuComp _gameplayMenu;

        private GameplayController _gameplayController;

        public void Init(GameplayController gameplayController)
        {
            _gameplayController = gameplayController;
        }

        public void ShowGameClear(int score)
        {
            _gameEndComp.Show($"Clear!\nYour score : {score}");
        }

        public void ShowGameOver(int score)
        {
            _gameEndComp.Show($"Game Over\nYour score : {score}");
        }

        public void ShowWaveChange(int waveNumber, int showTime)
        {
            _waveChangComp.ShowWave(waveNumber, showTime);
        }

        public void SetGameplayMenu(bool isShow)
        {
            if (isShow)
            {
                _gameplayMenu.Show();
            }
            else
            {
                _gameplayMenu.Hide(true);
            }
        }

        public void SetShowPlayerInfo(bool isShow)
        {
            if (isShow)
            {
                _playerScoreComp.Show();
                _playerHpComp.Show();
            }
            else
            {
                _playerScoreComp.Hide();
                _playerHpComp.Hide();
            }
        }

        public void SetShowGameStart(bool isShow)
        {
            if (isShow)
            {
                _gameStartComp.Show();
            }
            else
            {
                _gameStartComp.Hide();
            }
        }

        public void SetPlayerScore(int score)
        {
            _playerScoreComp.SetPlayerScore(score);
        }

        public void SetPlayerHp(int hp)
        {
            _playerHpComp.SetPlayerHp(hp);
        }
    }
}