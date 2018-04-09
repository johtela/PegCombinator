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
				.Trace ("SpaceBetweenWords");
			/*
			#### Emphasis
			*/
			var WSorPunct = SP.WhitespaceChar.Or (SP.Punctuation);

			Parser<string, char> LeftFlankDelim (char emphChar, int cnt) =>
				from check in (SP.Char (emphChar).Or (WSorPunct).And (lookback: 1))
					.Or (SP.Char (emphChar).OneOrMore ().Then (WSorPunct.Not ()).And ())
				from emph in SP.String (new string (emphChar, cnt))
				select emph;

			Parser<string, char> RightFlankDelim (char emphChar, int cnt) =>
				from check in (SP.Char (emphChar).Or (WSorPunct.Not ()).And (lookback: 1))
					.Or (SP.Char (emphChar).OneOrMore ().Then (WSorPunct).And ())
				from emph in SP.String (new string (emphChar, cnt))
				select emph;

			var Inlines = new Ref<Parser<string, char>> ();

			var Emph =
				from lfd in LeftFlankDelim ('*', 1)
				from startPos in Parser.Position<char> ()
				from ils in Inlines.ForwardRef ()
				from endPos in Parser.Position<char> ()
				from rfd in RightFlankDelim ('*', 1)
				select Emphasis (startPos, endPos, ils);

			/*
			#### Main Inline Selector
			*/
			var Inline =
				LineBreak
				.Or (Emph)
				.Or (SpaceBetweenWords)
				.Or (EscapedChar)
				.Or (Punct)
				.Or (UnformattedText)
				.Trace ("Inline");

			Inlines.Target =
				(from il in Inline.OneOrMore ()
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
				 from inline in Inline
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
				 from inline in Inline
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
				 from inlines in Inlines.Target
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
