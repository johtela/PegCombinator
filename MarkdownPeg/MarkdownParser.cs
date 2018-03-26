namespace MarkdownPeg
{
	using System;
	using System.Linq;
	using ExtensionCord;
	using PegCombinator;
	using SP = PegCombinator.StringParser;

	public class MarkdownParser
    {

		protected virtual string Heading (object start, object end, int headingLevel,
			string headingText) =>
			"#".Times (headingLevel) + " " + headingText;

		private Parser<string, char> Doc () => null;

		private Parser<string, char> Block () => 
				AtxHeading ();

		private Parser<int, char> AtxStart () =>
			from cs in SP.Char ('#').Occurrences (1, 6)
			select cs.Count ();

		private Parser<string, char> AtxInline () =>
			from nl in SP.NewLine ().Not ()
			from notext in Parser.Not (
				WS ()
				.Then (SP.Char ('#').ZeroOrMore ())
				.Then (WS ())
				.Then (SP.NewLine ()))
			from inline in Inline ()
			select inline;

		private Parser<string, char> AtxHeading () =>
			from start in Parser.Position<char> ()
			from level in AtxStart ()
			from inline in AtxInline ()
			from end in Parser.Position<char> ()
			select Heading (start, end, level, inline);

		private Parser<string, char> Inline () => 
			SP.SpacesOrTabs ()
			.Or (Text ());

		private Parser<string, char> Text () =>
			from nc in NormalChar ()
			from cs in Parser.NotSatisfy<char> (char.IsWhiteSpace).ZeroOrMore ()
			select (nc | cs).ToString ("", "", "");

		private Parser<char, char> SpecialChar () => SP.OneOf ('~', '*', '_', '`', '&',
			'[', ']', '(', ')', '<', '!', '#', '\\', '\'', '"');	

		private Parser<char, char> NormalChar () =>
			Parser.Not (SpecialChar ().Or (SP.WhitespaceChar ())).Then (SP.AnyChar ());

		private Parser<string, char> WS () => SP.WhiteSpace ().Optional ("");
	}
}
