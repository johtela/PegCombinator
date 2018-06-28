namespace MarkdownPeg
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using PegCombinator;

	public class MarkdownToHtml : MarkdownParser
	{
		private static class Tags
		{
			public static readonly object Emphasis = "Emphasis";
			public static readonly object Paragraph = "Paragraph";
			public static readonly object BlankLines = "BlankLines";
			public static readonly object MultiLines = "MultiLines";
		}

		public MarkdownToHtml (string newline) : base (newline) { }

		public MarkdownToHtml () : this (Environment.NewLine) { }

		protected override StringTree BlockQuote (long start, long end, 
			StringTree blocks) => 
			StringTree.From ("<blockquote>", _newline, blocks, "</blockquote>", _newline);

		private bool IsParagraph (StringTree st) =>
			st.TagValue == Tags.Paragraph;

		private bool IsBlankLines (StringTree st) =>
			st.TagValue == Tags.BlankLines;

		private StringTree ParagraphContents (StringTree st) =>
			st.TagTarget.ListValues.Skip (1).First ();

		private StringTree FormatTightParagraph (StringTree par) =>
			StringTree.From ("<li>", ParagraphContents (par), "</li>", _newline);

		private StringTree EmptyListItem => 
			StringTree.From ("<li>", "</li>", _newline);

		private bool IsLooseListItem (StringTree listItem, bool isLast)
		{
			if (IsParagraph (listItem))
				return false;
			if (IsBlankLines (listItem))
				return listItem.HasTag (Tags.MultiLines);
			var list = listItem.ListValues.Skip (1);
			var blank = list.FirstOrDefault (li => IsBlankLines (li));
			return blank != null && !(isLast && blank == list.Last ());
		}

		private StringTree FormatTightListItem (StringTree listItem)
		{
			if (IsBlankLines (listItem))
				return EmptyListItem;
			if (IsParagraph (listItem))
				return FormatTightParagraph (listItem);
			var list = listItem.ListValues.SkipWhile (IsBlankLines);
			var cnt = list.Count ();
			if (cnt == 0)
				return EmptyListItem;
			var first = list.First ();
			if (IsParagraph (first))
			{
				if (cnt == 1 || (cnt == 2 && IsBlankLines (list.Last ())))
					return FormatTightParagraph (first);
				return StringTree.From ("<li>", ParagraphContents (first), _newline,
					list.Skip (1).ToStringTree (), "</li>", _newline);
			}
			var items = list.Select (i => IsParagraph (i) ? ParagraphContents (i) : i);
			return StringTree.From ("<li>", _newline, items.ToStringTree (), "</li>", 
				_newline);
		}

		private StringTree FormatLooseListItem (StringTree listItem) => 
			IsBlankLines (listItem) ?
				EmptyListItem :
				StringTree.From ("<li>", _newline, listItem, "</li>", _newline);

		private StringTree FormatListItems (string startTag, string EndTag, 
			IEnumerable<StringTree> items)
		{
			var last = items.Last ();
			var isLoose = items.Any (i => IsLooseListItem (i, i == last));

			var res = StringTree.From (startTag, _newline,
				(isLoose ?
					items.Select (FormatLooseListItem) :
					items.Select (FormatTightListItem))
				.ToStringTree (),
				EndTag, _newline);
			// Lift the BlankLines tag if the last line of the list is empty
			if (last.IsList)
			{
				var lastBlock = last.ListValues.Last ();
				if (IsBlankLines (lastBlock))
					res = res.Tag (Tags.BlankLines);
			}
			return res;
		}

		protected override StringTree BulletList (long start, long end,
			StringTree listItems) =>
			FormatListItems ("<ul>", "</ul>", listItems.ListValues);

		protected override StringTree OrderedList (long start, long end, 
			string firstNumber, StringTree listItems)
		{
			var num = int.Parse (firstNumber);
			return FormatListItems (
				num == 1 ? "<ol>" : string.Format ("<ol start=\"{0}\">", num),
				"</ol>", listItems.ListValues);
		}

		protected override StringTree ThematicBreak (long start, long end, 
			StringTree text) =>
			"<hr />" + _newline;

		protected override StringTree Heading (long start, long end, int headingLevel,
			StringTree headingText)
		{
			var tag = "h" + headingLevel;
			return StringTree.From (HtmlHelper.OpenTag (tag), headingText,
				HtmlHelper.CloseTag (tag), _newline);
		}

		protected override StringTree Verbatim (long start, long end, 
			string verbatimText) =>
			StringTree.From ("<pre><code>", HtmlHelper.HtmlEncode (verbatimText), 
				"</code></pre>", _newline);

		protected override StringTree CodeBlock (long start, long end, 
			string codeBlock, string infoString) =>
			StringTree.From (
				infoString == null ? 
					"<pre><code>" :
					string.Format ("<pre><code class=\"language-{0}\">",
						infoString.Split (' ', '\t')[0]),
				HtmlHelper.HtmlEncode (codeBlock),
				"</code></pre>", _newline);

		protected override StringTree Paragraph (long start, long end, StringTree text) =>
			StringTree.From ("<p>", text, "</p>", _newline).Tag (Tags.Paragraph);

		protected override StringTree BlankLines (long start, long end, StringTree lines)
		{
			var res = StringTree.Empty;
			if (lines.ListValues.Count () > 1)
				res = res.Tag (Tags.MultiLines);
			return res.Tag (Tags.BlankLines);
		}

		protected override StringTree SoftLineBreak (long start, long end, StringTree text) =>
			_newline;

		protected override StringTree HardLineBreak (long start, long end, StringTree text) => 
			"<br />" + _newline;

		protected override StringTree Punctuation (long pos, char punctuation) =>
			HtmlHelper.BasicEncode (punctuation);

		protected override StringTree Emphasis (long start, long end, StringTree text) =>
			StringTree.From ("<em>", text, "</em>").Tag (Tags.Emphasis);


		protected override StringTree StrongEmphasis (long start, long end,
			StringTree text)
		{
			if (text.IsList && text.ListValues.Count () == 1 && 
				text.ListValues.First ().TagValue == Tags.Emphasis)
			{
				var list = text.ListValues.First ().TagTarget.ListValues;
				return Emphasis (start, end, 
					StringTree.From ("<strong>", list.Skip (1).First (), "</strong>"));
			}
			return StringTree.From ("<strong>", text, "</strong>");
		}

		protected override StringTree Link (long start, long end, StringTree text, 
			string dest, string title)
		{
			dest = HtmlHelper.HtmlEncode (Uri.EscapeUriString (dest), 
				HtmlHelper.ExtendedEncode);
			title = HtmlHelper.HtmlEncode (title);
			return StringTree.From ("<a href=\"", dest ?? StringTree.Empty, 
				title != null ? "\" title=\"" + title : StringTree.Empty, 
				"\">", text, "</a>");
		}

		protected override StringTree Image (long start, long end, StringTree alt, 
			string dest, string title)
		{
			dest = Uri.EscapeUriString (dest);
			title = HtmlHelper.HtmlEncode (title);
			return StringTree.From ("<img src=\"", dest ?? StringTree.Empty,
				"\" alt=\"", alt,
				title != null ? "\" title=\"" + title : StringTree.Empty,
				"\" />");
		}

		protected override StringTree CodeSpan (long start, long end, string code) => 
			StringTree.From ("<code>", HtmlHelper.HtmlEncode (code), "</code>");
	}
}
