namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Emphasis : TestBase
	{
		/* [Example 331](http://spec.commonmark.org/0.28/#example-331) */
		[TestMethod]
		public void Example331 () => TestParse (
			"*foo bar*", 
			"<p><em>foo bar</em></p>");
	}
}
