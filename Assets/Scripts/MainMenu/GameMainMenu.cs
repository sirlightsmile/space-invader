using System;
using System.Threading.Tasks;
using SmileProject.Generic.ResourceManagement;
using SmileProject.SpaceInvader.GameData;
using SmileProject.SpaceInvader.Gameplay.Input;
using SmileProject.SpaceInvader.MainMenu.UI;
using UnityEngine;

namespace SmileProject.SpaceInvader.MainMenu
{
    public class GameMainMenu : MonoBehaviour
    {
        public event Action ReadyForBattle;

        [SerializeField]
        private ScorePanel _scorePanel;

        private InputManager _inputManager;
        private GameDataManager _gamedataManager;
        private IResourceLoader _resourceLoader;

        public void Init(InputManager inputManager, GameDataManager gameDataManager, IResourceLoader resourceLoader)
        {
            _inputManager = inputManager;
            _gamedataManager = gameDataManager;
            _resourceLoader = resourceLoader;

            _inputManager.ConfirmInput += OnConfirmInput;
            _inputManager.SetAllowInput(true);
        }

        public async Task ShowScorePanel()
        {
            EnemySpaceshipModel[] models = _gamedataManager.GetEnemySpaceshipModels();
            ScorePanelModel[] panelModels = new ScorePanelModel[models.Length];
            for (int i = 0; i < models.Length; i++)
            {
                var model = models[i];
                panelModels[i] = new ScorePanelModel
                {
                    SpriteName = model.AssetName,
                    Score = model.Score
                };
            }
            Debug.Log("Show score panel" + panelModels.Length);
            await _scorePanel.Setup(panelModels, _resourceLoader);
        }

        private void OnConfirmInput()
        {
            _inputManager.ConfirmInput -= OnConfirmInput;
            ReadyForBattle?.Invoke();
            ReadyForBattle = null;
        }
    }
}
