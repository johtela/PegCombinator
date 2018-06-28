//#define markdig

namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;
#if markdig
	using Markdig;
#else
	using MarkdownPeg;
#endif

	public class TestBase
	{
#if markdig
		private static MarkdownPipeline _pipeline;

		static TestBase ()
		{
			_pipeline = new MarkdownPipelineBuilder ().Build ();
		}

		protected static void TestParse (string input, string output)
		{
			var parsed = Markdown.ToHtml (input, _pipeline);
			Assert.AreEqual (output.Trim (), parsed.Trim ());
		}
#else
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
#endif
	}
}
