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

		protected virtual string Heading (long start, long end,
			int headingLevel, string headingText) =>
			"#".Times (headingLevel) + " " + headingText;

		protected virtual string Verbatim (long start, long end,
			string verbatimText) => verbatimText;

		protected virtual string Text (long start, long end, 
			string text) => text;

		protected virtual string Space (long start, long end,
			string text) => text;

		protected virtual string SoftLineBreak (long start, long end,
			string text) => text;

		protected virtual string HardLineBreak (long start, long end,
			string text) => text;

		/*
		## Parsing Rules
		*/
		private Parser<string, char> Doc ()
		{
			/*
			### Special and Normal Characters
			*/
			var SpecialChar =
				SP.OneOf ('~', '*', '_', '`', '&', '[', ']', '(', ')', '<', '!', 
					'#', '\\', '\'', '"');

			var NormalChar =
				Parser.Not (SpecialChar.Or (SP.WhitespaceChar)).Then (SP.AnyChar);
			/*
			### Whitespace
			*/
			var OptionalSpace = SP.SpacesOrTabs.Optional ("");

			var NonindentSpace =
				from sp in SP.Char (' ').Occurrences (0, 3)
				select sp.AsString ();
			/*
			### Text Lines
			*/
			var Line =
				from chs in SP.NoneOf ('\r', '\n').ZeroOrMore ()
				from nl in SP.NewLine
				select chs.IsEmpty () ? nl : chs.ToString ("", "", nl);

			var NonblankLine =
				from nb in SP.BlankLine ().Not ()
				from ln in Line
				select ln;
			/*
			### Indented Code Blocks
			*/
			var Indent = 
				NonindentSpace.Then(SP.String ("\t"))
				.Or (SP.String ("    "));

			var IndentedLine =
				Indent.Then (Line);

			var NonblankIndentedLine =
				from nb in SP.BlankLine ().Not ()
				from il in IndentedLine
				select il;

			var VerbatimChunk =
				from bls in SP.BlankLine (true).ZeroOrMore ()
				from text in NonblankIndentedLine.OneOrMore ()
				select (bls.IsEmpty () ? text :
					bls.Select (_ => Environment.NewLine).Concat (text))
					.SeparateWith ("");

			var VerbatimBlock =
				from startPos in Parser.Position<char> ()
				from chunks in VerbatimChunk.OneOrMore ()
				from endPos in Parser.Position<char> ()
				select Verbatim (startPos, endPos, chunks.SeparateWith (""));
			/*
			### Inlines

			#### Determination Rules
			*/
			var AtxStart =
				from ni in NonindentSpace
				from cs in SP.Char ('#').Occurrences (1, 6)
				from sp in SP.SpacesOrTabs
				select cs.Count ();

			var IsBlockQuote =
				from ni in NonindentSpace
				from gt in SP.Char ('>')
				select true;

			var SetextUnderline =
				from ni in NonindentSpace
				from ul in SP.Char ('=').Or (SP.Char ('-')).OneOrMore ()
				select ni + ul;

			var IsNormalLine =
				from notbl in SP.BlankLine ().Not ()
				from notgt in IsBlockQuote.Not ()
				from notatx in AtxStart.Not ()
				from notsetext in SetextUnderline.Not ()
				select true;
			/*
			#### Line Breaks
			*/
			var EndLine =
				from sp in OptionalSpace
				from nl in SP.NewLine
				from next in IsNormalLine
				from ws in OptionalSpace
				select sp + nl + ws;

			var SoftLB =
				from startPos in Parser.Position<char> ()
				from el in EndLine
				from endPos in Parser.Position<char> ()
				select SoftLineBreak (startPos, endPos, el);

			var HardLB =
				from startPos in Parser.Position<char> ()
				from hb in SP.String ("  ").Or (SP.String ("\\"))
				from el in EndLine
				from endPos in Parser.Position<char> ()
				select HardLineBreak (startPos, endPos, hb + el);

			var LineBreak = HardLB.Or (SoftLB);

			//var Para =
			//	from ni in NonindentSpace
			//	from inlines in Inline.
			/*
			#### Unformatted Words
			*/
			var Word =
				from startPos in Parser.Position<char> ()
				from nc in NormalChar
				from cs in Parser.NotSatisfy<char> (char.IsWhiteSpace).ZeroOrMore ()
				from endPos in Parser.Position<char> ()
				select Text (startPos, endPos, (nc | cs).AsString ());
			/*
			#### Space between Words
			*/
			var SpaceBetweenWords =
				from startPos in Parser.Position<char> ()
				from sp in SP.SpacesOrTabs
				from endPos in Parser.Position<char> ()
				select Space (startPos, endPos, sp);
			/*
			#### Main Inline Selector
			*/
			var Inline =
				LineBreak
				.Or (SpaceBetweenWords)
				.Or (Word);
			/*
			### Headings

			#### ATX Headings
			*/
			var AtxEnd =
				OptionalSpace
				.Then (SP.Char ('#').ZeroOrMore ())
				.Then (OptionalSpace)
				.Optional ("");

			var AtxInline =
				from notAtEnd in AtxEnd.Then (SP.NewLine).Not ()
				from inline in Inline
				select inline;

			var AtxHeading =
				from startPos in Parser.Position<char> ()
				from level in AtxStart
				from inlines in AtxInline.OneOrMore ()
				from atxend in AtxEnd
				from nl in SP.NewLine
				from endPos in Parser.Position<char> ()
				select Heading (startPos, endPos, level, inlines.ToString ("", "", ""));
			/*
			#### Setext Headings
			*/
			var SetextInline =
				from notAtEnd in EndLine.Not ()
				from inline in Inline
				select inline;

			var SetextLine =
				from normal in IsNormalLine
				from line in Line
				select line;

			var IsSetextHeading =
				from lines in SetextLine.OneOrMore ()
				from ul in SetextUnderline
				select true;

			var SetextHeading =
				from issetext in IsSetextHeading.And ()
				from ni in NonindentSpace
				from startPos in Parser.Position<char> ()
				from inlines in SetextInline.OneOrMore ()
				from sp in OptionalSpace
				from nl in SP.NewLine
				from ul in SetextUnderline
				from endPos in Parser.Position<char> ()
				select Heading (startPos, endPos, ul.Contains ("=") ? 1 : 2, 
					inlines.ToString ("", "", ""));

			var AnyBlock =
				from blanks in SP.BlankLine ().ZeroOrMore ()
				from startPos in Parser.Position<char> ()
				from block in VerbatimBlock
					.Or (AtxHeading)
					.Or (SetextHeading)
				from endPos in Parser.Position<char> ()
				select Block (startPos, endPos, block);

			return
				from blocks in AnyBlock.ZeroOrMore ()
				select blocks.IsEmpty () ? "" : blocks.SeparateWith ("");
		}
	}
}
