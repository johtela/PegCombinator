namespace CommonMark
{
	using MarkdownPeg;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	public class TestBase
	{
		public static MarkdownToHtml Parser;

		static TestBase ()
		{
			Parser = new MarkdownToHtml ("\n");
		}

		protected static void TestParse (string input, string output)
		{
			var parsed = Parser.Run (input);
			Assert.AreEqual (output.Trim (), parsed.Trim ());
		}

	}
}
