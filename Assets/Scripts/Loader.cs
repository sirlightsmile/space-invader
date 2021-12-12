using System;
using System.Threading.Tasks;
using Configs;
using SmileProject.Generic.Audio;
using SmileProject.Generic.GameState;
using SmileProject.Generic.Pooling;
using SmileProject.Generic.ResourceManagement;
using SmileProject.SpaceInvader.GameData;
using SmileProject.SpaceInvader.Gameplay;
using SmileProject.SpaceInvader.Gameplay.Enemy;
using SmileProject.SpaceInvader.Gameplay.Input;
using SmileProject.SpaceInvader.Gameplay.Player;
using SmileProject.SpaceInvader.Gameplay.UI;
using SmileProject.SpaceInvader.Sounds;
using SmileProject.SpaceInvader.Weapon;
using UnityEngine;

namespace SmileProject.SpaceInvader
{
    public class Loader : MonoBehaviour
    {
        public bool IsInitialized { get; private set; }

        private GameDataManager _gameDataManager;
        private GameStateManager _gameStateManager;
        private AddressableResourceLoader _resourceLoader;
        private GameController _gameController;
        private GameplayController _gameplayController;
        private PoolManager _poolManager;
        private AudioManager _audioManager;

        private void Start()
        {
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
                InitPoolManager(_resourceLoader),
                InitAudioManager(_resourceLoader),

            });
            await InitGameplayController(_resourceLoader, _gameDataManager, _poolManager, _audioManager);

            IsInitialized = true;
        }

        public async Task InitGameplayController(IResourceLoader resourceLoader, GameDataManager gameDataManager, PoolManager poolManager, AudioManager audioManager)
        {
            // init async
            GameplayController gameplayController = null;
            GameplayUIManager uiManager = null;
            InputManager inputManager = null;
            EnemyFormationController enemyFormationController = null;
            Func<Task> initInputManager = async () => { inputManager = await resourceLoader.InstantiateAsync<InputManager>("InputManager"); };
            Func<Task> initFormationController = async () => { enemyFormationController = await resourceLoader.InstantiateAsync<EnemyFormationController>("EnemyFormationController"); };
            Func<Task> initGameController = async () => { gameplayController = await resourceLoader.InstantiateAsync<GameplayController>("GameplayController"); };
            Func<Task> initGameplayUIManager = async () => { uiManager = await resourceLoader.InstantiateAsync<GameplayUIManager>("GameplayUIManager"); };
            await Task.WhenAll(new Task[] { initInputManager(), initGameController(), initGameplayUIManager(), initFormationController() });

            WeaponFactory weaponFactory = new WeaponFactory(gameDataManager, poolManager, audioManager);

            // inject player controller
            PlayerSpaceshipBuilder playerBuilder = new PlayerSpaceshipBuilder(resourceLoader, gameDataManager, weaponFactory, audioManager);
            PlayerController playerController = new PlayerController(inputManager, playerBuilder);

            // inject enemy manager
            EnemySpaceshipBuilder enemiesBuilder = new EnemySpaceshipBuilder(resourceLoader, gameDataManager, weaponFactory, audioManager);
            enemyFormationController.Initialize(enemiesBuilder, gameDataManager);

            // enemyFormationController.Initialize(gameDataManager, enemiesBuilder);
            EnemyManager enemyManager = new EnemyManager(enemyFormationController);

            await Task.WhenAll
            (
                new Task[]
                {
                    gameplayController.Initialize(playerController, enemyManager, inputManager, audioManager, uiManager),
                    // TODO: adjust size
                    enemiesBuilder.SetupSpaceshipPool(poolManager, 20)
                }
            );
            _gameplayController = gameplayController;
            _gameplayController.WaveChange += enemyFormationController.OnWaveChanged;
            _gameplayController.GameStart();
        }


        private async Task InitGameStateManager(IResourceLoader resourceLoader, GameplayController gameplayController)
        {
            GameStateManager gameStateManager = await resourceLoader.InstantiateAsync<GameStateManager>("GameStateManager");
            _gameStateManager = gameStateManager;
            BaseGameState[] states = new BaseGameState[] {
                new MenuState(GameStates.MainMenu, "MainMenu", gameplayController),
                new BattleState(GameStates.Battle, "Battle", gameplayController),
                new PauseState(GameStates.Pause, "Pause", gameplayController),
            };
            await _gameStateManager.Init(states, GameStates.MainMenu);
        }

        private async Task InitPoolManager(IResourceLoader resourceLoader)
        {
            PoolManager poolManager = await resourceLoader.InstantiateAsync<PoolManager>("PoolManager");
            _poolManager = poolManager;
            _poolManager.Initialize(resourceLoader);
        }

        private async Task InitAudioManager(IResourceLoader resourceLoader)
        {
            AudioManager audioManager = await resourceLoader.InstantiateAsync<AudioManager>("AudioManager");
            _audioManager = audioManager;
            await _audioManager.Initialize(resourceLoader, MixerGroup.MainMixerKey);
        }
    }
}