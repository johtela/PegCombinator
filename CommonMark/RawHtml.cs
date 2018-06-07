namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class RawHtml : TestBase
	{
		/* [Example 584](http://spec.commonmark.org/0.28/#example-584) */
		[TestMethod]
		public void Example584 () => TestParse (
			"<a><bab><c2c>", 
			"<p><a><bab><c2c></p>");

		/* [Example 585](http://spec.commonmark.org/0.28/#example-585) */
		[TestMethod]
		public void Example585 () => TestParse (
			"<a/><b2/>", 
			"<p><a/><b2/></p>");

		/* [Example 586](http://spec.commonmark.org/0.28/#example-586) */
		[TestMethod]
		public void Example586 () => TestParse (
			"<a  /><b2\ndata=\"foo\" >", 
			"<p><a  /><b2\ndata=\"foo\" ></p>");

		/* [Example 587](http://spec.commonmark.org/0.28/#example-587) */
		[TestMethod]
		public void Example587 () => TestParse (
			"<a foo=\"bar\" bam = 'baz <em>\"</em>'\n_boolean zoop:33=zoop:33 />", 
			"<p><a foo=\"bar\" bam = 'baz <em>\"</em>'\n_boolean zoop:33=zoop:33 /></p>");

		/* [Example 588](http://spec.commonmark.org/0.28/#example-588) */
		[TestMethod]
		public void Example588 () => TestParse (
			"Foo <responsive-image src=\"foo.jpg\" />", 
			"<p>Foo <responsive-image src=\"foo.jpg\" /></p>");

		/* [Example 589](http://spec.commonmark.org/0.28/#example-589) */
		[TestMethod]
		public void Example589 () => TestParse (
			"<33> <__>", 
			"<p>&lt;33&gt; &lt;__&gt;</p>");

		/* [Example 590](http://spec.commonmark.org/0.28/#example-590) */
		[TestMethod]
		public void Example590 () => TestParse (
			"<a h*#ref=\"hi\">", 
			"<p>&lt;a h*#ref=&quot;hi&quot;&gt;</p>");

		/* [Example 591](http://spec.commonmark.org/0.28/#example-591) */
		[TestMethod]
		public void Example591 () => TestParse (
			"<a href=\"hi'> <a href=hi'>", 
			"<p>&lt;a href=&quot;hi'&gt; &lt;a href=hi'&gt;</p>");

		/* [Example 592](http://spec.commonmark.org/0.28/#example-592) */
		[TestMethod]
		public void Example592 () => TestParse (
			"< a><\nfoo><bar/ >", 
			"<p>&lt; a&gt;&lt;\nfoo&gt;&lt;bar/ &gt;</p>");

		/* [Example 593](http://spec.commonmark.org/0.28/#example-593) */
		[TestMethod]
		public void Example593 () => TestParse (
			"<a href='bar'title=title>", 
			"<p>&lt;a href='bar'title=title&gt;</p>");

		/* [Example 594](http://spec.commonmark.org/0.28/#example-594) */
		[TestMethod]
		public void Example594 () => TestParse (
			"</a></foo >", 
			"<p></a></foo ></p>");

		/* [Example 595](http://spec.commonmark.org/0.28/#example-595) */
		[TestMethod]
		public void Example595 () => TestParse (
			"</a href=\"foo\">", 
			"<p>&lt;/a href=&quot;foo&quot;&gt;</p>");

		/* [Example 596](http://spec.commonmark.org/0.28/#example-596) */
		[TestMethod]
		public void Example596 () => TestParse (
			"foo <!-- this is a\ncomment - with hyphen -->", 
			"<p>foo <!-- this is a\ncomment - with hyphen --></p>");

		/* [Example 597](http://spec.commonmark.org/0.28/#example-597) */
		[TestMethod]
		public void Example597 () => TestParse (
			"foo <!-- not a comment -- two hyphens -->", 
			"<p>foo &lt;!-- not a comment -- two hyphens --&gt;</p>");

		/* [Example 598](http://spec.commonmark.org/0.28/#example-598) */
		[TestMethod]
		public void Example598 () => TestParse (
			"foo <!--> foo -->\n\nfoo <!-- foo--->", 
			"<p>foo &lt;!--&gt; foo --&gt;</p>\n<p>foo &lt;!-- foo---&gt;</p>");

		/* [Example 599](http://spec.commonmark.org/0.28/#example-599) */
		[TestMethod]
		public void Example599 () => TestParse (
			"foo <?php echo $a; ?>", 
			"<p>foo <?php echo $a; ?></p>");

		/* [Example 600](http://spec.commonmark.org/0.28/#example-600) */
		[TestMethod]
		public void Example600 () => TestParse (
			"foo <!ELEMENT br EMPTY>", 
			"<p>foo <!ELEMENT br EMPTY></p>");

		/* [Example 601](http://spec.commonmark.org/0.28/#example-601) */
		[TestMethod]
		public void Example601 () => TestParse (
			"foo <![CDATA[>&<]]>", 
			"<p>foo <![CDATA[>&<]]></p>");

		/* [Example 602](http://spec.commonmark.org/0.28/#example-602) */
		[TestMethod]
		public void Example602 () => TestParse (
			"foo <a href=\"&ouml;\">", 
			"<p>foo <a href=\"&ouml;\"></p>");

		/* [Example 603](http://spec.commonmark.org/0.28/#example-603) */
		[TestMethod]
		public void Example603 () => TestParse (
			"foo <a href=\"\\*\">", 
			"<p>foo <a href=\"\\*\"></p>");

		/* [Example 604](http://spec.commonmark.org/0.28/#example-604) */
		[TestMethod]
		public void Example604 () => TestParse (
			"<a href=\"\\\"\">", 
			"<p>&lt;a href=&quot;&quot;&quot;&gt;</p>");
	}
}
