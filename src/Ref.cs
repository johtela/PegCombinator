namespace PegCombinator
{
	using System;

	public class Ref<T> where T: class
	{
		private T _target;

		public T Target
		{
			get
			{
				if (_target == null)
					throw new InvalidOperationException ("Target not initialized");
				return _target;
			}
			set => _target = value;
		}

		public Ref () { }

		public Ref (T target) => _target = target;

		public static implicit operator T (Ref<T> reference) => 
			reference.Target;
	}
}