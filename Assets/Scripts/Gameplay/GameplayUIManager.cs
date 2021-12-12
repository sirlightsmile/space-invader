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
        private TimerComp _timerComp;

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
            _gameplayController.Start += OnGameStart;
            _gameplayController.Pause += OnGamePause;
            _gameplayController.PlayerController.PlayerGetHit += OnPlayerGetHit;
        }

        #region Event listener
        private void OnGameStart()
        {
            SetShowGameStart(false);
            SetShowGameUI(true);
        }

        private void OnGamePause(bool isPause)
        {
            SetGameplayMenu(isPause);
        }

        private void OnPlayerGetHit(int hp)
        {
            SetPlayerHp(hp);
        }
        #endregion

        public void UpdateTimerUI(float timeLeft)
        {
            int time = Mathf.FloorToInt(timeLeft);
            _timerComp.SetTimeText(time);
        }

        public void ShowGameClear(int score)
        {
            _gameEndComp.Show($"Clear!\nYour score : {score}");
        }

        public void ShowGameOver(int score)
        {
            _gameEndComp.Show($"Game Over\nYour score : {score}");
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

        public void SetShowGameUI(bool isShow)
        {
            if (isShow)
            {
                _playerScoreComp.Show();
                _playerHpComp.Show();
                _timerComp.Show();
            }
            else
            {
                _playerScoreComp.Hide();
                _playerHpComp.Hide();
                _timerComp.Hide();
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