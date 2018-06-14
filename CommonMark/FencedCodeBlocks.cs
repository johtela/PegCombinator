namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class FencedCodeBlocks : TestBase
	{
		/* [Example 88](http://spec.commonmark.org/0.28/#example-88) */
		[TestMethod]
		public void Example88 () => TestParse (
			"```\n<\n >\n```", 
			"<pre><code>&lt;\n &gt;\n</code></pre>");

		/* [Example 89](http://spec.commonmark.org/0.28/#example-89) */
		[TestMethod]
		public void Example89 () => TestParse (
			"~~~\n<\n >\n~~~",
			"<pre><code>&lt;\n &gt;\n</code></pre>");

		/* [Example 90](http://spec.commonmark.org/0.28/#example-90) */
		[TestMethod]
		public void Example90 () => TestParse (
			"``\nfoo\n``", 
			"<p><code>foo</code></p>");

		/* [Example 91](http://spec.commonmark.org/0.28/#example-91) */
		[TestMethod]
		public void Example91 () => TestParse (
			"```\naaa\n~~~\n```", 
			"<pre><code>aaa\n~~~\n</code></pre>");

		/* [Example 92](http://spec.commonmark.org/0.28/#example-92) */
		[TestMethod]
		public void Example92 () => TestParse (
			"~~~\naaa\n```\n~~~", 
			"<pre><code>aaa\n```\n</code></pre>");

		/* [Example 93](http://spec.commonmark.org/0.28/#example-93) */
		[TestMethod]
		public void Example93 () => TestParse (
			"````\naaa\n```\n``````", 
			"<pre><code>aaa\n```\n</code></pre>");

		/* [Example 94](http://spec.commonmark.org/0.28/#example-94) */
		[TestMethod]
		public void Example94 () => TestParse (
			"~~~~\naaa\n~~~\n~~~~", 
			"<pre><code>aaa\n~~~\n</code></pre>");

		/* [Example 95](http://spec.commonmark.org/0.28/#example-95) */
		[TestMethod]
		public void Example95 () => TestParse (
			"```", 
			"<pre><code></code></pre>");

		/* [Example 96](http://spec.commonmark.org/0.28/#example-96) */
		[TestMethod]
		public void Example96 () => TestParse (
			"`````\n\n```\naaa", 
			"<pre><code>\n```\naaa\n</code></pre>");

		/* [Example 97](http://spec.commonmark.org/0.28/#example-97) */
		[TestMethod]
		public void Example97 () => TestParse (
			"> ```\n> aaa\n\nbbb", 
			"<blockquote>\n<pre><code>aaa\n</code></pre>\n</blockquote>\n<p>bbb</p>");

		/* [Example 98](http://spec.commonmark.org/0.28/#example-98) */
		[TestMethod]
		public void Example98 () => TestParse (
			"```\n\n  \n```", 
			"<pre><code>\n  \n</code></pre>");

		/* [Example 99](http://spec.commonmark.org/0.28/#example-99) */
		[TestMethod]
		public void Example99 () => TestParse (
			"```\n```", 
			"<pre><code></code></pre>");

		/* [Example 100](http://spec.commonmark.org/0.28/#example-100) */
		[TestMethod]
		public void Example100 () => TestParse (
			" ```\n aaa\naaa\n```", 
			"<pre><code>aaa\naaa\n</code></pre>");

		/* [Example 101](http://spec.commonmark.org/0.28/#example-101) */
		[TestMethod]
		public void Example101 () => TestParse (
			"  ```\naaa\n  aaa\naaa\n  ```", 
			"<pre><code>aaa\naaa\naaa\n</code></pre>");

		/* [Example 102](http://spec.commonmark.org/0.28/#example-102) */
		[TestMethod]
		public void Example102 () => TestParse (
			"   ```\n   aaa\n    aaa\n  aaa\n   ```", 
			"<pre><code>aaa\n aaa\naaa\n</code></pre>");

		/* [Example 103](http://spec.commonmark.org/0.28/#example-103) */
		[TestMethod]
		public void Example103 () => TestParse (
			"    ```\n    aaa\n    ```", 
			"<pre><code>```\naaa\n```\n</code></pre>");

		/* [Example 104](http://spec.commonmark.org/0.28/#example-104) */
		[TestMethod]
		public void Example104 () => TestParse (
			"```\naaa\n  ```", 
			"<pre><code>aaa\n</code></pre>");

		/* [Example 105](http://spec.commonmark.org/0.28/#example-105) */
		[TestMethod]
		public void Example105 () => TestParse (
			"   ```\naaa\n  ```", 
			"<pre><code>aaa\n</code></pre>");

		/* [Example 106](http://spec.commonmark.org/0.28/#example-106) */
		[TestMethod]
		public void Example106 () => TestParse (
			"```\naaa\n    ```", 
			"<pre><code>aaa\n    ```\n</code></pre>");

		/* [Example 107](http://spec.commonmark.org/0.28/#example-107) */
		[TestMethod]
		public void Example107 () => TestParse (
			"``` ```\naaa", 
			"<p><code></code>\naaa</p>");

		/* [Example 108](http://spec.commonmark.org/0.28/#example-108) */
		[TestMethod]
		public void Example108 () => TestParse (
			"~~~~~~\naaa\n~~~ ~~", 
			"<pre><code>aaa\n~~~ ~~\n</code></pre>");

		/* [Example 109](http://spec.commonmark.org/0.28/#example-109) */
		[TestMethod]
		public void Example109 () => TestParse (
			"foo\n```\nbar\n```\nbaz", 
			"<p>foo</p>\n<pre><code>bar\n</code></pre>\n<p>baz</p>");

		/* [Example 110](http://spec.commonmark.org/0.28/#example-110) */
		[TestMethod]
		public void Example110 () => TestParse (
			"foo\n---\n~~~\nbar\n~~~\n# baz", 
			"<h2>foo</h2>\n<pre><code>bar\n</code></pre>\n<h1>baz</h1>");

		/* [Example 111](http://spec.commonmark.org/0.28/#example-111) */
		[TestMethod]
		public void Example111 () => TestParse (
			"```ruby\ndef foo(x)\n  return 3\nend\n```", 
			"<pre><code class=\"language-ruby\">def foo(x)\n  return 3\nend\n</code></pre>");

		/* [Example 112](http://spec.commonmark.org/0.28/#example-112) */
		[TestMethod]
		public void Example112 () => TestParse (
			"~~~~    ruby startline=3 $%@#$\ndef foo(x)\n  return 3\nend\n~~~~~~~", 
			"<pre><code class=\"language-ruby\">def foo(x)\n  return 3\nend\n</code></pre>");

		/* [Example 113](http://spec.commonmark.org/0.28/#example-113) */
		[TestMethod]
		public void Example113 () => TestParse (
			"````;\n````", 
			"<pre><code class=\"language-;\"></code></pre>");

		/* [Example 114](http://spec.commonmark.org/0.28/#example-114) */
		[TestMethod]
		public void Example114 () => TestParse (
			"``` aa ```\nfoo", 
			"<p><code>aa</code>\nfoo</p>");

		/* [Example 115](http://spec.commonmark.org/0.28/#example-115) */
		[TestMethod]
		public void Example115 () => TestParse (
			"```\n``` aaa\n```", 
			"<pre><code>``` aaa\n</code></pre>");
	}
}
