namespace MarkdownPeg
{
	using System;
	using System.Linq;
	using PegCombinator;

	public class MarkdownToHtml : MarkdownParser
	{
		private static class Tags
		{
			public static readonly object Emphasis = new object ();
			public static readonly object Paragraph = new object ();
		}

		public MarkdownToHtml (string newline) : base (newline) { }

		public MarkdownToHtml () : this (Environment.NewLine) { }

		protected override StringTree BlockQuote (long start, long end, 
			StringTree blocks) => 
			StringTree.From ("<blockquote>", _newline, blocks, "</blockquote>", _newline);

		protected override StringTree ListItem (long start, long end,
			StringTree blocks) => 
			blocks.TagValue == Tags.Paragraph ?
				StringTree.From ("<li>",
					blocks.TagTarget.ListValues.Skip (1).First (), "</li>", _newline) :
				StringTree.From ("<li>", _newline, blocks, "</li>", _newline);

		protected override StringTree BulletList (long start, long end,
			StringTree listItems) => 
			StringTree.From ("<ul>", _newline, listItems, "</ul>", _newline);

		protected override StringTree OrderedList (long start, long end, 
			string firstNumber, StringTree listItems)
		{
			var num = int.Parse (firstNumber);
			return StringTree.From (
				num == 1 ? "<ol>" : string.Format ("<ol start=\"{0}\">", num), 
				_newline, listItems, "</ol>", _newline);
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
			if (text.TagValue == Tags.Emphasis)
			{
				var list = text.TagTarget.ListValues;
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
