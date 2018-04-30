namespace PegCombinator
{
	using System;

	public struct Thunk<T>
	{
		private bool _valueCreated;
		private T _value;
		private Func<T> _getValue;

		public Thunk (Func<T> getValue)
		{
			_getValue = getValue;
			_value = default (T);
			_valueCreated = false;
		}

		public T Value
		{
			get
			{
				if (!_valueCreated)
				{
					_value = _getValue ();
					_valueCreated = true;
				}
				return _value;
			}
		}

		public static implicit operator T (Thunk<T> thunk) => thunk.Value;
	}

	public static class Thunk
	{
		public static Thunk<T> New<T> (Func<T> getValue) => 
			new Thunk<T> (getValue);
	}
}
