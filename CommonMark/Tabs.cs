namespace CommonMark
{
	using PegCombinator;
	using MarkdownPeg;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Tabs
	{
		public MarkdownToHtml Parser;

		public Tabs ()
		{
			Parser = new MarkdownToHtml ();
		}

		private void TestParse (string input, string output) => 
			Assert.AreEqual (output,
				Parser.Run (ParserInput.String (input).TerminateWith ('\n')));

		[TestMethod]
		public void Example1 () => 
			TestParse (
				"\tfoo\tbaz\t\tbim",
				"<pre><code>foo\tbaz\t\tbim</code></pre>");
	}
}
