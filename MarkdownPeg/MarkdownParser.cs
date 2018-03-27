namespace MarkdownPeg
{
	using System;
	using System.Linq;
	using ExtensionCord;
	using PegCombinator;
	using SP = PegCombinator.StringParser;

	public class MarkdownParser
    {
		protected virtual string Block (object start, object end, string blockText) =>
			blockText;

		protected virtual string Heading (object start, object end, 
			int headingLevel, string headingText) =>
			"#".Times (headingLevel) + " " + headingText;

		protected virtual string Text (object start, object end, string text) =>
			text;

		private Parser<Seq<string>, char> Doc () => 
			Block ().ZeroOrMore ();

		private Parser<string, char> Block () => 
			from blanks in SP.BlankLine ().ZeroOrMore ()
			from startPos in Parser.Position<char> ()
			from block in AtxHeading ()
			from endPos in Parser.Position<char> ()
			select Block (startPos, endPos, block);

		private Parser<int, char> AtxStart () =>
			from cs in SP.Char ('#').Occurrences (1, 6)
			from sp in SP.SpacesOrTabs ()
			select cs.Count ();

		private Parser<string, char> AtxEnd () =>
			Sp ()
			.Then (SP.Char ('#').ZeroOrMore ())
			.Then (Sp ())
			.Optional ("");

		private Parser<string, char> AtxInline () =>
			from notAtEnd in AtxEnd ().Then (SP.NewLine ()).Not ()
			from inline in Inline ()
			select inline;

		private Parser<string, char> AtxHeading () =>
			from ni in NonindentSpace ()
			from startPos in Parser.Position<char> ()
			from level in AtxStart ()
			from inlines in AtxInline ().OneOrMore ()
			from atxend in AtxEnd ()
			from nl in SP.NewLine ()
			from endPos in Parser.Position<char> ()
			select Heading (startPos, endPos, level, inlines.ToString ("", "", ""));

		private Parser<string, char> Inline () => 
			SP.SpacesOrTabs ()
			.Or (Text ());

		private Parser<string, char> Text () =>
			from startPos in Parser.Position<char> ()
			from nc in NormalChar ()
			from cs in Parser.NotSatisfy<char> (char.IsWhiteSpace).ZeroOrMore ()
			from endPos in Parser.Position<char> ()
			select Text(startPos, endPos, (nc | cs).AsString ());

		private Parser<char, char> SpecialChar () => SP.OneOf ('~', '*', '_', '`', '&',
			'[', ']', '(', ')', '<', '!', '#', '\\', '\'', '"');	

		private Parser<char, char> NormalChar () =>
			Parser.Not (SpecialChar ().Or (SP.WhitespaceChar ())).Then (SP.AnyChar ());

		private Parser<string, char> Sp () => SP.SpacesOrTabs ().Optional ("");

		private Parser<string, char> NonindentSpace () =>
			from sp in SP.Char (' ').Occurrences (0, 3)
			select sp.AsString ();
	}
}
