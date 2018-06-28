namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Lists : TestBase
	{
		/* [Example 264](http://spec.commonmark.org/0.28/#example-264) */
		[TestMethod]
		public void Example264 () => TestParse (
			"- foo\n- bar\n+ baz",
			"<ul>\n<li>foo</li>\n<li>bar</li>\n</ul>\n<ul>\n<li>baz</li>\n</ul>");

		/* [Example 265](http://spec.commonmark.org/0.28/#example-265) */
		[TestMethod]
		public void Example265 () => TestParse (
			"1. foo\n2. bar\n3) baz",
			"<ol>\n<li>foo</li>\n<li>bar</li>\n</ol>\n<ol start=\"3\">\n<li>baz</li>\n</ol>");

		/* [Example 266](http://spec.commonmark.org/0.28/#example-266) */
		[TestMethod]
		public void Example266 () => TestParse (
			"Foo\n- bar\n- baz",
			"<p>Foo</p>\n<ul>\n<li>bar</li>\n<li>baz</li>\n</ul>");

		/* [Example 267](http://spec.commonmark.org/0.28/#example-267) */
		[TestMethod]
		public void Example267 () => TestParse (
			"The number of windows in my house is\n14.  The number of doors is 6.",
			"<p>The number of windows in my house is\n14.  The number of doors is 6.</p>");

		/* [Example 268](http://spec.commonmark.org/0.28/#example-268) */
		[TestMethod]
		public void Example268 () => TestParse (
			"The number of windows in my house is\n1.  The number of doors is 6.",
			"<p>The number of windows in my house is</p>\n<ol>\n<li>The number of doors is 6.</li>\n</ol>");

		/* [Example 269](http://spec.commonmark.org/0.28/#example-269) */
		[TestMethod]
		public void Example269 () => TestParse (
			"- foo\n\n- bar\n\n\n- baz",
			"<ul>\n<li>\n<p>foo</p>\n</li>\n<li>\n<p>bar</p>\n</li>\n<li>\n<p>baz</p>\n</li>\n</ul>");

		/* [Example 270](http://spec.commonmark.org/0.28/#example-270) */
		[TestMethod]
		public void Example270 () => TestParse (
			"- foo\n  - bar\n    - baz\n\n\n      bim",
			"<ul>\n<li>foo\n<ul>\n<li>bar\n<ul>\n<li>\n<p>baz</p>\n<p>bim</p>\n</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>");

		/* [Example 271](http://spec.commonmark.org/0.28/#example-271) */
		[TestMethod]
		public void Example271 () => TestParse (
			"- foo\n- bar\n\n<!-- -->\n\n- baz\n- bim",
			"<ul>\n<li>foo</li>\n<li>bar</li>\n</ul>\n<!-- -->\n<ul>\n<li>baz</li>\n<li>bim</li>\n</ul>");

		/* [Example 272](http://spec.commonmark.org/0.28/#example-272) */
		[TestMethod]
		public void Example272 () => TestParse (
			"-   foo\n\n    notcode\n\n-   foo\n\n<!-- -->\n\n    code",
			"<ul>\n<li>\n<p>foo</p>\n<p>notcode</p>\n</li>\n<li>\n<p>foo</p>\n</li>\n</ul>\n<!-- -->\n<pre><code>code\n</code></pre>");

		/* [Example 273](http://spec.commonmark.org/0.28/#example-273) */
		[TestMethod]
		public void Example273 () => TestParse (
			"- a\n - b\n  - c\n   - d\n    - e\n   - f\n  - g\n - h\n- i",
			"<ul>\n<li>a</li>\n<li>b</li>\n<li>c</li>\n<li>d</li>\n<li>e</li>\n<li>f</li>\n<li>g</li>\n<li>h</li>\n<li>i</li>\n</ul>");

		/* [Example 274](http://spec.commonmark.org/0.28/#example-274) */
		[TestMethod]
		public void Example274 () => TestParse (
			"1. a\n\n  2. b\n\n    3. c",
			"<ol>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>c</p>\n</li>\n</ol>");

		/* [Example 275](http://spec.commonmark.org/0.28/#example-275) */
		[TestMethod]
		public void Example275 () => TestParse (
			"- a\n- b\n\n- c",
			"<ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>c</p>\n</li>\n</ul>");

		/* [Example 276](http://spec.commonmark.org/0.28/#example-276) */
		[TestMethod]
		public void Example276 () => TestParse (
			"* a\n*\n\n* c",
			"<ul>\n<li>\n<p>a</p>\n</li>\n<li></li>\n<li>\n<p>c</p>\n</li>\n</ul>");

		/* [Example 277](http://spec.commonmark.org/0.28/#example-277) */
		[TestMethod]
		public void Example277 () => TestParse (
			"- a\n- b\n\n  c\n- d",
			"<ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n<p>c</p>\n</li>\n<li>\n<p>d</p>\n</li>\n</ul>");

		/* [Example 278](http://spec.commonmark.org/0.28/#example-278) */
		[TestMethod]
		public void Example278 () => TestParse (
			"- a\n- b\n\n  [ref]: /url\n- d",
			"<ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>d</p>\n</li>\n</ul>");

		/* [Example 279](http://spec.commonmark.org/0.28/#example-279) */
		[TestMethod]
		public void Example279 () => TestParse (
			"- a\n- ```\n  b\n\n\n  ```\n- c",
			"<ul>\n<li>a</li>\n<li>\n<pre><code>b\n\n\n</code></pre>\n</li>\n<li>c</li>\n</ul>");

		/* [Example 280](http://spec.commonmark.org/0.28/#example-280) */
		[TestMethod]
		public void Example280 () => TestParse (
			"- a\n  - b\n\n    c\n- d",
			"<ul>\n<li>a\n<ul>\n<li>\n<p>b</p>\n<p>c</p>\n</li>\n</ul>\n</li>\n<li>d</li>\n</ul>");

		/* [Example 281](http://spec.commonmark.org/0.28/#example-281) */
		[TestMethod]
		public void Example281 () => TestParse (
			"* a\n  > b\n  >\n* c",
			"<ul>\n<li>a\n<blockquote>\n<p>b</p>\n</blockquote>\n</li>\n<li>c</li>\n</ul>");

		/* [Example 282](http://spec.commonmark.org/0.28/#example-282) */
		[TestMethod]
		public void Example282 () => TestParse (
			"- a\n  > b\n  ```\n  c\n  ```\n- d",
			"<ul>\n<li>a\n<blockquote>\n<p>b</p>\n</blockquote>\n<pre><code>c\n</code></pre>\n</li>\n<li>d</li>\n</ul>");

		/* [Example 283](http://spec.commonmark.org/0.28/#example-283) */
		[TestMethod]
		public void Example283 () => TestParse (
			"- a",
			"<ul>\n<li>a</li>\n</ul>");

		/* [Example 284](http://spec.commonmark.org/0.28/#example-284) */
		[TestMethod]
		public void Example284 () => TestParse (
			"- a\n  - b",
			"<ul>\n<li>a\n<ul>\n<li>b</li>\n</ul>\n</li>\n</ul>");

		/* [Example 285](http://spec.commonmark.org/0.28/#example-285) */
		[TestMethod]
		public void Example285 () => TestParse (
			"1. ```\n   foo\n   ```\n\n   bar",
			"<ol>\n<li>\n<pre><code>foo\n</code></pre>\n<p>bar</p>\n</li>\n</ol>");

		/* [Example 286](http://spec.commonmark.org/0.28/#example-286) */
		[TestMethod]
		public void Example286 () => TestParse (
			"* foo\n  * bar\n\n  baz", 
			"<ul>\n<li>\n<p>foo</p>\n<ul>\n<li>bar</li>\n</ul>\n<p>baz</p>\n</li>\n</ul>");

		/* [Example 287](http://spec.commonmark.org/0.28/#example-287) */
		[TestMethod]
		public void Example287 () => TestParse (
			"- a\n  - b\n  - c\n\n- d\n  - e\n  - f", 
			"<ul>\n<li>\n<p>a</p>\n<ul>\n<li>b</li>\n<li>c</li>\n</ul>\n</li>\n<li>\n<p>d</p>\n<ul>\n<li>e</li>\n<li>f</li>\n</ul>\n</li>\n</ul>");
	}
}
