namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class TextualContent : TestBase
	{
		/* [Example 622](http://spec.commonmark.org/0.28/#example-622) */
		[TestMethod]
		public void Example622 () => TestParse (
			"hello $.;'there", 
			"<p>hello $.;'there</p>");

		/* [Example 623](http://spec.commonmark.org/0.28/#example-623) */
		[TestMethod]
		public void Example623 () => TestParse (
			"Foo χρῆν", 
			"<p>Foo χρῆν</p>");

		/* [Example 624](http://spec.commonmark.org/0.28/#example-624) */
		[TestMethod]
		public void Example624 () => TestParse (
			"Multiple     spaces", 
			"<p>Multiple     spaces</p>");
	}
}
