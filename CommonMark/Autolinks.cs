namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Autolinks : TestBase
	{
		/* [Example 565](http://spec.commonmark.org/0.28/#example-565) */
		[TestMethod]
		public void Example565 () => TestParse (
			"<http://foo.bar.baz>", 
			"<p><a href=\"http://foo.bar.baz\">http://foo.bar.baz</a></p>");

		/* [Example 566](http://spec.commonmark.org/0.28/#example-566) */
		[TestMethod]
		public void Example566 () => TestParse (
			"<http://foo.bar.baz/test?q=hello&id=22&boolean>", 
			"<p><a href=\"http://foo.bar.baz/test?q=hello&amp;id=22&amp;boolean\">http://foo.bar.baz/test?q=hello&amp;id=22&amp;boolean</a></p>");

		/* [Example 567](http://spec.commonmark.org/0.28/#example-567) */
		[TestMethod]
		public void Example567 () => TestParse (
			"<irc://foo.bar:2233/baz>", 
			"<p><a href=\"irc://foo.bar:2233/baz\">irc://foo.bar:2233/baz</a></p>");

		/* [Example 568](http://spec.commonmark.org/0.28/#example-568) */
		[TestMethod]
		public void Example568 () => TestParse (
			"<MAILTO:FOO@BAR.BAZ>", 
			"<p><a href=\"MAILTO:FOO@BAR.BAZ\">MAILTO:FOO@BAR.BAZ</a></p>");

		/* [Example 569](http://spec.commonmark.org/0.28/#example-569) */
		[TestMethod]
		public void Example569 () => TestParse (
			"<a+b+c:d>", 
			"<p><a href=\"a+b+c:d\">a+b+c:d</a></p>");

		/* [Example 570](http://spec.commonmark.org/0.28/#example-570) */
		[TestMethod]
		public void Example570 () => TestParse (
			"<made-up-scheme://foo,bar>", 
			"<p><a href=\"made-up-scheme://foo,bar\">made-up-scheme://foo,bar</a></p>");

		/* [Example 571](http://spec.commonmark.org/0.28/#example-571) */
		[TestMethod]
		public void Example571 () => TestParse (
			"<http://../>", 
			"<p><a href=\"http://../\">http://../</a></p>");

		/* [Example 572](http://spec.commonmark.org/0.28/#example-572) */
		[TestMethod]
		public void Example572 () => TestParse (
			"<localhost:5001/foo>", 
			"<p><a href=\"localhost:5001/foo\">localhost:5001/foo</a></p>");

		/* [Example 573](http://spec.commonmark.org/0.28/#example-573) */
		[TestMethod]
		public void Example573 () => TestParse (
			"<http://foo.bar/baz bim>", 
			"<p>&lt;http://foo.bar/baz bim&gt;</p>");

		/* [Example 574](http://spec.commonmark.org/0.28/#example-574) */
		[TestMethod]
		public void Example574 () => TestParse (
			"<http://example.com/\\[\\>", 
			"<p><a href=\"http://example.com/%5C%5B%5C\">http://example.com/\\[\\</a></p>");

		/* [Example 575](http://spec.commonmark.org/0.28/#example-575) */
		[TestMethod]
		public void Example575 () => TestParse (
			"<foo@bar.example.com>", 
			"<p><a href=\"mailto:foo@bar.example.com\">foo@bar.example.com</a></p>");

		/* [Example 576](http://spec.commonmark.org/0.28/#example-576) */
		[TestMethod]
		public void Example576 () => TestParse (
			"<foo+special@Bar.baz-bar0.com>", 
			"<p><a href=\"mailto:foo+special@Bar.baz-bar0.com\">foo+special@Bar.baz-bar0.com</a></p>");

		/* [Example 577](http://spec.commonmark.org/0.28/#example-577) */
		[TestMethod]
		public void Example577 () => TestParse (
			"<foo\\+@bar.example.com>", 
			"<p>&lt;foo+@bar.example.com&gt;</p>");

		/* [Example 578](http://spec.commonmark.org/0.28/#example-578) */
		[TestMethod]
		public void Example578 () => TestParse (
			"<>", 
			"<p>&lt;&gt;</p>");

		/* [Example 579](http://spec.commonmark.org/0.28/#example-579) */
		[TestMethod]
		public void Example579 () => TestParse (
			"< http://foo.bar >", 
			"<p>&lt; http://foo.bar &gt;</p>");

		/* [Example 580](http://spec.commonmark.org/0.28/#example-580) */
		[TestMethod]
		public void Example580 () => TestParse (
			"<m:abc>", 
			"<p>&lt;m:abc&gt;</p>");

		/* [Example 581](http://spec.commonmark.org/0.28/#example-581) */
		[TestMethod]
		public void Example581 () => TestParse (
			"<foo.bar.baz>", 
			"<p>&lt;foo.bar.baz&gt;</p>");

		/* [Example 582](http://spec.commonmark.org/0.28/#example-582) */
		[TestMethod]
		public void Example582 () => TestParse (
			"http://example.com", 
			"<p>http://example.com</p>");

		/* [Example 583](http://spec.commonmark.org/0.28/#example-583) */
		[TestMethod]
		public void Example583 () => TestParse (
			"foo@bar.example.com", 
			"<p>foo@bar.example.com</p>");
	}
}
