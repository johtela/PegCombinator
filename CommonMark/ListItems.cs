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

		/* [Example 242](http://spec.commonmark.org/0.28/#example-242) */
		[TestMethod]
		public void Example242 () => TestParse (
			"-   \n  foo",
			"<ul>\n<li>foo</li>\n</ul>");

		/* [Example 243](http://spec.commonmark.org/0.28/#example-243) */
		[TestMethod]
		public void Example243 () => TestParse (
			"-\n\n  foo",
			"<ul>\n<li></li>\n</ul>\n<p>foo</p>");

		/* [Example 244](http://spec.commonmark.org/0.28/#example-244) */
		[TestMethod]
		public void Example244 () => TestParse (
			"- foo\n-\n- bar",
			"<ul>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ul>");

		/* [Example 245](http://spec.commonmark.org/0.28/#example-245) */
		[TestMethod]
		public void Example245 () => TestParse (
			"- foo\n-   \n- bar",
			"<ul>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ul>");

		/* [Example 246](http://spec.commonmark.org/0.28/#example-246) */
		[TestMethod]
		public void Example246 () => TestParse (
			"1. foo\n2.\n3. bar",
			"<ol>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ol>");

		/* [Example 247](http://spec.commonmark.org/0.28/#example-247) */
		[TestMethod]
		public void Example247 () => TestParse (
			"*",
			"<ul>\n<li></li>\n</ul>");

		/* [Example 248](http://spec.commonmark.org/0.28/#example-248) */
		[TestMethod]
		public void Example248 () => TestParse (
			"foo\n*\n\nfoo\n1.",
			"<p>foo\n*</p>\n<p>foo\n1.</p>");

		/* [Example 249](http://spec.commonmark.org/0.28/#example-249) */
		[TestMethod]
		public void Example249 () => TestParse (
			" 1.  A paragraph\n     with two lines.\n\n         indented code\n\n     > A block quote.",
			"<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>");

		/* [Example 250](http://spec.commonmark.org/0.28/#example-250) */
		[TestMethod]
		public void Example250 () => TestParse (
			"  1.  A paragraph\n      with two lines.\n\n          indented code\n\n      > A block quote.",
			"<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>");

		/* [Example 251](http://spec.commonmark.org/0.28/#example-251) */
		[TestMethod]
		public void Example251 () => TestParse (
			"   1.  A paragraph\n       with two lines.\n\n           indented code\n\n       > A block quote.",
			"<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>");

		/* [Example 252](http://spec.commonmark.org/0.28/#example-252) */
		[TestMethod]
		public void Example252 () => TestParse (
			"    1.  A paragraph\n        with two lines.\n\n            indented code\n\n        > A block quote.",
			"<pre><code>1.  A paragraph\n    with two lines.\n\n        indented code\n\n    &gt; A block quote.\n</code></pre>");

		/* [Example 253](http://spec.commonmark.org/0.28/#example-253) */
		[TestMethod]
		public void Example253 () => TestParse (
			"  1.  A paragraph\nwith two lines.\n\n          indented code\n\n      > A block quote.",
			"<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>");

		/* [Example 254](http://spec.commonmark.org/0.28/#example-254) */
		[TestMethod]
		public void Example254 () => TestParse (
			"  1.  A paragraph\n    with two lines.",
			"<ol>\n<li>A paragraph\nwith two lines.</li>\n</ol>");

		/* [Example 255](http://spec.commonmark.org/0.28/#example-255) */
		[TestMethod]
		public void Example255 () => TestParse (
			"> 1. > Blockquote\ncontinued here.",
			"<blockquote>\n<ol>\n<li>\n<blockquote>\n<p>Blockquote\ncontinued here.</p>\n</blockquote>\n</li>\n</ol>\n</blockquote>");

		/* [Example 256](http://spec.commonmark.org/0.28/#example-256) */
		[TestMethod]
		public void Example256 () => TestParse (
			"> 1. > Blockquote\n> continued here.",
			"<blockquote>\n<ol>\n<li>\n<blockquote>\n<p>Blockquote\ncontinued here.</p>\n</blockquote>\n</li>\n</ol>\n</blockquote>");

		/* [Example 257](http://spec.commonmark.org/0.28/#example-257) */
		[TestMethod]
		public void Example257 () => TestParse (
			"- foo\n  - bar\n    - baz\n      - boo",
			"<ul>\n<li>foo\n<ul>\n<li>bar\n<ul>\n<li>baz\n<ul>\n<li>boo</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>");

		/* [Example 258](http://spec.commonmark.org/0.28/#example-258) */
		[TestMethod]
		public void Example258 () => TestParse (
			"- foo\n - bar\n  - baz\n   - boo",
			"<ul>\n<li>foo</li>\n<li>bar</li>\n<li>baz</li>\n<li>boo</li>\n</ul>");

		/* [Example 259](http://spec.commonmark.org/0.28/#example-259) */
		[TestMethod]
		public void Example259 () => TestParse (
			"10) foo\n    - bar",
			"<ol start=\"10\">\n<li>foo\n<ul>\n<li>bar</li>\n</ul>\n</li>\n</ol>");

		/* [Example 260](http://spec.commonmark.org/0.28/#example-260) */
		[TestMethod]
		public void Example260 () => TestParse (
			"10) foo\n   - bar",
			"<ol start=\"10\">\n<li>foo</li>\n</ol>\n<ul>\n<li>bar</li>\n</ul>");

		/* [Example 261](http://spec.commonmark.org/0.28/#example-261) */
		[TestMethod]
		public void Example261 () => TestParse (
			"- - foo",
			"<ul>\n<li>\n<ul>\n<li>foo</li>\n</ul>\n</li>\n</ul>");

		/* [Example 262](http://spec.commonmark.org/0.28/#example-262) */
		[TestMethod]
		public void Example262 () => TestParse (
			"1. - 2. foo",
			"<ol>\n<li>\n<ul>\n<li>\n<ol start=\"2\">\n<li>foo</li>\n</ol>\n</li>\n</ul>\n</li>\n</ol>");

		/* [Example 263](http://spec.commonmark.org/0.28/#example-263) */
		[TestMethod]
		public void Example263 () => TestParse (
			"- # Foo\n- Bar\n  ---\n  baz", 
			"<ul>\n<li>\n<h1>Foo</h1>\n</li>\n<li>\n<h2>Bar</h2>\nbaz</li>\n</ul>");
	}
}
