﻿namespace PegCombinator
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.InteropServices;

	public enum ParseDirection { Forward = 1, Backward = -1 }

	public interface IParserInput<S> : IEnumerator<S>
	{
		long Position { get; set; }
		ParseDirection Direction { get; set; }
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

			public long Position
			{
				get => _position;
				set => _position = (int)value;
			}

			public ParseDirection Direction { get; set; }

			public char Current =>
				_position < 0 || _position >= _input.Length ?
					'\0' :
					_input[_position];

			object IEnumerator.Current => Current;

			public void Dispose () { }

			public bool MoveNext ()
			{
				var len = _input.Length;
				_position += (int)Direction;
				if (_position < -1 || _position >= len)
				{
					_position = Math.Max (-1, Math.Min (len, _position));
					return false;
				}
				return true;
			}

			public void Reset ()
			{
				_position = -1;
				Direction = ParseDirection.Forward;
			}
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

			public long Position
			{
				get => _position;
				set => _position = (int)value;
			}

			public ParseDirection Direction { get; set; }

			public S Current =>
				_position < 0 || _position >= _input.Length ?
					default (S) :
					_input[_position];

			object IEnumerator.Current => Current;

			public void Dispose () { }

			public bool MoveNext ()
			{
				var len = _input.Length;
				_position += (int)Direction;
				if (_position < -1 || _position >= len)
				{
					_position = Math.Max (-1, Math.Min (len, _position));
					return false;
				}
				return true;
			}

			public void Reset ()
			{
				_position = -1;
				Direction = ParseDirection.Forward;
			}
		}

		private class StreamInput<S> : IParserInput<S>
		{
			private Stream _input;
			private S _current;

			public StreamInput (Stream input)
			{
				_input = input;
				Direction = ParseDirection.Forward;
			}

			public long Position
			{
				get => _input.Position;
				set => _input.Position = value;
			}

			public ParseDirection Direction { get; set; }

			public S Current => _current;

			object IEnumerator.Current => Current;

			public void Dispose () => _input.Dispose ();

			public bool MoveNext ()
			{
				var size = Marshal.SizeOf<S> ();
				if (Direction == ParseDirection.Backward)
					_input.Seek (size * -2, SeekOrigin.Current);
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
				Direction = ParseDirection.Forward;
			}
		}

		private class Terminator<S> : IParserInput<S>
		{
			private IParserInput<S> _input;
			private S _terminator;
			private long _endPos;

			public Terminator (IParserInput<S> input, S terminator)
			{
				_input = input;
				_terminator = terminator;
				_endPos = long.MaxValue;
			}

			private bool AtEnd () => _input.Position == _endPos;

			public long Position
			{
				get => _input.Position;
				set => _input.Position = value;
			}

			public ParseDirection Direction
			{
				get => _input.Direction;
				set => _input.Direction = value;
			}

			public S Current =>
				AtEnd () ?
					_terminator :
					_input.Current;

			object IEnumerator.Current => Current;

			public void Dispose () => _input.Dispose ();

			public bool MoveNext ()
			{
				if (Direction == ParseDirection.Backward)
					return _input.MoveNext ();
				if (AtEnd ())
					return false;
				if (!_input.MoveNext ())
					_endPos = _input.Position;
				return true;
			}

			public void Reset () => _input.Reset ();
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