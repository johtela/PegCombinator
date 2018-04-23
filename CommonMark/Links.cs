namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Links : TestBase
	{
		/* [Example 459](http://spec.commonmark.org/0.28/#example-459) */
		[TestMethod]
		public void Example459 () => TestParse (
			"[link](/uri \"title\")", 
			"<p><a href=\"/uri\" title=\"title\">link</a></p>");

		/* [Example 460](http://spec.commonmark.org/0.28/#example-460) */
		[TestMethod]
		public void Example460 () => TestParse (
			"[link](/uri)", 
			"<p><a href=\"/uri\">link</a></p>");

		/* [Example 461](http://spec.commonmark.org/0.28/#example-461) */
		[TestMethod]
		public void Example461 () => TestParse (
			"[link]()", 
			"<p><a href=\"\">link</a></p>");

		/* [Example 462](http://spec.commonmark.org/0.28/#example-462) */
		[TestMethod]
		public void Example462 () => TestParse (
			"[link](<>)", 
			"<p><a href=\"\">link</a></p>");

		/* [Example 463](http://spec.commonmark.org/0.28/#example-463) */
		[TestMethod]
		public void Example463 () => TestParse (
			"[link](/my uri)", 
			"<p>[link](/my uri)</p>");

		/* [Example 464](http://spec.commonmark.org/0.28/#example-464) */
		[TestMethod]
		public void Example464 () => TestParse (
			"[link](</my uri>)", 
			"<p>[link](&lt;/my uri&gt;)</p>");

		/* [Example 465](http://spec.commonmark.org/0.28/#example-465) */
		[TestMethod]
		public void Example465 () => TestParse (
			"[link](foo\nbar)", 
			"<p>[link](foo\nbar)</p>");

		/* [Example 466](http://spec.commonmark.org/0.28/#example-466) */
		[TestMethod]
		public void Example466 () => TestParse (
			"[link](<foo\nbar>)", 
			"<p>[link](<foo\nbar>)</p>");

		/* [Example 467](http://spec.commonmark.org/0.28/#example-467) */
		[TestMethod]
		public void Example467 () => TestParse (
			"[link](\\(foo\\))", 
			"<p><a href=\"(foo)\">link</a></p>");

		/* [Example 468](http://spec.commonmark.org/0.28/#example-468) */
		[TestMethod]
		public void Example468 () => TestParse (
			"[link](foo(and(bar)))", 
			"<p><a href=\"foo(and(bar))\">link</a></p>");

		/* [Example 469](http://spec.commonmark.org/0.28/#example-469) */
		[TestMethod]
		public void Example469 () => TestParse (
			"[link](foo\\(and\\(bar\\))", 
			"<p><a href=\"foo(and(bar)\">link</a></p>");

		/* [Example 470](http://spec.commonmark.org/0.28/#example-470) */
		[TestMethod]
		public void Example470 () => TestParse (
			"[link](<foo(and(bar)>)", 
			"<p><a href=\"foo(and(bar)\">link</a></p>");

		/* [Example 471](http://spec.commonmark.org/0.28/#example-471) */
		[TestMethod]
		public void Example471 () => TestParse (
			"[link](foo\\)\\:)", 
			"<p><a href=\"foo):\">link</a></p>");

		/* [Example 472](http://spec.commonmark.org/0.28/#example-472) */
		[TestMethod]
		public void Example472 () => TestParse (
			"[link](#fragment)\n\n[link](http://example.com#fragment)\n\n[link](http://example.com?foo=3#frag)", 
			"<p><a href=\"#fragment\">link</a></p>\n<p><a href=\"http://example.com#fragment\">link</a></p>\n<p><a href=\"http://example.com?foo=3#frag\">link</a></p>");

		/* [Example 473](http://spec.commonmark.org/0.28/#example-473) */
		[TestMethod]
		public void Example473 () => TestParse (
			"[link](foo\\bar)", 
			"<p><a href=\"foo%5Cbar\">link</a></p>");

		/* [Example 474](http://spec.commonmark.org/0.28/#example-474) */
		[TestMethod]
		public void Example474 () => TestParse (
			"[link](foo%20b&auml;)", 
			"<p><a href=\"foo%20b%C3%A4\">link</a></p>");

		/* [Example 475](http://spec.commonmark.org/0.28/#example-475) */
		[TestMethod]
		public void Example475 () => TestParse (
			"[link](\"title\")", 
			"<p><a href=\"%22title%22\">link</a></p>");

		/* [Example 476](http://spec.commonmark.org/0.28/#example-476) */
		[TestMethod]
		public void Example476 () => TestParse (
			"[link](/url \"title\")\n[link](/url 'title')\n[link](/url (title))", 
			"<p><a href=\"/url\" title=\"title\">link</a>\n<a href=\"/url\" title=\"title\">link</a>\n<a href=\"/url\" title=\"title\">link</a></p>");

		/* [Example 477](http://spec.commonmark.org/0.28/#example-477) */
		[TestMethod]
		public void Example477 () => TestParse (
			"[link](/url \"title \\\"&quot;\")", 
			"<p><a href=\"/url\" title=\"title &quot;&quot;\">link</a></p>");

		/* [Example 478](http://spec.commonmark.org/0.28/#example-478) */
		[TestMethod]
		public void Example478 () => TestParse (
			"[link](/url\u00a0\"title\")", 
			"<p><a href=\"/url%C2%A0%22title%22\">link</a></p>");

		/* [Example 479](http://spec.commonmark.org/0.28/#example-479) */
		[TestMethod]
		public void Example479 () => TestParse (
			"[link](/url \"title \"and\" title\")", 
			"<p>[link](/url &quot;title &quot;and&quot; title&quot;)</p>");

		/* [Example 480](http://spec.commonmark.org/0.28/#example-480) */
		[TestMethod]
		public void Example480 () => TestParse (
			"[link](/url 'title \"and\" title')", 
			"<p><a href=\"/url\" title=\"title &quot;and&quot; title\">link</a></p>");

		/* [Example 481](http://spec.commonmark.org/0.28/#example-481) */
		[TestMethod]
		public void Example481 () => TestParse (
			"[link](   /uri\n  \"title\"  )", 
			"<p><a href=\"/uri\" title=\"title\">link</a></p>");

		/* [Example 482](http://spec.commonmark.org/0.28/#example-482) */
		[TestMethod]
		public void Example482 () => TestParse (
			"[link] (/uri)", 
			"<p>[link] (/uri)</p>");

		/* [Example 483](http://spec.commonmark.org/0.28/#example-483) */
		[TestMethod]
		public void Example483 () => TestParse (
			"[link [foo [bar]]](/uri)", 
			"<p><a href=\"/uri\">link [foo [bar]]</a></p>");

		/* [Example 484](http://spec.commonmark.org/0.28/#example-484) */
		[TestMethod]
		public void Example484 () => TestParse (
			"[link] bar](/uri)", 
			"<p>[link] bar](/uri)</p>");

		/* [Example 485](http://spec.commonmark.org/0.28/#example-485) */
		[TestMethod]
		public void Example485 () => TestParse (
			"[link [bar](/uri)", 
			"<p>[link <a href=\"/uri\">bar</a></p>");

		/* [Example 486](http://spec.commonmark.org/0.28/#example-486) */
		[TestMethod]
		public void Example486 () => TestParse (
			"[link \\[bar](/uri)", 
			"<p><a href=\"/uri\">link [bar</a></p>");

		/* [Example 487](http://spec.commonmark.org/0.28/#example-487) */
		[TestMethod]
		public void Example487 () => TestParse (
			"[link *foo **bar** `#`*](/uri)", 
			"<p><a href=\"/uri\">link <em>foo <strong>bar</strong> <code>#</code></em></a></p>");

		/* [Example 488](http://spec.commonmark.org/0.28/#example-488) */
		[TestMethod]
		public void Example488 () => TestParse (
			"[![moon](moon.jpg)](/uri)", 
			"<p><a href=\"/uri\"><img src=\"moon.jpg\" alt=\"moon\" /></a></p>");

		/* [Example 489](http://spec.commonmark.org/0.28/#example-489) */
		[TestMethod]
		public void Example489 () => TestParse (
			"[foo [bar](/uri)](/uri)", 
			"<p>[foo <a href=\"/uri\">bar</a>](/uri)</p>");

		/* [Example 490](http://spec.commonmark.org/0.28/#example-490) */
		[TestMethod]
		public void Example490 () => TestParse (
			"[foo *[bar [baz](/uri)](/uri)*](/uri)", 
			"<p>[foo <em>[bar <a href=\"/uri\">baz</a>](/uri)</em>](/uri)</p>");

	}
}
