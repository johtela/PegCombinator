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

		/* [Example 491](http://spec.commonmark.org/0.28/#example-491) */
		[TestMethod]
		public void Example491 () => TestParse (
			"![[[foo](uri1)](uri2)](uri3)", 
			"<p><img src=\"uri3\" alt=\"[foo](uri2)\" /></p>");

		/* [Example 492](http://spec.commonmark.org/0.28/#example-492) */
		[TestMethod]
		public void Example492 () => TestParse (
			"*[foo*](/uri)",
			"<p>*<a href=\"/uri\">foo*</a></p>");

		/* [Example 493](http://spec.commonmark.org/0.28/#example-493) */
		[TestMethod, Ignore]
		public void Example493 () => TestParse (
			"[foo *bar](baz*)", 
			"<p><a href=\"baz*\">foo *bar</a></p>");

		/* [Example 494](http://spec.commonmark.org/0.28/#example-494) */
		[TestMethod, Ignore]
		public void Example494 () => TestParse (
			"*foo [bar* baz]", 
			"<p><em>foo [bar</em> baz]</p>");

		/* [Example 495](http://spec.commonmark.org/0.28/#example-495) */
		[TestMethod]
		public void Example495 () => TestParse (
			"[foo <bar attr=\"](baz)\">", 
			"<p>[foo <bar attr=\"](baz)\"></p>");

		/* [Example 496](http://spec.commonmark.org/0.28/#example-496) */
		[TestMethod]
		public void Example496 () => TestParse (
			"[foo`](/uri)`", 
			"<p>[foo<code>](/uri)</code></p>");

		/* [Example 497](http://spec.commonmark.org/0.28/#example-497) */
		[TestMethod]
		public void Example497 () => TestParse (
			"[foo<http://example.com/?search=](uri)>", 
			"<p>[foo<a href=\"http://example.com/?search=%5D(uri)\">http://example.com/?search=](uri)</a></p>");

		/* [Example 498](http://spec.commonmark.org/0.28/#example-498) */
		[TestMethod]
		public void Example498 () => TestParse (
			"[foo][bar]\n\n[bar]: /url \"title\"", 
			"<p><a href=\"/url\" title=\"title\">foo</a></p>");

		/* [Example 499](http://spec.commonmark.org/0.28/#example-499) */
		[TestMethod]
		public void Example499 () => TestParse (
			"[link [foo [bar]]][ref]\n\n[ref]: /uri", 
			"<p><a href=\"/uri\">link [foo [bar]]</a></p>");

		/* [Example 500](http://spec.commonmark.org/0.28/#example-500) */
		[TestMethod]
		public void Example500 () => TestParse (
			"[link \\[bar][ref]\n\n[ref]: /uri", 
			"<p><a href=\"/uri\">link [bar</a></p>");

		/* [Example 501](http://spec.commonmark.org/0.28/#example-501) */
		[TestMethod]
		public void Example501 () => TestParse (
			"[link *foo **bar** `#`*][ref]\n\n[ref]: /uri", 
			"<p><a href=\"/uri\">link <em>foo <strong>bar</strong> <code>#</code></em></a></p>");

		/* [Example 502](http://spec.commonmark.org/0.28/#example-502) */
		[TestMethod]
		public void Example502 () => TestParse (
			"[![moon](moon.jpg)][ref]\n\n[ref]: /uri", 
			"<p><a href=\"/uri\"><img src=\"moon.jpg\" alt=\"moon\" /></a></p>");

		/* [Example 503](http://spec.commonmark.org/0.28/#example-503) */
		[TestMethod]
		public void Example503 () => TestParse (
			"[foo [bar](/uri)][ref]\n\n[ref]: /uri", 
			"<p>[foo <a href=\"/uri\">bar</a>]<a href=\"/uri\">ref</a></p>");

		/* [Example 504](http://spec.commonmark.org/0.28/#example-504) */
		[TestMethod]
		public void Example504 () => TestParse (
			"[foo *bar [baz][ref]*][ref]\n\n[ref]: /uri", 
			"<p>[foo <em>bar <a href=\"/uri\">baz</a></em>]<a href=\"/uri\">ref</a></p>");

		/* [Example 505](http://spec.commonmark.org/0.28/#example-505) */
		[TestMethod]
		public void Example505 () => TestParse (
			"*[foo*][ref]\n\n[ref]: /uri", 
			"<p>*<a href=\"/uri\">foo*</a></p>");

		/* [Example 506](http://spec.commonmark.org/0.28/#example-506) */
		[TestMethod]
		public void Example506 () => TestParse (
			"[foo *bar][ref]\n\n[ref]: /uri", 
			"<p><a href=\"/uri\">foo *bar</a></p>");

		/* [Example 507](http://spec.commonmark.org/0.28/#example-507) */
		[TestMethod]
		public void Example507 () => TestParse (
			"[foo <bar attr=\"][ref]\">\n\n[ref]: /uri", 
			"<p>[foo <bar attr=\"][ref]\"></p>");

		/* [Example 508](http://spec.commonmark.org/0.28/#example-508) */
		[TestMethod]
		public void Example508 () => TestParse (
			"[foo`][ref]`\n\n[ref]: /uri", 
			"<p>[foo<code>][ref]</code></p>");

		/* [Example 509](http://spec.commonmark.org/0.28/#example-509) */
		[TestMethod]
		public void Example509 () => TestParse (
			"[foo<http://example.com/?search=][ref]>\n\n[ref]: /uri", 
			"<p>[foo<a href=\"http://example.com/?search=%5D%5Bref%5D\">http://example.com/?search=][ref]</a></p>");

		/* [Example 510](http://spec.commonmark.org/0.28/#example-510) */
		[TestMethod]
		public void Example510 () => TestParse (
			"[foo][BaR]\n\n[bar]: /url \"title\"", 
			"<p><a href=\"/url\" title=\"title\">foo</a></p>");

		/* [Example 511](http://spec.commonmark.org/0.28/#example-511) */
		[TestMethod]
		public void Example511 () => TestParse (
			"[Толпой][Толпой] is a Russian word.\n\n[ТОЛПОЙ]: /url", 
			"<p><a href=\"/url\">Толпой</a> is a Russian word.</p>");

		/* [Example 512](http://spec.commonmark.org/0.28/#example-512) */
		[TestMethod]
		public void Example512 () => TestParse (
			"[Foo\n  bar]: /url\n\n[Baz][Foo bar]", 
			"<p><a href=\"/url\">Baz</a></p>");

		/* [Example 513](http://spec.commonmark.org/0.28/#example-513) */
		[TestMethod]
		public void Example513 () => TestParse (
			"[foo] [bar]\n\n[bar]: /url \"title\"", 
			"<p>[foo] <a href=\"/url\" title=\"title\">bar</a></p>");

		/* [Example 514](http://spec.commonmark.org/0.28/#example-514) */
		[TestMethod]
		public void Example514 () => TestParse (
			"[foo]\n[bar]\n\n[bar]: /url \"title\"", 
			"<p>[foo]\n<a href=\"/url\" title=\"title\">bar</a></p>");

		/* [Example 515](http://spec.commonmark.org/0.28/#example-515) */
		[TestMethod]
		public void Example515 () => TestParse (
			"[foo]: /url1\n\n[foo]: /url2\n\n[bar][foo]", 
			"<p><a href=\"/url1\">bar</a></p>");

		/* [Example 516](http://spec.commonmark.org/0.28/#example-516) */
		[TestMethod]
		public void Example516 () => TestParse (
			"[bar][foo\\!]\n\n[foo!]: /url", 
			"<p>[bar][foo!]</p>");

		/* [Example 517](http://spec.commonmark.org/0.28/#example-517) */
		[TestMethod]
		public void Example517 () => TestParse (
			"[foo][ref[]\n\n[ref[]: /uri", 
			"<p>[foo][ref[]</p>\n<p>[ref[]: /uri</p>");

		/* [Example 518](http://spec.commonmark.org/0.28/#example-518) */
		[TestMethod]
		public void Example518 () => TestParse (
			"[foo][ref[bar]]\n\n[ref[bar]]: /uri", 
			"<p>[foo][ref[bar]]</p>\n<p>[ref[bar]]: /uri</p>");

		/* [Example 519](http://spec.commonmark.org/0.28/#example-519) */
		[TestMethod]
		public void Example519 () => TestParse (
			"[[[foo]]]\n\n[[[foo]]]: /url", 
			"<p>[[[foo]]]</p>\n<p>[[[foo]]]: /url</p>");

		/* [Example 520](http://spec.commonmark.org/0.28/#example-520) */
		[TestMethod]
		public void Example520 () => TestParse (
			"[foo][ref\\[]\n\n[ref\\[]: /uri", 
			"<p><a href=\"/uri\">foo</a></p>");

		/* [Example 521](http://spec.commonmark.org/0.28/#example-521) */
		[TestMethod]
		public void Example521 () => TestParse (
			"[bar\\\\]: /uri\n\n[bar\\\\]", 
			"<p><a href=\"/uri\">bar\\</a></p>");

	}
}
