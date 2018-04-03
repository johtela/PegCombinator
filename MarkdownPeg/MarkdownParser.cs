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

		protected virtual string Text (long start, long end, string text) =>
			text;

		/*
		## Parsing Rules
		*/
		private Parser<string, char> Doc ()
		{
			var SpecialChar =
				SP.OneOf ('~', '*', '_', '`', '&', '[', ']', '(', ')', '<', '!', 
					'#', '\\', '\'', '"');

			var NormalChar =
				Parser.Not (SpecialChar.Or (SP.WhitespaceChar)).Then (SP.AnyChar);

			var NormalText =
				from startPos in Parser.Position<char> ()
				from nc in NormalChar
				from cs in Parser.NotSatisfy<char> (char.IsWhiteSpace).ZeroOrMore ()
				from endPos in Parser.Position<char> ()
				select Text (startPos, endPos, (nc | cs).AsString ());

			var Sp = SP.SpacesOrTabs.Optional ("");

			var NonindentSpace =
				from sp in SP.Char (' ').Occurrences (0, 3)
				select sp.AsString ();

			var Indent = 
				NonindentSpace.Then(SP.String ("\t"))
				.Or (SP.String ("    "));

			var Line =
				from chs in SP.NoneOf ('\r', '\n').ZeroOrMore ()
				from nl in SP.NewLine
				select chs.ToString ("", "", nl);

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

			var Inline =
				SP.SpacesOrTabs
				.Or (NormalText);

			var AtxEnd =
				Sp
				.Then (SP.Char ('#').ZeroOrMore ())
				.Then (Sp)
				.Optional ("");

			var AtxInline =
				from notAtEnd in AtxEnd.Then (SP.NewLine).Not ()
				from inline in Inline
				select inline;

			var AtxStart =
				from cs in SP.Char ('#').Occurrences (1, 6)
				from sp in SP.SpacesOrTabs
				select cs.Count ();

			var AtxHeading =
				from ni in NonindentSpace
				from startPos in Parser.Position<char> ()
				from level in AtxStart
				from inlines in AtxInline.OneOrMore ()
				from atxend in AtxEnd
				from nl in SP.NewLine
				from endPos in Parser.Position<char> ()
				select Heading (startPos, endPos, level, inlines.ToString ("", "", ""));

			//var Para =
			//	from ni in NonindentSpace
			//	from inlines in Inline.

			var AnyBlock =
				from blanks in SP.BlankLine ().ZeroOrMore ()
				from startPos in Parser.Position<char> ()
				from block in VerbatimBlock
					.Or (AtxHeading)
				from endPos in Parser.Position<char> ()
				select Block (startPos, endPos, block);

			return
				from blocks in AnyBlock.ZeroOrMore ()
				select blocks.IsEmpty () ? "" : blocks.SeparateWith ("");
		}
	}
}
