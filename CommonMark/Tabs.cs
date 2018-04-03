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
	}
}
