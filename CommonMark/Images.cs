namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Images : TestBase
	{
		/* [Example 543](http://spec.commonmark.org/0.28/#example-543) */
		[TestMethod]
		public void Example543 () => TestParse (
			"![foo](/url \"title\")", 
			"<p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>");

		/* [Example 544](http://spec.commonmark.org/0.28/#example-544) */
		[TestMethod]
		public void Example544 () => TestParse (
			"![foo *bar*]\n\n[foo *bar*]: train.jpg \"train & tracks\"", 
			"<p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>");

		/* [Example 545](http://spec.commonmark.org/0.28/#example-545) */
		[TestMethod]
		public void Example545 () => TestParse (
			"![foo ![bar](/url)](/url2)", 
			"<p><img src=\"/url2\" alt=\"foo bar\" /></p>");

		/* [Example 546](http://spec.commonmark.org/0.28/#example-546) */
		[TestMethod]
		public void Example546 () => TestParse (
			"![foo [bar](/url)](/url2)", 
			"<p><img src=\"/url2\" alt=\"foo bar\" /></p>");

		/* [Example 547](http://spec.commonmark.org/0.28/#example-547) */
		[TestMethod]
		public void Example547 () => TestParse (
			"![foo *bar*][]\n\n[foo *bar*]: train.jpg \"train & tracks\"", 
			"<p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>");

		/* [Example 548](http://spec.commonmark.org/0.28/#example-548) */
		[TestMethod]
		public void Example548 () => TestParse (
			"![foo *bar*][foobar]\n\n[FOOBAR]: train.jpg \"train & tracks\"", 
			"<p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>");

		/* [Example 549](http://spec.commonmark.org/0.28/#example-549) */
		[TestMethod]
		public void Example549 () => TestParse (
			"![foo](train.jpg)", 
			"<p><img src=\"train.jpg\" alt=\"foo\" /></p>");

		/* [Example 550](http://spec.commonmark.org/0.28/#example-550) */
		[TestMethod]
		public void Example550 () => TestParse (
			"My ![foo bar](/path/to/train.jpg  \"title\"   )", 
			"<p>My <img src=\"/path/to/train.jpg\" alt=\"foo bar\" title=\"title\" /></p>");

		/* [Example 551](http://spec.commonmark.org/0.28/#example-551) */
		[TestMethod]
		public void Example551 () => TestParse (
			"![foo](<url>)", 
			"<p><img src=\"url\" alt=\"foo\" /></p>");

		/* [Example 552](http://spec.commonmark.org/0.28/#example-552) */
		[TestMethod]
		public void Example552 () => TestParse (
			"![](/url)", 
			"<p><img src=\"/url\" alt=\"\" /></p>");

		/* [Example 553](http://spec.commonmark.org/0.28/#example-553) */
		[TestMethod]
		public void Example553 () => TestParse (
			"![foo][bar]\n\n[bar]: /url", 
			"<p><img src=\"/url\" alt=\"foo\" /></p>");

		/* [Example 554](http://spec.commonmark.org/0.28/#example-554) */
		[TestMethod]
		public void Example554 () => TestParse (
			"![foo][bar]\n\n[BAR]: /url", 
			"<p><img src=\"/url\" alt=\"foo\" /></p>");

		/* [Example 555](http://spec.commonmark.org/0.28/#example-555) */
		[TestMethod]
		public void Example555 () => TestParse (
			"![foo][]\n\n[foo]: /url \"title\"", 
			"<p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>");

		/* [Example 556](http://spec.commonmark.org/0.28/#example-556) */
		[TestMethod]
		public void Example556 () => TestParse (
			"![*foo* bar][]\n\n[*foo* bar]: /url \"title\"", 
			"<p><img src=\"/url\" alt=\"foo bar\" title=\"title\" /></p>");

		/* [Example 557](http://spec.commonmark.org/0.28/#example-557) */
		[TestMethod]
		public void Example557 () => TestParse (
			"![Foo][]\n\n[foo]: /url \"title\"", 
			"<p><img src=\"/url\" alt=\"Foo\" title=\"title\" /></p>");

		/* [Example 558](http://spec.commonmark.org/0.28/#example-558) */
		[TestMethod]
		public void Example558 () => TestParse (
			"![foo] \n[]\n\n[foo]: /url \"title\"", 
			"<p><img src=\"/url\" alt=\"foo\" title=\"title\" />\n[]</p>");

		/* [Example 559](http://spec.commonmark.org/0.28/#example-559) */
		[TestMethod]
		public void Example559 () => TestParse (
			"![foo]\n\n[foo]: /url \"title\"", 
			"<p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>");

		/* [Example 560](http://spec.commonmark.org/0.28/#example-560) */
		[TestMethod]
		public void Example560 () => TestParse (
			"![*foo* bar]\n\n[*foo* bar]: /url \"title\"", 
			"<p><img src=\"/url\" alt=\"foo bar\" title=\"title\" /></p>");

		/* [Example 561](http://spec.commonmark.org/0.28/#example-561) */
		[TestMethod]
		public void Example561 () => TestParse (
			"![[foo]]\n\n[[foo]]: /url \"title\"", 
			"<p>![[foo]]</p>\n<p>[[foo]]: /url &quot;title&quot;</p>");

		/* [Example 562](http://spec.commonmark.org/0.28/#example-562) */
		[TestMethod]
		public void Example562 () => TestParse (
			"![Foo]\n\n[foo]: /url \"title\"", 
			"<p><img src=\"/url\" alt=\"Foo\" title=\"title\" /></p>");

		/* [Example 563](http://spec.commonmark.org/0.28/#example-563) */
		[TestMethod]
		public void Example563 () => TestParse (
			"!\\[foo]\n\n[foo]: /url \"title\"", 
			"<p>![foo]</p>");

		/* [Example 564](http://spec.commonmark.org/0.28/#example-564) */
		[TestMethod]
		public void Example564 () => TestParse (
			"\\![foo]\n\n[foo]: /url \"title\"", 
			"<p>!<a href=\"/url\" title=\"title\">foo</a></p>");
	}
}
