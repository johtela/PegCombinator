namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class CodeSpans : TestBase
	{
		/* [Example 314](http://spec.commonmark.org/0.28/#example-314) */
		[TestMethod]
		public void Example314 () => TestParse (
			"`foo`", 
			"<p><code>foo</code></p>");

		/* [Example 315](http://spec.commonmark.org/0.28/#example-315) */
		[TestMethod]
		public void Example315 () => TestParse (
			"`` foo ` bar  ``", 
			"<p><code>foo ` bar</code></p>");

		/* [Example 316](http://spec.commonmark.org/0.28/#example-316) */
		[TestMethod]
		public void Example316 () => TestParse (
			"` `` `", 
			"<p><code>``</code></p>");

		/* [Example 317](http://spec.commonmark.org/0.28/#example-317) */
		[TestMethod]
		public void Example317 () => TestParse (
			"``\nfoo\n``", 
			"<p><code>foo</code></p>");

		/* [Example 318](http://spec.commonmark.org/0.28/#example-318) */
		[TestMethod]
		public void Example318 () => TestParse (
			"`foo   bar\n  baz`", 
			"<p><code>foo bar baz</code></p>");

		/* [Example 319](http://spec.commonmark.org/0.28/#example-319) */
		[TestMethod]
		public void Example319 () => TestParse (
			"`a\u00A0\u00A0b`", "<p><code>a\u00A0\u00A0b</code></p>");

		/* [Example 320](http://spec.commonmark.org/0.28/#example-320) */
		[TestMethod]
		public void Example320 () => TestParse (
			"`foo `` bar`", 
			"<p><code>foo `` bar</code></p>");

		/* [Example 321](http://spec.commonmark.org/0.28/#example-321) */
		[TestMethod]
		public void Example321 () => TestParse (
			"`foo\\`bar`", 
			"<p><code>foo\\</code>bar`</p>");

		/* [Example 322](http://spec.commonmark.org/0.28/#example-322) */
		[TestMethod]
		public void Example322 () => TestParse (
			"*foo`*`", 
			"<p>*foo<code>*</code></p>");

		/* [Example 323](http://spec.commonmark.org/0.28/#example-323) */
		[TestMethod]
		public void Example323 () => TestParse (
			"[not a `link](/foo`)", 
			"<p>[not a <code>link](/foo</code>)</p>");

		/* [Example 324](http://spec.commonmark.org/0.28/#example-324) */
		[TestMethod]
		public void Example324 () => TestParse (
			"`<a href=\"`\">`", 
			"<p><code>&lt;a href=&quot;</code>&quot;&gt;`</p>");

		/* [Example 325](http://spec.commonmark.org/0.28/#example-325) */
		[TestMethod]
		public void Example325 () => TestParse (
			"<a href=\"`\">`", 
			"<p><a href=\"`\">`</p>");

		/* [Example 326](http://spec.commonmark.org/0.28/#example-326) */
		[TestMethod]
		public void Example326 () => TestParse (
			"`<http://foo.bar.`baz>`", 
			"<p><code>&lt;http://foo.bar.</code>baz&gt;`</p>");

		/* [Example 327](http://spec.commonmark.org/0.28/#example-327) */
		[TestMethod]
		public void Example327 () => TestParse (
			"<http://foo.bar.`baz>`", 
			"<p><a href=\"http://foo.bar.%60baz\">http://foo.bar.`baz</a>`</p>");

		/* [Example 328](http://spec.commonmark.org/0.28/#example-328) */
		[TestMethod]
		public void Example328 () => TestParse (
			"```foo``", 
			"<p>```foo``</p>");

		/* [Example 329](http://spec.commonmark.org/0.28/#example-329) */
		[TestMethod]
		public void Example329 () => TestParse (
			"`foo", 
			"<p>`foo</p>");

		/* [Example 330](http://spec.commonmark.org/0.28/#example-330) */
		[TestMethod]
		public void Example330 () => TestParse (
			"`foo``bar``", 
			"<p>`foo<code>bar</code></p>");
	}
}
