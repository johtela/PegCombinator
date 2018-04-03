namespace PegCombinator
{
    using System;
    using ExtensionCord;

    public abstract class ParseResult<T>
    {
        public abstract T Result { get; }
        public abstract string Found { get; }
        public abstract Seq<string> Expected { get; protected set; }

		public long Position { get; private set; }

		private class Ok : ParseResult<T>
        {
            private T _result;

            public Ok (long position, T result)
            {
				Position = position;
                _result = result;
            }

            public override T Result => _result;

            public override string Found => 
                throw new InvalidOperationException ("Terminal not available");

            public override Seq<string> Expected
            {
                get => null;
                protected set => throw new InvalidOperationException ("Expexted terminals not available");
            }
        }

        private class Fail : ParseResult<T>
        {
            private string _found;
            private Seq<string> _expected;

            public Fail (long position, string found, Seq<string> expected)
            {
                Position = position;
                _found = found;
                _expected = expected;
            }

            public override T Result => 
                throw new InvalidOperationException ("Result not available");

            public override string Found => _found;

            public override Seq<string> Expected
            {
                get => _expected;
                protected set => _expected = value;
            }
        }

        public Seq<string> MergeExpected (ParseResult<T> other)
        {
            var result = Expected;
            if (other is Fail && !other.Expected.IsEmpty ())
                foreach (var exp in other.Expected)
                    result = exp | result;
            return result;
        }

        public static implicit operator bool (ParseResult<T> result)
        {
            return result is Ok;
        }

        public static ParseResult<T> Succeeded (long position, T result)
        {
            return new Ok (position, result);
        }

        public static ParseResult<T> Failed (long position, string found)
        {
            return new Fail (position, found, null);
        }

        public static ParseResult<T> Failed (long position, string found, Seq<string> expected)
        {
            return new Fail (position, found, expected);
        }
    }
}