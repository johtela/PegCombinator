namespace MarkdownPeg
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class MarkdownToHtml : MarkdownVisitor
	{
		public override string Heading (object start, object end, int headingLevel, string headingText)
		{
			return string.Format ("<h{0}>{1}</h{0}>", headingLevel, headingText);
		}

		public override string Verbatim (object start, object end, string verbatimText)
		{
			return string.Format ("<pre><code>{0}</code></pre>", verbatimText);
		}
	}
}
