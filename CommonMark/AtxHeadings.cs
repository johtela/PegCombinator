namespace CommonMark
{
	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class AtxHeadings : TestBase
	{
		/* [Example 32](http://spec.commonmark.org/0.28/#example-32) */
		[TestMethod]
		public void Example32 () => TestParse (
			"# foo\n## foo\n### foo\n#### foo\n##### foo\n###### foo\n",
			"<h1>foo</h1>\n<h2>foo</h2>\n<h3>foo</h3>\n<h4>foo</h4>\n<h5>foo</h5>\n<h6>foo</h6>\n");

		/* [Example 33](http://spec.commonmark.org/0.28/#example-33) */
		[TestMethod]
		public void Example33 () => TestParse (
			"####### foo\n",
			"<p>####### foo</p>");
	}
}
