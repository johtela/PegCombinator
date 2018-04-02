using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonMark
{
	using PegCombinator;
	using MarkdownPeg;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Tabs
	{
		public MarkdownToHtml Visitor;

		public Tabs ()
		{
			Visitor = new MarkdownToHtml ();
		}

		private void TestParse (string input, string output) => 
			Assert.AreEqual (output,
				MarkdownParser.Run (
					ParserInput.String (input).TerminateWith ('\n'), 
					Visitor));

		[TestMethod]
		public void Example1 () => 
			TestParse (
				"\tfoo\tbaz\t\tbim",
				"<pre><code>foo\tbaz\t\tbim</code></pre>");
	}
}
