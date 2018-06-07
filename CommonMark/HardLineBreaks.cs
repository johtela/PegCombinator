namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class HardLineBreaks : TestBase
	{
		/* [Example 605](http://spec.commonmark.org/0.28/#example-605) */
		[TestMethod]
		public void Example605 () => TestParse (
			"foo  \nbaz", 
			"<p>foo<br />\nbaz</p>");

		/* [Example 606](http://spec.commonmark.org/0.28/#example-606) */
		[TestMethod]
		public void Example606 () => TestParse (
			"foo\\\nbaz", 
			"<p>foo<br />\nbaz</p>");

		/* [Example 607](http://spec.commonmark.org/0.28/#example-607) */
		[TestMethod]
		public void Example607 () => TestParse (
			"foo       \nbaz", 
			"<p>foo<br />\nbaz</p>");

		/* [Example 608](http://spec.commonmark.org/0.28/#example-608) */
		[TestMethod]
		public void Example608 () => TestParse (
			"foo  \n     bar", 
			"<p>foo<br />\nbar</p>");

		/* [Example 609](http://spec.commonmark.org/0.28/#example-609) */
		[TestMethod]
		public void Example609 () => TestParse (
			"foo\\\n     bar", "<p>foo<br />\nbar</p>");

		/* [Example 610](http://spec.commonmark.org/0.28/#example-610) */
		[TestMethod]
		public void Example610 () => TestParse (
			"*foo  \nbar*", 
			"<p><em>foo<br />\nbar</em></p>");

		/* [Example 611](http://spec.commonmark.org/0.28/#example-611) */
		[TestMethod]
		public void Example611 () => TestParse (
			"*foo\\\nbar*", 
			"<p><em>foo<br />\nbar</em></p>");

		/* [Example 612](http://spec.commonmark.org/0.28/#example-612) */
		[TestMethod]
		public void Example612 () => TestParse (
			"`code  \nspan`", 
			"<p><code>code span</code></p>");

		/* [Example 613](http://spec.commonmark.org/0.28/#example-613) */
		[TestMethod]
		public void Example613 () => TestParse (
			"`code\\\nspan`", 
			"<p><code>code\\ span</code></p>");

		/* [Example 614](http://spec.commonmark.org/0.28/#example-614) */
		[TestMethod]
		public void Example614 () => TestParse (
			"<a href=\"foo  \nbar\">", 
			"<p><a href=\"foo  \nbar\"></p>");

		/* [Example 615](http://spec.commonmark.org/0.28/#example-615) */
		[TestMethod]
		public void Example615 () => TestParse (
			"<a href=\"foo\\\nbar\">", 
			"<p><a href=\"foo\\\nbar\"></p>");

		/* [Example 616](http://spec.commonmark.org/0.28/#example-616) */
		[TestMethod]
		public void Example616 () => TestParse (
			"foo\\", 
			"<p>foo\\</p>");

		/* [Example 617](http://spec.commonmark.org/0.28/#example-617) */
		[TestMethod]
		public void Example617 () => TestParse (
			"foo  ", 
			"<p>foo</p>");

		/* [Example 618](http://spec.commonmark.org/0.28/#example-618) */
		[TestMethod]
		public void Example618 () => TestParse (
			"### foo\\", 
			"<h3>foo\\</h3>");

		/* [Example 619](http://spec.commonmark.org/0.28/#example-619) */
		[TestMethod]
		public void Example619 () => TestParse (
			"### foo  ", 
			"<h3>foo</h3>");
	}
}
