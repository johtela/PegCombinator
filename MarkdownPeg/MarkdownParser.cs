namespace MarkdownPeg
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using ExtensionCord;
	using PegCombinator;
	using SP = PegCombinator.StringParser;

	public enum HtmlTagType
	{
		OpenTag, ClosingTag, Comment, ProcessingInstruction, Declaration, CDataSection
	}

	public class MarkdownParser
    {
		private Parser<StringTree, char> _doc;
		protected string _newline;

		public MarkdownParser (string newline)
		{
			_newline = newline;
			_doc = Doc ();
		}

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
			string verbatimText) => verbatimText;

		protected virtual StringTree CodeBlock (long start, long end,
			string codeBlock, string infoString) => codeBlock;

		protected virtual StringTree HtmlBlock (long start, long end,
			StringTree htmlBlock) => htmlBlock;

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

		protected virtual StringTree Image (long start, long end, StringTree alt,
			string dest, string title) =>
			"![" + alt + "](" + dest + " \"" + title + "\"";

		protected virtual StringTree CodeSpan (long start, long end, string code) =>
			"`" + code + "`";

		protected virtual StringTree HtmlTag (long start, long end, HtmlTagType type,
			string tag) => tag;
		/*
		## Helpers
		*/
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

		private bool IsPunctuation (char c) =>
			char.IsPunctuation (c) || c.In ('<', '>', '`');

		private string TrimLeadingSpaces (string str, int maxSpaces)
		{
			var cnt = Math.Min (str.Length, maxSpaces);
			var i = 0;
			while (i < cnt && str[i] == ' ')
				i++;
			return i == 0 ? str :
				str.Remove (0, i);
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
			private int _nestedImages;
			private long _blockStop;

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

			public void StartImage () =>
				_nestedImages++;

			public void EndImage () =>
				_nestedImages--;

			public bool InsideImage =>
				_nestedImages > 0;

			public void SetBlockStop (long endPos) =>
				_blockStop = endPos;

			public void ClearBlockStop () =>
				_blockStop = 0;

			public bool PastBlockStop (long pos)
				=> _blockStop != 0 && pos > _blockStop;
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
			var PunctChar = SP.Punctuation.Or (SP.OneOf ('<', '>', '`'));

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
				.Trace ("OptionalWsWithUpTo1NL");
			/*
			### Nonempty Lines
			*/
			var Line =
				(from chs in SP.NoneOf ('\r', '\n').ZeroOrMore ()
				 from nl in SP.NewLine
				 select chs.IsEmpty () ? _newline : chs.ToString ("", "", _newline))
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
				(from ind in Indent
				 from line in Line
				 select Tuple.Create (ind, line))
				.Trace ("IndentedLine");

			var NonblankIndentedLine =
				(from nb in SP.BlankLine ().Not ()
				 from il in IndentedLine
				 select il)
				.Trace ("NonblankIndentedLine");

			var VerbatimChunk =
				(from bls in SP.BlankLine (true).ZeroOrMore ()
				 from nbls in NonblankIndentedLine.OneOrMore ()
				 let ind = nbls[0].Item1
				 let text = nbls.Select (TupleExt.Second)
				 select (bls.IsEmpty () ? text :
					 bls.Select (b => 
						b.StartsWith (ind) ? b.Remove (0, ind.Length) : _newline)
					.Concat (text))
					.AsString ())
				.Trace ("VerbatimChunk");

			var VerbatimBlock =
				(from startPos in Parser.Position<char> ()
				 from chunks in VerbatimChunk.OneOrMore ()
				 from endPos in Parser.Position<char> ()
				 select Verbatim (startPos, endPos, chunks.AsString ()))
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
			### Block Selection Rules
			*/
			var NotAtEnd = SP.AnyChar.Trace ("NotAtEnd");

			Parser<string, char> AtEnd (string res) =>
				SP.AnyChar.Not ().Select (_ => res);

			Parser<Tuple<string, string>, char> CodeFence (char ch, int minLen) =>
				(from ni in NonindentSpace
				 from ts in SP.Char (ch).Occurrences (minLen, int.MaxValue)
				 select Tuple.Create (ni, ts.AsString ()))
				.Trace (string.Format ("CodeFence '{0}' minlen: {1}", ch, minLen));

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

			var IsFencedCodeBlock =
				(from ni in NonindentSpace
				 from fence in CodeFence ('`', 3)
					.Or (CodeFence ('~', 3))
				 select true)
				.Trace ("IsFencedCodeBlock");

			var SetextUnderline =
				(from ni in NonindentSpace
				 from ul in SP.Char ('=').OneOrMore ()
					.Or (SP.Char ('-').OneOrMore ())
				 from ws in OptionalSpace
				 from nl in SP.NewLine
				 select ni + ul.AsString () + ws + nl)
				.Trace ("SetextUnderline");

			var IsNormalLine =
				(from notend in NotAtEnd.And ()
				 from notbl in SP.BlankLine ().Not ()
				 from notgt in IsBlockQuote.Not ()
				 from notatx in AtxStart.Not ()
				 from notsetext in SetextUnderline.Not ()
				 from notthmbrk in ThemaBreak.Not ()
				 from notcodefence in IsFencedCodeBlock.Not ()
				 select true)
				.Trace ("IsNormalLine");

			var EndPosInsideBlock =
				 (from endPos in Parser.Position<char> ()
				  from st in Parser.GetState<ParseState, char> ()
				  where !st.PastBlockStop (endPos)
				  select endPos)
				 .Trace ("EndPosInsideBlock");

			/*
			### Inlines

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
				EscapedChar.Select (ch => "\\" + ch)
				.Trace ("EscapedCharWithBackslash");

			var BackslashEscape =
				(from pos in Parser.Position<char> ()
				 from c in EscapedChar
				 select IsPunctuation (c) ?
					 Punctuation (pos, c) : c)
				.Trace ("BackslashEscape");
			/*
			#### Entity Character References
			*/
			StringTree CharsToStringTree (long pos, string chars) =>
				(from c in chars
				 select IsPunctuation (c) ? Punctuation (pos, c) : c)
				.ToStringTree ();

			var EntityAsString =
				(from amp in SP.Char ('&')
				 from ent in SP.AlphaNumeric.OneOrMore ()
				 from sc in SP.Char (';')
				 let res = HtmlHelper.DecodeEntity (ent.AsString ())
				 where res != null
				 select res)
				.Trace ("EntityAsString");

			var Entity =
				(from pos in Parser.Position<char> ()
				 from res in EntityAsString
				 select CharsToStringTree (pos, res))
				.Trace ("Entity");
			/*
			#### Decimal and Hexadecimal Numeric Characters
			*/
			var HexNumber =
				from x in SP.OneOf ('X', 'x')
				from num in SP.HexadecimalInteger 
				select num;

			var NumericCharAsString =
				(from amp in SP.Char ('&')
				 from hash in SP.Char ('#')
				 from num in SP.PositiveInteger.Or (HexNumber)
				 from sc in SP.Char (';')
				 select num == 0 || num > 0x10ffff ?
					"\uFFFD" :
					char.ConvertFromUtf32 (num))
				.Trace ("NumericCharAsString");

			var NumericChar =
				(from pos in Parser.Position<char> ()
				 from res in NumericCharAsString
				 select CharsToStringTree (pos, res))
				.Trace ("NumericChar");
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
				 from notnl in SP.NewLine.Not ()
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
				 select il.ToStringTree ())
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
					 from endPos in EndPosInsideBlock
					 from cd in emphDelim
					 from rfd in AsteriskEmphEnd
					 from st in Parser.GetState<ParseState, char> ()
					 select st.InsideImage ? ils :
						transform (startPos, endPos, ils))
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
					 from endPos in EndPosInsideBlock
					 from cd in emphDelim
					 from rfd in UnderscoreEmphEnd
					 from st in Parser.GetState<ParseState, char> ()
					 select st.InsideImage ? ils :
						 transform (startPos, endPos, ils))
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
				 select ilb.ToStringTree ())
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
					 EscapedChar.ToStringParser ()
					 .Or (EntityAsString)
					 .Or (NumericCharAsString)
					 .Or (SP.NoneOf (close).ToStringParser ())
					 .ZeroOrMore ())
				 from cq in SP.Char (close)
				 let title = chs.AsString ()
				 select Tuple.Create (title, StringTree.From (open, title, close)))
				.Trace (string.Format ("LinkTitle {0}...{1}", open, close));

			var LinkTitle = LTitle ('\'', '\'')
				.Or (LTitle ('"', '"'))
				.Or (LTitle ('(', ')'))
				.Trace ("LinkTitle");

			Parser<StringTree, char> InlineLink (long startPos, StringTree text) =>
				(from op in SP.Char ('(')
				 from ws1 in SP.WhitespaceChar.ZeroOrMore ()
				 from dest in LinkDestination.OptionalRef ()
				 from ws2 in SP.WhitespaceChar.ZeroOrMore ()
				 from title in LinkTitle.OptionalRef ()
				 from ws3 in SP.WhitespaceChar.ZeroOrMore ()
				 from cp in SP.Char (')')
				 from endPos in EndPosInsideBlock
				 from st in Parser.GetState<ParseState, char> ()
				 select st.InsideImage ?
						text :
					text.HasTag ("link") ?
						StringTree.From ('[', text, ']', '(', 
							ws1.AsString (), dest, ws2.AsString (),
							title == null ? "" : title.Item2, ws3.AsString (), ')') :
					Link (startPos, endPos, text,
						HtmlHelper.DecodeUri (dest), title?.Item1)
						.Tag ("link"))
				.Trace ("InlineLink");
			/*
			##### Full Reference Links
			*/
			var LinkLabel =
				(from op in SP.Char ('[')
				 from ws in SP.OptionalWhitespace ("")
				 from chs in (
					Brackets.Not ()
					.Then (
						EscapedCharWithBackslash
						.Or (SP.NonWhitespaceChar.ToStringParser ())))
						.Or (SP.Whitespace (" "))
					.OneOrMore ()
				 where !chs.All (string.IsNullOrWhiteSpace)
				 from cp in SP.Char (']')
				 select chs.AsString ().TrimEnd ().ToLower ())
				 .Trace ("LinkLabel");

			Parser<StringTree, char> FullReferenceLink (long startPos,
				StringTree text) =>
				(from labelStart in Parser.Position<char> ()
				 from label in LinkLabel
				 from endPos in EndPosInsideBlock
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
									HtmlHelper.DecodeUri (linkRef.Destination),
									linkRef.Title));
					 }
					 return Link (startPos, endPos, text,
						 HtmlHelper.DecodeUri (linkRef.Destination),
						 linkRef.Title)
						 .Tag ("link");
				 }))
				.Trace ("FullReferenceLink");

			Parser<StringTree, char> CollapsedOrShortcutReferenceLink (
				long startPos, StringTree text) =>
				(from label in LinkLabel.OptionalRef ().Backtrack (startPos)
				 from brackets in SP.String ("[]").OptionalRef ()
				 from endPos in EndPosInsideBlock
				 from st in Parser.GetState<ParseState, char> ()
				 select StringTree.Lazy (() =>
				 {
					 if (label != null)
					 {
						 var linkRef = st.GetLinkReference (label);
						 return linkRef == null ?
							 StringTree.From ("[", text, "]") :
							 Link (startPos, endPos, text,
								 HtmlHelper.DecodeUri (linkRef.Destination),
								 linkRef.Title);
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
			#### Images
			*/
			Parser<StringTree, char> InlineImage (long startPos, StringTree alt) =>
				(from op in SP.Char ('(')
				 from ws1 in SP.WhitespaceChar.ZeroOrMore ()
				 from dest in LinkDestination.OptionalRef ()
				 from ws2 in SP.WhitespaceChar.ZeroOrMore ()
				 from title in LinkTitle.OptionalRef ()
				 from ws3 in SP.WhitespaceChar.ZeroOrMore ()
				 from cp in SP.Char (')')
				 from endPos in EndPosInsideBlock
				 from st in Parser.GetState<ParseState, char> ()
				 select st.InsideImage ? alt :
					Image (startPos, endPos, alt,
						 HtmlHelper.DecodeUri (dest),
						 title?.Item1))
				.Trace ("InlineImage");

			Parser<StringTree, char> FullReferenceImage (long startPos,
				StringTree alt) =>
				(from labelStart in Parser.Position<char> ()
				 from label in LinkLabel
				 from endPos in EndPosInsideBlock
				 from st in Parser.GetState<ParseState, char> ()
				 select StringTree.Lazy (() =>
				 {
					 var linkRef = st.GetLinkReference (label);
					 return linkRef == null ?
						StringTree.From ("![", alt, "][", EscapeBackslashes (label), "]") :
						Image (startPos, endPos, alt, 
							HtmlHelper.DecodeUri (linkRef.Destination),
							linkRef.Title);
				 }))
				.Trace ("FullReferenceImage");

			Parser<StringTree, char> CollapsedOrShortcutReferenceImage (
				long startPos, StringTree alt) =>
				(from label in LinkLabel.OptionalRef ().Backtrack (startPos + 1)
				 from brackets in SP.String ("[]").OptionalRef ()
				 from endPos in EndPosInsideBlock
				 from st in Parser.GetState<ParseState, char> ()
				 select StringTree.Lazy (() =>
				 {
					 if (label != null)
					 {
						 var linkRef = st.GetLinkReference (label);
						 return linkRef == null ?
							 StringTree.From ("![", alt, "]") :
							 Image (startPos, endPos, alt,
								 HtmlHelper.DecodeUri (linkRef.Destination),
								 linkRef.Title);
					 }
					 return StringTree.From ("![", alt, "]");
				 }))
				.Trace ("CollapsedOrShortcutReferenceImage");

			var AnyImage =
				(from startPos in Parser.Position<char> ()
				 from em in SP.Char ('!')
				 from _ in Parser.ModifyState<ParseState, char> (st => st.StartImage ())
				 from alt in LinkText
					.CleanupState<StringTree, ParseState, char> (st => st.EndImage ())
				 from img in InlineImage (startPos, alt)
					 .Or (FullReferenceImage (startPos, alt))
					 .Or (CollapsedOrShortcutReferenceImage (startPos, alt))
				 select img)
				.Trace ("AnyImage");
			/*
			#### Autolinks
			*/
			var Scheme =
				(from fst in SP.AsciiLetter
				 from rest in SP.AsciiLetter
					 .Or (SP.Number)
					 .Or (SP.OneOf ('+', '.', '-'))
					 .OneOrMore ()
				 from col in SP.Char (':')
				 select rest.AddToFront (fst).AddToBack (col).AsString ())
				.Trace ("Scheme");

			var UriAutolink =
				(from sch in Scheme
				 from adr in SP.AsciiWhitespaceChar
					 .Or (SP.AsciiControl)
					 .Or (SP.OneOf ('<', '>'))
					 .Not ()
					 .Then (SP.AnyChar)
					 .ZeroOrMore ()
				 select Tuple.Create (false, sch + adr.AsString ()))
				.Trace ("UriAutoLink");

			var EmailAccount =
				SP.AsciiAlphaNumeric
				.Or (SP.OneOf ('.', '!', '#', '$', '%', '&', '\'', '*', '+',
					'/', '=', '?', '^', '_', '`', '{', '|', '}', '~', '-'))
				.OneOrMore ()
				.Trace ("EmailAccount");

			var EmailServerPart =
				(from fst in SP.AsciiAlphaNumeric
				 from rest in SP.AsciiAlphaNumeric
					 .Or (from hyp in SP.Char ('-')
						  from alph in SP.AsciiAlphaNumeric.And ()
						  select hyp)
					 .Occurrences (0, 62)
				 select rest.AddToFront (fst))
				.Trace ("EmailServerPart");

			var EmailAddress =
				(from acc in EmailAccount
				 from at in SP.Char ('@')
				 from fpart in EmailServerPart
				 from rparts in
					 (from dot in SP.Char ('.')
					  from prt in EmailServerPart
					  select prt.AddToFront (dot))
				     .ZeroOrMore ()
				 select Tuple.Create (true,
					rparts.Aggregate (
						acc.AddToBack (at).AddToBack (fpart), 
						(r, l) => r.AddToBack (l))
						.AsString ()))
				.Trace ("EmailAddress");

			var Autolink =
				(from startPos in Parser.Position<char> ()
				 from lt in SP.Char ('<')
				 from addr in UriAutolink
					.Or (EmailAddress)
				 from gt in SP.Char ('>')
				 from endPos in EndPosInsideBlock
				 let dest = addr.Item1 ?
					"mailto:" + addr.Item2 : 
					addr.Item2
				 select Link (startPos, endPos, HtmlHelper.HtmlEncode (addr.Item2), 
					dest, null))
				.Trace ("Autolink");
			/*
			#### Code Spans
			*/
			var BacktickString =
				(from bts in SP.Char ('`').OneOrMore ()
				 select bts.AsString ())
				.Trace ("BacktickString");

			Parser<string, char> CodeContent (Parser<string, char> close) =>
				(from ws in SP.OptionalWhitespace ("")
				 from code in close.Not ()
					 .Then (
						 SP.AsciiWhitespace (" ")
						 .Or (BacktickString)
						 .Or (SP.AnyChar.ToStringParser ()))
					 .ZeroOrMore ()
				 from cl in close
				 select code.AsString ().TrimEnd ())
				.Trace ("CodeContent");

			var Code =
				(from startPos in Parser.Position<char> ()
				 from bts in BacktickString
				 let len = bts.Length
				 let close = BacktickString.Where (bs => bs.Length == len)
				 from code in CodeContent (close).OptionalRef ()
				 from endPos in EndPosInsideBlock
				 select code != null ?
					CodeSpan (startPos, endPos, code) :
					CharsToStringTree (startPos, bts))
				.Trace ("Code");
			/*
			#### Raw HTML
			*/
			var TagName =
				(from fst in SP.AsciiLetter
				 from rest in SP.AsciiAlphaNumeric
					 .Or (SP.Char ('-'))
					 .ZeroOrMore ()
				 select rest.AddToFront (fst).AsString ())
				.Trace ("TagName");

			var AttributeName =
				(from fst in SP.AsciiLetter.Or (SP.OneOf ('_', ':'))
				 from rest in SP.AsciiAlphaNumeric
					 .Or (SP.OneOf ('_', '.', ':', '-'))
					 .ZeroOrMore ()
				 select rest.AddToFront (fst).AsString ())
				.Trace ("AttributeName");

			var UnquotedAttributeValue =
				SP.NoneOf (' ', '"', '\'', '=', '<', '>', '`')
				.OneOrMore ()
				.ToStringParser ()
				.Trace ("UnquotedAttributeValue");

			Parser<string, char> QuotedAttributeValue (char quote) =>
				(from oq in SP.Char (quote)
				 from val in SP.NoneOf (quote).ZeroOrMore ()
				 from cq in SP.Char (quote)
				 select val.AddToFront (oq).AddToBack (cq).AsString ())
				.Trace ("QuotedAttributeValue: " + quote);

			var AttributeValue =
				(from ws1 in SP.OptionalWhitespace (null)
				 from eq in SP.Char ('=')
				 from ws2 in SP.OptionalWhitespace (null)
				 from val in UnquotedAttributeValue
					 .Or (QuotedAttributeValue ('\''))
					 .Or (QuotedAttributeValue ('"'))
				 select ws1 + eq + ws2 + val)
				.Trace ("AttributeValue");

			var Attribute =
				(from ws in SP.Whitespace (null)
				 from name in AttributeName
				 from value in AttributeValue.Optional ("")
				 select ws + name + value)
				.Trace ("Attribute");

			Parser<StringTree, char> OpenTag (long startPos) =>
				(from name in TagName
				 from attrs in Attribute.ZeroOrMore ()
				 from ws in SP.OptionalWhitespace (null)
				 from sl in SP.Char ('/').OptionalVal ()
				 from gt in SP.Char ('>')
				 from endPos in EndPosInsideBlock
				 select HtmlTag (startPos, endPos, HtmlTagType.OpenTag,
					"<" + name + attrs.AsString () + ws + (sl.HasValue ? "/>" : ">")))
				.Trace ("OpenTag");

			Parser<StringTree, char> ClosingTag (long startPos) =>
				(from lt in SP.Char ('/')
				 from name in TagName
				 from ws in SP.OptionalWhitespace (null)
				 from gt in SP.Char ('>')
				 from endPos in EndPosInsideBlock
				 select HtmlTag (startPos, endPos, HtmlTagType.ClosingTag,
					"</" + name + ws + gt))
				.Trace ("ClosingTag");

			Parser<StringTree, char> HtmlComment (long startPos) =>
				(from open in SP.String ("!--")
				 from notend in SP.String (">").Or (SP.String ("->")).Not ()
				 from text in SP.String ("--").Not ()
					 .Then (SP.AnyChar)
					 .ZeroOrMore ()
				 from close in SP.String ("-->")
				 from endPos in EndPosInsideBlock
				 select HtmlTag (startPos, endPos, HtmlTagType.Comment,
					"<!--" + text.AsString () + close))
				.Trace ("HtmlComment");

			Parser<StringTree, char> ProcessingInstruction (long startPos) =>
				(from open in SP.Char ('?')
				 from text in SP.String ("?>").Not ()
					 .Then (SP.AnyChar)
					 .ZeroOrMore ()
				 from close in SP.String ("?>")
				 from endPos in EndPosInsideBlock
				 select HtmlTag (startPos, endPos, HtmlTagType.ProcessingInstruction,
					"<?" + text.AsString () + close))
				.Trace ("ProcessingInstruction");

			Parser<StringTree, char> CDataSection (long startPos) =>
				(from open in SP.String ("![CDATA[")
				 from text in SP.String ("]]>").Not ()
					 .Then (SP.AnyChar)
					 .ZeroOrMore ()
				 from close in SP.String ("]]>")
				 from endPos in EndPosInsideBlock
				 select HtmlTag (startPos, endPos, HtmlTagType.CDataSection,
					"<![CDATA[" + text.AsString () + close))
				.Trace ("CDataSection");

			Parser<StringTree, char> Declaration (long startPos) =>
				(from open in SP.Char ('!')
				 from name in SP.Upper.OneOrMore ()
				 from ws in SP.Whitespace (null)
				 from text in SP.Char ('>').Not ()
					 .Then (SP.AnyChar)
					 .ZeroOrMore ()
				 from close in SP.Char ('>')
				 from endPos in EndPosInsideBlock
				 select HtmlTag (startPos, endPos, HtmlTagType.Declaration,
					"<!" + name.AsString () + ws + text.AsString () + close))
				.Trace ("Declaration");

			var AnyTag =
				(from startPos in Parser.Position<char> ()
				 from lt in SP.Char ('<')
				 from tag in OpenTag (startPos)
					 .Or (ClosingTag (startPos))
					 .Or (HtmlComment (startPos))
					 .Or (ProcessingInstruction (startPos))
					 .Or (CDataSection (startPos))
					 .Or (Declaration (startPos))
				 select tag)
				.Trace ("AnyTag");

			/*
			#### Main Inline Selector
			*/
			Inline.Target =
				LineBreak
				.Or (AnyLink)
				.Or (AnyImage)
				.Or (Autolink)
				.Or (AnyTag)
				.Or (Emphasized)
				.Or (Code)
				.Or (SpaceBetweenWords)
				.Or (BackslashEscape)
				.Or (Entity)
				.Or (NumericChar)
				.Or (Punct)
				.Or (UnformattedText)
				.Trace ("Inline");

			var Inlines =
				(from il in Inline.Target.OneOrMore ()
				 select il.ToStringTree ())
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
						inlines.Skip (1) : inlines).ToStringTree ()))
				.Trace ("AtxHeading");
			/*
			#### Setext Headings
			*/
			var SetextInline =
				(from pos in Parser.Position<char> ()
				 from st in Parser.GetState<ParseState, char> ()
				 where !st.PastBlockStop (pos)
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
				 from endPos in Parser.Position<char> ()
				 from ul in SetextUnderline
				 select endPos)
				.Trace ("IsSetextHeading");

			var SetextHeading =
				(from endPos in IsSetextHeading.And ()
				 from ni in NonindentSpace
				 from startPos in Parser.Position<char> ()
				 from _ in Parser.ModifyState<ParseState, char> (
					 st => st.SetBlockStop (endPos))
				 from inlines in SetextInline.OneOrMore ()
					.CleanupState<List<StringTree>, ParseState, char> (
						 st => st.ClearBlockStop ())
				 from sp in OptionalSpace
				 from nl in SP.NewLine
				 from ul in SetextUnderline
				 select Heading (startPos, endPos, ul.Contains ("=") ? 1 : 2,
					 inlines.ToStringTree ()))
				.Trace ("SetextHeading");
			/*
			#### Fenced Code Block
			*/
			var InfoString =
				(from ws in OptionalSpace
				 from ch in EntityAsString
					 .Or (EscapedChar.ToStringParser ())
					 .Or (SP.NoneOf ('`', '\n', '\r').ToStringParser ())
					.ZeroOrMore ()
				 from nl in SP.NewLine
				 let info = ch.AsString ().TrimEnd ()
				 select string.IsNullOrEmpty (info) ? null : info)
				.Trace ("InfoString");

			var FencedCodeBlock =
				(from startPos in Parser.Position<char> ()
				 from open in CodeFence ('`', 3)
					 .Or (CodeFence ('~', 3))
				 from info in InfoString
				 let indlen = open.Item1.Length
				 let closeFence = CodeFence (open.Item2[0], open.Item2.Length)
					.Then (OptionalSpace)
					.Then (SP.NewLine)
				 from lines in closeFence.Not ()
					 .Then (Line).ZeroOrMore ()
				 from close in closeFence.OptionalRef ()
				 from endPos in Parser.Position<char> ()
				 let trimmed = lines.Select (l =>
					 TrimLeadingSpaces (l, indlen)).AsString ()
				 select CodeBlock (startPos, endPos, trimmed, info))
				.Trace ("FencedCodeBlock");
			/*
			#### HTML Blocks
			*/
			Parser<string, char> StartTag (Parser<string, char> tagName) =>
				from tag in tagName
				from ws in OptionalSpace
				from gt in SP.Char ('>')
				from nl in SP.NewLine
				select tag + ws + gt + nl;

			Parser<string, char> EndTag (Parser<string, char> tagName) =>
				from lt in SP.Char ('<')
				from sl in SP.Char ('/')
				from tag in tagName
				from gt in SP.Char ('>')
				select lt + sl + tag + gt;

			Parser<string, char> HtmlLine (Parser<string, char> endMarker) =>
				(from ln in endMarker.Not ()
					.Then (SP.NoneOf ('\r', '\n'))
					.ZeroOrMore ()
				 select ln.AsString ())
				.Trace ("HTML block line");

			Parser<StringTree, char> HtmlBlock (Parser<string, char> start,
				Parser<string, char> end) =>
				from s in start
				from lines in HtmlLine (end).ZeroOrMore ()
				from e in end
				from rest in Line
				select StringTree.From (s, lines.ToStringTree (), e, rest);

			var Tag1 = SP.CaseInsensitiveString ("script")
				.Or (SP.CaseInsensitiveString ("pre"))
				.Or (SP.CaseInsensitiveString ("style"));

			var HtmlBlock1 = HtmlBlock (StartTag (Tag1), EndTag (Tag1))
				.Trace ("HtmlBlock1 (<script>, <pre>, <style>)");

			var HtmlBlock2 = HtmlBlock (SP.String ("!--"), SP.String ("-->"))
				.Trace ("HtmlBlock2 (<!-- comment -->)");

			var HtmlBlock3 = HtmlBlock (SP.String ("?"), SP.String ("?>"))
				.Trace ("HtmlBlock3 (<? processing instruction ?>)");

			var HtmlBlock4 = HtmlBlock (SP.String ("![CDATA["), SP.String ("]]>"))
				.Trace ("HtmlBlock4 (<![CDATA[ cdata ]]>)");

			var HtmlBlock5 = HtmlBlock (SP.String ("!"), SP.String (">"))
				.Trace ("HtmlBlock5 (<! declaration >)");

			var Start6 =
				from sl1 in SP.Char ('/').OptionalVal ()
				from tag in SP.VariableName
				where HtmlHelper.ValidTag (tag)
				from ws in OptionalSpace
				from end in SP.NewLine
					.Or (SP.String (">"))
					.Or (SP.String ("/>"))
				select sl1 + tag + ws + end;

			//var End67 =

			/*
			#### Paragraphs
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
					 .Or (FencedCodeBlock)
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
				 select blocks.IsEmpty () ? StringTree.Empty : blocks.ToStringTree ())
				.Trace ("Doc");
		}
	}
}
