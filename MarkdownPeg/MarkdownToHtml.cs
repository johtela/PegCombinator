namespace MarkdownPeg
{
	using System;
	using System.Linq;
	using PegCombinator;

	public class MarkdownToHtml : MarkdownParser
	{
		public MarkdownToHtml (string newline) : base (newline) { }

		public MarkdownToHtml () : this (Environment.NewLine) { }

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

		protected override StringTree Paragraph (long start, long end, StringTree text) =>
			StringTree.From ("<p>", text, "</p>", _newline);

		protected override StringTree SoftLineBreak (long start, long end, StringTree text) =>
			_newline;

		protected override StringTree HardLineBreak (long start, long end, StringTree text) => 
			"<br />" + _newline;

		protected override StringTree Punctuation (long pos, char punctuation) =>
			HtmlHelper.BasicEncode (punctuation);

		protected override StringTree Emphasis (long start, long end, StringTree text) =>
			StringTree.From ("<em>", text, "</em>");

		protected override StringTree StrongEmphasis (long start, long end,
			StringTree text)
		{
			if (text.IsList () && text.ListValues ().Count () == 1)
			{
				var list = text.ListValues ().First ().ListValues ();
				if (list != null)
				{
					var first = list.FirstOrDefault ();
					if (first != null && first.LeafValue () == "<em>")
						return StringTree.From (
							"<em>",
							StringTree.From ("<strong>", list.Skip (1).First (), "</strong>"),
							"</em>");
				}
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
