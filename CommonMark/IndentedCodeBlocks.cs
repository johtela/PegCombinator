namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class IndentedCodeBlocks : TestBase
	{
		/* [Example 76](http://spec.commonmark.org/0.28/#example-76) */
		[TestMethod]
		public void Example76 () => TestParse (
			"    a simple\n      indented code block", 
			"<pre><code>a simple\n  indented code block\n</code></pre>");

		/* [Example 77](http://spec.commonmark.org/0.28/#example-77) */
		[TestMethod]
		public void Example77 () => TestParse (
			"  - foo\n\n    bar", 
			"<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>");

		/* [Example 78](http://spec.commonmark.org/0.28/#example-78) */
		[TestMethod]
		public void Example78 () => TestParse (
			"1.  foo\n\n    - bar", 
			"<ol>\n<li>\n<p>foo</p>\n<ul>\n<li>bar</li>\n</ul>\n</li>\n</ol>");

		/* [Example 79](http://spec.commonmark.org/0.28/#example-79) */
		[TestMethod]
		public void Example79 () => TestParse (
			"    <a/>\n    *hi*\n\n    - one", 
			"<pre><code>&lt;a/&gt;\n*hi*\n\n- one\n</code></pre>");

		/* [Example 80](http://spec.commonmark.org/0.28/#example-80) */
		[TestMethod]
		public void Example80 () => TestParse (
			"    chunk1\n\n    chunk2\n  \n \n \n    chunk3", 
			"<pre><code>chunk1\n\nchunk2\n\n\n\nchunk3\n</code></pre>");

		/* [Example 81](http://spec.commonmark.org/0.28/#example-81) */
		[TestMethod]
		public void Example81 () => TestParse (
			"    chunk1\n      \n      chunk2", 
			"<pre><code>chunk1\n  \n  chunk2\n</code></pre>");

		/* [Example 82](http://spec.commonmark.org/0.28/#example-82) */
		[TestMethod]
		public void Example82 () => TestParse (
			"Foo\n    bar\n", 
			"<p>Foo\nbar</p>");

		/* [Example 83](http://spec.commonmark.org/0.28/#example-83) */
		[TestMethod]
		public void Example83 () => TestParse (
			"    foo\nbar", 
			"<pre><code>foo\n</code></pre>\n<p>bar</p>");

		/* [Example 84](http://spec.commonmark.org/0.28/#example-84) */
		[TestMethod]
		public void Example84 () => TestParse (
			"# Heading\n    foo\nHeading\n------\n    foo\n----", 
			"<h1>Heading</h1>\n<pre><code>foo\n</code></pre>\n<h2>Heading</h2>\n<pre><code>foo\n</code></pre>\n<hr />");

		/* [Example 85](http://spec.commonmark.org/0.28/#example-85) */
		[TestMethod]
		public void Example85 () => TestParse (
			"        foo\n    bar", 
			"<pre><code>    foo\nbar\n</code></pre>");

		/* [Example 86](http://spec.commonmark.org/0.28/#example-86) */
		[TestMethod]
		public void Example86 () => TestParse (
			"    \n    foo\n    \n", 
			"<pre><code>foo\n</code></pre>");

		/* [Example 87](http://spec.commonmark.org/0.28/#example-87) */
		[TestMethod]
		public void Example87 () => TestParse (
			"    foo  ", 
			"<pre><code>foo  \n</code></pre>");
	}
}
