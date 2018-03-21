﻿namespace PegCombinator
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using ExtensionCord;

	/// <summary>
	/// Parser combinator for characters and strings.
	/// </summary>
	public static class StringParser
	{
		/// <summary>
		/// Parse a given character.
		/// </summary>
		public static Parser<char, char> Char (char x)
		{
			return Parser.Satisfy<char> (y => x == y).Expect (x.ToString ());
		}

		/// <summary>
		/// Parse a number [0-9]
		/// </summary>
		public static Parser<char, char> Number ()
		{
			return Parser.Satisfy<char> (char.IsNumber).Expect ("number");
		}

		/// <summary>
		/// Parse a lower case character [a-z]
		/// </summary>
		public static Parser<char, char> Lower ()
		{
			return Parser.Satisfy<char> (char.IsLower).Expect ("lowercase character");
		}

		/// <summary>
		/// Parse an upper case character [A-Z]
		/// </summary>
		public static Parser<char, char> Upper ()
		{
			return Parser.Satisfy<char> (char.IsUpper).Expect ("uppercase character");
		}

		/// <summary>
		/// Parse any letter.
		/// </summary>
		public static Parser<char, char> Letter ()
		{
			return Parser.Satisfy<char> (char.IsLetter).Expect ("letter");
		}

		/// <summary>
		/// Parse on alphanumeric character.
		/// </summary>
		public static Parser<char, char> AlphaNumeric ()
		{
			return Parser.Satisfy<char> (char.IsLetterOrDigit).Expect ("alphanumeric character");
		}

		/// <summary>
		/// Parse a word (sequence of consecutive letters)
		/// </summary>
		/// <returns></returns>
		public static Parser<string, char> Word ()
		{
			return from xs in Letter ().OneOrMore ()
				   select xs.ToString ("", "", "");
		}

		/// <summary>
		/// Parse a character that is in the set of given characters.
		/// </summary>
		public static Parser<char, char> OneOf (params char[] chars)
		{
			return Parser.Satisfy<char> (c => chars.Contains (c));
		}

		/// <summary>
		/// Parse a character that is NOT in the set of given characters.
		/// </summary>
		public static Parser<char, char> NoneOf (params char[] chars)
		{
			return Parser.Satisfy<char> (c => !chars.Contains (c));
		}

		/// <summary>
		/// Parse a given sequence of characters.
		/// </summary>
		public static Parser<IEnumerable<char>, char> Chars (IEnumerable<char> chars)
		{
			return chars.None () ? chars.ToParser<IEnumerable<char>, char> () :
				Char (chars.First ()).Then (
				Chars (chars.Skip (1)).Then (
				chars.ToParser<IEnumerable<char>, char> ()));
		}

		/// <summary>
		/// Convert a parser that returns StrictList[char] to one that returns a string.
		/// </summary>
		public static Parser<string, char> AsString (this Parser<IEnumerable<char>, char> parser)
		{
			return from cs in parser
				   select cs.CharsToString ();
		}

		/// <summary>
		/// Parse a given string.
		/// </summary>
		public static Parser<string, char> String (string str)
		{
			return Chars (str).AsString ().Expect (str);
		}

		/// <summary>
		/// Parse a positive integer without a leading '+' character.
		/// </summary>
		public static Parser<int, char> PositiveInteger ()
		{
			return (from x in Number ()
					select x - '0').Chain1 (
					Parser.ToParser<Func<int, int, int>, char> (
						(m, n) => 10 * m + n));
		}

		/// <summary>
		/// Parse a possibly negative integer.
		/// </summary>
		public static Parser<int, char> Integer ()
		{
			return from sign in Char ('-').OptionalVal ()
				   from number in PositiveInteger ()
				   select sign.HasValue ? -number : number;
		}

		/// <summary>
		/// Creates a parser that skips whitespace, i.e. just consumes white space 
		/// from the sequence and returns an empty string. Note that the parser
		/// expects find some whitespace, or else it fails.
		/// </summary>
		public static Parser<string, char> WhiteSpace ()
		{
			return from _ in Parser.Satisfy<char> (char.IsWhiteSpace).OneOrMore ()
				   select string.Empty;
		}

		public static Parser<string, char> Junk ()
		{
			return from _ in WhiteSpace ().OptionalRef ()
				   select string.Empty;
		}

		public static Parser<T, char> SkipJunk<T> (this Parser<T, char> parser)
		{
			return from _ in Junk ()
				   from v in parser
				   select v;
		}

		public static Parser<string, char> Identifier ()
		{
			return from x in Letter ()
				   from xs in AlphaNumeric ().ZeroOrMore ()
				   select (x | xs).ToString ("", "", "");
		}

		public static Parser<T, char> Token<T> (this Parser<T, char> parser)
		{
			return from v in parser
				   from _ in Junk ()
				   select v;
		}
	}
}
