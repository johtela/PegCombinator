namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class LinkReferenceDefinitions : TestBase
	{
		/* [Example 159](http://spec.commonmark.org/0.28/#example-159) */
		[TestMethod]
		public void Example159 () => TestParse (
			"[foo]: /url \"title\"\n\n[foo]", 
			"<p><a href=\"/url\" title=\"title\">foo</a></p>");

		/* [Example 160](http://spec.commonmark.org/0.28/#example-160) */
		[TestMethod]
		public void Example160 () => TestParse (
			"   [foo]: \n      /url  \n           'the title'  \n\n[foo]", 
			"<p><a href=\"/url\" title=\"the title\">foo</a></p>");

		/* [Example 161](http://spec.commonmark.org/0.28/#example-161) */
		[TestMethod]
		public void Example161 () => TestParse (
			"[Foo*bar\\]]:my_(url) 'title (with parens)'\n\n[Foo*bar\\]]", 
			"<p><a href=\"my_(url)\" title=\"title (with parens)\">Foo*bar]</a></p>");

		/* [Example 162](http://spec.commonmark.org/0.28/#example-162) */
		[TestMethod]
		public void Example162 () => TestParse (
			"[Foo bar]:\n<my%20url>\n'title'\n\n[Foo bar]", 
			"<p><a href=\"my%20url\" title=\"title\">Foo bar</a></p>");

		/* [Example 163](http://spec.commonmark.org/0.28/#example-163) */
		[TestMethod]
		public void Example163 () => TestParse (
			"[foo]: /url '\ntitle\nline1\nline2\n'\n\n[foo]", 
			"<p><a href=\"/url\" title=\"\ntitle\nline1\nline2\n\">foo</a></p>");

		/* [Example 164](http://spec.commonmark.org/0.28/#example-164) */
		[TestMethod]
		public void Example164 () => TestParse (
			"[foo]: /url 'title\n\nwith blank line'\n\n[foo]", 
			"<p>[foo]: /url 'title</p>\n<p>with blank line'</p>\n<p>[foo]</p>");

		/* [Example 165](http://spec.commonmark.org/0.28/#example-165) */
		[TestMethod]
		public void Example165 () => TestParse (
			"[foo]:\n/url\n\n[foo]", 
			"<p><a href=\"/url\">foo</a></p>");

		/* [Example 166](http://spec.commonmark.org/0.28/#example-166) */
		[TestMethod]
		public void Example166 () => TestParse (
			"[foo]:\n\n[foo]", 
			"<p>[foo]:</p>\n<p>[foo]</p>");

		/* [Example 167](http://spec.commonmark.org/0.28/#example-167) */
		[TestMethod]
		public void Example167 () => TestParse (
			"[foo]: /url\\bar\\*baz \"foo\\\"bar\\baz\"\n\n[foo]", 
			"<p><a href=\"/url%5Cbar*baz\" title=\"foo&quot;bar\\baz\">foo</a></p>");

		/* [Example 168](http://spec.commonmark.org/0.28/#example-168) */
		[TestMethod]
		public void Example168 () => TestParse (
			"[foo]\n\n[foo]: url", 
			"<p><a href=\"url\">foo</a></p>");

		/* [Example 169](http://spec.commonmark.org/0.28/#example-169) */
		[TestMethod]
		public void Example169 () => TestParse (
			"[foo]\n\n[foo]: first\n[foo]: second", 
			"<p><a href=\"first\">foo</a></p>");

		/* [Example 170](http://spec.commonmark.org/0.28/#example-170) */
		[TestMethod]
		public void Example170 () => TestParse (
			"[FOO]: /url\n\n[Foo]", 
			"<p><a href=\"/url\">Foo</a></p>");

		/* [Example 171](http://spec.commonmark.org/0.28/#example-171) */
		[TestMethod]
		public void Example171 () => TestParse (
			"[ΑΓΩ]: /φου\n\n[αγω]", 
			"<p><a href=\"/%CF%86%CE%BF%CF%85\">αγω</a></p>");

		/* [Example 172](http://spec.commonmark.org/0.28/#example-172) */
		[TestMethod]
		public void Example172 () => TestParse (
			"[foo]: /url", 
			"");

		/* [Example 173](http://spec.commonmark.org/0.28/#example-173) */
		[TestMethod]
		public void Example173 () => TestParse (
			"[\nfoo\n]: /url\nbar", 
			"<p>bar</p>");

		/* [Example 174](http://spec.commonmark.org/0.28/#example-174) */
		[TestMethod]
		public void Example174 () => TestParse (
			"[foo]: /url \"title\" ok", 
			"<p>[foo]: /url &quot;title&quot; ok</p>");

		/* [Example 175](http://spec.commonmark.org/0.28/#example-175) */
		[TestMethod]
		public void Example175 () => TestParse (
			"[foo]: /url\n\"title\" ok", 
			"<p>&quot;title&quot; ok</p>");

		/* [Example 176](http://spec.commonmark.org/0.28/#example-176) */
		[TestMethod]
		public void Example176 () => TestParse (
			"    [foo]: /url \"title\"\n\n[foo]", 
			"<pre><code>[foo]: /url &quot;title&quot;\n</code></pre>\n<p>[foo]</p>");

		/* [Example 177](http://spec.commonmark.org/0.28/#example-177) */
		[TestMethod]
		public void Example177 () => TestParse (
			"```\n[foo]: /url\n```\n\n[foo]", 
			"<pre><code>[foo]: /url\n</code></pre>\n<p>[foo]</p>");

		/* [Example 178](http://spec.commonmark.org/0.28/#example-178) */
		[TestMethod]
		public void Example178 () => TestParse (
			"Foo\n[bar]: /baz\n\n[bar]", 
			"<p>Foo\n[bar]: /baz</p>\n<p>[bar]</p>");

		/* [Example 179](http://spec.commonmark.org/0.28/#example-179) */
		[TestMethod]
		public void Example179 () => TestParse (
			"# [Foo]\n[foo]: /url\n> bar", 
			"<h1><a href=\"/url\">Foo</a></h1>\n<blockquote>\n<p>bar</p>\n</blockquote>");

		/* [Example 180](http://spec.commonmark.org/0.28/#example-180) */
		[TestMethod]
		public void Example180 () => TestParse (
			"[foo]: /foo-url \"foo\"\n[bar]: /bar-url\n  \"bar\"\n[baz]: /baz-url\n\n[foo],\n[bar],\n[baz]", 
			"<p><a href=\"/foo-url\" title=\"foo\">foo</a>,\n<a href=\"/bar-url\" title=\"bar\">bar</a>,\n<a href=\"/baz-url\">baz</a></p>");

		/* [Example 181](http://spec.commonmark.org/0.28/#example-181) */
		[TestMethod]
		public void Example181 () => TestParse (
			"[foo]\n\n> [foo]: /url", 
			"<p><a href=\"/url\">foo</a></p>\n<blockquote>\n</blockquote>");
	}
}
