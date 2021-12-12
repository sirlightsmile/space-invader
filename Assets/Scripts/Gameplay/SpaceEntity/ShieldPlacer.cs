using System.Collections.Generic;
using System.Threading.Tasks;
using SmileProject.Generic.Audio;
using SmileProject.Generic.ResourceManagement;
using SmileProject.SpaceInvader.Sounds;
using UnityEngine;

namespace SmileProject.SpaceInvader.Gameplay
{
    public class ShieldPlacer
    {
        private const string PrefabKey = "ShieldPrefab";
        private const int TotalShield = 6;
        private IResourceLoader _resourceLoader;
        private AudioManager _audioManager;

        public ShieldPlacer(IResourceLoader resourceLoader, AudioManager audioManager)
        {
            _resourceLoader = resourceLoader;
            _audioManager = audioManager;
        }

        public async Task PlaceShields()
        {
            int screenIntervalX = Screen.width / TotalShield;
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < TotalShield; i++)
            {
                bool isFlip = i % 2 == 0;
                int intervalIndex = isFlip ? i + 1 : i;
                tasks.Add(PlaceShield(screenIntervalX * intervalIndex, isFlip));
            }
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Place pair of shield at target position
        /// </summary>
        /// <param name="screenPosX"></param>
        /// <returns></returns>
        private async Task PlaceShield(float screenPosX, bool isFlip)
        {
            Debug.Log("screenPosX" + screenPosX);
            Shield shield = await _resourceLoader.InstantiateAsync<Shield>(PrefabKey);
            float targetScreenX = isFlip ? screenPosX - (shield.Width / 2) : screenPosX + (shield.Width / 2);
            float posX = Camera.main.ScreenToWorldPoint(new Vector2(targetScreenX, 0)).x;
            Vector2 targetPosition = new Vector2(posX, shield.transform.position.y);
            //TODO: get from config
            shield.SetDurability(5).SetSpriteFlipX(isFlip).SetPosition(targetPosition);
            shield.SetSounds(_audioManager, GameSoundKeys.Hit, GameSoundKeys.Explosion);
            shield.Destroyed += OnObstacleDestroy;
        }

        private void OnObstacleDestroy(Shield shield)
        {
            shield.Destroyed -= OnObstacleDestroy;
            _resourceLoader.ReleaseInstance(shield.gameObject);
        }
    }
}