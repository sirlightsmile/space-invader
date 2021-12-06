namespace SmileProject.Generic
{
	[System.Serializable]
	public class PoolOptions
	{
		/// <summary>
		/// Asset key
		/// </summary>
		public string AssetKey { get; set; }

		/// <summary>
		/// Pool name
		/// </summary>
		public string PoolName { get; set; }

		/// <summary>
		/// Number of initial pool objects in pool when create
		/// </summary>
		public int InitialSize { get; set; } = 2;

		/// <summary>
		/// Whether this pool can extends when call 'spawn' over than initial size limit
		/// </summary>
		public bool CanExtend { get; set; } = true;

		/// <summary>
		/// Amount of items that which will increase when resize pool after reach limit
		/// </summary>
		public int ExtendAmount { get; set; } = 2;
	}
}