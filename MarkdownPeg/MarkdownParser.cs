namespace MarkdownPeg
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Text;
	using ExtensionCord;
	using PegCombinator;
	using SP = PegCombinator.StringParser;

	public class MarkdownParser
    {
		private Parser<StringTree, char> _doc;

		public MarkdownParser () => 
			_doc = Doc ();

		public string Run (IParserInput<char> input) => 
			_doc.Parse (input.TerminateWith ('\n')).ToString ();

		public string Run (string input) => 
			Run (ParserInput.String (input));

		/*
		## Visitor Methods
		*/
		protected virtual StringTree Block (long start, long end,
			StringTree blockText) => blockText;

		protected virtual StringTree ThematicBreak (long start, long end,
			StringTree text) => text;

		protected virtual StringTree Heading (long start, long end,
			int headingLevel, StringTree headingText) =>
			"#".Times (headingLevel) + " " + headingText + Environment.NewLine;

		protected virtual StringTree Verbatim (long start, long end,
			StringTree verbatimText) => verbatimText;

		protected virtual StringTree Paragraph (long start, long end,
			StringTree text) => text;

		protected virtual StringTree Text (long start, long end, 
			StringTree text) => text;

		protected virtual StringTree Space (long start, long end,
			StringTree text) => text;

		protected virtual StringTree Punctuation (long pos, char punctuation) => 
			new string (punctuation, 1);

		protected virtual StringTree SoftLineBreak (long start, long end,
			StringTree text) => text;

		protected virtual StringTree HardLineBreak (long start, long end,
			StringTree text) => text;

		protected virtual StringTree Emphasis (long start, long end,
			StringTree text) => text;

		protected virtual StringTree StrongEmphasis (long start, long end,
			StringTree text) => text;

		protected virtual StringTree Link (long start, long end, StringTree text,
			string dest, string title) =>
			"[" + text + "](" + dest + " \"" + title + "\"";

		/*
		## Helpers
		*/
		private string DecodeUri (string uri) => 
			Uri.UnescapeDataString (WebUtility.HtmlDecode (uri));

		private string DecodeLinkTitle (string title) =>
			title == null ? null : WebUtility.HtmlDecode (title);

		private string EscapeBackslashes (string str)
		{
			var res = new StringBuilder ();
			var i = 0;
			while (i < str.Length)
				if (str[i] == '\\' && i < str.Length - 1)
				{
					res.Append (str[i + 1]);
					i += 2;
				}
				else
					res.Append (str[i++]);
			return res.ToString ();
		}
		/*
		## Parsing State
		*/
		private class LinkReference
		{
			public readonly string Label;
			public readonly string Destination;
			public readonly string Title;

			public LinkReference (string label, string dest, string title)
			{
				Label = label;
				Destination = dest;
				Title = title;
			}
		}

		private class ParseState
		{
			private Dictionary<string, LinkReference> _linkReferences =
				new Dictionary<string, LinkReference> ();

			public void AddLinkReference (string label, string dest,
				string title)
			{
				if (!_linkReferences.ContainsKey (label))
					_linkReferences.Add (label,
						new LinkReference (label, dest, title));
			}

			public LinkReference GetLinkReference (string label) =>
				_linkReferences.TryGetValue (label, out var res) ?
					res : null;
		}

		/*
		## Parsing Rules
		*/
		private Parser<StringTree, char> Doc ()
		{
			Parser.Debugging = false;
			Parser.UseMemoization = false;
			/*
			### Special and Normal Characters
			*/
			var PunctChar = SP.Punctuation.Or (SP.OneOf ('<', '>'));

			var NormalChar =
				PunctChar
				.Or (SP.WhitespaceChar)
				.Not ()
				.Then (SP.AnyChar)
				.Trace ("NormalChar");
			/*
			### Whitespace
			*/
			var OptionalSpace = SP.SpacesOrTabs.Optional ("").Trace ("OptionalSpace");

			var NonindentSpace =
				(from sp in SP.Char (' ').Occurrences (0, 3)
				 select sp.AsString ())
				.Trace ("NonindentSpace");

			var OptionalWsWithUpTo1NL =
				(from ws1 in OptionalSpace
				 from nl in SP.NewLine.Optional ("")
				 from ws2 in OptionalSpace
				 select ws1 + nl + ws2)
				.Trace ("WsWithUpTo1NL");
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
					 .FromEnumerable ())
				.Trace ("VerbatimChunk");

			var VerbatimBlock =
				(from startPos in Parser.Position<char> ()
				 from chunks in VerbatimChunk.OneOrMore ()
				 from endPos in Parser.Position<char> ()
				 select Verbatim (startPos, endPos, chunks.FromEnumerable ()))
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
				select  rest.ToString (new string (ch, 1), "", "");

			var ThemaBreak =
				(from ni in NonindentSpace
				 from startPos in Parser.Position<char> ()
				 from rule in TB ('*').Or (TB ('-')).Or (TB ('_'))
				 from endPos in Parser.Position<char> ()
				 from sp in OptionalSpace
				 from nl in SP.NewLine
				 select ThematicBreak (startPos, endPos, 
					(StringTree)rule + sp + nl))
				.Trace ("ThemaBreak");
			/*
			### Inlines

			#### Determination Rules
			*/
			var NotAtEnd = SP.AnyChar.Trace ("NotAtEnd");

			Parser<string, char> AtEnd (string res) =>
				SP.AnyChar.Not ().Select (_ => res);

			var AtxStart =
				(from ni in NonindentSpace
				 from cs in SP.Char ('#').Occurrences (1, 6)
				 from ws in SP.Whitespace ().And ()
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
				 select (StringTree)sp + nl + ws)
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
			var PossibleEscapes = SP.OneOf ('!', '"', '#', '$', '%', '&', '\'', '(', ')',
					 '*', '+', ',', '-', '.', '/', ':', ';', '<', '=', '>', '?',
					 '@', '[', '\\', ']', '^', '_', '`', '{', '|', '}', '~');

			var EscapedChar =
				(from bs in SP.Char ('\\')
				 from sc in PossibleEscapes
				 select sc)
				.Trace ("EscapedChar");

			var EscapedCharWithBackslash =
				EscapedChar.Select (ch => "\\" + ch);

			var BackslashEscape =
				from pos in Parser.Position<char> ()
				from c in EscapedChar
				select char.IsPunctuation (c) || c.In ('<','>') ?	
					Punctuation (pos, c) : c;
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
				(from punc in PunctChar
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

			var Inline = new Ref<Parser<StringTree, char>> ();

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
				from canopen in LeftFlankDelimRun (delim[0]).OptionalVal ()
				where !canopen.HasValue || (lfd + cfd) % 3 != 0
				select cfd;

			Parser<StringTree, char> EmphInlines (string delim, int lfd,
				Parser<int, char> terminator) =>
				(from il in ClosingDelim (delim, lfd, terminator).Not ()
					.Then (Inline.ForwardRef ())
					.OneOrMore ()
				 select il.FromEnumerable ())
				.Trace ("EmphInlines");

			Parser<StringTree, char> AsteriskEmph (int cnt,
				Func<long, long, StringTree, StringTree> transform)
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

			Parser<StringTree, char> UnderscoreEmph (int cnt,
				Func<long, long, StringTree, StringTree> transform)
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
			#### Links
			*/
			var Brackets = SP.OneOf ('[', ']');

			var LinkText =
				(from op in SP.Char ('[')
				 from ilb in SP.Char (']').Not ()
					.Then (Inline.ForwardRef ())
					.ZeroOrMore ()
				 from cp in SP.Char (']')
				 select ilb.FromEnumerable ())
				.Trace ("LinkText");

			var LinkDestAngle =
				(from op in SP.Char ('<')
				 from chs in SP.NoneOf (' ', '\t', '\n', '\r', '\\', '<', '>')
					 .Or (EscapedChar)
					 .ZeroOrMore ()
				 from cp in SP.Char ('>')
				 select chs.AsString ())
				.Trace ("LinkDestAngle");

			var LinkDestPart =
				(from chs in SP.AsciiWhitespaceChar
					.Or (SP.AsciiControl)
					.Or (SP.OneOf ('(', ')'))
					.Not ()
					.Then (EscapedChar.Or (SP.AnyChar)).ZeroOrMore ()
				 select chs.AsString ())
				.Trace ("LinkDestPart");

			var LinkDestNormal = new Ref<Parser<string, char>> ();

			var LinkDestNested =
				(from op in SP.Char ('(')
				 from link in LinkDestNormal.ForwardRef ()
				 from cp in SP.Char (')')
				 select "(" + link + ")")
				.Trace ("LinkDestNested");

			LinkDestNormal.Target =
				(from head in LinkDestPart.Optional ("")
				 from nested in LinkDestNested.Optional ("")
				 from tail in LinkDestPart.Optional ("")
				 select head + nested + tail)
				.Trace ("LinkDestNormal");

			var LinkDestination = 
				LinkDestAngle.Or (LinkDestNormal).Trace ("LinkDestination");

			Parser<Tuple<string, StringTree>, char> LTitle (char open, char close) =>
				(from oq in SP.Char (open)
				from chs in SP.BlankLine ().Not ().Then (
					EscapedChar.Or (SP.NoneOf (close))
					.ZeroOrMore ())
				from cq in SP.Char (close)
				let title = chs.AsString ()
				 select Tuple.Create (title, StringTree.From (open, title, close)))
				.Trace (string.Format ("LinkTitle {0}...{1}", open, close));

			var LinkTitle = LTitle ('\'', '\'')
				.Or (LTitle ('"', '"'))
				.Or (LTitle ('(', ')'))
				.Trace ("LinkTitle");

			Parser<StringTree, char> InlineLink (long startPos,
				StringTree text) =>
				(from op in SP.Char ('(')
				 from ws1 in SP.WhitespaceChar.ZeroOrMore ()
				 from dest in LinkDestination.OptionalRef ()
				 from ws2 in SP.WhitespaceChar.ZeroOrMore ()
				 from title in LinkTitle.OptionalRef ()
				 from ws3 in SP.WhitespaceChar.ZeroOrMore ()
				 from cp in SP.Char (')')
				 from endPos in Parser.Position<char> ()
				 select text.HasTag ("link") ?
					StringTree.From ('[', text, ']', '(', 
						ws1.AsString (), dest, ws2.AsString (),
						title == null ? "" : title.Item2, ws3.AsString (), ')') :
					Link (startPos, endPos, text,
						 DecodeUri (dest), DecodeLinkTitle (title?.Item1))
						.Tag ("link"))
				.Trace ("InlineLink");
			/*
			##### Full Reference Links
			*/
			var LinkLabel =
				(from op in SP.Char ('[')
				 from ws in SP.Whitespace ().Optional ("")
				 from chs in (
					Brackets.Not ()
					.Then (
						EscapedCharWithBackslash
						.Or (SP.NonWhitespaceChar.ToStringParser ())))
						.Or (SP.Whitespace (" "))
					.OneOrMore ()
				 where !chs.All (string.IsNullOrWhiteSpace)
				 from cp in SP.Char (']')
				 select chs.ToString ("", "", "").TrimEnd ().ToLower ())
				 .Trace ("LinkLabel");

			Parser<StringTree, char> FullReferenceLink (long startPos,
				StringTree text) =>
				(from labelStart in Parser.Position<char> ()
				 from label in LinkLabel
				 from endPos in Parser.Position<char> ()
				 from st in Parser.GetState<ParseState, char> ()
				 select StringTree.Lazy (() =>
				 {
					 var linkRef = st.GetLinkReference (label);
					 if (text.HasTag ("link") || linkRef == null)
					 {
						 var esclab = EscapeBackslashes (label);
						 return StringTree.From ("[", text, "]",
							linkRef == null ?
								StringTree.From ("[", esclab, "]") :
								Link (startPos, endPos, esclab,
									DecodeUri (linkRef.Destination),
									DecodeLinkTitle (linkRef.Title)));
					 }
					 return Link (startPos, endPos, text,
						 DecodeUri (linkRef.Destination),
						 DecodeLinkTitle (linkRef.Title))
						 .Tag ("link");
				 }))
				.Trace ("FullReferenceLink");

			Parser<StringTree, char> CollapsedOrShortcutReferenceLink (
				long startPos, StringTree text) =>
				(from label in LinkLabel.OptionalRef ().Backtrack (startPos)
				 from brackets in SP.String ("[]").OptionalRef ()
				 from endPos in Parser.Position<char> ()
				 from st in Parser.GetState<ParseState, char> ()
				 select StringTree.Lazy (() =>
				 {
					 if (label != null)
					 {
						 var linkRef = st.GetLinkReference (label);
						 return linkRef == null ?
							 StringTree.From ("[", text, "]") :
							 Link (startPos, endPos, text,
								 DecodeUri (linkRef.Destination),
								 DecodeLinkTitle (linkRef.Title));
					 }
					 return StringTree.From ("[", text, "]");
				 }))
				.Trace ("CollapsedOrShortcutReferenceLink");

			var AnyLink =
				(from startPos in Parser.Position<char> ()
				 from text in LinkText
				 from res in InlineLink (startPos, text)
					 .Or (FullReferenceLink (startPos, text))
					 .Or (CollapsedOrShortcutReferenceLink (startPos, text))
				 select res)
				.Trace ("AnyLink");

			var LinkReferenceDefinition =
				(from ni in NonindentSpace
				 from label in LinkLabel
				 from col in SP.Char (':')
				 from ws1 in OptionalWsWithUpTo1NL
				 from dest in LinkDestination
				 from title in OptionalWsWithUpTo1NL
					.Then (LinkTitle).OptionalRef ()
				 from ws3 in OptionalSpace
				 from end in SP.NewLine.Or (AtEnd (""))
				 from _ in Parser.ModifyState<ParseState, char> (st => 
					st.AddLinkReference (label, dest, title?.Item1))
				 select StringTree.Empty)
				.Trace ("LinkReferenceDefinition");
			/*
			#### Main Inline Selector
			*/
			Inline.Target =
				LineBreak
				.Or (AnyLink)
				.Or (Emphasized)
				.Or (SpaceBetweenWords)
				.Or (BackslashEscape)
				.Or (Punct)
				.Or (UnformattedText)
				.Trace ("Inline");

			var Inlines =
				(from il in Inline.Target.OneOrMore ()
				 select il.FromEnumerable ())
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
					 inlines.IsEmpty () ? StringTree.Empty : 
					 (inlines[0].IsLeaf () && inlines[0].LeafValue ().All (char.IsWhiteSpace) ? 
						inlines.Skip (1) : inlines).FromEnumerable ()))
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
					 inlines.FromEnumerable ()))
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
					 .Or (LinkReferenceDefinition)
					 .Or (Para)
				 from endPos in Parser.Position<char> ()
				 select Block (startPos, endPos, block))
				.Trace ("AnyBlock");

			return
				(from _ in Parser.SetState<ParseState, char> (() => new ParseState ())
				 from blocks in AnyBlock.ZeroOrMore ()
				 select blocks.IsEmpty () ? StringTree.Empty : blocks.FromEnumerable ())
				.Trace ("Doc");
		}
	}
}
