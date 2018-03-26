﻿namespace PegCombinator
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using ExtensionCord;

    public static class SequenceParsers
    {
		private static Parser<Seq<T>, T> SeqPart<T> (T[] items, int index)
		{
			return index >= items.Length ? Parser.ToParser<Seq<T>, T> (null) :
				from x in Parser.Satisfy<T> (x => x.Equals (items[index]))
				from ys in SeqPart (items, index + 1)
				select x | ys;
		}

		public static Parser<Seq<T>, T> Sequence<T> (params T[] items)
		{
			return SeqPart (items, 0);
		}

		/// <summary>
		/// Creates a parser that will read a list of items separated by a separator.
		/// The list needs to have at least one item.
		/// </summary>
		public static Parser<Seq<T>, S> SeparatedBy1<T, U, S> (this Parser<T, S> parser,
			Parser<U, S> separator)
		{
			return from x in parser
				   from xs in
					   (from y in separator.Then (parser)
						select y).ZeroOrMore ()
				   select x | xs;
		}

		/// <summary>
		/// Creates a parser that will read a list of items separated by a separator.
		/// The list can also be empty.
		/// </summary>
		public static Parser<Seq<T>, S> SeparatedBy<T, U, S> (this Parser<T, S> parser,
			Parser<U, S> separator)
		{
			return SeparatedBy1 (parser, separator).Or (
				Parser.ToParser<Seq<T>, S> (null));
		}

		/// <summary>
		/// Creates a parser the reads a bracketed input.
		/// </summary>
		public static Parser<T, S> Bracket<T, U, V, S> (this Parser<T, S> parser,
			Parser<U, S> open, Parser<V, S> close)
		{
			return from o in open
				   from x in parser
				   from c in close
				   select x;
		}

		/// <summary>
		/// Creates a parser that reads an expression with multiple terms separated
		/// by an operator. The operator is returned as a function and the terms are
		/// evaluated left to right.
		/// </summary>
		public static Parser<T, S> Chain1<T, S> (this Parser<T, S> parser,
			Parser<Func<T, T, T>, S> operation)
		{
			return from x in parser
				   from fys in
					   (from f in operation
						from y in parser
						select new { f, y }).ZeroOrMore ()
				   select fys.Aggregate (x, (z, fy) => fy.f (z, fy.y));
		}

		/// <summary>
		/// Creates a parser that reads an expression with multiple terms separated
		/// by an operator. The operator is returned as a function and the terms are
		/// evaluated left to right. If the parsing of the expression fails, the value
		/// given as an argument is returned as a parser.
		/// </summary>
		public static Parser<T, S> Chain<T, S> (this Parser<T, S> parser,
			Parser<Func<T, T, T>, S> operation, T value)
		{
			return parser.Chain1 (operation).Or (value.ToParser<T, S> ());
		}

		/// <summary>
		/// Create a combined parser that will parse any of the given operators. 
		/// The operators are specified in a sequence which contains (parser, result)
		/// pairs. If the parser succeeds the result is returned, otherwise the next 
		/// parser in the sequence is tried.
		/// </summary>
		public static Parser<U, S> Operators<T, U, S> (IEnumerable<Tuple<Parser<T, S>, U>> ops)
		{
			return ops.Select (op => from _ in op.Item1
									 select op.Item2).Aggregate (Parser.Or);
		}
	}
}
