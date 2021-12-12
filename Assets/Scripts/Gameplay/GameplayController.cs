using System;
using System.Threading.Tasks;
using SmileProject.Generic.Audio;
using SmileProject.Generic.Utilities;
using SmileProject.SpaceInvader.Gameplay.Enemy;
using SmileProject.SpaceInvader.Gameplay.Input;
using SmileProject.SpaceInvader.Gameplay.Player;
using SmileProject.SpaceInvader.Gameplay.UI;
using SmileProject.SpaceInvader.Sounds;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SmileProject.SpaceInvader.Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        /// <summary>
        /// Invoke when game started
        /// </summary>
        public event Action Start;

        /// <summary>
        /// Invoke when game pause or resume
        /// </summary>
        public event Action<bool> Pause;

        /// <summary>
        /// Event invoke when wave changed
        /// </summary>
        public event Action WaveChange;

        /// <summary>
        /// Is Game pause
        /// </summary>
        /// <value></value>
        public bool IsPause { get; private set; } = true;

        /// <summary>
        /// Game timer. Start counting on GameStart and stop when pause or game over.
        /// </summary>
        /// <value></value>
        public float Timer { get; private set; }

        /// <summary>
        /// Game total time. Can set from Game Config
        /// </summary>
        /// <value></value>
        public float TotalTime { get; private set; }

        /// <summary>
        /// Constant time (ms) before each wave start
        /// </summary>
        private const int WaveInterval = 500;

        [SerializeField]
        private Vector2 _playerSpawnPoint;

        private PlayerController _playerController;
        private EnemyManager _enemyManager;
        private InputManager _inputManager;
        private AudioManager _audioManager;
        private GameplayUIManager _uiManager;

        private int _currentWave, _waveCount = 0;
        private bool _isGameEnded, _isGameStarted = false;

        /// <summary>
        /// Initialize gameplay controller
        /// </summary>
        public async Task Initialize(PlayerController playerController, EnemyManager enemyManager, InputManager inputManager, AudioManager audioManager, GameplayUIManager uiManager)
        {
            _playerController = playerController;
            _inputManager = inputManager;
            _audioManager = audioManager;
            _enemyManager = enemyManager;
            _uiManager = uiManager;

            // setup listener
            inputManager.ConfirmInput += OnPressConfirm;
            inputManager.MenuInput += () => { SetGamePause(!IsPause); };
            playerController.PlayerDestroyed += OnPlayerDestroyed;
            playerController.PlayerGetHit += OnPlayerGetHit;
            enemyManager.EnemyDestroyed += OnEnemyDestroyed;
            enemyManager.AllSpaceshipDestroyed += OnWaveClear;
            enemyManager.EnemyReadyStatusChanged += OnEnemyReadyStatusChanged;

            await playerController.CreatePlayer(_playerSpawnPoint);
            uiManager.SetPlayerHp(playerController.PlayerSpaceship.HP);
        }

        public void StandBy()
        {
            _uiManager.SetShowGameStart(true);
        }

        public void GameStart()
        {
            Timer = 0;
            //TODO: calculate total time
            TotalTime = 0;
            IsPause = false;
            _currentWave = 0;
            _isGameEnded = false;
            _isGameStarted = true;
            _uiManager.SetShowGameStart(false);
            Start?.Invoke();
            PlayGameplayBGM();
            var _ = NextWave();
            Debug.Log("Game Started");
        }

        private void OnPressConfirm()
        {
            if (!_isGameStarted)
            {
                GameStart();
            }
            else if (_isGameEnded)
            {
                ResetGame();
            }
        }

        private void ResetGame()
        {
            Debug.Log("Reset scene");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void SetGamePause(bool isPause)
        {
            IsPause = isPause;
            Pause?.Invoke(isPause);
            _uiManager.SetGameplayMenu(isPause);
            Time.timeScale = isPause ? 0f : 1f;
        }

        private void ClearGame()
        {
            GameEnd();
            _uiManager.ShowGameClear(_playerController.TotalScore);
            SafeInvoke.InvokeAsync(async () => await _audioManager.PlaySound(GameSoundKeys.Succeed));
        }

        private void GameOver()
        {
            GameEnd();
            // TODO: show game clear too
            _uiManager.ShowGameOver();
            SafeInvoke.InvokeAsync(async () => await _audioManager.PlaySound(GameSoundKeys.Failed));
        }

        private void GameEnd()
        {
            _isGameEnded = true;
            CalculateExtraScore();
            IsPause = true;
        }

        private void PlayGameplayBGM()
        {
            SafeInvoke.InvokeAsync(async () => await _audioManager.PlaySound(GameSoundKeys.GameplayBGM, true));
        }

        private void OnEnemyDestroyed(int score)
        {
            _playerController.AddKillScore(score);
            _uiManager.SetPlayerScore(_playerController.KillScore);
        }

        private void OnEnemyReadyStatusChanged(bool isReady)
        {
            _inputManager.SetAllowAttack(isReady);
        }

        private void OnPlayerGetHit(int hp)
        {
            _uiManager.SetPlayerHp(hp);
        }

        private async Task NextWave()
        {
            _uiManager.ShowWaveChange(_currentWave + 1, WaveInterval);
            await Task.Delay(WaveInterval);
            _currentWave++;
            WaveChange?.Invoke();
        }

        private void OnWaveClear()
        {
            // wait for next wave generate
            _inputManager.SetAllowAttack(false);
            if (_waveCount > _currentWave)
            {
                var _ = NextWave();
            }
            else
            {
                ClearGame();
            }
        }

        private void OnPlayerDestroyed()
        {
            GameOver();
        }

        private void CalculateExtraScore()
        {
            float scoreRatio = Timer / TotalTime;
            // TODO: Config later
            int timeBonus = 10;
            int timerScore = Mathf.FloorToInt(timeBonus * scoreRatio);
            _playerController.SetTimerScore(timerScore);
        }

        private void Update()
        {
            if (IsPause || _isGameEnded)
            {
                return;
            }

            _enemyManager.Update();
            Timer += Time.time;
        }
    }
}