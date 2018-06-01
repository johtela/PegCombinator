namespace MarkdownPeg
{
	using System;
	using System.Linq;
	using PegCombinator;
	using System.Text;

	public class MarkdownToHtml : MarkdownParser
	{
		private StringTree _newline;

		public MarkdownToHtml (string newline)
		{
			_newline = newline;
		}

		public MarkdownToHtml () : this (Environment.NewLine) { }

		private string OpenTag (string tagName) =>
			string.Format ("<{0}>", tagName);

		private string CloseTag (string tagName) =>
			string.Format ("</{0}>", tagName);

		private string HtmlEncode (char ch)
		{
			switch (ch)
			{
				case '&': return "&amp;";
				case '<': return "&lt;";
				case '>': return "&gt;";
				case '"': return "&quot;";
				default: return new string (ch, 1);
			}
		}

		private string HtmlEncode (string str)
		{
			if (str == null)
				return null;
			var res = new StringBuilder ();
			for (int i = 0; i < str.Length; i++)
				res.Append (HtmlEncode (str[i]));
			return res.ToString ();
		}

		protected override StringTree ThematicBreak (long start, long end, StringTree text) =>
			"<hr />" + _newline;

		protected override StringTree Heading (long start, long end, int headingLevel,
			StringTree headingText)
		{
			var tag = "h" + headingLevel;
			return StringTree.From (OpenTag (tag), headingText, CloseTag (tag), _newline);
		}

		protected override StringTree Verbatim (long start, long end, StringTree verbatimText) =>
			StringTree.From ("<pre><code>", HtmlEncode (verbatimText.ToString ()), 
				"</code></pre>", _newline);

		protected override StringTree Paragraph (long start, long end, StringTree text) =>
			StringTree.From ("<p>", text, "</p>", _newline);

		protected override StringTree SoftLineBreak (long start, long end, StringTree text) =>
			_newline;

		protected override StringTree HardLineBreak (long start, long end, StringTree text) => 
			"<br />" + _newline;

		protected override StringTree Punctuation (long pos, char punctuation) => 
			HtmlEncode (punctuation);

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
			dest = Uri.EscapeUriString (dest);
			title = HtmlEncode (title);
			return StringTree.From ("<a href=\"", dest ?? StringTree.Empty, 
				title != null ? "\" title=\"" + title : StringTree.Empty, 
				"\">", text, "</a>");
		}
	}
}
