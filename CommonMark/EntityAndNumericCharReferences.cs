namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class EntityAndNumericCharReferences : TestBase
	{
		/* [Example 302](http://spec.commonmark.org/0.28/#example-302) */
		[TestMethod]
		public void Example302 () => TestParse (
			"&nbsp; &amp; &copy; &AElig; &Dcaron;\n&frac34; &HilbertSpace; &DifferentialD;\n&ClockwiseContourIntegral; &ngE;",
			"<p>\u00A0 &amp; © Æ Ď\n¾ ℋ ⅆ\n∲ ≧̸</p>");

		/* [Example 303](http://spec.commonmark.org/0.28/#example-303) */
		[TestMethod]
		public void Example303 () => TestParse (
			"&#35; &#1234; &#992; &#98765432; &#0;", 
			"<p># Ӓ Ϡ � �</p>");

		/* [Example 304](http://spec.commonmark.org/0.28/#example-304) */
		[TestMethod]
		public void Example304 () => TestParse (
			"&#X22; &#XD06; &#xcab;", 
			"<p>&quot; ആ ಫ</p>");

		/* [Example 305](http://spec.commonmark.org/0.28/#example-305) */
		[TestMethod]
		public void Example305 () => TestParse (
			"&nbsp &x; &#; &#x;\n&ThisIsNotDefined; &hi?;", 
			"<p>&amp;nbsp &amp;x; &amp;#; &amp;#x;\n&amp;ThisIsNotDefined; &amp;hi?;</p>");

		/* [Example 306](http://spec.commonmark.org/0.28/#example-306) */
		[TestMethod]
		public void Example306 () => TestParse (
			"&copy", 
			"<p>&amp;copy</p>");

		/* [Example 307](http://spec.commonmark.org/0.28/#example-307) */
		[TestMethod]
		public void Example307 () => TestParse (
			"&MadeUpEntity;", 
			"<p>&amp;MadeUpEntity;</p>");

		/* [Example 308](http://spec.commonmark.org/0.28/#example-308) */
		[TestMethod]
		public void Example308 () => TestParse (
			"<a href=\"&ouml;&ouml;.html\">", 
			"<p><a href=\"&ouml;&ouml;.html\"></p>");

		/* [Example 309](http://spec.commonmark.org/0.28/#example-309) */
		[TestMethod]
		public void Example309 () => TestParse (
			"[foo](/f&ouml;&ouml; \"f&ouml;&ouml;\")", 
			"<p><a href=\"/f%C3%B6%C3%B6\" title=\"föö\">foo</a></p>");

		/* [Example 310](http://spec.commonmark.org/0.28/#example-310) */
		[TestMethod]
		public void Example310 () => TestParse (
			"[foo]\n\n[foo]: /f&ouml;&ouml; \"f&ouml;&ouml;\"", 
			"<p><a href=\"/f%C3%B6%C3%B6\" title=\"föö\">foo</a></p>");

		/* [Example 311](http://spec.commonmark.org/0.28/#example-311) */
		[TestMethod]
		public void Example311 () => TestParse (
			"``` f&ouml;&ouml;\nfoo\n```", 
			"<pre><code class=\"language-föö\">foo\n</code></pre>");

		/* [Example 312](http://spec.commonmark.org/0.28/#example-312) */
		[TestMethod]
		public void Example312 () => TestParse (
			"`f&ouml;&ouml;`", 
			"<p><code>f&amp;ouml;&amp;ouml;</code></p>");

		/* [Example 313](http://spec.commonmark.org/0.28/#example-313) */
		[TestMethod]
		public void Example313 () => TestParse (
			"    f&ouml;f&ouml;", 
			"<pre><code>f&amp;ouml;f&amp;ouml;\n</code></pre>");
	}
}
