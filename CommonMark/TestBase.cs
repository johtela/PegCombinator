namespace CommonMark
{
	using MarkdownPeg;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	public class TestBase
	{
		public MarkdownToHtml Parser;

		public TestBase ()
		{
			Parser = new MarkdownToHtml ("\n");
		}

		protected void TestParse (string input, string output)
		{
			var parsed = Parser.Run (input);
			Assert.AreEqual (output, parsed);
		}

	}
}
