namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Links : TestBase
	{
		/* [Example 459](http://spec.commonmark.org/0.28/#example-459) */
		[TestMethod]
		public void Example459 () => TestParse (
			"[link](/uri \"title\")", 
			"<p><a href=\"/uri\" title=\"title\">link</a></p>");

		/* [Example 460](http://spec.commonmark.org/0.28/#example-460) */
		[TestMethod]
		public void Example460 () => TestParse (
			"[link](/uri)", 
			"<p><a href=\"/uri\">link</a></p>");

		/* [Example 461](http://spec.commonmark.org/0.28/#example-461) */
		[TestMethod]
		public void Example461 () => TestParse (
			"[link]()", 
			"<p><a href=\"\">link</a></p>");

		/* [Example 462](http://spec.commonmark.org/0.28/#example-462) */
		[TestMethod]
		public void Example462 () => TestParse (
			"[link](<>)", 
			"<p><a href=\"\">link</a></p>");

		/* [Example 463](http://spec.commonmark.org/0.28/#example-463) */
		[TestMethod]
		public void Example463 () => TestParse (
			"[link](/my uri)", 
			"<p>[link](/my uri)</p>");

		/* [Example 464](http://spec.commonmark.org/0.28/#example-464) */
		[TestMethod]
		public void Example464 () => TestParse (
			"[link](</my uri>)", 
			"<p>[link](&lt;/my uri&gt;)</p>");

		/* [Example 465](http://spec.commonmark.org/0.28/#example-465) */
		[TestMethod]
		public void Example465 () => TestParse (
			"[link](foo\nbar)", 
			"<p>[link](foo\nbar)</p>");

		/* [Example 466](http://spec.commonmark.org/0.28/#example-466) */
		[TestMethod]
		public void Example466 () => TestParse (
			"[link](<foo\nbar>)", 
			"<p>[link](<foo\nbar>)</p>");

		/* [Example 467](http://spec.commonmark.org/0.28/#example-467) */
		[TestMethod]
		public void Example467 () => TestParse (
			"[link](\\(foo\\))", 
			"<p><a href=\"(foo)\">link</a></p>");

		/* [Example 468](http://spec.commonmark.org/0.28/#example-468) */
		[TestMethod]
		public void Example468 () => TestParse (
			"[link](foo(and(bar)))", 
			"<p><a href=\"foo(and(bar))\">link</a></p>");

	}
}
