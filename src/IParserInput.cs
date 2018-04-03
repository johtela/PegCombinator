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
				Reset ();
			}

			public object Position
			{
				get => _position;
				set => _position = (int)value;
			}

			public char Current => _input[_position];

			object IEnumerator.Current => Current;

			public void Dispose () { }

			public bool MoveNext () => ++_position < _input.Length;

			public void Reset () => _position = -1;
		}

		private class ArrayInput<S> : IParserInput<S>
		{
			private S[] _input;
			private int _position;

			public ArrayInput (S[] input)
			{
				_input = input;
				Reset ();
			}

			public object Position
			{
				get => _position;
				set => _position = (int)value;
			}

			public S Current => _input[_position];

			object IEnumerator.Current => Current;

			public void Dispose () { }

			public bool MoveNext () => ++_position < _input.Length;

			public void Reset () => _position = -1;
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

			public void Dispose () => _input.Dispose ();

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

			public void Reset () => _input.Seek (0, SeekOrigin.Begin);
		}

		private class Terminator<S> : IParserInput<S>
		{
			private IParserInput<S> _input;
			private S _terminator;
			private bool _atEnd;

			public Terminator (IParserInput<S> input, S terminator)
			{
				_input = input;
				_terminator = terminator;
			}

			public object Position
			{
				get => _input.Position;
				set
				{
					_input.Position = value;
					_atEnd = false;
				}
			}

			public S Current => _atEnd ? _terminator : _input.Current;

			object IEnumerator.Current => Current;

			public void Dispose () => _input.Dispose ();

			public bool MoveNext ()
			{
				if (_atEnd)
					return false;
				if (!_input.MoveNext ())
					_atEnd = true;
				return true;
			}

			public void Reset ()
			{
				_input.Reset ();
				_atEnd = false;
			}
		}

		public static IParserInput<char> String (string input) => 
			new StringInput (input);

		public static IParserInput<S> Array<S> (S[] input) => 
			new ArrayInput<S> (input);

		public static IParserInput<S> Stream<S> (Stream input) => 
			new StreamInput<S> (input);

		public static IParserInput<S> TerminateWith<S> (this IParserInput<S> input,
			S terminator) => 
			new Terminator<S> (input, terminator);
	}
}