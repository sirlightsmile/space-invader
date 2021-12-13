using System.Collections.Generic;
using System.Threading.Tasks;
using SmileProject.Generic.ResourceManagement;
using SmileProject.SpaceInvader.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace SmileProject.SpaceInvader.MainMenu.UI
{
    public struct ScorePanelModel
    {
        public string SpriteName;
        public int Score;
    }

    public class ScorePanel : BaseUIComponent
    {
        [SerializeField]
        private Transform _container;

        [SerializeField]
        private VerticalLayoutGroup _layoutGroup;

        [SerializeField]
        private AssetReference _scorePanelItem;

        private List<ScoreItem> _scoreItems = new List<ScoreItem>();
        private IResourceLoader _resourceLoader;

        public async Task Setup(ScorePanelModel[] models, IResourceLoader resourceLoader)
        {
            _resourceLoader = resourceLoader;
            _layoutGroup.enabled = true;

            int count = models.Length;
            ScoreItem[] scoreItems = new ScoreItem[count];
            for (int i = 0; i < count; i++)
            {
                await AddScoreItem(models[i]);
            }
            _layoutGroup.enabled = false;
        }

        private async Task<ScoreItem> AddScoreItem(ScorePanelModel model)
        {
            Debug.Log("Add score item");
            GameObject obj = await _scorePanelItem.InstantiateAsync(_container, false).Task;
            ScoreItem item = obj.GetComponent<ScoreItem>();
            Sprite sprite = await _resourceLoader.Load<Sprite>(model.SpriteName);
            item.Setup(sprite, model.Score);
            return item;
        }
    }
}