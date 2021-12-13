using System;
using System.Threading.Tasks;
using SmileProject.Generic.Audio;
using SmileProject.Generic.Pooling;
using SmileProject.Generic.Utilities;
using SmileProject.SpaceInvader.Config;
using SmileProject.SpaceInvader.Gameplay.Enemy;
using SmileProject.SpaceInvader.Gameplay.Input;
using SmileProject.SpaceInvader.Gameplay.Player;
using SmileProject.SpaceInvader.Gameplay.UI;
using SmileProject.SpaceInvader.Sounds;
using UnityEngine;

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
        /// Invoke when game end and attemp to reset
        /// </summary>
        public event Action ResetGame;

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
        public int TotalTime { get; private set; }

        /// <summary>
        /// Current time left from total time
        /// </summary>
        /// <value></value>
        public float CurrentTime { get { return Mathf.Clamp(TotalTime - Timer, 0, TotalTime); } }

        public PlayerController PlayerController { get; private set; }

        /// <summary>
        /// Constant time (ms) before each wave start
        /// </summary>
        private const int WaveInterval = 500;

        [SerializeField]
        private Vector2 _playerSpawnPoint;

        private EnemyManager _enemyManager;
        private InputManager _inputManager;
        private AudioManager _audioManager;
        private GameplayUIManager _uiManager;
        private PoolManager _poolManager;
        private ShieldPlacer _shieldPlacer;
        private GameConfig _gameConfig;

        private int _extraBonusScore = 0;
        private bool _isGameEnded, _isGameStarted = false;
        private float _startTime;
        private int _bgmSoundPlayId = -1;

        /// <summary>
        /// Initialize gameplay controller
        /// </summary>
        public void Initialize(
            PlayerController playerController,
            EnemyManager enemyManager,
            InputManager inputManager,
            AudioManager audioManager,
            GameplayUIManager uiManager,
            ShieldPlacer shieldPlacer,
            GameConfig gameConfig,
            PoolManager poolManager
        )
        {
            PlayerController = playerController;
            _inputManager = inputManager;
            _audioManager = audioManager;
            _enemyManager = enemyManager;
            _uiManager = uiManager;
            _shieldPlacer = shieldPlacer;
            _gameConfig = gameConfig;
            _poolManager = poolManager;

            AddListeners();
            ApplyConfig(gameConfig);
            _uiManager.Init(this);
        }

        public void ApplyConfig(GameConfig gameConfig)
        {
            PlayerController.ApplyPlayerConfig(gameConfig.PlayerConfig);
            _enemyManager.ApplyEnemyConfig(gameConfig.EnemyConfig);
            _extraBonusScore = gameConfig.ExtraBonusScore;
            TotalTime = gameConfig.TotalTime;
        }

        public async Task GameStart()
        {
            Timer = 0;
            _isGameEnded = false;
            _isGameStarted = true;
            Start?.Invoke();
            _inputManager.SetAllowAttack(false);
            await Task.WhenAll
            (
                new Task[]
                {
                    PlayGameplayBGM(),
                    _enemyManager.GenerateEnemies(),
                    _shieldPlacer.PlaceShields(_gameConfig.ShieldDurability),
                    PlayerController.CreatePlayer(_playerSpawnPoint)
                }
            );
            _uiManager.SetPlayerHp(PlayerController.PlayerSpaceship.HP);
            _inputManager.SetAllowAttack(true);
            _startTime = Time.time;
            IsPause = false;
            Debug.Log("Game Started | Total time : " + TotalTime);
        }

        private async Task PlayGameplayBGM()
        {
            _bgmSoundPlayId = await SoundHelper.PlaySound(GameSoundKeys.GameplayBGM, _audioManager, true);
        }

        /// <summary>
        /// Handle press confirm in gameplay scene
        /// </summary>
        private void OnPressConfirm()
        {
            if (!_isGameStarted)
            {
                var _ = GameStart();
            }
            else if (_isGameEnded)
            {
                ResetGame?.Invoke();
                CleanUp();
            }
        }

        /// <summary>
        /// Set pause status
        /// </summary>
        /// <param name="isPause"></param>
        public void SetGamePause(bool isPause)
        {
            IsPause = isPause;
            Pause?.Invoke(isPause);
            Time.timeScale = isPause ? 0f : 1f;
        }

        private void TriggerGamePause()
        {
            SetGamePause(!IsPause);
        }

        /// <summary>
        /// Invoke when game end cause by all enemies dead
        /// </summary>
        private void ClearGame()
        {
            Debug.Log("Game Clear");
            OnGameEnd();
            _uiManager.ShowGameClear(GetTotalScore());
            SafeInvoke.InvokeAsync(async () => await _audioManager.PlaySound(GameSoundKeys.Succeed));
        }

        /// <summary>
        /// Invoke when game end cause by player dead
        /// </summary>
        private void GameOver()
        {
            Debug.Log("Game Over");
            OnGameEnd();
            _uiManager.ShowGameOver(PlayerController.KillScore);
            SafeInvoke.InvokeAsync(async () => await _audioManager.PlaySound(GameSoundKeys.Failed));
        }

        /// <summary>
        /// Invoke when game end, unconditionally
        /// </summary>
        private void OnGameEnd()
        {
            _isGameEnded = true;
            IsPause = true;
        }

        /// <summary>
        /// Handle enemy destroy. Add score to player
        /// </summary>
        /// <param name="score"></param>
        private void OnEnemyDestroyed(int score)
        {
            PlayerController.AddKillScore(score);
            _uiManager.SetPlayerScore(PlayerController.KillScore);
        }

        /// <summary>
        /// Get player extra bonus score from time left
        /// </summary>
        /// <returns></returns>
        private int GetExtraBonusScore()
        {
            float scoreRatio = CurrentTime / TotalTime;
            int timerScore = Mathf.FloorToInt(_extraBonusScore * scoreRatio);
            return timerScore;
        }

        /// <summary>
        /// Get player total score
        /// </summary>
        /// <returns></returns>
        private int GetTotalScore()
        {
            int bonus = GetExtraBonusScore();
            int killScore = PlayerController.KillScore;
            return bonus + killScore;
        }

        private void Update()
        {
            if (IsPause || _isGameEnded)
            {
                return;
            }

            _enemyManager.Update();
            Timer = Time.time - _startTime;
            _uiManager.UpdateTimerUI(CurrentTime);

            if (Timer > TotalTime)
            {
                // game over cause by timeout
                GameOver();
                Debug.Log("Time out");
            }
        }

        private void AddListeners()
        {
            _inputManager.ConfirmInput += OnPressConfirm;
            _inputManager.MenuInput += TriggerGamePause;
            _enemyManager.AllSpaceshipDestroyed += ClearGame;
            _enemyManager.EnemyDestroyed += OnEnemyDestroyed;
            PlayerController.PlayerDestroyed += GameOver;
        }

        private void RemoveAllListeners()
        {
            _inputManager.ConfirmInput -= OnPressConfirm;
            _inputManager.MenuInput -= TriggerGamePause;
            _enemyManager.AllSpaceshipDestroyed -= ClearGame;
            _enemyManager.EnemyDestroyed -= OnEnemyDestroyed;
            PlayerController.PlayerDestroyed -= GameOver;
        }

        /// <summary>
        /// Clean up assets in game scene to free memories
        /// </summary>
        private void CleanUp()
        {
            if (_bgmSoundPlayId > -1)
            {
                _audioManager.StopSound(_bgmSoundPlayId);
            }
            Start = null;
            Pause = null;
            ResetGame = null;
            _audioManager.CleanAll();
            _poolManager.DestroyAllPool();
            RemoveAllListeners();
        }
    }
}