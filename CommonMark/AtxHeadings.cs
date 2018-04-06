namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class AtxHeadings : TestBase
	{
		/* [Example 32](http://spec.commonmark.org/0.28/#example-32) */
		[TestMethod]
		public void Example32 () => TestParse (
			"# foo\n## foo\n### foo\n#### foo\n##### foo\n###### foo",
			"<h1>foo</h1>\n<h2>foo</h2>\n<h3>foo</h3>\n<h4>foo</h4>\n<h5>foo</h5>\n<h6>foo</h6>");

		/* [Example 33](http://spec.commonmark.org/0.28/#example-33) */
		[TestMethod]
		public void Example33 () => TestParse (
			"####### foo",
			"<p>####### foo</p>");

		/* [Example 34](http://spec.commonmark.org/0.28/#example-34) */
		[TestMethod]
		public void Example34 () => TestParse (
			"#5 bolt\n\n#hashtag",
			"<p>#5 bolt</p>\n<p>#hashtag</p>");

		/* [Example 35](http://spec.commonmark.org/0.28/#example-35) */
		[TestMethod]
		public void Example35 () => TestParse (
			"\\## foo",
			"<p>## foo</p>");

		/* [Example 36](http://spec.commonmark.org/0.28/#example-36) */
		[TestMethod]
		public void Example36 () => TestParse (
			"# foo *bar* \\*baz\\*",
			"<h1>foo <em>bar</em> *baz*</h1>");

		/* [Example 37](http://spec.commonmark.org/0.28/#example-37) */
		[TestMethod]
		public void Example37 () => TestParse (
			"#                  foo                     ",
			"<h1>foo</h1>");

		/* [Example 38](http://spec.commonmark.org/0.28/#example-38) */
		[TestMethod]
		public void Example38 () => TestParse (
			" ### foo\n  ## foo\n   # foo",
			"<h3>foo</h3>\n<h2>foo</h2>\n<h1>foo</h1>");

		/* [Example 39](http://spec.commonmark.org/0.28/#example-39) */
		[TestMethod]
		public void Example39 () => TestParse (
			"    # foo",
			"<pre><code># foo\n</code></pre>");

		/* [Example 40](http://spec.commonmark.org/0.28/#example-40) */
		[TestMethod]
		public void Example40 () => TestParse (
			"foo\n    # bar",
			"<p>foo\n# bar</p>");

		/* [Example 41](http://spec.commonmark.org/0.28/#example-41) */
		[TestMethod]
		public void Example41 () => TestParse (
			"## foo ##\n  ###   bar    ###",
			"<h2>foo</h2>\n<h3>bar</h3>");

		/* [Example 42](http://spec.commonmark.org/0.28/#example-42) */
		[TestMethod]
		public void Example42 () => TestParse (
			"# foo ##################################\n##### foo ##",
			"<h1>foo</h1>\n<h5>foo</h5>");

		/* [Example 43](http://spec.commonmark.org/0.28/#example-43) */
		[TestMethod]
		public void Example43 () => TestParse (
			"### foo ###     ",
			"<h3>foo</h3>");

		/* [Example 44](http://spec.commonmark.org/0.28/#example-44) */
		[TestMethod]
		public void Example44 () => TestParse (
			"### foo ### b",
			"<h3>foo ### b</h3>");

		/* [Example 45](http://spec.commonmark.org/0.28/#example-45) */
		[TestMethod]
		public void Example45 () => TestParse (
			"# foo#",
			"<h1>foo#</h1>");

		/* [Example 46](http://spec.commonmark.org/0.28/#example-46) */
		[TestMethod]
		public void Example46 () => TestParse (
			"### foo \\###\n## foo #\\##\n# foo \\#",
			"<h3>foo ###</h3>\n<h2>foo ###</h2>\n<h1>foo #</h1>");

		/* [Example 47](http://spec.commonmark.org/0.28/#example-47) */
		[TestMethod]
		public void Example47 () => TestParse (
			"****\n## foo\n****",
			"<hr />\n<h2>foo</h2>\n<hr />");

		/* [Example 48](http://spec.commonmark.org/0.28/#example-48) */
		[TestMethod]
		public void Example48 () => TestParse (
			"Foo bar\n# baz\nBar foo",
			"<p>Foo bar</p>\n<h1>baz</h1>\n<p>Bar foo</p>");

		/* [Example 49](http://spec.commonmark.org/0.28/#example-49) */
		[TestMethod]
		public void Example49 () => TestParse (
			"## \n#\n### ###",
			"<h2></h2>\n<h1></h1>\n<h3></h3>");
	}
}
