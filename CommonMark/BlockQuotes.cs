namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class BlockQuotes : TestBase
	{
		/* [Example 191](http://spec.commonmark.org/0.28/#example-191) */
		[TestMethod]
		public void Example191 () => TestParse (
			"> # Foo\n> bar\n> baz", 
			"<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>");

		/* [Example 192](http://spec.commonmark.org/0.28/#example-192) */
		[TestMethod]
		public void Example192 () => TestParse (
			"># Foo\n>bar\n> baz", 
			"<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>");

		/* [Example 193](http://spec.commonmark.org/0.28/#example-193) */
		[TestMethod]
		public void Example193 () => TestParse (
			"   > # Foo\n   > bar\n > baz", 
			"<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>");

		/* [Example 194](http://spec.commonmark.org/0.28/#example-194) */
		[TestMethod]
		public void Example194 () => TestParse (
			"    > # Foo\n    > bar\n    > baz", 
			"<pre><code>&gt; # Foo\n&gt; bar\n&gt; baz\n</code></pre>");

		/* [Example 195](http://spec.commonmark.org/0.28/#example-195) */
		[TestMethod]
		public void Example195 () => TestParse (
			"> # Foo\n> bar\nbaz", 
			"<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>");

		/* [Example 196](http://spec.commonmark.org/0.28/#example-196) */
		[TestMethod]
		public void Example196 () => TestParse (
			"> bar\nbaz\n> foo", 
			"<blockquote>\n<p>bar\nbaz\nfoo</p>\n</blockquote>");

		/* [Example 197](http://spec.commonmark.org/0.28/#example-197) */
		[TestMethod]
		public void Example197 () => TestParse (
			"> foo\n---", 
			"<blockquote>\n<p>foo</p>\n</blockquote>\n<hr />");

		/* [Example 198](http://spec.commonmark.org/0.28/#example-198) */
		[TestMethod]
		public void Example198 () => TestParse (
			"> - foo\n- bar", 
			"<blockquote>\n<ul>\n<li>foo</li>\n</ul>\n</blockquote>\n<ul>\n<li>bar</li>\n</ul>");

		/* [Example 199](http://spec.commonmark.org/0.28/#example-199) */
		[TestMethod]
		public void Example199 () => TestParse (
			">     foo\n    bar", 
			"<blockquote>\n<pre><code>foo\n</code></pre>\n</blockquote>\n<pre><code>bar\n</code></pre>");

		/* [Example 200](http://spec.commonmark.org/0.28/#example-200) */
		[TestMethod]
		public void Example200 () => TestParse (
			"> ```\nfoo\n```", 
			"<blockquote>\n<pre><code></code></pre>\n</blockquote>\n<p>foo</p>\n<pre><code></code></pre>");

		/* [Example 201](http://spec.commonmark.org/0.28/#example-201) */
		[TestMethod]
		public void Example201 () => TestParse (
			"> foo\n    - bar", 
			"<blockquote>\n<p>foo\n- bar</p>\n</blockquote>");

		/* [Example 202](http://spec.commonmark.org/0.28/#example-202) */
		[TestMethod]
		public void Example202 () => TestParse (
			">", 
			"<blockquote>\n</blockquote>");

		/* [Example 203](http://spec.commonmark.org/0.28/#example-203) */
		[TestMethod]
		public void Example203 () => TestParse (
			">\n>  \n> ", 
			"<blockquote>\n</blockquote>");

		/* [Example 204](http://spec.commonmark.org/0.28/#example-204) */
		[TestMethod]
		public void Example204 () => TestParse (
			">\n> foo\n>  ", 
			"<blockquote>\n<p>foo</p>\n</blockquote>");

		/* [Example 205](http://spec.commonmark.org/0.28/#example-205) */
		[TestMethod]
		public void Example205 () => TestParse (
			"> foo\n\n> bar", 
			"<blockquote>\n<p>foo</p>\n</blockquote>\n<blockquote>\n<p>bar</p>\n</blockquote>");

		/* [Example 206](http://spec.commonmark.org/0.28/#example-206) */
		[TestMethod]
		public void Example206 () => TestParse (
			"> foo\n> bar", 
			"<blockquote>\n<p>foo\nbar</p>\n</blockquote>");

		/* [Example 207](http://spec.commonmark.org/0.28/#example-207) */
		[TestMethod]
		public void Example207 () => TestParse (
			"> foo\n>\n> bar", 
			"<blockquote>\n<p>foo</p>\n<p>bar</p>\n</blockquote>");

		/* [Example 208](http://spec.commonmark.org/0.28/#example-208) */
		[TestMethod]
		public void Example208 () => TestParse (
			"foo\n> bar", 
			"<p>foo</p>\n<blockquote>\n<p>bar</p>\n</blockquote>");

		/* [Example 209](http://spec.commonmark.org/0.28/#example-209) */
		[TestMethod]
		public void Example209 () => TestParse (
			"> aaa\n***\n> bbb", 
			"<blockquote>\n<p>aaa</p>\n</blockquote>\n<hr />\n<blockquote>\n<p>bbb</p>\n</blockquote>");

		/* [Example 210](http://spec.commonmark.org/0.28/#example-210) */
		[TestMethod]
		public void Example210 () => TestParse (
			"> bar\nbaz", 
			"<blockquote>\n<p>bar\nbaz</p>\n</blockquote>");

		/* [Example 211](http://spec.commonmark.org/0.28/#example-211) */
		[TestMethod]
		public void Example211 () => TestParse (
			"> bar\n\nbaz", 
			"<blockquote>\n<p>bar</p>\n</blockquote>\n<p>baz</p>");

		/* [Example 212](http://spec.commonmark.org/0.28/#example-212) */
		[TestMethod]
		public void Example212 () => TestParse (
			"> bar\n>\nbaz", 
			"<blockquote>\n<p>bar</p>\n</blockquote>\n<p>baz</p>");

		/* [Example 213](http://spec.commonmark.org/0.28/#example-213) */
		[TestMethod]
		public void Example213 () => TestParse (
			"> > > foo\nbar", 
			"<blockquote>\n<blockquote>\n<blockquote>\n<p>foo\nbar</p>\n</blockquote>\n</blockquote>\n</blockquote>");

		/* [Example 214](http://spec.commonmark.org/0.28/#example-214) */
		[TestMethod]
		public void Example214 () => TestParse (
			">>> foo\n> bar\n>>baz", 
			"<blockquote>\n<blockquote>\n<blockquote>\n<p>foo\nbar\nbaz</p>\n</blockquote>\n</blockquote>\n</blockquote>");

		/* [Example 215](http://spec.commonmark.org/0.28/#example-215) */
		[TestMethod]
		public void Example215 () => TestParse (
			">     code\n\n>    not code", 
			"<blockquote>\n<pre><code>code\n</code></pre>\n</blockquote>\n<blockquote>\n<p>not code</p>\n</blockquote>");
	}
}
