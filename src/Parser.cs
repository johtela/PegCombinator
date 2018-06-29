namespace PegCombinator
{
    using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
    using ExtensionCord;

    public delegate ParseResult<T> Parser<T, S> (IParserInput<S> input);

    /// </summary>
    public static class Parser
    {
		public static bool Debugging { get; set; }
		public static bool ErrorMessages { get; set; }
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
            return input =>
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
            };
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
			return input =>
			{
				var res1 = parser (input);
				if (res1)
					return res1;
				var res2 = other (input);
				if (res2)
					return res2;
				return ParseResult<T>.Failed (input.Position, res2.Found, res1.MergeExpected (res2));
			};
        }

        public static Parser<T, S> Expect<T, S> (this Parser<T, S> parser, string expected)
        {
			if (!ErrorMessages)
				return parser;
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

		public static Parser<T, S> Where<T, S> (this Parser<T, S> parser, 
			Func<T, bool> predicate)
		{
			return parser.Bind (x =>
				predicate (x) ?
					x.ToParser<T, S> () :
					Fail<T, S> (x.ToString (), "predicate"));
		}

        /// <summary>
        /// Creates a parser that will run a given parser zero or more times. The results
        /// of the input parser are added to a list.
        /// </summary>
        public static Parser<List<T>, S> ZeroOrMore<T, S> (this Parser<T, S> parser)
        {
			return input =>
			{
				var list = new List<T> (0);
				while (true)
				{
					var res = parser (input);
					if (!res)
						return ParseResult<List<T>>.Succeeded (input.Position, list);
					list.Add (res.Result);
				}
			};
        }

        /// <summary>
        /// Creates a parser that will run a given parser one or more times. The results
        /// of the input parser are added to a list.
        /// </summary>
        public static Parser<List<T>, S> OneOrMore<T, S> (this Parser<T, S> parser)
        {
			return input =>
			{
				var res = parser (input);
				if (!res)
					return ParseResult<List<T>>.Failed (input.Position, res.Found, null);
				var list = new List<T> (1) { res.Result };
				while (true)
				{
					res = parser (input);
					if (!res)
						return ParseResult<List<T>>.Succeeded (input.Position, list);
					list.Add (res.Result);
				}
			};
        }

		/// <summary>
		/// Parsing succeeds if the given parser can be executed between min to max times.
		/// </summary>
		public static Parser<List<T>, S> Occurrences<T, S> (this Parser<T, S> parser, 
			int min, int max)
		{
			return parser.ZeroOrMore ().Bind (list => 
			{
				var cnt = list.Count ();
				return cnt >= min && cnt <= max ?  
					ToParser<List<T>, S> (list) :
					Fail<List<T>, S> (cnt + " occurrences", min + " to " + max + " occurrences");
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
            return input =>
            {
                var pos = input.Position;
                var res = parser (input);
                input.Position = pos;
                return res;
            };
        }

        public static Parser<T, S> Not<T, S> (this Parser<T, S> parser)
        {
            return input =>
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
            };
        }

		public static Parser<T, S> Choose<T, S> (Func<S, Parser<T, S>> selector) =>
			from item in Peek<S> ()
			let parser = selector (item)
			from res in parser
			select res;

		public static Parser<T, S> Any<T, S> (IEnumerable<Parser<T, S>> options)
		{
			if (options.None ())
				throw new ArgumentException ("Must provide at least one option.", "options");
			return input =>
			{
				ParseResult<T> res = null;
				foreach (var parser in options)
					if (res = parser (input))
						return res;
				return res;
			};
		}

		public static Parser<T, S> Any<T, S> (params Parser<T, S>[] options) =>
			Any ((IEnumerable<Parser<T, S>>)options);

		public static Parser<S, S> Peek<S> ()
		{
			return input =>
			{
				var pos = input.Position;
				if (!input.MoveNext ())
					return ParseResult<S>.Failed (pos, "end of input");
				var item = input.Current;
				input.Position = pos;
				return ParseResult<S>.Succeeded (pos, item);
			};
		}

		public static Parser<T, S> LookBack<T, S> (this Parser<T, S> parser)
		{
			return input =>
			{
				var pos = input.Position;
				input.Direction = ParseDirection.Backward;
				var res = parser (input);
				input.Direction = ParseDirection.Forward;
				input.Position = pos;
				return res;
			};
		}

		public static Parser<long, S> Position<S> ()
		{
			return input => ParseResult<long>.Succeeded (input.Position, input.Position);
		}

		public static Parser<T, S> Backtrack<T, S> (this Parser<T, S> parser,
			long position)
		{
			return input =>
			{
				var pos = input.Position;
				input.Position = position;
				var res = parser (input);
				input.Position = pos;
				return res;
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

		public static Parser<T, S> ForwardRef<T, S> (this Ref<Parser<T, S>> parser)
		{
			return input => parser.Target (input);
		}

		public static Parser<T, S> GetState<T, S> ()
		{
			return input => ParseResult<T>.Succeeded (input.Position, (T)input.State);
		}

		public static Parser<T, S> SetState<T, S> (Func<T> setter)
		{
			return input =>
				ParseResult<T>.Succeeded (input.Position, (T)(input.State = setter ()));
		}

		public static Parser<T, S> ModifyState<T, S> (Action<T> modify)
		{
			return input =>
			{
				modify ((T)input.State);
				return ParseResult<T>.Succeeded (input.Position, (T)input.State);
			};
		}

		public static Parser<T, S> CheckState<T, S> (Func<T, bool> predicate)
		{
			return input =>
				predicate ((T)input.State) ?
					ParseResult<T>.Succeeded (input.Position, (T)input.State) :
					ParseResult<T>.Failed (input.Position, "State predicate failed");
		}

		public static Parser<T, S> CleanupState<T, U, S> (this Parser<T, S> parser, Action<U> cleanup)
		{
			return input =>
			{
				var res = parser (input);
				cleanup ((U)input.State);
				return res;
			};
		}
    }
}
