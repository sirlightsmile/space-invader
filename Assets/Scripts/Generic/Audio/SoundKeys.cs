using System.Collections.Generic;

namespace SmileProject.Generic
{
	public abstract class SoundKeys : StringEnum<SoundKeys>
	{
		protected static List<SoundKeys> _soundList = new List<SoundKeys>();
		protected readonly string _assetKey;
		protected readonly string _mixerKey;
		protected SoundKeys(string value, string assetKey, string mixerKey) : base(value)
		{
			_assetKey = assetKey;
			_mixerKey = mixerKey;
			_soundList.Add(this);
		}

		public virtual string GetAssetKey()
		{
			return _assetKey;
		}

		public virtual string GetMixerKey()
		{
			return _mixerKey;
		}

		public virtual IEnumerable<SoundKeys> GetAll()
		{
			return _soundList;
		}
	}
}