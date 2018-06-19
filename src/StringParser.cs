namespace PegCombinator
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
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

		public static Parser<char, char> CaseInsensitiveChar (char x) =>
			Parser.Satisfy<char> (y => char.ToLower (x) == char.ToLower (y))
				.Expect (x.ToString ());

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

		public static readonly Parser<char, char> AsciiLetter =
			Parser.Satisfy<char> (c => c < 0x80 && char.IsLetter (c))
				.Expect ("ASCII letter");

		public static readonly Parser<char, char> Symbol =
			Parser.Satisfy<char> (char.IsSymbol).Expect ("symbol");

		public static readonly Parser<char, char> Punctuation =
			Parser.Satisfy<char> (char.IsPunctuation).Expect ("punctuation");

		public static readonly Parser<char, char> NotPunctuation =
			Parser.Satisfy<char> (c => !char.IsPunctuation (c))
				.Expect ("not punctuation");

		/// <summary>
		/// Parse on alphanumeric character.
		/// </summary>
		public static readonly Parser<char, char> AlphaNumeric =
			Parser.Satisfy<char> (char.IsLetterOrDigit).Expect ("alphanumeric character"); 

		public static readonly Parser<char, char> AsciiAlphaNumeric =
			Parser.Satisfy<char> (c => c < 0x80 && char.IsLetterOrDigit (c))
			.Expect ("alphanumeric character");

		public static readonly Parser<char, char> WhitespaceChar =
			Parser.Satisfy<char> (char.IsWhiteSpace).Expect ("whitespace character");

		public static readonly Parser<char, char> NonWhitespaceChar =
			Parser.Satisfy<char> (c => !char.IsWhiteSpace (c)).Expect ("non-whitespace character");

		public static readonly Parser<char, char> Control =
			Parser.Satisfy<char> (char.IsControl).Expect ("control character");

		public static readonly Parser<char, char> AsciiControl =
			Parser.Satisfy<char> (c => c < 0x80 && char.IsControl (c))
				.Expect ("ASCII control character");

		public static readonly Parser<char, char> AsciiWhitespaceChar =
			Parser.Satisfy<char> (c => c < 0x80 && char.IsWhiteSpace (c))
			.Expect ("ASCII whitespace character");

		/// <summary>
		/// Parse a word (sequence of consecutive letters)
		/// </summary>
		/// <returns></returns>
		public static readonly Parser<string, char> Word =
			(from xs in Letter.OneOrMore ()
			 select xs.AsString ())
			.Expect ("Word containing one or more letters");

		public static readonly Parser<string, char> VariableName =
			(from lt in Letter
			 from an in AlphaNumeric.ZeroOrMore ()
			 select an.AddToFront (lt).AsString ())
			.Expect ("Variable name starting with a letter and followed " +
				"by optional alphanumeric characters");

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
		/// Parse a given string.
		/// </summary>
		public static Parser<string, char> String (string str) => 
			CollectionParsers.List<string, char> (str, (c1, c2) => c1 == c2)
			.Expect (str);

		public static Parser<string, char> String (char chr) =>
			Char (chr).ToStringParser ();

		public static Parser<string, char> CaseInsensitiveString (string str) =>
			CollectionParsers.List<string, char> (str, 
				(c1, c2) => char.ToLower (c1) == char.ToLower (c2))
			.Expect (str + " (case insensitive)");

		/// <summary>
		/// Parse a positive integer without a leading '+' character.
		/// </summary>
		public static readonly Parser<int, char> PositiveInteger =
			from xs in Number.OneOrMore ()
			select int.Parse (xs.AsString ());

		/// <summary>
		/// Parse a possibly negative integer.
		/// </summary>
		public static readonly Parser<int, char> Integer =
			from sign in Char ('-').OptionalVal ()
			from number in PositiveInteger
			select sign.HasValue ? -number : number;

		public static readonly Parser<int, char> HexadecimalInteger =
			from xs in Number
				.Or (OneOf ('A', 'B', 'C', 'D', 'E', 'F', 'a', 'b', 'c', 'd', 'e', 'f'))
				.OneOrMore ()
			select int.Parse (xs.AsString (), NumberStyles.HexNumber);

		/// <summary>
		/// Creates a parser that skips whitespace, i.e. just consumes white space 
		/// from the sequence and returns an empty string. Note that the parser
		/// expects find some whitespace, or else it fails.
		/// </summary>
		public static Parser<string, char> Whitespace (string result = "") =>
			from ws in WhitespaceChar.OneOrMore ()
			select result ?? ws.AsString ();

		public static Parser<string, char> OptionalWhitespace (string result = "") =>
			from ws in WhitespaceChar.ZeroOrMore ()
			select result ?? ws.AsString ();

		public static Parser<string, char> AsciiWhitespace (string result = "") =>
			from ws in AsciiWhitespaceChar.OneOrMore ()
			select result ?? ws.AsString ();

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
			from s in SpacesOrTabs.Optional ("")
			from nl in NewLine
			select keepLinefeed ? s + nl : s;

		public static readonly Parser<string, char> Identifier =
			from x in Letter
			from xs in AlphaNumeric.ZeroOrMore ()
			select xs.AddToFront (x).AsString ();

		public static Parser<T, char> Token<T> (this Parser<T, char> parser) =>
			from v in parser
			from _ in Whitespace ()
			select v;

		public static string AsString (this IEnumerable<char> list)
		{
			var res = new StringBuilder ();
			foreach (var ch in list)
				res.Append (ch);
			return res.ToString ();
		}

		public static string AsString (this IEnumerable<string> list)
		{
			var res = new StringBuilder ();
			foreach (var str in list)
				res.Append (str);
			return res.ToString ();
		}

		public static Parser<string, char> ToStringParser (
			this Parser<char, char> parser) =>
			parser.Select (chr => new string (chr, 1));

		public static Parser<string, char> ToStringParser (
			this Parser<List<char>, char> parser) => 
			parser.Select (AsString);

		public static string EscapeWhitespace (this char chr)
		{
			switch (chr)
			{
				case '\0': return @"\0";
				case '\t': return @"\t";
				case '\n': return @"\n";
				case '\r': return @"\r";
				default: return new string (chr, 1);
			}
		}

		public static string EscapeWhitespace (this string str)
		{
			var res = new StringBuilder ();
			for (int i = 0; i < str.Length; i++)
				res.Append (EscapeWhitespace (str[i]));
			return res.ToString ();
		}
	}
}
