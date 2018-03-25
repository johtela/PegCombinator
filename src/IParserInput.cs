namespace PegCombinator
{
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.InteropServices;

	public interface IParserInput<S> : IEnumerator<S>
    {
        object Position { get; set; }
    }

	public static class ParserInput
	{
		private class StringInput : IParserInput<char>
		{
			private string _input;
			private int _position;

			public StringInput (string input)
			{
				_input = input;
			}

			public object Position
			{
				get => _position;
				set => _position = (int)value;
			}

			public char Current => _input[_position];

			object IEnumerator.Current => Current;

			public void Dispose () { }

			public bool MoveNext ()
			{
				return ++_position < _input.Length;
			}

			public void Reset ()
			{
				_position = 0;
			}
		}

		private class ArrayInput<S> : IParserInput<S>
		{
			private S[] _input;
			private int _position;

			public ArrayInput (S[] input)
			{
				_input = input;
			}

			public object Position
			{
				get => _position;
				set => _position = (int)value;
			}

			public S Current => _input[_position];

			object IEnumerator.Current => Current;

			public void Dispose () { }

			public bool MoveNext ()
			{
				return ++_position < _input.Length;
			}

			public void Reset ()
			{
				_position = 0;
			}
		}

		private class StreamInput<S> : IParserInput<S>
		{
			private Stream _input;
			private S _current;

			public StreamInput (Stream input)
			{
				_input = input;
			}

			public object Position
			{
				get => _input.Position;
				set => _input.Position = (long)value;
			}

			public S Current => _current;

			object IEnumerator.Current => Current;

			public void Dispose ()
			{
				_input.Dispose ();
			}

			public bool MoveNext ()
			{
				var size = Marshal.SizeOf<S> ();
				var buffer = new byte[size];
				var read = _input.Read (buffer, 0, size);
				if (read < size)
					return false;
				var handle = GCHandle.Alloc (buffer, GCHandleType.Pinned);
				_current = Marshal.PtrToStructure<S> (handle.AddrOfPinnedObject ());
				return true;
			}

			public void Reset ()
			{
				_input.Seek (0, SeekOrigin.Begin);
			}
		}

		public static IParserInput<char> String (string input)
		{
			return new StringInput (input);
		}

		public static IParserInput<S> Array<S> (S[] input)
		{
			return new ArrayInput<S> (input);
		}

		public static IParserInput<S> Stream<S> (Stream input)
		{
			return new StreamInput<S> (input);
		}
	}
}