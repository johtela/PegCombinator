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

		[TestMethod]
		public void Example197b () => TestParse (
			"> foo\n> ---",
			"<blockquote>\n<h2>foo</h2>\n</blockquote>\n");

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

		[TestMethod]
		public void Example200b () => TestParse (
			"> ```\n> foo\n> ```",
			"<blockquote>\n<pre><code>foo\n</code></pre>\n</blockquote>\n");

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
	}
}
