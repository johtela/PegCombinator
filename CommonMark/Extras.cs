namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Extras : TestBase
	{
		[TestMethod]
		public void Example4b () => TestParse (
			"- foo\n-\tbar",
			"<ul>\n<li>foo</li>\n<li>bar</li>\n</ul>");

		[TestMethod]
		public void Example4c () => TestParse (
			"  -\tfoo\n\n    bar",
			"<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>");

		[TestMethod]
		public void Example4d () => TestParse (
			"  -\tfoo\n\n\tbar",
			"<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>");

		[TestMethod]
		public void Example197b () => TestParse (
			"> foo\n> ---",
			"<blockquote>\n<h2>foo</h2>\n</blockquote>\n");

		[TestMethod]
		public void Example200b () => TestParse (
			"> ```\n> foo\n> ```",
			"<blockquote>\n<pre><code>foo\n</code></pre>\n</blockquote>\n");

		[TestMethod]
		public void Example900 () => TestParse (
			"[foo]:\n  /url\n  \"title\"\nTest\n====\n\n[foo]",
			"<h1>Test</h1>\n<p><a href=\"/url\" title=\"title\">foo</a></p>");

		[TestMethod]
		public void Example901 () => TestParse (
			"*   [Miscellaneous](#misc)\n    *   [Backslash Escapes](#backslash)\n    *   [Automatic Links](#autolink)\n\n",
			"<ul>\n<li><a href=\"#misc\">Miscellaneous</a>\n<ul>\n<li><a href=\"#backslash\">Backslash Escapes</a></li>\n<li><a href=\"#autolink\">Automatic Links</a></li>\n</ul>\n</li>\n</ul>");

	}
}
