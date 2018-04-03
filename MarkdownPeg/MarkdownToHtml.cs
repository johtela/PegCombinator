namespace MarkdownPeg
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class MarkdownToHtml : MarkdownParser
	{
		private string _newline;

		public MarkdownToHtml (string newline)
		{
			_newline = newline;
		}

		protected override string Heading (object start, object end, int headingLevel,
			string headingText) => 
			string.Format ("<h{0}>{1}</h{0}>{2}", headingLevel, headingText, _newline);

		protected override string Verbatim (object start, object end, string verbatimText) => 
			string.Format ("<pre><code>{0}</code></pre>", verbatimText);
	}
}
