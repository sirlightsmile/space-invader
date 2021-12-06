using System;

namespace SmileProject.Generic
{
	public abstract class StringEnum<T> where T : StringEnum<T>
	{
		public string Value { get; private set; }

		public StringEnum(string value)
		{
			this.Value = value;
		}

		public override string ToString()
		{
			return Value;
		}

		public override bool Equals(object other)
		{
			if (other is StringEnum<T> otherValue)
			{
				var typeMatches = GetType().Equals(other.GetType());
				var valueMatches = Value.Equals(otherValue.Value);
				return typeMatches && valueMatches;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}