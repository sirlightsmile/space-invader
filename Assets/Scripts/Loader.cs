using System;
using System.Threading.Tasks;
using Configs;
using SmileProject.Generic.Audio;
using SmileProject.Generic.Pooling;
using SmileProject.Generic.ResourceManagement;
using SmileProject.SpaceInvader.Config;
using SmileProject.SpaceInvader.GameData;
using SmileProject.SpaceInvader.Gameplay;
using SmileProject.SpaceInvader.Gameplay.Enemy;
using SmileProject.SpaceInvader.Gameplay.Input;
using SmileProject.SpaceInvader.Gameplay.Player;
using SmileProject.SpaceInvader.Gameplay.UI;
using SmileProject.SpaceInvader.MainMenu;
using SmileProject.SpaceInvader.Sounds;
using SmileProject.SpaceInvader.Weapon;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SmileProject.SpaceInvader
{
    public class Loader : MonoBehaviour
    {
        public bool IsInitialized { get; private set; }

        private GameDataManager _gameDataManager;
        private AddressableResourceLoader _resourceLoader;
        private GameplayController _gameplayController;
        private PoolManager _poolManager;
        private AudioManager _audioManager;
        private InputManager _inputManager;

        private void Start()
        {
            DontDestroyOnLoad(this);
            Initialize();
        }

        /// <summary>
        /// Async initialize. Call only once per session.
        /// </summary>
        /// <returns></returns>
        public async void Initialize()
        {
            Debug.Assert(!IsInitialized, "Re-initialization not allowed");

            _resourceLoader = new AddressableResourceLoader();
            _gameDataManager = new GameDataManager();
            await _resourceLoader.InitializeAsync();
            await Task.WhenAll(new Task[]
            {
                _gameDataManager.Initialize(_resourceLoader),
                InitAudioManager(_resourceLoader),
                InitInputManager(_resourceLoader)
            });
            IsInitialized = true;

            await LoadMainMenu();
        }

        public async Task LoadMainMenu()
        {
            SceneManager.LoadScene(GameScenes.MainMenu);
            var mainMenu = await _resourceLoader.InstantiateAsync<GameMainMenu>("MainMenu");
            mainMenu.Init(_inputManager, _gameDataManager, _resourceLoader);
            await mainMenu.ShowScorePanel();
        }

        public async Task LoadBattleScene()
        {
            SceneManager.LoadScene(GameScenes.Battle);
            await InitGameplayController(_resourceLoader, _gameDataManager, _audioManager);
        }

        private async Task InitGameplayController(IResourceLoader resourceLoader, GameDataManager gameDataManager, AudioManager audioManager)
        {
            // init async
            GameConfig gameConfig = null;
            PoolManager poolManager = null;
            GameplayUIManager uiManager = null;
            GameplayController gameplayController = null;
            EnemyFormationController enemyFormationController = null;
            Func<Task> loadGameConfig = async () => { gameConfig = await resourceLoader.Load<GameConfig>("GameConfig"); };
            Func<Task> initPoolManager = async () =>
            {
                poolManager = await resourceLoader.InstantiateAsync<PoolManager>("PoolManager");
                poolManager.Initialize(resourceLoader);
            };
            Func<Task> initFormationController = async () => { enemyFormationController = await resourceLoader.InstantiateAsync<EnemyFormationController>("EnemyFormationController"); };
            Func<Task> initGameController = async () => { gameplayController = await resourceLoader.InstantiateAsync<GameplayController>("GameplayController"); };
            Func<Task> initGameplayUIManager = async () => { uiManager = await resourceLoader.InstantiateAsync<GameplayUIManager>("GameplayUIManager"); };
            await Task.WhenAll(new Task[] { loadGameConfig(), initGameController(), initGameplayUIManager(), initFormationController(), initPoolManager() });

            WeaponFactory weaponFactory = new WeaponFactory(gameDataManager, poolManager, audioManager);

            // inject player controller
            PlayerSpaceshipBuilder playerBuilder = new PlayerSpaceshipBuilder(resourceLoader, gameDataManager, weaponFactory, audioManager);
            PlayerController playerController = new PlayerController(_inputManager, playerBuilder);

            // inject enemy manager
            EnemySpaceshipBuilder enemiesBuilder = new EnemySpaceshipBuilder(resourceLoader, gameDataManager, weaponFactory, audioManager);
            await enemiesBuilder.SetupSpaceshipPool(poolManager);
            enemyFormationController.Initialize(enemiesBuilder);
            EnemyManager enemyManager = new EnemyManager(enemyFormationController);

            ShieldPlacer shieldPlacer = new ShieldPlacer(resourceLoader, audioManager);
            gameplayController.Initialize(playerController, enemyManager, _inputManager, audioManager, uiManager, shieldPlacer, gameConfig);
            _gameplayController = gameplayController;
            _gameplayController.StandBy();
        }

        private async Task InitAudioManager(IResourceLoader resourceLoader)
        {
            AudioManager audioManager = await resourceLoader.InstantiateAsync<AudioManager>("AudioManager");
            _audioManager = audioManager;
            await _audioManager.Initialize(resourceLoader, MixerGroup.MainMixerKey);
        }

        private async Task InitInputManager(IResourceLoader resourceLoader)
        {
            InputManager inputManager = await resourceLoader.InstantiateAsync<InputManager>("InputManager");
            _inputManager = inputManager;
        }
    }
}