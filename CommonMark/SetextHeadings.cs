namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class SetextHeadings : TestBase
	{
		/* [Example 050](http://spec.commonmark.org/0.28/#example-050) */
		[TestMethod]
		public void Example050 () => TestParse (
			"Foo *bar*\n=========\n\nFoo *bar*\n---------", 
			"<h1>Foo <em>bar</em></h1>\n<h2>Foo <em>bar</em></h2>");

		/* [Example 051](http://spec.commonmark.org/0.28/#example-051) */
		[TestMethod]
		public void Example051 () => TestParse (
			"Foo *bar\nbaz*\n====", 
			"<h1>Foo <em>bar\nbaz</em></h1>");

		/* [Example 052](http://spec.commonmark.org/0.28/#example-052) */
		[TestMethod]
		public void Example052 () => TestParse (
			"Foo\n-------------------------\n\nFoo\n=", 
			"<h2>Foo</h2>\n<h1>Foo</h1>");

		/* [Example 053](http://spec.commonmark.org/0.28/#example-053) */
		[TestMethod]
		public void Example053 () => TestParse (
			"   Foo\n---\n\n  Foo\n-----\n\n  Foo\n  ===", 
			"<h2>Foo</h2>\n<h2>Foo</h2>\n<h1>Foo</h1>");

		/* [Example 054](http://spec.commonmark.org/0.28/#example-054) */
		[TestMethod]
		public void Example054 () => TestParse (
			"    Foo\n    ---\n\n    Foo\n---", 
			"<pre><code>Foo\n---\n\nFoo\n</code></pre>\n<hr />");

		/* [Example 055](http://spec.commonmark.org/0.28/#example-055) */
		[TestMethod]
		public void Example055 () => TestParse (
			"Foo\n   ----      ", 
			"<h2>Foo</h2>");

		/* [Example 056](http://spec.commonmark.org/0.28/#example-056) */
		[TestMethod]
		public void Example056 () => TestParse (
			"Foo\n    ---", 
			"<p>Foo\n---</p>");

		/* [Example 057](http://spec.commonmark.org/0.28/#example-057) */
		[TestMethod]
		public void Example057 () => TestParse (
			"Foo\n= =\n\nFoo\n--- -", 
			"<p>Foo\n= =</p>\n<p>Foo</p>\n<hr />");

		/* [Example 058](http://spec.commonmark.org/0.28/#example-058) */
		[TestMethod]
		public void Example058 () => TestParse (
			"Foo  \n-----", 
			"<h2>Foo</h2>");

		/* [Example 059](http://spec.commonmark.org/0.28/#example-059) */
		[TestMethod]
		public void Example059 () => TestParse (
			"Foo\\\n----", 
			"<h2>Foo\\</h2>");

		/* [Example 060](http://spec.commonmark.org/0.28/#example-060) */
		[TestMethod]
		public void Example060 () => TestParse (
			"`Foo\n----\n`\n\n<a title=\"a lot\n---\nof dashes\"/>", 
			"<h2>`Foo</h2>\n<p>`</p>\n<h2>&lt;a title=&quot;a lot</h2>\n<p>of dashes&quot;/&gt;</p>");
	}
}
