namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Tabs : TestBase
	{
		/* [Example 1](http://spec.commonmark.org/0.28/#example-1) */
		[TestMethod]
		public void Example1 () => TestParse (
			"\tfoo\tbaz\t\tbim",
			"<pre><code>foo\tbaz\t\tbim\n</code></pre>");

		/* [Example 2](http://spec.commonmark.org/0.28/#example-2) */
		[TestMethod]
		public void Example2 () => TestParse (
			"  \tfoo\tbaz\t\tbim",
			"<pre><code>foo\tbaz\t\tbim\n</code></pre>");

		/* [Example 3](http://spec.commonmark.org/0.28/#example-3) */
		[TestMethod]
		public void Example3 () => TestParse (
			"    a\ta\n    ὐ\ta",
			"<pre><code>a\ta\nὐ\ta\n</code></pre>");

		/* [Example 4](http://spec.commonmark.org/0.28/#example-4) */
		[TestMethod]
		public void Example4 () => TestParse (
			"  - foo\n\n\tbar", 
			"<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>");

		/* [Example 5](http://spec.commonmark.org/0.28/#example-5) */
		[TestMethod]
		public void Example5 () => TestParse (
			"- foo\n\n\t\tbar", 
			"<ul>\n<li>\n<p>foo</p>\n<pre><code>  bar\n</code></pre>\n</li>\n</ul>");

		/* [Example 6](http://spec.commonmark.org/0.28/#example-6) */
		[TestMethod]
		public void Example6 () => TestParse (
			">\t\tfoo", 
			"<blockquote>\n<pre><code>  foo\n</code></pre>\n</blockquote>");

	}
}
