namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class ListItems : TestBase
	{
		/* [Example 216](http://spec.commonmark.org/0.28/#example-216) */
		[TestMethod]
		public void Example216 () => TestParse (
			"A paragraph\nwith two lines.\n\n    indented code\n\n> A block quote.", 
			"<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>");

		/* [Example 217](http://spec.commonmark.org/0.28/#example-217) */
		[TestMethod]
		public void Example217 () => TestParse (
			"1.  A paragraph\n    with two lines.\n\n        indented code\n\n    > A block quote.", 
			"<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>");

		/* [Example 218](http://spec.commonmark.org/0.28/#example-218) */
		[TestMethod]
		public void Example218 () => TestParse (
			"- one\n\n two", 
			"<ul>\n<li>one</li>\n</ul>\n<p>two</p>");

		/* [Example 219](http://spec.commonmark.org/0.28/#example-219) */
		[TestMethod]
		public void Example219 () => TestParse (
			"- one\n\n  two", 
			"<ul>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ul>");

		/* [Example 220](http://spec.commonmark.org/0.28/#example-220) */
		[TestMethod]
		public void Example220 () => TestParse (
			" -    one\n\n     two", 
			"<ul>\n<li>one</li>\n</ul>\n<pre><code> two\n</code></pre>");

		/* [Example 221](http://spec.commonmark.org/0.28/#example-221) */
		[TestMethod]
		public void Example221 () => TestParse (
			" -    one\n\n      two", 
			"<ul>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ul>");

		/* [Example 222](http://spec.commonmark.org/0.28/#example-222) */
		[TestMethod]
		public void Example222 () => TestParse (
			"   > > 1.  one\n>>\n>>     two", 
			"<blockquote>\n<blockquote>\n<ol>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ol>\n</blockquote>\n</blockquote>");

		/* [Example 223](http://spec.commonmark.org/0.28/#example-223) */
		[TestMethod]
		public void Example223 () => TestParse (
			">>- one\n>>\n  >  > two", 
			"<blockquote>\n<blockquote>\n<ul>\n<li>one</li>\n</ul>\n<p>two</p>\n</blockquote>\n</blockquote>");

		/* [Example 224](http://spec.commonmark.org/0.28/#example-224) */
		[TestMethod]
		public void Example224 () => TestParse (
			"-one\n\n2.two", 
			"<p>-one</p>\n<p>2.two</p>");

		/* [Example 225](http://spec.commonmark.org/0.28/#example-225) */
		[TestMethod]
		public void Example225 () => TestParse (
			"- foo\n\n\n  bar", 
			"<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>");

		/* [Example 226](http://spec.commonmark.org/0.28/#example-226) */
		[TestMethod]
		public void Example226 () => TestParse (
			"1.  foo\n\n    ```\n    bar\n    ```\n\n    baz\n\n    > bam", 
			"<ol>\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n<p>baz</p>\n<blockquote>\n<p>bam</p>\n</blockquote>\n</li>\n</ol>");

		/* [Example 227](http://spec.commonmark.org/0.28/#example-227) */
		[TestMethod]
		public void Example227 () => TestParse (
			"- Foo\n\n      bar\n\n\n      baz", 
			"<ul>\n<li>\n<p>Foo</p>\n<pre><code>bar\n\n\nbaz\n</code></pre>\n</li>\n</ul>");

		/* [Example 228](http://spec.commonmark.org/0.28/#example-228) */
		[TestMethod]
		public void Example228 () => TestParse (
			"123456789. ok", 
			"<ol start=\"123456789\">\n<li>ok</li>\n</ol>");

		/* [Example 229](http://spec.commonmark.org/0.28/#example-229) */
		[TestMethod]
		public void Example229 () => TestParse (
			"1234567890. not ok", 
			"<p>1234567890. not ok</p>");

		/* [Example 230](http://spec.commonmark.org/0.28/#example-230) */
		[TestMethod]
		public void Example230 () => TestParse (
			"0. ok", 
			"<ol start=\"0\">\n<li>ok</li>\n</ol>");

		/* [Example 231](http://spec.commonmark.org/0.28/#example-231) */
		[TestMethod]
		public void Example231 () => TestParse (
			"003. ok", 
			"<ol start=\"3\">\n<li>ok</li>\n</ol>");

		/* [Example 232](http://spec.commonmark.org/0.28/#example-232) */
		[TestMethod]
		public void Example232 () => TestParse (
			"-1. not ok", 
			"<p>-1. not ok</p>");

		/* [Example 233](http://spec.commonmark.org/0.28/#example-233) */
		[TestMethod]
		public void Example233 () => TestParse (
			"- foo\n\n      bar", 
			"<ul>\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n</li>\n</ul>");

		/* [Example 234](http://spec.commonmark.org/0.28/#example-234) */
		[TestMethod]
		public void Example234 () => TestParse (
			"  10.  foo\n\n           bar", 
			"<ol start=\"10\">\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n</li>\n</ol>");

		/* [Example 235](http://spec.commonmark.org/0.28/#example-235) */
		[TestMethod]
		public void Example235 () => TestParse (
			"    indented code\n\nparagraph\n\n    more code", 
			"<pre><code>indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>");

		/* [Example 236](http://spec.commonmark.org/0.28/#example-236) */
		[TestMethod]
		public void Example236 () => TestParse (
			"1.     indented code\n\n   paragraph\n\n       more code", 
			"<ol>\n<li>\n<pre><code>indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>\n</li>\n</ol>");

		/* [Example 237](http://spec.commonmark.org/0.28/#example-237) */
		[TestMethod]
		public void Example237 () => TestParse (
			"1.      indented code\n\n   paragraph\n\n       more code", 
			"<ol>\n<li>\n<pre><code> indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>\n</li>\n</ol>");

		/* [Example 238](http://spec.commonmark.org/0.28/#example-238) */
		[TestMethod]
		public void Example238 () => TestParse (
			"   foo\n\nbar", 
			"<p>foo</p>\n<p>bar</p>");

		/* [Example 239](http://spec.commonmark.org/0.28/#example-239) */
		[TestMethod]
		public void Example239 () => TestParse (
			"-    foo\n\n  bar", 
			"<ul>\n<li>foo</li>\n</ul>\n<p>bar</p>");

		/* [Example 240](http://spec.commonmark.org/0.28/#example-240) */
		[TestMethod]
		public void Example240 () => TestParse (
			"-  foo\n\n   bar", 
			"<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>");

		/* [Example 241](http://spec.commonmark.org/0.28/#example-241) */
		[TestMethod]
		public void Example241 () => TestParse (
			"-\n  foo\n-\n  ```\n  bar\n  ```\n-\n      baz", 
			"<ul>\n<li>foo</li>\n<li>\n<pre><code>bar\n</code></pre>\n</li>\n<li>\n<pre><code>baz\n</code></pre>\n</li>\n</ul>");

	}
}
