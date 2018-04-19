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

		/* [Example 345](http://spec.commonmark.org/0.28/#example-345) */
		[TestMethod]
		public void Example345 () => TestParse (
			"_foo*", 
			"<p>_foo*</p>");

		/* [Example 346](http://spec.commonmark.org/0.28/#example-346) */
		[TestMethod]
		public void Example346 () => TestParse (
			"*foo bar *", 
			"<p>*foo bar *</p>");

		/* [Example 347](http://spec.commonmark.org/0.28/#example-347) */
		[TestMethod]
		public void Example347 () => TestParse (
			"*foo bar\n*", 
			"<p>*foo bar\n*</p>");

		/* [Example 348](http://spec.commonmark.org/0.28/#example-348) */
		[TestMethod]
		public void Example348 () => TestParse (
			"*(*foo)", 
			"<p>*(*foo)</p>");

		/* [Example 349](http://spec.commonmark.org/0.28/#example-349) */
		[TestMethod]
		public void Example349 () => TestParse (
			"*(*foo*)*", 
			"<p><em>(<em>foo</em>)</em></p>");

		/* [Example 350](http://spec.commonmark.org/0.28/#example-350) */
		[TestMethod]
		public void Example350 () => TestParse (
			"*foo*bar", 
			"<p><em>foo</em>bar</p>");

		/* [Example 351](http://spec.commonmark.org/0.28/#example-351) */
		[TestMethod]
		public void Example351 () => TestParse (
			"_foo bar _", 
			"<p>_foo bar _</p>");

		/* [Example 352](http://spec.commonmark.org/0.28/#example-352) */
		[TestMethod]
		public void Example352 () => TestParse (
			"_(_foo)", 
			"<p>_(_foo)</p>");

		/* [Example 353](http://spec.commonmark.org/0.28/#example-353) */
		[TestMethod]
		public void Example353 () => TestParse (
			"_(_foo_)_", 
			"<p><em>(<em>foo</em>)</em></p>");

		/* [Example 354](http://spec.commonmark.org/0.28/#example-354) */
		[TestMethod]
		public void Example354 () => TestParse (
			"_foo_bar", 
			"<p>_foo_bar</p>");

		/* [Example 355](http://spec.commonmark.org/0.28/#example-355) */
		[TestMethod]
		public void Example355 () => TestParse (
			"_пристаням_стремятся", 
			"<p>_пристаням_стремятся</p>");

		/* [Example 356](http://spec.commonmark.org/0.28/#example-356) */
		[TestMethod]
		public void Example356 () => TestParse (
			"_foo_bar_baz_", 
			"<p><em>foo_bar_baz</em></p>");

		/* [Example 357](http://spec.commonmark.org/0.28/#example-357) */
		[TestMethod]
		public void Example357 () => TestParse (
			"_(bar)_.", 
			"<p><em>(bar)</em>.</p>");

		/* [Example 358](http://spec.commonmark.org/0.28/#example-358) */
		[TestMethod]
		public void Example358 () => TestParse (
			"**foo bar**", 
			"<p><strong>foo bar</strong></p>");

		/* [Example 359](http://spec.commonmark.org/0.28/#example-359) */
		[TestMethod]
		public void Example359 () => TestParse (
			"** foo bar**", 
			"<p>** foo bar**</p>");

		/* [Example 360](http://spec.commonmark.org/0.28/#example-360) */
		[TestMethod]
		public void Example360 () => TestParse (
			"a**\"foo\"**", 
			"<p>a**&quot;foo&quot;**</p>");

		/* [Example 361](http://spec.commonmark.org/0.28/#example-361) */
		[TestMethod]
		public void Example361 () => TestParse (
			"foo**bar**", 
			"<p>foo<strong>bar</strong></p>");

		/* [Example 362](http://spec.commonmark.org/0.28/#example-362) */
		[TestMethod]
		public void Example362 () => TestParse (
			"__foo bar__", 
			"<p><strong>foo bar</strong></p>");

		/* [Example 363](http://spec.commonmark.org/0.28/#example-363) */
		[TestMethod]
		public void Example363 () => TestParse (
			"__ foo bar__", 
			"<p>__ foo bar__</p>");

		/* [Example 364](http://spec.commonmark.org/0.28/#example-364) */
		[TestMethod]
		public void Example364 () => TestParse (
			"__\nfoo bar__", 
			"<p>__\nfoo bar__</p>");

		/* [Example 365](http://spec.commonmark.org/0.28/#example-365) */
		[TestMethod]
		public void Example365 () => TestParse (
			"a__\"foo\"__", 
			"<p>a__&quot;foo&quot;__</p>");

		/* [Example 366](http://spec.commonmark.org/0.28/#example-366) */
		[TestMethod]
		public void Example366 () => TestParse (
			"foo__bar__", 
			"<p>foo__bar__</p>");

		/* [Example 367](http://spec.commonmark.org/0.28/#example-367) */
		[TestMethod]
		public void Example367 () => TestParse (
			"5__6__78", 
			"<p>5__6__78</p>");

		/* [Example 368](http://spec.commonmark.org/0.28/#example-368) */
		[TestMethod]
		public void Example368 () => TestParse (
			"пристаням__стремятся__", 
			"<p>пристаням__стремятся__</p>");

		/* [Example 369](http://spec.commonmark.org/0.28/#example-369) */
		[TestMethod]
		public void Example369 () => TestParse (
			"__foo, __bar__, baz__", 
			"<p><strong>foo, <strong>bar</strong>, baz</strong></p>");

		/* [Example 370](http://spec.commonmark.org/0.28/#example-370) */
		[TestMethod]
		public void Example370 () => TestParse (
			"foo-__(bar)__", 
			"<p>foo-<strong>(bar)</strong></p>");

		/* [Example 371](http://spec.commonmark.org/0.28/#example-371) */
		[TestMethod]
		public void Example371 () => TestParse (
			"**foo bar **", 
			"<p>**foo bar **</p>");

		/* [Example 372](http://spec.commonmark.org/0.28/#example-372) */
		[TestMethod]
		public void Example372 () => TestParse (
			"**(**foo)", 
			"<p>**(**foo)</p>");

		/* [Example 373](http://spec.commonmark.org/0.28/#example-373) */
		[TestMethod]
		public void Example373 () => TestParse (
			"*(**foo**)*", 
			"<p><em>(<strong>foo</strong>)</em></p>");

		/* [Example 374](http://spec.commonmark.org/0.28/#example-374) */
		[TestMethod]
		public void Example374 () => TestParse (
			"**Gomphocarpus (*Gomphocarpus physocarpus*, syn.\n*Asclepias physocarpa*)**", 
			"<p><strong>Gomphocarpus (<em>Gomphocarpus physocarpus</em>, syn.\n<em>Asclepias physocarpa</em>)</strong></p>");

		/* [Example 375](http://spec.commonmark.org/0.28/#example-375) */
		[TestMethod]
		public void Example375 () => TestParse (
			"**foo \"*bar*\" foo**", 
			"<p><strong>foo &quot;<em>bar</em>&quot; foo</strong></p>");

		/* [Example 376](http://spec.commonmark.org/0.28/#example-376) */
		[TestMethod]
		public void Example376 () => TestParse (
			"**foo**bar", 
			"<p><strong>foo</strong>bar</p>");

		/* [Example 377](http://spec.commonmark.org/0.28/#example-377) */
		[TestMethod]
		public void Example377 () => TestParse (
			"__foo bar __", 
			"<p>__foo bar __</p>");

		/* [Example 378](http://spec.commonmark.org/0.28/#example-378) */
		[TestMethod]
		public void Example378 () => TestParse (
			"__(__foo)", 
			"<p>__(__foo)</p>");

		/* [Example 379](http://spec.commonmark.org/0.28/#example-379) */
		[TestMethod]
		public void Example379 () => TestParse (
			"_(__foo__)_", 
			"<p><em>(<strong>foo</strong>)</em></p>");

		/* [Example 380](http://spec.commonmark.org/0.28/#example-380) */
		[TestMethod]
		public void Example380 () => TestParse (
			"__foo__bar", 
			"<p>__foo__bar</p>");

		/* [Example 381](http://spec.commonmark.org/0.28/#example-381) */
		[TestMethod]
		public void Example381 () => TestParse (
			"__пристаням__стремятся", 
			"<p>__пристаням__стремятся</p>");

		/* [Example 382](http://spec.commonmark.org/0.28/#example-382) */
		[TestMethod]
		public void Example382 () => TestParse (
			"__foo__bar__baz__", 
			"<p><strong>foo__bar__baz</strong></p>");

		/* [Example 383](http://spec.commonmark.org/0.28/#example-383) */
		[TestMethod]
		public void Example383 () => TestParse (
			"__(bar)__.", 
			"<p><strong>(bar)</strong>.</p>");

		/* [Example 384](http://spec.commonmark.org/0.28/#example-384) */
		[TestMethod]
		public void Example384 () => TestParse (
			"*foo [bar](/url)*", 
			"<p><em>foo <a href=\"/url\">bar</a></em></p>");

		/* [Example 385](http://spec.commonmark.org/0.28/#example-385) */
		[TestMethod]
		public void Example385 () => TestParse (
			"*foo\nbar*", 
			"<p><em>foo\nbar</em></p>");

		/* [Example 386](http://spec.commonmark.org/0.28/#example-386) */
		[TestMethod]
		public void Example386 () => TestParse (
			"_foo __bar__ baz_", 
			"<p><em>foo <strong>bar</strong> baz</em></p>");

		/* [Example 387](http://spec.commonmark.org/0.28/#example-387) */
		[TestMethod]
		public void Example387 () => TestParse (
			"_foo _bar_ baz_", 
			"<p><em>foo <em>bar</em> baz</em></p>");

		/* [Example 388](http://spec.commonmark.org/0.28/#example-388) */
		[TestMethod]
		public void Example388 () => TestParse (
			"__foo_ bar_", 
			"<p><em><em>foo</em> bar</em></p>");

		/* [Example 389](http://spec.commonmark.org/0.28/#example-389) */
		[TestMethod]
		public void Example389 () => TestParse (
			"*foo *bar**", 
			"<p><em>foo <em>bar</em></em></p>");

		/* [Example 390](http://spec.commonmark.org/0.28/#example-390) */
		[TestMethod]
		public void Example390 () => TestParse (
			"*foo **bar** baz*", 
			"<p><em>foo <strong>bar</strong> baz</em></p>");

		/* [Example 391](http://spec.commonmark.org/0.28/#example-391) */
		[TestMethod]
		public void Example391 () => TestParse (
			"*foo**bar**baz*", 
			"<p><em>foo<strong>bar</strong>baz</em></p>");

		/* [Example 392](http://spec.commonmark.org/0.28/#example-392) */
		[TestMethod]
		public void Example392 () => TestParse (
			"***foo** bar*", 
			"<p><em><strong>foo</strong> bar</em></p>");

		/* [Example 393](http://spec.commonmark.org/0.28/#example-393) */
		[TestMethod]
		public void Example393 () => TestParse (
			"*foo **bar***", 
			"<p><em>foo <strong>bar</strong></em></p>");

		/* [Example 394](http://spec.commonmark.org/0.28/#example-394) */
		[TestMethod]
		public void Example394 () => TestParse (
			"*foo**bar***", "<p><em>foo<strong>bar</strong></em></p>");

		/* [Example 395](http://spec.commonmark.org/0.28/#example-395) */
		[TestMethod]
		public void Example395 () => TestParse (
			"*foo **bar *baz* bim** bop*", 
			"<p><em>foo <strong>bar <em>baz</em> bim</strong> bop</em></p>");

		/* [Example 396](http://spec.commonmark.org/0.28/#example-396) */
		[TestMethod]
		public void Example396 () => TestParse (
			"*foo [*bar*](/url)*", 
			"<p><em>foo <a href=\"/url\"><em>bar</em></a></em></p>");

		/* [Example 397](http://spec.commonmark.org/0.28/#example-397) */
		[TestMethod]
		public void Example397 () => TestParse (
			"** is not an empty emphasis", 
			"<p>** is not an empty emphasis</p>");

		/* [Example 398](http://spec.commonmark.org/0.28/#example-398) */
		[TestMethod]
		public void Example398 () => TestParse (
			"**** is not an empty strong emphasis", 
			"<p>**** is not an empty strong emphasis</p>");

		/* [Example 399](http://spec.commonmark.org/0.28/#example-399) */
		[TestMethod]
		public void Example399 () => TestParse (
			"**foo [bar](/url)**", 
			"<p><strong>foo <a href=\"/url\">bar</a></strong></p>");

		/* [Example 400](http://spec.commonmark.org/0.28/#example-400) */
		[TestMethod]
		public void Example400 () => TestParse (
			"**foo\nbar**", 
			"<p><strong>foo\nbar</strong></p>");

		/* [Example 401](http://spec.commonmark.org/0.28/#example-401) */
		[TestMethod]
		public void Example401 () => TestParse (
			"__foo _bar_ baz__", 
			"<p><strong>foo <em>bar</em> baz</strong></p>");

		/* [Example 402](http://spec.commonmark.org/0.28/#example-402) */
		[TestMethod]
		public void Example402 () => TestParse (
			"__foo __bar__ baz__", 
			"<p><strong>foo <strong>bar</strong> baz</strong></p>");

		/* [Example 403](http://spec.commonmark.org/0.28/#example-403) */
		[TestMethod]
		public void Example403 () => TestParse (
			"____foo__ bar__", 
			"<p><strong><strong>foo</strong> bar</strong></p>");

		/* [Example 404](http://spec.commonmark.org/0.28/#example-404) */
		[TestMethod]
		public void Example404 () => TestParse (
			"**foo **bar****", 
			"<p><strong>foo <strong>bar</strong></strong></p>");

		/* [Example 405](http://spec.commonmark.org/0.28/#example-405) */
		[TestMethod]
		public void Example405 () => TestParse (
			"**foo *bar* baz**", 
			"<p><strong>foo <em>bar</em> baz</strong></p>");

		/* [Example 406](http://spec.commonmark.org/0.28/#example-406) */
		[TestMethod]
		public void Example406 () => TestParse (
			"**foo*bar*baz**", 
			"<p><strong>foo<em>bar</em>baz</strong></p>");

		/* [Example 407](http://spec.commonmark.org/0.28/#example-407) */
		[TestMethod]
		public void Example407 () => TestParse (
			"***foo* bar**", 
			"<p><strong><em>foo</em> bar</strong></p>");

		/* [Example 408](http://spec.commonmark.org/0.28/#example-408) */
		[TestMethod]
		public void Example408 () => TestParse (
			"**foo *bar***", 
			"<p><strong>foo <em>bar</em></strong></p>");

		/* [Example 409](http://spec.commonmark.org/0.28/#example-409) */
		[TestMethod]
		public void Example409 () => TestParse (
			"**foo *bar **baz**\nbim* bop**", 
			"<p><strong>foo <em>bar <strong>baz</strong>\nbim</em> bop</strong></p>");

		/* [Example 410](http://spec.commonmark.org/0.28/#example-410) */
		[TestMethod]
		public void Example410 () => TestParse (
			"**foo [*bar*](/url)**", 
			"<p><strong>foo <a href=\"/url\"><em>bar</em></a></strong></p>");

		/* [Example 411](http://spec.commonmark.org/0.28/#example-411) */
		[TestMethod]
		public void Example411 () => TestParse (
			"__ is not an empty emphasis", 
			"<p>__ is not an empty emphasis</p>");

		/* [Example 412](http://spec.commonmark.org/0.28/#example-412) */
		[TestMethod]
		public void Example412 () => TestParse (
			"____ is not an empty strong emphasis", 
			"<p>____ is not an empty strong emphasis</p>");

		/* [Example 413](http://spec.commonmark.org/0.28/#example-413) */
		[TestMethod]
		public void Example413 () => TestParse (
			"foo ***", 
			"<p>foo ***</p>");

		/* [Example 414](http://spec.commonmark.org/0.28/#example-414) */
		[TestMethod]
		public void Example414 () => TestParse (
			"foo *\\**", 
			"<p>foo <em>*</em></p>");

		/* [Example 415](http://spec.commonmark.org/0.28/#example-415) */
		[TestMethod]
		public void Example415 () => TestParse (
			"foo *_*", 
			"<p>foo <em>_</em></p>");

		/* [Example 416](http://spec.commonmark.org/0.28/#example-416) */
		[TestMethod]
		public void Example416 () => TestParse (
			"foo *****", 
			"<p>foo *****</p>");

		/* [Example 417](http://spec.commonmark.org/0.28/#example-417) */
		[TestMethod]
		public void Example417 () => TestParse (
			"foo **\\***", 
			"<p>foo <strong>*</strong></p>");

		/* [Example 418](http://spec.commonmark.org/0.28/#example-418) */
		[TestMethod]
		public void Example418 () => TestParse (
			"foo **_**", 
			"<p>foo <strong>_</strong></p>");

		/* [Example 419](http://spec.commonmark.org/0.28/#example-419) */
		[TestMethod]
		public void Example419 () => TestParse (
			"**foo*", 
			"<p>*<em>foo</em></p>");

		/* [Example 420](http://spec.commonmark.org/0.28/#example-420) */
		[TestMethod]
		public void Example420 () => TestParse (
			"*foo**", 
			"<p><em>foo</em>*</p>");

		/* [Example 421](http://spec.commonmark.org/0.28/#example-421) */
		[TestMethod]
		public void Example421 () => TestParse (
			"***foo**", 
			"<p>*<strong>foo</strong></p>");

		/* [Example 422](http://spec.commonmark.org/0.28/#example-422) */
		[TestMethod]
		public void Example422 () => TestParse (
			"****foo*", 
			"<p>***<em>foo</em></p>");

		/* [Example 423](http://spec.commonmark.org/0.28/#example-423) */
		[TestMethod]
		public void Example423 () => TestParse (
			"**foo***", 
			"<p><strong>foo</strong>*</p>");

		/* [Example 424](http://spec.commonmark.org/0.28/#example-424) */
		[TestMethod]
		public void Example424 () => TestParse (
			"*foo****", 
			"<p><em>foo</em>***</p>");

		/* [Example 425](http://spec.commonmark.org/0.28/#example-425) */
		[TestMethod]
		public void Example425 () => TestParse (
			"foo ___", 
			"<p>foo ___</p>");

		/* [Example 426](http://spec.commonmark.org/0.28/#example-426) */
		[TestMethod]
		public void Example426 () => TestParse (
			"foo _\\__", 
			"<p>foo <em>_</em></p>");

		/* [Example 427](http://spec.commonmark.org/0.28/#example-427) */
		[TestMethod]
		public void Example427 () => TestParse (
			"foo _*_", 
			"<p>foo <em>*</em></p>");

		/* [Example 428](http://spec.commonmark.org/0.28/#example-428) */
		[TestMethod]
		public void Example428 () => TestParse (
			"foo _____", 
			"<p>foo _____</p>");

		/* [Example 429](http://spec.commonmark.org/0.28/#example-429) */
		[TestMethod]
		public void Example429 () => TestParse (
			"foo __\\___", 
			"<p>foo <strong>_</strong></p>");

		/* [Example 430](http://spec.commonmark.org/0.28/#example-430) */
		[TestMethod]
		public void Example430 () => TestParse (
			"foo __*__", 
			"<p>foo <strong>*</strong></p>");

		/* [Example 431](http://spec.commonmark.org/0.28/#example-431) */
		[TestMethod]
		public void Example431 () => TestParse (
			"__foo_", 
			"<p>_<em>foo</em></p>");

		/* [Example 432](http://spec.commonmark.org/0.28/#example-432) */
		[TestMethod]
		public void Example432 () => TestParse (
			"_foo__", 
			"<p><em>foo</em>_</p>");

		/* [Example 433](http://spec.commonmark.org/0.28/#example-433) */
		[TestMethod]
		public void Example433 () => TestParse (
			"___foo__", 
			"<p>_<strong>foo</strong></p>");

		/* [Example 434](http://spec.commonmark.org/0.28/#example-434) */
		[TestMethod]
		public void Example434 () => TestParse (
			"____foo_", 
			"<p>___<em>foo</em></p>");

		/* [Example 435](http://spec.commonmark.org/0.28/#example-435) */
		[TestMethod]
		public void Example435 () => TestParse (
			"__foo___", 
			"<p><strong>foo</strong>_</p>");

		/* [Example 436](http://spec.commonmark.org/0.28/#example-436) */
		[TestMethod]
		public void Example436 () => TestParse (
			"_foo____", 
			"<p><em>foo</em>___</p>");

		/* [Example 437](http://spec.commonmark.org/0.28/#example-437) */
		[TestMethod]
		public void Example437 () => TestParse (
			"**foo**", "<p><strong>foo</strong></p>");

		/* [Example 438](http://spec.commonmark.org/0.28/#example-438) */
		[TestMethod]
		public void Example438 () => TestParse (
			"*_foo_*", 
			"<p><em><em>foo</em></em></p>");

		/* [Example 439](http://spec.commonmark.org/0.28/#example-439) */
		[TestMethod]
		public void Example439 () => TestParse (
			"__foo__", 
			"<p><strong>foo</strong></p>");

		/* [Example 440](http://spec.commonmark.org/0.28/#example-440) */
		[TestMethod]
		public void Example440 () => TestParse (
			"_*foo*_", 
			"<p><em><em>foo</em></em></p>");

		/* [Example 441](http://spec.commonmark.org/0.28/#example-441) */
		[TestMethod]
		public void Example441 () => TestParse (
			"****foo****", 
			"<p><strong><strong>foo</strong></strong></p>");

		/* [Example 442](http://spec.commonmark.org/0.28/#example-442) */
		[TestMethod]
		public void Example442 () => TestParse (
			"____foo____", 
			"<p><strong><strong>foo</strong></strong></p>");

		/* [Example 443](http://spec.commonmark.org/0.28/#example-443) */
		[TestMethod]
		public void Example443 () => TestParse (
			"******foo******", 
			"<p><strong><strong><strong>foo</strong></strong></strong></p>");

		/* [Example 444](http://spec.commonmark.org/0.28/#example-444) */
		[TestMethod]
		public void Example444 () => TestParse (
			"***foo***", 
			"<p><em><strong>foo</strong></em></p>");

		/* [Example 445](http://spec.commonmark.org/0.28/#example-445) */
		[TestMethod]
		public void Example445 () => TestParse (
			"_____foo_____", 
			"<p><em><strong><strong>foo</strong></strong></em></p>");

		/* [Example 446](http://spec.commonmark.org/0.28/#example-446) */
		[TestMethod]
		public void Example446 () => TestParse (
			"*foo _bar* baz_", 
			"<p><em>foo _bar</em> baz_</p>");

		/* [Example 447](http://spec.commonmark.org/0.28/#example-447) */
		[TestMethod]
		public void Example447 () => TestParse (
			"*foo __bar *baz bim__ bam*", 
			"<p><em>foo <strong>bar *baz bim</strong> bam</em></p>");

		/* [Example 448](http://spec.commonmark.org/0.28/#example-448) */
		[TestMethod]
		public void Example448 () => TestParse (
			"**foo **bar baz**", 
			"<p>**foo <strong>bar baz</strong></p>");

		/* [Example 449](http://spec.commonmark.org/0.28/#example-449) */
		[TestMethod]
		public void Example449 () => TestParse (
			"*foo *bar baz*", 
			"<p>*foo <em>bar baz</em></p>");

		/* [Example 450](http://spec.commonmark.org/0.28/#example-450) */
		[TestMethod]
		public void Example450 () => TestParse (
			"*[bar*](/url)", 
			"<p>*<a href=\"/url\">bar*</a></p>");

		/* [Example 451](http://spec.commonmark.org/0.28/#example-451) */
		[TestMethod]
		public void Example451 () => TestParse (
			"_foo [bar_](/url)", "<p>_foo <a href=\"/url\">bar_</a></p>");

		/* [Example 452](http://spec.commonmark.org/0.28/#example-452) */
		[TestMethod]
		public void Example452 () => TestParse (
			"*<img src=\"foo\" title=\"*\"/>", 
			"<p>*<img src=\"foo\" title=\"*\"/></p>");

		/* [Example 453](http://spec.commonmark.org/0.28/#example-453) */
		[TestMethod]
		public void Example453 () => TestParse (
			"**<a href=\"**\">", 
			"<p>**<a href=\"**\"></p>");

		/* [Example 454](http://spec.commonmark.org/0.28/#example-454) */
		[TestMethod]
		public void Example454 () => TestParse (
			"__<a href=\"__\">", 
			"<p>__<a href=\"__\"></p>");

		/* [Example 455](http://spec.commonmark.org/0.28/#example-455) */
		[TestMethod]
		public void Example455 () => TestParse (
			"*a `*`*", 
			"<p><em>a <code>*</code></em></p>");

		/* [Example 456](http://spec.commonmark.org/0.28/#example-456) */
		[TestMethod]
		public void Example456 () => TestParse (
			"_a `_`_", 
			"<p><em>a <code>_</code></em></p>");

		/* [Example 457](http://spec.commonmark.org/0.28/#example-457) */
		[TestMethod]
		public void Example457 () => TestParse (
			"**a<http://foo.bar/?q=**>", 
			"<p>**a<a href=\"http://foo.bar/?q=**\">http://foo.bar/?q=**</a></p>");

		/* [Example 458](http://spec.commonmark.org/0.28/#example-458) */
		[TestMethod]
		public void Example458 () => TestParse (
			"__a<http://foo.bar/?q=__>", 
			"<p>__a<a href=\"http://foo.bar/?q=__\">http://foo.bar/?q=__</a></p>");


	}
}
