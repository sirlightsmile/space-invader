using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SmileProject.Generic
{
	public delegate void SetSpriteHandler(Sprite sprite);
	public interface IResourceLoader
	{
		Task InitializeAsync();
		Task Preload(IEnumerable<string> assetKeys);
		Task<T> Load<T>(string key);
		Task<T> LoadPrefab<T>(string key);
		Task<T> LoadJsonAsModel<T>(string key);
		Task<T> InstantiateAsync<T>(object key, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true) where T : MonoBehaviour;
		void Release(System.Object key);
		void ReleaseInstance(GameObject obj);
		Task SetSpriteAsync(string key, SetSpriteHandler eventHandler);
	}
}