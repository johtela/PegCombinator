namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class ThematicBreaks : TestBase
	{
		/* [Example 13](http://spec.commonmark.org/0.28/#example-13) */
		[TestMethod]
		public void Example13 () => TestParse (
			"***\n---\n___",
			"<hr />\n<hr />\n<hr />");

		/* [Example 14](http://spec.commonmark.org/0.28/#example-14) */
		[TestMethod]
		public void Example14 () => TestParse (
			"+++",
			"<p>+++</p>");

		/* [Example 15](http://spec.commonmark.org/0.28/#example-15) */
		[TestMethod]
		public void Example15 () => TestParse (
			"===",
			"<p>===</p>");

		/* [Example 16](http://spec.commonmark.org/0.28/#example-16) */
		[TestMethod]
		public void Example16 () => TestParse (
			"--\n**\n__",
			"<p>--\n**\n__</p>");

		/* [Example 17](http://spec.commonmark.org/0.28/#example-17) */
		[TestMethod]
		public void Example17 () => TestParse (
			" ***\n  ***\n   ***",
			"<hr />\n<hr />\n<hr />");

		/* [Example 18](http://spec.commonmark.org/0.28/#example-18) */
		[TestMethod]
		public void Example18 () => TestParse (
			"    ***",
			"<pre><code>***\n</code></pre>");

		/* [Example 19](http://spec.commonmark.org/0.28/#example-19) */
		[TestMethod]
		public void Example19 () => TestParse (
			"Foo\n    ***",
			"<p>Foo\n***</p>");

		/* [Example 20](http://spec.commonmark.org/0.28/#example-20) */
		[TestMethod]
		public void Example20 () => TestParse (
			"_____________________________________",
			"<hr />");

		/* [Example 21](http://spec.commonmark.org/0.28/#example-21) */
		[TestMethod]
		public void Example21 () => TestParse (
			" - - -",
			"<hr />");

		/* [Example 22](http://spec.commonmark.org/0.28/#example-22) */
		[TestMethod]
		public void Example22 () => TestParse (
			" **  * ** * ** * **",
			"<hr />");

		/* [Example 23](http://spec.commonmark.org/0.28/#example-23) */
		[TestMethod]
		public void Example23 () => TestParse (
			"-     -      -      -",
			"<hr />");

		/* [Example 24](http://spec.commonmark.org/0.28/#example-24) */
		[TestMethod]
		public void Example24 () => TestParse (
			"- - - -    ",
			"<hr />");

		/* [Example 25](http://spec.commonmark.org/0.28/#example-25) */
		[TestMethod]
		public void Example25 () => TestParse (
			"_ _ _ _ a\n\na------\n\n---a---",
			"<p>_ _ _ _ a</p>\n<p>a------</p>\n<p>---a---</p>");

		/* [Example 26](http://spec.commonmark.org/0.28/#example-26) */
		[TestMethod]
		public void Example26 () => TestParse (
			" *-*",
			"<p><em>-</em></p>");

		/* [Example 27](http://spec.commonmark.org/0.28/#example-27) */
		[TestMethod]
		public void Example27 () => TestParse (
			"- foo\n***\n- bar",
			"<ul>\n<li>foo</li>\n</ul>\n<hr />\n<ul>\n<li>bar</li>\n</ul>");

		/* [Example 28](http://spec.commonmark.org/0.28/#example-28) */
		[TestMethod]
		public void Example28 () => TestParse (
			"Foo\n***\nbar\n",
			"<p>Foo</p>\n<hr />\n<p>bar</p>");

		/* [Example 29](http://spec.commonmark.org/0.28/#example-29) */
		[TestMethod]
		public void Example29 () => TestParse (
			"Foo\n---\nbar",
			"<h2>Foo</h2>\n<p>bar</p>");

		/* [Example 30](http://spec.commonmark.org/0.28/#example-30) */
		[TestMethod]
		public void Example30 () => TestParse (
			"* Foo\n* * *\n* Bar", 
			"<ul>\n<li>Foo</li>\n</ul>\n<hr />\n<ul>\n<li>Bar</li>\n</ul>");

		/* [Example 31](http://spec.commonmark.org/0.28/#example-31) */
		[TestMethod]
		public void Example31 () => TestParse (
			"- Foo\n- * * *",
			"<ul>\n<li>Foo</li>\n<li>\n<hr />\n</li>\n</ul>");
	}
}
