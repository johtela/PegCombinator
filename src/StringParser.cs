namespace PegCombinator
{
	using System;
	using System.Text;
	using ExtensionCord;

	/// <summary>
	/// Parser combinator for characters and strings.
	/// </summary>
	public static class StringParser
	{
		public static readonly Parser<char, char> AnyChar =
			Parser.Satisfy<char> (_ => true);

		/// <summary>
		/// Parse a given character.
		/// </summary>
		public static Parser<char, char> Char (char x) => 
			Parser.Satisfy<char> (y => x == y).Expect (x.ToString ());

		/// <summary>
		/// Parse a number [0-9]
		/// </summary>
		public static readonly Parser<char, char> Number =
			Parser.Satisfy<char> (char.IsNumber).Expect ("number");

		/// <summary>
		/// Parse a lower case character [a-z]
		/// </summary>
		public static readonly Parser<char, char> Lower =
			Parser.Satisfy<char> (char.IsLower).Expect ("lowercase character");

		/// <summary>
		/// Parse an upper case character [A-Z]
		/// </summary>
		public static readonly Parser<char, char> Upper =
			Parser.Satisfy<char> (char.IsUpper).Expect ("uppercase character");

		/// <summary>
		/// Parse any letter.
		/// </summary>
		public static readonly Parser<char, char> Letter =
			Parser.Satisfy<char> (char.IsLetter).Expect ("letter");

		/// <summary>
		/// Parse on alphanumeric character.
		/// </summary>
		public static readonly Parser<char, char> AlphaNumeric =
			Parser.Satisfy<char> (char.IsLetterOrDigit).Expect ("alphanumeric character");

		public static readonly Parser<char, char> WhitespaceChar =
			Parser.Satisfy<char> (char.IsWhiteSpace).Expect ("whitespace character");

		/// <summary>
		/// Parse a word (sequence of consecutive letters)
		/// </summary>
		/// <returns></returns>
		public static readonly Parser<string, char> Word =
			from xs in Letter.OneOrMore ()
			select xs.AsString ();

		/// <summary>
		/// Parse a character that is in the set of given characters.
		/// </summary>
		public static Parser<char, char> OneOf (params char[] chars)
		{
			Array.Sort (chars);
			return Parser.Satisfy<char> (c => Array.BinarySearch (chars, c) >= 0)
				.Expect ("one of: " + chars.ToString ("", "", ", "));
		}

		/// <summary>
		/// Parse a character that is NOT in the set of given characters.
		/// </summary>
		public static Parser<char, char> NoneOf (params char[] chars)
		{
			Array.Sort (chars);
			return Parser.Satisfy<char> (c => Array.BinarySearch (chars, c) < 0)
				.Expect ("any char except: " + chars.ToString ("", "", ", "));
		}

		/// <summary>
		/// Parse a given sequence of characters.
		/// </summary>
		private static Parser<string, char> StringChars (string str, int index) => 
			index >= str.Length ?
				str.ToParser<string, char> () :
				Char (str[index]).Then (StringChars (str, index + 1));

		/// <summary>
		/// Parse a given string.
		/// </summary>
		public static Parser<string, char> String (string str) => 
			StringChars (str, 0).Expect (str);

		/// <summary>
		/// Parse a positive integer without a leading '+' character.
		/// </summary>
		public static readonly Parser<int, char> PositiveInteger =
			(from x in Number
			 select x - '0').Chain1 (
				Parser.ToParser<Func<int, int, int>, char> (
					(m, n) => 10 * m + n));

		/// <summary>
		/// Parse a possibly negative integer.
		/// </summary>
		public static readonly Parser<int, char> Integer =
			from sign in Char ('-').OptionalVal ()
			from number in PositiveInteger
			select sign.HasValue ? -number : number;

		/// <summary>
		/// Creates a parser that skips whitespace, i.e. just consumes white space 
		/// from the sequence and returns an empty string. Note that the parser
		/// expects find some whitespace, or else it fails.
		/// </summary>
		public static readonly Parser<string, char> WhiteSpace =
			from _ in WhitespaceChar.OneOrMore ()
			select string.Empty;

		public static readonly Parser<string, char> SpacesOrTabs =
			from s in OneOf (' ', '\t').OneOrMore ()
			select s.AsString ();

		public static readonly Parser<string, char> NewLine =
			from cr in Char ('\r').OptionalVal ()
			from lf in Char ('\n')
			select cr.HasValue ? "\r\n" : "\n";

		public static Parser<string, char> Line (bool keepLinefeed = false) =>
			from s in NoneOf ('\r', '\n').ZeroOrMore ()
			from nl in NewLine
			select s.AsString () + (keepLinefeed ? nl : "");

		public static Parser<string, char> BlankLine (bool keepLinefeed = false) =>
			from s in SpacesOrTabs
			from nl in NewLine
			select keepLinefeed ? s + nl : s;

		public static readonly Parser<string, char> Identifier =
			from x in Letter
			from xs in AlphaNumeric.ZeroOrMore ()
			select (x | xs).AsString ();

		public static Parser<T, char> Token<T> (this Parser<T, char> parser) =>
			from v in parser
			from _ in WhiteSpace
			select v;

		public static string AsString (this Seq<char> sequence)
		{
			var res = new StringBuilder ();
			for (var s = sequence; s != null; s = s.Rest)
				res.Append (s.First);
			return res.ToString ();
		}
	}
}
