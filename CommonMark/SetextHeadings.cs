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
	}
}
