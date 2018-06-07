namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class SoftLineBreaks : TestBase
	{
		/* [Example 620](http://spec.commonmark.org/0.28/#example-620) */
		[TestMethod]
		public void Example620 () => TestParse (
			"foo\nbaz", 
			"<p>foo\nbaz</p>");

		/* [Example 621](http://spec.commonmark.org/0.28/#example-621) */
		[TestMethod]
		public void Example621 () => TestParse (
			"foo \n baz", 
			"<p>foo\nbaz</p>");
	}
}
