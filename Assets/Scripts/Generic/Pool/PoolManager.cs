using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SmileProject.Generic
{
	public class PoolInfo
	{
		public readonly PoolOptions Options;
		public readonly Transform Container;
		public readonly List<PoolObject> PoolList;
		public readonly PoolObject Prefab;

		public PoolInfo(PoolOptions options, Transform container, PoolObject prefab)
		{
			this.Options = options;
			this.Container = container;
			this.Prefab = prefab;
			this.PoolList = new List<PoolObject>(options.InitialSize);
		}
	}

	public class PoolManager : MonoBehaviour
	{
		[SerializeField]
		private Transform _poolContainer;
		private Dictionary<string, PoolInfo> _poolInfoDict = new Dictionary<string, PoolInfo>();
		private IResourceLoader _resourceLoader;

		public void Initialize(IResourceLoader resourceLoader)
		{
			_resourceLoader = resourceLoader;
		}

		/// <summary>
		/// Get item from pool
		/// </summary>
		/// <returns>Pool object from pool</returns>
		/// <typeparam name="T">T inherited PoolObject</typeparam>
		/// <returns></returns>
		public T GetItem<T>(string poolName) where T : PoolObject
		{
			PoolInfo poolInfo = GetPoolInfo(poolName);
			if (poolInfo == null)
			{
				return null;
			}

			List<PoolObject> poolObjectList = poolInfo.PoolList;
			int index = poolObjectList.FindIndex(obj => !obj.IsActive);
			if (index == -1)
			{
				int newIndex = poolObjectList.Count;
				bool resizeSucceed = Resize(poolInfo);
				if (resizeSucceed)
				{
					// index of first item which been added
					index = newIndex;
				}
			}
			PoolObject poolObject = poolObjectList[index];
			poolObject.SetParent(null);
			poolObject.OnSpawn();
			return poolObject as T;
		}

		/// <summary>
		/// Return item to pool or destroy it if pool not found
		/// </summary>
		/// <param name="poolObj">Pool object to be return</param>
		/// <typeparam name="T">T inherited PoolObject</typeparam>
		public void ReturnItem<T>(string poolName, T poolObj) where T : PoolObject
		{
			PoolInfo poolInfo = GetPoolInfo(poolName);
			Transform container = poolInfo?.Container;
			if (poolInfo != null)
			{
				poolObj.SetParent(container);
				poolObj.OnDespawn();
			}
			else
			{
				// pool already destroyed
				Destroy(poolObj.gameObject);
			}
		}

		/// <summary>
		/// Create new pool on pool manager
		/// </summary>
		/// <param name="options">Pool options</param>
		/// <typeparam name="T">T inherited PoolObject</typeparam>
		public async Task CreatePool<T>(PoolOptions options) where T : PoolObject
		{
			string poolName = options.PoolName;
			if (HasPool(poolName))
			{
				Debug.LogAssertion($"Pool name [{poolName}] already exist.");
				return;
			}

			// prevent duplicate while creating
			_poolInfoDict.Add(poolName, null);

			GameObject container = new GameObject(poolName);
			container.transform.SetParent(_poolContainer);
			string assetKey = options.AssetKey;
			T poolObject = await _resourceLoader.LoadPrefab<T>(assetKey);
			_resourceLoader.Release(poolObject.gameObject);
			PoolInfo poolInfo = new PoolInfo(options, container.transform, poolObject);
			_poolInfoDict[poolName] = poolInfo;
			AddObjectsToPool(poolInfo, options.InitialSize);
		}

		/// <summary>
		/// Destroy pool from pool manager
		/// </summary>
		/// <param name="poolName">Pool name to destroy</param>
		public void DestroyPool(string poolName)
		{
			PoolInfo poolInfo = GetPoolInfo(poolName);
			if (poolInfo == null)
			{
				Debug.LogAssertion($"Pool name [{poolName}] not exist.");
				return;
			}
			Destroy(poolInfo.Container.gameObject);
			_poolInfoDict.Remove(poolName);
			Debug.Log($"Destroy pool : [{poolName}]");
		}

		/// <summary>
		/// Check whether pool manager contain this pool name or not
		/// /// </summary>
		/// <param name="poolName">target pool name</param>
		/// <returns></returns>
		public bool HasPool(string poolName)
		{
			return _poolInfoDict.ContainsKey(poolName);
		}

		private void AddObjectsToPool(PoolInfo poolInfo, int extendAmount)
		{
			for (int i = 0; i < extendAmount; i++)
			{
				AddObjectToPoolAsync(poolInfo);
			}
		}

		private void AddObjectToPoolAsync(PoolInfo poolInfo)
		{
			PoolOptions options = poolInfo.Options;
			PoolObject poolObj = GameObject.Instantiate<PoolObject>(poolInfo.Prefab, _poolContainer);
			Transform container = poolInfo.Container;
			if (container)
			{
				poolObj.SetParent(container);
			}
			poolObj.SetPool(options.PoolName, this);
			poolObj.SetActive(false);
			poolInfo.PoolList.Add(poolObj);
		}

		private bool Resize(PoolInfo poolInfo)
		{
			PoolOptions options = poolInfo.Options;
			if (!options.CanExtend || options.ExtendAmount < 1)
			{
				return false;
			}

			int extendSize = options.ExtendAmount;
			AddObjectsToPool(poolInfo, extendSize);
			return true;
		}

		private PoolInfo GetPoolInfo(string poolName)
		{
			PoolInfo poolInfo;
			_poolInfoDict.TryGetValue(poolName, out poolInfo);
			if (poolInfo == null)
			{
				Debug.Log($"Pool info name {poolName} not found");
				return null;
			}
			return poolInfo;
		}
	}
}
