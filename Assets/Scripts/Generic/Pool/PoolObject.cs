using UnityEngine;

namespace SmileProject.Generic
{
	public abstract class PoolObject : MonoBehaviour
	{
		public bool IsActive { get { return gameObject?.activeInHierarchy ?? false; } }
		private string _poolName;
		private PoolManager _poolManager;

		/// <summary>
		/// Set pool name and pool manager as reference for return self
		/// </summary>
		/// <param name="poolName">Pool name</param>
		/// <param name="poolManager">Pool manager</param>
		public void SetPool(string poolName, PoolManager poolManager)
		{
			_poolName = poolName;
			_poolManager = poolManager;
		}

		/// <summary>
		/// Return self to pool manager
		/// </summary>
		public virtual void ReturnToPool()
		{
			SetActive(false);
			_poolManager.ReturnItem(_poolName, this);
		}

		public virtual void SetActive(bool isActive)
		{
			this.gameObject.SetActive(isActive);
		}

		public virtual void SetParent(Transform parent)
		{
			this.transform.SetParent(parent);
		}

		public abstract void OnSpawn();

		public abstract void OnDespawn();
	}
}