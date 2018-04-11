namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Emphasis : TestBase
	{
		/* [Example 331](http://spec.commonmark.org/0.28/#example-331) */
		[TestMethod]
		public void Example331 () => TestParse (
			"*foo bar*", 
			"<p><em>foo bar</em></p>");

		/* [Example 332](http://spec.commonmark.org/0.28/#example-332) */
		[TestMethod]
		public void Example332 () => TestParse (
			"a * foo bar*", 
			"<p>a * foo bar*</p>");

		/* [Example 333](http://spec.commonmark.org/0.28/#example-333) */
		[TestMethod]
		public void Example333 () => TestParse (
			"a*\"foo\"*", 
			"<p>a*&quot;foo&quot;*</p>");

		/* [Example 334](http://spec.commonmark.org/0.28/#example-334) */
		[TestMethod]
		public void Example334 () => TestParse (
			"* a *",
			"<p>* a *</p>");

		/* [Example 335](http://spec.commonmark.org/0.28/#example-335) */
		[TestMethod]
		public void Example335 () => TestParse (
			"foo*bar*", 
			"<p>foo<em>bar</em></p>");

		/* [Example 336](http://spec.commonmark.org/0.28/#example-336) */
		[TestMethod]
		public void Example336 () => TestParse (
			"5*6*78", 
			"<p>5<em>6</em>78</p>");

		/* [Example 337](http://spec.commonmark.org/0.28/#example-337) */
		[TestMethod]
		public void Example337 () => TestParse (
			"_foo bar_", 
			"<p><em>foo bar</em></p>");

		/* [Example 338](http://spec.commonmark.org/0.28/#example-338) */
		[TestMethod]
		public void Example338 () => TestParse (
			"_ foo bar_", 
			"<p>_ foo bar_</p>");

		/* [Example 339](http://spec.commonmark.org/0.28/#example-339) */
		[TestMethod]
		public void Example339 () => TestParse (
			"a_\"foo\"_", 
			"<p>a_&quot;foo&quot;_</p>");

		/* [Example 340](http://spec.commonmark.org/0.28/#example-340) */
		[TestMethod]
		public void Example340 () => TestParse (
			"foo_bar_", 
			"<p>foo_bar_</p>");

		/* [Example 341](http://spec.commonmark.org/0.28/#example-341) */
		[TestMethod]
		public void Example341 () => TestParse (
			"5_6_78", 
			"<p>5_6_78</p>");

		/* [Example 342](http://spec.commonmark.org/0.28/#example-342) */
		[TestMethod]
		public void Example342 () => TestParse (
			"пристаням_стремятся_",
			"<p>пристаням_стремятся_</p>");

		/* [Example 343](http://spec.commonmark.org/0.28/#example-343) */
		[TestMethod]
		public void Example343 () => TestParse (
			"aa_\"bb\"_cc", 
			"<p>aa_&quot;bb&quot;_cc</p>");

		/* [Example 344](http://spec.commonmark.org/0.28/#example-344) */
		[TestMethod]
		public void Example344 () => TestParse (
			"foo-_(bar)_", 
			"<p>foo-<em>(bar)</em></p>");

	}
}
