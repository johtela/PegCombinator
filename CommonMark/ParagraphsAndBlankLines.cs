namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class ParagraphsAndBlankLines : TestBase
	{
		/* [Example 182](http://spec.commonmark.org/0.28/#example-182) */
		[TestMethod]
		public void Example182 () => TestParse (
			"aaa\n\nbbb", 
			"<p>aaa</p>\n<p>bbb</p>");

		/* [Example 183](http://spec.commonmark.org/0.28/#example-183) */
		[TestMethod]
		public void Example183 () => TestParse (
			"aaa\nbbb\n\nccc\nddd", 
			"<p>aaa\nbbb</p>\n<p>ccc\nddd</p>");

		/* [Example 184](http://spec.commonmark.org/0.28/#example-184) */
		[TestMethod]
		public void Example184 () => TestParse (
			"aaa\n\n\nbbb", 
			"<p>aaa</p>\n<p>bbb</p>");

		/* [Example 185](http://spec.commonmark.org/0.28/#example-185) */
		[TestMethod]
		public void Example185 () => TestParse (
			"  aaa\n bbb", 
			"<p>aaa\nbbb</p>");

		/* [Example 186](http://spec.commonmark.org/0.28/#example-186) */
		[TestMethod]
		public void Example186 () => TestParse (
			"aaa\n             bbb\n                                       ccc", 
			"<p>aaa\nbbb\nccc</p>");

		/* [Example 187](http://spec.commonmark.org/0.28/#example-187) */
		[TestMethod]
		public void Example187 () => TestParse (
			"   aaa\nbbb", 
			"<p>aaa\nbbb</p>");

		/* [Example 188](http://spec.commonmark.org/0.28/#example-188) */
		[TestMethod]
		public void Example188 () => TestParse (
			"    aaa\nbbb", 
			"<pre><code>aaa\n</code></pre>\n<p>bbb</p>");

		/* [Example 189](http://spec.commonmark.org/0.28/#example-189) */
		[TestMethod]
		public void Example189 () => TestParse (
			"aaa     \nbbb     ", 
			"<p>aaa<br />\nbbb</p>");

		/* [Example 190](http://spec.commonmark.org/0.28/#example-190) */
		[TestMethod]
		public void Example190 () => TestParse (
			"  \n\naaa\n  \n\n# aaa\n\n  ", 
			"<p>aaa</p>\n<h1>aaa</h1>");
	}
}
