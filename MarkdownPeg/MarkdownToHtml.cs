namespace MarkdownPeg
{
	using System;

	public class MarkdownToHtml : MarkdownParser
	{
		private string _newline;

		public MarkdownToHtml (string newline)
		{
			_newline = newline;
		}

		public MarkdownToHtml () : this (Environment.NewLine) { }

		protected override string ThematicBreak (long start, long end, string text) => 
			"<hr />" + _newline;

		protected override string Heading (long start, long end, int headingLevel,
			string headingText) => 
			string.Format ("<h{0}>{1}</h{0}>{2}", headingLevel, headingText, _newline);

		protected override string Verbatim (long start, long end, string verbatimText) => 
			string.Format ("<pre><code>{0}</code></pre>{1}", verbatimText, _newline);

		protected override string Paragraph (long start, long end, string text) =>
			string.Format ("<p>{0}</p>{1}", text, _newline);

		protected override string SoftLineBreak (long start, long end, string text) => 
			_newline;

		protected override string Punctuation (long pos, char punctuation)
		{
			switch (punctuation)
			{
				case '&': return "&amp;";
				case '<': return "&lt;";
				case '>': return "&gt;";
				default: return base.Punctuation (pos, punctuation);
			}
		}
	}
}
