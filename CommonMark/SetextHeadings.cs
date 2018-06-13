namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class SetextHeadings : TestBase
	{
		/* [Example 50](http://spec.commonmark.org/0.28/#example-50) */
		[TestMethod]
		public void Example50 () => TestParse (
			"Foo *bar*\n=========\n\nFoo *bar*\n---------", 
			"<h1>Foo <em>bar</em></h1>\n<h2>Foo <em>bar</em></h2>");

		/* [Example 51](http://spec.commonmark.org/0.28/#example-51) */
		[TestMethod]
		public void Example51 () => TestParse (
			"Foo *bar\nbaz*\n====", 
			"<h1>Foo <em>bar\nbaz</em></h1>");

		/* [Example 52](http://spec.commonmark.org/0.28/#example-52) */
		[TestMethod]
		public void Example52 () => TestParse (
			"Foo\n-------------------------\n\nFoo\n=", 
			"<h2>Foo</h2>\n<h1>Foo</h1>");

		/* [Example 53](http://spec.commonmark.org/0.28/#example-53) */
		[TestMethod]
		public void Example53 () => TestParse (
			"   Foo\n---\n\n  Foo\n-----\n\n  Foo\n  ===", 
			"<h2>Foo</h2>\n<h2>Foo</h2>\n<h1>Foo</h1>");

		/* [Example 54](http://spec.commonmark.org/0.28/#example-54) */
		[TestMethod]
		public void Example54 () => TestParse (
			"    Foo\n    ---\n\n    Foo\n---", 
			"<pre><code>Foo\n---\n\nFoo\n</code></pre>\n<hr />");

		/* [Example 55](http://spec.commonmark.org/0.28/#example-55) */
		[TestMethod]
		public void Example55 () => TestParse (
			"Foo\n   ----      ", 
			"<h2>Foo</h2>");

		/* [Example 56](http://spec.commonmark.org/0.28/#example-56) */
		[TestMethod]
		public void Example56 () => TestParse (
			"Foo\n    ---", 
			"<p>Foo\n---</p>");

		/* [Example 57](http://spec.commonmark.org/0.28/#example-57) */
		[TestMethod]
		public void Example57 () => TestParse (
			"Foo\n= =\n\nFoo\n--- -", 
			"<p>Foo\n= =</p>\n<p>Foo</p>\n<hr />");

		/* [Example 58](http://spec.commonmark.org/0.28/#example-58) */
		[TestMethod]
		public void Example58 () => TestParse (
			"Foo  \n-----", 
			"<h2>Foo</h2>");

		/* [Example 59](http://spec.commonmark.org/0.28/#example-59) */
		[TestMethod]
		public void Example59 () => TestParse (
			"Foo\\\n----", 
			"<h2>Foo\\</h2>");

		/* [Example 60](http://spec.commonmark.org/0.28/#example-60) */
		[TestMethod]
		public void Example60 () => TestParse (
			"`Foo\n----\n`\n\n<a title=\"a lot\n---\nof dashes\"/>", 
			"<h2>`Foo</h2>\n<p>`</p>\n<h2>&lt;a title=&quot;a lot</h2>\n<p>of dashes&quot;/&gt;</p>");

		/* [Example 61](http://spec.commonmark.org/0.28/#example-61) */
		[TestMethod]
		public void Example61 () => TestParse (
			"> Foo\n---", 
			"<blockquote>\n<p>Foo</p>\n</blockquote>\n<hr />");

		/* [Example 62](http://spec.commonmark.org/0.28/#example-62) */
		[TestMethod]
		public void Example62 () => TestParse (
			"> foo\nbar\n===", 
			"<blockquote>\n<p>foo\nbar\n===</p>\n</blockquote>");

		/* [Example 63](http://spec.commonmark.org/0.28/#example-63) */
		[TestMethod]
		public void Example63 () => TestParse (
			"- Foo\n---", 
			"<ul>\n<li>Foo</li>\n</ul>\n<hr />");

		/* [Example 64](http://spec.commonmark.org/0.28/#example-64) */
		[TestMethod]
		public void Example64 () => TestParse (
			"Foo\nBar\n---", 
			"<h2>Foo\nBar</h2>");

		/* [Example 65](http://spec.commonmark.org/0.28/#example-65) */
		[TestMethod]
		public void Example65 () => TestParse (
			"---\nFoo\n---\nBar\n---\nBaz", 
			"<hr />\n<h2>Foo</h2>\n<h2>Bar</h2>\n<p>Baz</p>");

		/* [Example 66](http://spec.commonmark.org/0.28/#example-66) */
		[TestMethod]
		public void Example66 () => TestParse (
			"====", 
			"<p>====</p>");

		/* [Example 67](http://spec.commonmark.org/0.28/#example-67) */
		[TestMethod]
		public void Example67 () => TestParse (
			"---\n---", 
			"<hr />\n<hr />");

		/* [Example 68](http://spec.commonmark.org/0.28/#example-68) */
		[TestMethod]
		public void Example68 () => TestParse (
			"- foo\n-----", 
			"<ul>\n<li>foo</li>\n</ul>\n<hr />");

		/* [Example 69](http://spec.commonmark.org/0.28/#example-69) */
		[TestMethod]
		public void Example69 () => TestParse (
			"    foo\n---", 
			"<pre><code>foo\n</code></pre>\n<hr />");

		/* [Example 70](http://spec.commonmark.org/0.28/#example-70) */
		[TestMethod]
		public void Example70 () => TestParse (
			"> foo\n-----", 
			"<blockquote>\n<p>foo</p>\n</blockquote>\n<hr />");

		/* [Example 71](http://spec.commonmark.org/0.28/#example-71) */
		[TestMethod]
		public void Example71 () => TestParse (
			"\\> foo\n------", 
			"<h2>&gt; foo</h2>");

		/* [Example 72](http://spec.commonmark.org/0.28/#example-72) */
		[TestMethod]
		public void Example72 () => TestParse (
			"Foo\n\nbar\n---\nbaz", 
			"<p>Foo</p>\n<h2>bar</h2>\n<p>baz</p>");

		/* [Example 73](http://spec.commonmark.org/0.28/#example-73) */
		[TestMethod]
		public void Example73 () => TestParse (
			"Foo\nbar\n\n---\n\nbaz", 
			"<p>Foo\nbar</p>\n<hr />\n<p>baz</p>");

		/* [Example 74](http://spec.commonmark.org/0.28/#example-74) */
		[TestMethod]
		public void Example74 () => TestParse (
			"Foo\nbar\n* * *\nbaz", 
			"<p>Foo\nbar</p>\n<hr />\n<p>baz</p>");

		/* [Example 75](http://spec.commonmark.org/0.28/#example-75) */
		[TestMethod]
		public void Example75 () => TestParse (
			"Foo\nbar\n\\---\nbaz", 
			"<p>Foo\nbar\n---\nbaz</p>");
	}
}
