namespace PegCombinator.src
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class RingStack<T> : IEnumerable<T>
	{
		private T[] _buffer;
		private int _count;
		private int _top;

		public RingStack (int capacity)
		{
			_buffer = new T[capacity];
		}

		public int Capacity { get => _buffer.Length; }

		public int Count { get => _count; }

		private int Inc (int index, int offs) =>
			(index + offs) % Capacity;

		private int Dec (int index, int offs)
		{
			var res = index - offs;
			return res < 0 ? Capacity + res : res;
		}

		public void Push (T item)
		{
			_buffer[_top] = item;
			if (_count < Capacity)
				_count++;
			_top = Inc (_top, 1);
		}

		public T Pop ()
		{
			if (_count == 0)
				throw new InvalidOperationException ("Stack is empty");
			_count--;
			_top = Dec (_top, 1);
			return _buffer[_top];
		}

		public IEnumerator<T> GetEnumerator ()
		{
			for (int i = 0, index = _top; i < _count; i++, index = Dec (index, 1))
				yield return _buffer[index];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}
}
