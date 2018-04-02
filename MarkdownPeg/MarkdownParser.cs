namespace MarkdownPeg
{
	using System;
	using System.Linq;
	using ExtensionCord;
	using PegCombinator;
	using SP = PegCombinator.StringParser;

	public class MarkdownVisitor
	{
		/*
		## Visitor Methods
		*/
		public virtual string Block (object start, object end,
			string blockText) => blockText;

		public virtual string Heading (object start, object end,
			int headingLevel, string headingText) =>
			"#".Times (headingLevel) + " " + headingText;

		public virtual string Verbatim (object start, object end,
			string verbatimText) => verbatimText;

		public virtual string Text (object start, object end, string text) =>
			text;
	}

	public static class MarkdownParser
    {
		private static MarkdownVisitor _visitor;

		public static string Run (IParserInput<char> input, MarkdownVisitor visitor)
		{
			_visitor = visitor;
			return Doc.Parse (input);
		}

		/*
		## Parsing Rules
		*/
		private static Parser<string, char> Doc =
			from blocks in Block.ZeroOrMore ()
			select blocks.IsEmpty () ? "" : blocks.SeparateWith ("");

		private static Parser<string, char> Block =
			from blanks in SP.BlankLine ().ZeroOrMore ()
			from startPos in Parser.Position<char> ()
			from block in AtxHeading
				.Or (Verbatim)
			from endPos in Parser.Position<char> ()
			select _visitor.Block (startPos, endPos, block);

		private static Parser<int, char> AtxStart =
			from cs in SP.Char ('#').Occurrences (1, 6)
			from sp in SP.SpacesOrTabs ()
			select cs.Count ();

		private static Parser<string, char> AtxEnd =
			Sp
			.Then (SP.Char ('#').ZeroOrMore ())
			.Then (Sp)
			.Optional ("");

		private static Parser<string, char> AtxInline =
			from notAtEnd in AtxEnd.Then (SP.NewLine ()).Not ()
			from inline in Inline
			select inline;

		private static Parser<string, char> AtxHeading =
			from ni in NonindentSpace
			from startPos in Parser.Position<char> ()
			from level in AtxStart
			from inlines in AtxInline.OneOrMore ()
			from atxend in AtxEnd
			from nl in SP.NewLine ()
			from endPos in Parser.Position<char> ()
			select _visitor.Heading (startPos, endPos, level, inlines.ToString ("", "", ""));

		private static Parser<string, char> Inline =
			SP.SpacesOrTabs ()
			.Or (Text);

		private static Parser<string, char> Text =
			from startPos in Parser.Position<char> ()
			from nc in NormalChar
			from cs in Parser.NotSatisfy<char> (char.IsWhiteSpace).ZeroOrMore ()
			from endPos in Parser.Position<char> ()
			select _visitor.Text (startPos, endPos, (nc | cs).AsString ());

		private static Parser<char, char> SpecialChar =
			SP.OneOf ('~', '*', '_', '`', '&', '[', ']', '(', ')', '<', '!', '#', '\\', '\'', '"');	

		private static Parser<char, char> NormalChar =
			Parser.Not (SpecialChar.Or (SP.WhitespaceChar ())).Then (SP.AnyChar ());

		private static Parser<string, char> Sp = 
			SP.SpacesOrTabs ().Optional ("");

		private static Parser<string, char> VerbatimChunk =
			from bls in SP.BlankLine (true).ZeroOrMore ()
			from text in NonblankIndentedLine.OneOrMore ()
			select (bls.IsEmpty () ? text :
				bls.Select (_ => Environment.NewLine).Concat (text))
				.SeparateWith ("");

		private static Parser<string, char> Verbatim =
			from startPos in Parser.Position<char> ()
			from chunks in VerbatimChunk.OneOrMore ()
			from endPos in Parser.Position<char> ()
			select _visitor.Verbatim (startPos, endPos, chunks.SeparateWith (""));

		private static Parser<string, char> NonblankIndentedLine =
			from nb in SP.BlankLine ().Not ()
			from il in IndentedLine
			select il;

		private static Parser<string, char> IndentedLine =
			Indent.Then (Line);

		private static Parser<string, char> NonindentSpace =
			from sp in SP.Char (' ').Occurrences (0, 3)
			select sp.AsString ();

		private static Parser<string, char> Indent =
			SP.String ("\t").Or (SP.String ("    "));

		private static Parser<string, char> Line =
			from chs in SP.NoneOf ('\r', '\n').ZeroOrMore ()
			from nl in SP.NewLine ()
			select chs.ToString ("", "", nl);
	}
}
