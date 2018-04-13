namespace MarkdownPeg
{
	using System;
	using System.Linq;
	using ExtensionCord;
	using PegCombinator;
	using SP = PegCombinator.StringParser;

	public class MarkdownParser
    {
		private Parser<string, char> _doc;

		public MarkdownParser () => 
			_doc = Doc ();

		public string Run (IParserInput<char> input) => 
			_doc.Parse (input.TerminateWith ('\n'));

		public string Run (string input) => 
			Run (ParserInput.String (input));

		/*
		## Visitor Methods
		*/
		protected virtual string Block (long start, long end,
			string blockText) => blockText;

		protected virtual string ThematicBreak (long start, long end,
			string text) => text;

		protected virtual string Heading (long start, long end,
			int headingLevel, string headingText) =>
			"#".Times (headingLevel) + " " + headingText + Environment.NewLine;

		protected virtual string Verbatim (long start, long end,
			string verbatimText) => verbatimText;

		protected virtual string Paragraph (long start, long end,
			string text) => text;

		protected virtual string Text (long start, long end, 
			string text) => text;

		protected virtual string Space (long start, long end,
			string text) => text;

		protected virtual string Punctuation (long pos, char punctuation) => 
			new string (punctuation, 1);

		protected virtual string SoftLineBreak (long start, long end,
			string text) => text;

		protected virtual string HardLineBreak (long start, long end,
			string text) => text;

		protected virtual string Emphasis (long start, long end,
			string text) => text;

		protected virtual string StrongEmphasis (long start, long end,
			string text) => text;

		/*
		## Parsing Rules
		*/
		private Parser<string, char> Doc ()
		{
			Parser.Debugging = false;
			Parser.UseMemoization = false;
			/*
			### Special and Normal Characters
			*/
			var NormalChar =
				Parser.Not (SP.Punctuation.Or (SP.WhitespaceChar)).Then (SP.AnyChar)
				.Trace ("NormalChar");
			/*
			### Whitespace
			*/
			var OptionalSpace = SP.SpacesOrTabs.Optional ("").Trace ("OptionalSpace");

			var NonindentSpace =
				(from sp in SP.Char (' ').Occurrences (0, 3)
				 select sp.AsString ())
				.Trace ("NonindentSpace");
			/*
			### Text Lines
			*/
			var Line =
				(from chs in SP.NoneOf ('\r', '\n').ZeroOrMore ()
				 from nl in SP.NewLine
				 select chs.IsEmpty () ? nl : chs.ToString ("", "", nl))
				.Trace ("Line");

			var NonblankLine =
				(from nb in SP.BlankLine ().Not ()
				 from ln in Line
				 select ln)
				.Trace ("NonblankLine");
			/*
			### Indented Code Blocks
			*/
			var Indent = 
				NonindentSpace.Then(SP.String ("\t"))
				.Or (SP.String ("    "))
				.Trace ("Indent");

			var IndentedLine =
				Indent.Then (Line).Trace ("IndentedLine");

			var NonblankIndentedLine =
				(from nb in SP.BlankLine ().Not ()
				 from il in IndentedLine
				 select il)
				.Trace ("NonblankIndentedLine");

			var VerbatimChunk =
				(from bls in SP.BlankLine (true).ZeroOrMore ()
				 from text in NonblankIndentedLine.OneOrMore ()
				 select (bls.IsEmpty () ? text :
					 bls.Select (_ => Environment.NewLine).Concat (text))
					 .SeparateWith (""))
				.Trace ("VerbatimChunk");

			var VerbatimBlock =
				(from startPos in Parser.Position<char> ()
				 from chunks in VerbatimChunk.OneOrMore ()
				 from endPos in Parser.Position<char> ()
				 select Verbatim (startPos, endPos, chunks.SeparateWith ("")))
				.Trace ("VerbatimBlock");
			/*
			### Thematic Breaks
			*/
			Parser<string, char> TB (char c) =>
				from ch in SP.Char (c)
				from rest in (
					from si in OptionalSpace
					from ci in SP.Char (c)
					select si + new string (ci, 1))
					.Occurrences (2, int.MaxValue)
				select new string (ch, 1) + rest.ToString ("", "", "");

			var ThemaBreak =
				(from ni in NonindentSpace
				 from startPos in Parser.Position<char> ()
				 from rule in TB ('*').Or (TB ('-')).Or (TB ('_'))
				 from endPos in Parser.Position<char> ()
				 from sp in OptionalSpace
				 from nl in SP.NewLine
				 select ThematicBreak (startPos, endPos, rule + sp + nl))
				.Trace ("ThemaBreak");
			/*
			### Inlines

			#### Determination Rules
			*/
			var NotAtEnd = SP.AnyChar.Trace ("NotAtEnd");

			var AtxStart =
				(from ni in NonindentSpace
				 from cs in SP.Char ('#').Occurrences (1, 6)
				 from ws in SP.WhiteSpace.And ()
				 select cs.Count ())
				.Trace ("AtxStart");

			var IsBlockQuote =
				(from ni in NonindentSpace
				 from gt in SP.Char ('>')
				 select true)
				.Trace ("IsBlockQuote");

			var SetextUnderline =
				(from ni in NonindentSpace
				 from ul in SP.Char ('=').Or (SP.Char ('-')).OneOrMore ()
				 select ni + ul)
				.Trace ("SetextUnderline");

			var IsNormalLine =
				(from notend in NotAtEnd.And ()
				 from notbl in SP.BlankLine ().Not ()
				 from notgt in IsBlockQuote.Not ()
				 from notatx in AtxStart.Not ()
				 from notsetext in SetextUnderline.Not ()
				 from notthmbrk in ThemaBreak.Not ()
				 select true)
				.Trace ("IsNormalLine");
			/*
			#### Line Breaks
			*/
			var EndLine =
				(from sp in OptionalSpace
				 from nl in SP.NewLine
				 from next in IsNormalLine
				 from ws in OptionalSpace
				 select sp + nl + ws)
				.Trace ("EndLine");

			var SoftLB =
				(from startPos in Parser.Position<char> ()
				 from el in EndLine
				 from endPos in Parser.Position<char> ()
				 select SoftLineBreak (startPos, endPos, el))
				.Trace ("SoftLB");

			var HardLB =
				(from startPos in Parser.Position<char> ()
				 from hb in SP.String ("  ").Or (SP.String ("\\"))
				 from el in EndLine
				 from endPos in Parser.Position<char> ()
				 select HardLineBreak (startPos, endPos, hb + el))
				.Trace ("HardLB");

			var LineBreak = HardLB.Or (SoftLB).Trace ("LineBreak");
			/*
			#### Escaped Characters
			*/
			var EscapedChar =
				(from bs in SP.Char ('\\')
				 from sc in SP.OneOf ('!', '"', '#', '$', '%', '&', '\'', '(', ')',
					 '*', '+', ',', '-', '.', '/', ':', ';', '<', '=', '>', '?',
					 '@', '[', '\\', ']', '^', '_', '`', '{', '|', '}', '~')
				 select new string (sc, 1))
				.Trace ("EscapedChar");
			/*
			#### Unformatted Text
			*/
			var UnformattedText =
				(from startPos in Parser.Position<char> ()
				 from chars in NormalChar.OneOrMore ()
				 from endPos in Parser.Position<char> ()
				 select Text (startPos, endPos, chars.AsString ()))
				.Trace ("UnformattedText");
			/*
			#### Space between Words
			*/
			var SpaceBetweenWords =
				(from startPos in Parser.Position<char> ()
				 from sp in SP.SpacesOrTabs
				 from endPos in Parser.Position<char> ()
				 select Space (startPos, endPos, sp))
				.Trace ("SpaceBetweenWords");
			/*
			#### Punctuation
			*/
			var Punct =
				(from punc in SP.Punctuation
				 from pos in Parser.Position<char> ()
				 select Punctuation (pos, punc))
				.Trace ("Punct");
			/*
			#### Emphasis
			*/
			var WS = Parser.Satisfy<char> (c => char.IsWhiteSpace (c) || c == '\0');
			var NonWS = Parser.Satisfy<char> (c => !char.IsWhiteSpace (c) && c != '\0');
			var WSorPunct = WS.Or (SP.Punctuation);

			Parser<long, char> DelimFollowedBy (char delim, 
				Parser<char, char> parser) =>
				(from skip in SP.Char (delim).ZeroOrMore ()
				 from p in Parser.Position<char> ()
				 from c in parser
				 select p)
				.And ();

			Parser<long, char> DelimPrecededBy (char delim,
				Parser<char, char> parser) =>
				(from skip in SP.Char (delim).ZeroOrMore ()
				 from p in Parser.Position<char> ()
				 from c in parser
				 select p)
				.LookBack ();

			Parser<int, char> LeftFlankDelimRun (char delim) =>
				(from f in DelimFollowedBy (delim, NonWS)
				 from a in DelimFollowedBy (delim, SP.NotPunctuation)
					.Or (DelimPrecededBy (delim, WSorPunct))
				 from p in DelimPrecededBy (delim, SP.AnyChar)
				 select (int)(f - p) + 1)
				.Trace ("LeftFlankDelimRun " + new string (delim, 1));

			Parser<int, char> RightFlankDelimRun (char delim) =>
				(from p in DelimPrecededBy (delim, NonWS)
				 from a in DelimPrecededBy (delim, SP.NotPunctuation)
					.Or (DelimFollowedBy (delim, WSorPunct))
				 from f in DelimFollowedBy (delim, SP.AnyChar)
				 select (int)(f - p) + 1)
				.Trace ("RightFlankDelimRun " + new string (delim, 1));

			var Inline = new Ref<Parser<string, char>> ();

			var AsteriskEmphEnd = RightFlankDelimRun ('*');

			var UnderscoreEmphEnd =
				from rfd in RightFlankDelimRun ('_')
				from notlfd in LeftFlankDelimRun ('_').Not ()
				   .Or (DelimFollowedBy ('_', SP.Punctuation).Select (_ => 0))
				select rfd;

			Parser<int, char> ClosingDelim (string delim, int lfd,
				Parser<int, char> terminator) =>
				from d in SP.String (delim)
				from cfd in terminator
				//where (lfd + cfd) % 3 != 0
				select cfd;

			Parser<string, char> EmphInlines (string delim, int lfd,
				Parser<int, char> terminator) =>
				(from il in ClosingDelim (delim, lfd, terminator).Not ()
					.Then (Inline.ForwardRef ())
					.OneOrMore ()
				 select il.ToString ("", "", ""))
				.Trace ("EmphInlines");

			Parser<string, char> AsteriskEmph (int cnt,
				Func<long, long, string, string> transform)
			{
				var delim = new string ('*', cnt);
				var emphDelim = SP.String (delim).Trace ("EmphDelim " + delim);
				return
					(from od in emphDelim
					 from lfd in LeftFlankDelimRun ('*')
					 from startPos in Parser.Position<char> ()
					 from ils in EmphInlines (delim, lfd, AsteriskEmphEnd)
					 from endPos in Parser.Position<char> ()
					 from cd in emphDelim
					 from rfd in AsteriskEmphEnd
					 select transform (startPos, endPos, ils))
					.Trace ("AsteriskEmph");
			}

			Parser<string, char> UnderscoreEmph (int cnt,
				Func<long, long, string, string> transform)
			{
				var delim = new string ('_', cnt);
				var emphDelim = SP.String (delim).Trace ("EmphDelim " + delim);
				return
					(from od in emphDelim
					 from lfd in LeftFlankDelimRun ('_')
					 from notrfd in RightFlankDelimRun ('_').Not ()
						.Or (DelimPrecededBy ('_', SP.Punctuation).Select (_ => 0))
					 from startPos in Parser.Position<char> ()
					 from ils in EmphInlines (delim, lfd, UnderscoreEmphEnd)
					 from endPos in Parser.Position<char> ()
					 from cd in emphDelim
					 from rfd in UnderscoreEmphEnd
					 select transform (startPos, endPos, ils))
					.Trace ("UnderscoreEmph");
			}

			var Emphasized =
				AsteriskEmph (2, StrongEmphasis)
				.Or (UnderscoreEmph (2, StrongEmphasis))
				.Or (AsteriskEmph (1, Emphasis))
				.Or (UnderscoreEmph (1, Emphasis))
				.Trace ("Emphasized");
			/*
			#### Main Inline Selector
			*/
			Inline.Target =
				LineBreak
				.Or (Emphasized)
				.Or (SpaceBetweenWords)
				.Or (EscapedChar)
				.Or (Punct)
				.Or (UnformattedText)
				.Trace ("Inline");

			var Inlines =
				(from il in Inline.Target.OneOrMore ()
				 select il.ToString ("", "", ""))
				.Trace ("Inlines"); ;
			/*
			### Headings

			#### ATX Headings
			*/
			var AtxEnd =
				(SP.SpacesOrTabs
					.Then (SP.Char ('#').OneOrMore ())
					.Then (OptionalSpace)
					.Then (SP.NewLine))
				.Or (OptionalSpace
					.Then (SP.NewLine))
				.Trace ("AtxEnd");

			var AtxInline =
				(from notAtEnd in AtxEnd.Not ()
				 from inline in Inline.Target
				 select inline)
				.Trace ("AtxInline");

			var AtxHeading =
				(from startPos in Parser.Position<char> ()
				 from level in AtxStart
				 from inlines in AtxInline.ZeroOrMore ()
				 from atxend in AtxEnd
				 from endPos in Parser.Position<char> ()
				 select Heading (startPos, endPos, level,
					 inlines.IsEmpty () ? "" : 
					 (inlines.First.All (char.IsWhiteSpace) ? 
						inlines.Rest : inlines).ToString ("", "", "")))
				.Trace ("AtxHeading");
			/*
			#### Setext Headings
			*/
			var SetextInline =
				(from notAtEnd in EndLine.Not ()
				 from inline in Inline.Target
				 select inline)
				.Trace ("SetextInline");

			var SetextLine =
				(from normal in IsNormalLine
				 from line in Line
				 select line)
				.Trace ("SetextLine");

			var IsSetextHeading =
				(from lines in SetextLine.OneOrMore ()
				 from ul in SetextUnderline
				 select true)
				.Trace ("IsSetextHeading");

			var SetextHeading =
				(from issetext in IsSetextHeading.And ()
				 from ni in NonindentSpace
				 from startPos in Parser.Position<char> ()
				 from inlines in SetextInline.OneOrMore ()
				 from sp in OptionalSpace
				 from nl in SP.NewLine
				 from ul in SetextUnderline
				 from endPos in Parser.Position<char> ()
				 select Heading (startPos, endPos, ul.Contains ("=") ? 1 : 2,
					 inlines.ToString ("", "", "")))
				.Trace ("SetextHeading");
			/*
			### Paragraphs
			*/
			var Para =
				(from ni in NonindentSpace
				 from startPos in Parser.Position<char> ()
				 from inlines in Inlines
				 from endPos in Parser.Position<char> ()
				 select Paragraph (startPos, endPos, inlines))
				.Trace ("Para");

			var AnyBlock =
				(from blanks in SP.BlankLine ().ZeroOrMore ()
				 from notend in NotAtEnd.And ()
				 from startPos in Parser.Position<char> ()
				 from block in VerbatimBlock
					 .Or (AtxHeading)
					 .Or (SetextHeading)
					 .Or (ThemaBreak)
					 .Or (Para)
				 from endPos in Parser.Position<char> ()
				 select Block (startPos, endPos, block))
				.Trace ("AnyBlock");

			return
				(from blocks in AnyBlock.ZeroOrMore ()
				 select blocks.IsEmpty () ? "" : blocks.SeparateWith (""))
				.Trace ("Doc");
		}
	}
}
