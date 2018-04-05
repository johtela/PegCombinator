namespace PegCombinator
{
    using System;
	using System.Diagnostics;
	using System.Linq;
    using ExtensionCord;

    public delegate ParseResult<T> Parser<T, S> (IParserInput<S> input);

    /// </summary>
    public static class Parser
    {
		public static bool UseMemoization { get; set; }
		public static bool Debugging { get; set; }
		public static int RulesEvaluated
		{
			get => _rulesEvaluated;
		}

		[ThreadStatic]
		private static int _rulesEvaluated;

		/// <summary>
		/// Attempt to parse an input with a given parser.
		/// </summary>
		public static Either<T, ParseError> TryParse<T, S> (this Parser<T, S> parser, 
            IParserInput<S> input)
        {
			_rulesEvaluated = 0;
            var res = parser (input);
			if (Debugging)
				Debug.WriteLine ("Number of rules evaluated: {0}", _rulesEvaluated);
			return res ?
                Either<T, ParseError>.Create (res.Result) :
                Either<T, ParseError>.Create (ParseError.FromParseResult (res));
        }

        /// <summary>
        /// Parses an input, or throws an ParseError exception, if the parse fails.
        /// </summary>
        public static T Parse<T, S> (this Parser<T, S> parser, IParserInput<S> input)
        {
            return TryParse (parser, input).Match (
                value => value,
                error => { throw error; });
        }

        /// <summary>
        /// The monadic return. Lifts a value to the parser monad, i.e. creates
        /// a parser that just returns a value without consuming any input.
        /// </summary>
        public static Parser<T, S> ToParser<T, S> (this T value)
        {
            return input => ParseResult<T>.Succeeded (input.Position, value);
        }

        public static Parser<T, S> Fail<T, S> (string found, string expected)
        {
            return input => ParseResult<T>.Failed (input.Position, found, Seq.Cons (expected));
        }

        /// <summary>
        /// Creates a parser that reads one item from input and returns it, if
        /// it satisfies a given predicate; otherwise the parser will fail.
        /// </summary>
        public static Parser<T, T> Satisfy<T> (Func<T, bool> predicate)
        {
            return Memoize<T, T> (input =>
            {
                var pos = input.Position;
                if (!input.MoveNext ())
                    return ParseResult<T>.Failed (input.Position, "end of input");
                var item = input.Current;
                if (predicate (item))
                    return ParseResult<T>.Succeeded (input.Position, item);
                var res = ParseResult<T>.Failed (input.Position, item.ToString ());
                input.Position = pos;
                return res;
            });
        }

		public static Parser<T, T> NotSatisfy<T> (Func<T, bool> predicate)
		{
			return Satisfy<T> (x => !predicate (x));
		}

        /// <summary>
        /// The monadic bind. Runs the first parser, and if it succeeds, feeds the
        /// result to the second parser. Corresponds to Haskell's >>= operator.
        /// </summary>
        public static Parser<U, S> Bind<T, U, S> (this Parser<T, S> parser, 
            Func<T, Parser<U, S>> func)
        {
			if (parser == null)
				throw new ArgumentNullException (nameof (parser));
			parser = parser.Memoize ();
            return input =>
            {
                var pos = input.Position;
                var res1 = parser (input);
                if (res1)
                {
                    var res2 = func (res1.Result) (input);
                    if (!res2 && pos != input.Position)
                        input.Position = pos;   // backtrack
                    return res2;
                }
                return ParseResult<U>.Failed (res1.Position, res1.Found, res1.Expected);
            };
        }

        /// <summary>
        /// The sequence operator. Runs the first parser, and if it succeeds, runs the second
        /// parser ignoring the result of the first one.
        /// </summary>
        public static Parser<U, S> Then<T, U, S> (this Parser<T, S> parser, Parser<U, S> other)
        {
            return parser.Bind (_ => other);
        }

        /// <summary>
        /// The ordered choice operation. Creates a parser that runs the first parser, and if
        /// that fails, runs the second one. Corresponds to the / operation in PEG grammars.
        /// </summary>
        public static Parser<T, S> Or<T, S> (this Parser<T, S> parser, Parser<T, S> other)
        {
			parser = parser.Memoize ();
			other = other.Memoize ();
			return Memoize<T, S> (input =>
			{
				var res1 = parser (input);
				if (res1)
					return res1;
				var res2 = other (input);
				if (res2)
					return res2;
				return ParseResult<T>.Failed (input.Position, res2.Found, res1.MergeExpected (res2));
			});
        }

        public static Parser<T, S> Expect<T, S> (this Parser<T, S> parser, string expected)
        {
            return input =>
            {
                var res = parser (input);
                if (!res)
                    return ParseResult<T>.Failed (res.Position, res.Found, 
                        Seq.Cons (expected, res.Expected));
                return res;
            };
        }

        /// <summary>
        /// Select extension method needed to enable Linq's syntactic sugaring.
        /// </summary>
        public static Parser<U, S> Select<T, U, S> (this Parser<T, S> parser, Func<T, U> select)
        {
            return parser.Bind (x => select (x).ToParser<U, S> ());
        }

        /// <summary>
        /// SelectMany extension method needed to enable Linq's syntactic sugaring.
        /// </summary>
        public static Parser<V, S> SelectMany<T, U, V, S> (this Parser<T, S> parser,
            Func<T, Parser<U, S>> project, Func<T, U, V> select)
        {
            return parser.Bind (x => project (x).Bind (y => select (x, y).ToParser<V, S> ()));
        }

        /// <summary>
        /// Creates a parser that will run a given parser zero or more times. The results
        /// of the input parser are added to a list.
        /// </summary>
        public static Parser<Seq<T>, S> ZeroOrMore<T, S> (this Parser<T, S> parser)
        {
            return (from x in parser
                    from xs in parser.ZeroOrMore ()
                    select x | xs)
                    .Or (ToParser<Seq<T>, S> (null));
        }

        /// <summary>
        /// Creates a parser that will run a given parser one or more times. The results
        /// of the input parser are added to a list.
        /// </summary>
        public static Parser<Seq<T>, S> OneOrMore<T, S> (this Parser<T, S> parser)
        {
            return from x in parser
                   from xs in parser.ZeroOrMore ()
                   select x | xs;
        }

		/// <summary>
		/// Parsing succeeds if the given parser can be executed between min to max times.
		/// </summary>
		public static Parser<Seq<T>, S> Occurrences<T, S> (this Parser<T, S> parser, 
			int min, int max)
		{
			return parser.ZeroOrMore ().Bind (s => 
			{
				var cnt = s.IsEmpty () ? 0 : s.Count ();
				return cnt >= min && cnt <= max ?  
					ToParser<Seq<T>, S> (s) :
					Fail<Seq<T>, S> (cnt + " occurrences", min + " to " + max + " occurrences");
			});
		}

        /// <summary>
        /// Parses an optional input.
        /// </summary>
        public static Parser<T?, S> OptionalVal<T, S> (this Parser<T, S> parser)
            where T: struct
        {
            return parser.Bind (x => new T? (x).ToParser<T?, S> ())
                .Or (ToParser<T?, S> (null));
        }

        public static Parser<T, S> OptionalRef<T, S> (this Parser<T, S> parser)
            where T : class
        {
            return parser.Bind (x => x.ToParser<T, S> ())
                .Or (ToParser<T, S> (null));
        }

        /// <summary>
        /// Optionally parses an input, if the parser fails then the default value is returned.
        /// </summary>
        public static Parser<T, S> Optional<T, S> (this Parser<T, S> parser, T defaultValue)
        {
            return parser.Or (defaultValue.ToParser<T, S> ());
        }

        public static Parser<T, S> And<T, S> (this Parser<T, S> parser)
        {
            return Memoize<T, S> (input =>
            {
                var pos = input.Position;
                var res = parser (input);
                input.Position = pos;
                return res;
            });
        }

        public static Parser<T, S> Not<T, S> (this Parser<T, S> parser)
        {
            return Memoize<T, S> (input =>
            {
                var pos = input.Position;
                var res = parser (input);
                input.Position = pos;
                if (res)
                {
                    var found = res.Result.ToString ();
                    return ParseResult<T>.Failed (input.Position, found, Seq.Cons ("not " + found));
                }
                return ParseResult<T>.Succeeded (input.Position, default (T));
            });
        }

		public static Parser<long, S> Position<S> ()
		{
			return input => ParseResult<long>.Succeeded (input.Position, input.Position);
		}

		public static Parser<T, S> Memoize<T, S> (this Parser<T, S> parser)
		{
			if (!UseMemoization)
				return parser;
			ParseResult<T> lastResult = null;
			long lastPos = long.MinValue;
			return input =>
			{
				var pos = input.Position;
				if (pos == lastPos && lastResult != null)
					return lastResult;
				lastPos = pos;
				lastResult = parser (input);
				return lastResult;
			};
		}

		public static Parser<T, S> Trace<T, S> (this Parser<T, S> parser, string ruleName)
		{
			if (!Debugging)
				return parser;
			return input =>
			{
				Debug.IndentSize = 2;
				Debug.WriteLine ("{0} called with input '{1}' at position {2}",
					ruleName, input.Current.ToString ().EscapeWhitespace (), input.Position);
				Debug.Indent ();
				var res = parser (input);
				_rulesEvaluated++;
				Debug.Unindent ();
				Debug.WriteLine ("{0} {1} with input '{2}' at position {3}",
					ruleName, res ? "SUCCEEDED" : "FAILED", 
					input.Current.ToString ().EscapeWhitespace (), input.Position);
				return res;
			};
		}

        /// <summary>
        /// Upcast the result of the parser.
        /// </summary>
        public static Parser<U, S> Cast<T, U, S> (this Parser<T, S> parser) where T : U
        {
            return from x in parser
                   select (U)x;
        }
    }
}
