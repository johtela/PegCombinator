namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class BackslashEscapes : TestBase
	{
		/* [Example 288](http://spec.commonmark.org/0.28/#example-288) */
		[TestMethod]
		public void Example288 () => TestParse (
			"`hi`lo`", 
			"<p><code>hi</code>lo`</p>");

		/* [Example 289](http://spec.commonmark.org/0.28/#example-289) */
		[TestMethod]
		public void Example289 () => TestParse (
			"\\!\\\"\\#\\$\\%\\&\\'\\(\\)\\*\\+\\,\\-\\.\\/\\:\\;\\<\\=\\>\\?\\@\\[\\\\\\]\\^\\_\\`\\{\\|\\}\\~", 
			"<p>!&quot;#$%&amp;'()*+,-./:;&lt;=&gt;?@[\\]^_`{|}~</p>");

		/* [Example 290](http://spec.commonmark.org/0.28/#example-290) */
		[TestMethod]
		public void Example290 () => TestParse (
			"\\\t\\A\\a\\ \\3\\φ\\«", 
			"<p>\\\t\\A\\a\\ \\3\\φ\\«</p>");

		/* [Example 291](http://spec.commonmark.org/0.28/#example-291) */
		[TestMethod]
		public void Example291 () => TestParse (
			"\\*not emphasized*\n\\<br/> not a tag\n\\[not a link](/foo)\n\\`not code`\n1\\. not a list\n\\* not a list\n\\# not a heading\n\\[foo]: /url \"not a reference\"", 
			"<p>*not emphasized*\n&lt;br/&gt; not a tag\n[not a link](/foo)\n`not code`\n1. not a list\n* not a list\n# not a heading\n[foo]: /url &quot;not a reference&quot;</p>");

		/* [Example 292](http://spec.commonmark.org/0.28/#example-292) */
		[TestMethod]
		public void Example292 () => TestParse (
			"\\\\*emphasis*", 
			"<p>\\<em>emphasis</em></p>");

		/* [Example 293](http://spec.commonmark.org/0.28/#example-293) */
		[TestMethod]
		public void Example293 () => TestParse (
			"foo\\\nbar", 
			"<p>foo<br />\nbar</p>");

		/* [Example 294](http://spec.commonmark.org/0.28/#example-294) */
		[TestMethod]
		public void Example294 () => TestParse (
			"`` \\[\\` ``", 
			"<p><code>\\[\\`</code></p>");

		/* [Example 295](http://spec.commonmark.org/0.28/#example-295) */
		[TestMethod]
		public void Example295 () => TestParse (
			"    \\[\\]", 
			"<pre><code>\\[\\]\n</code></pre>");

		/* [Example 296](http://spec.commonmark.org/0.28/#example-296) */
		[TestMethod]
		public void Example296 () => TestParse (
			"~~~\n\\[\\]\n~~~", 
			"<pre><code>\\[\\]\n</code></pre>");

		/* [Example 297](http://spec.commonmark.org/0.28/#example-297) */
		[TestMethod]
		public void Example297 () => TestParse (
			"<http://example.com?find=\\*>", 
			"<p><a href=\"http://example.com?find=%5C*\">http://example.com?find=\\*</a></p>");

		/* [Example 298](http://spec.commonmark.org/0.28/#example-298) */
		[TestMethod]
		public void Example298 () => TestParse (
			"<a href=\"/bar\\/)\">", 
			"<a href=\"/bar\\/)\">");

		/* [Example 299](http://spec.commonmark.org/0.28/#example-299) */
		[TestMethod]
		public void Example299 () => TestParse (
			"[foo](/bar\\* \"ti\\*tle\")", 
			"<p><a href=\"/bar*\" title=\"ti*tle\">foo</a></p>");

		/* [Example 300](http://spec.commonmark.org/0.28/#example-300) */
		[TestMethod]
		public void Example300 () => TestParse (
			"[foo]\n\n[foo]: /bar\\* \"ti\\*tle\"", 
			"<p><a href=\"/bar*\" title=\"ti*tle\">foo</a></p>");

		/* [Example 301](http://spec.commonmark.org/0.28/#example-301) */
		[TestMethod]
		public void Example301 () => TestParse (
			"``` foo\\+bar\nfoo\n```", 
			"<pre><code class=\"language-foo+bar\">foo\n</code></pre>");
	}
}
