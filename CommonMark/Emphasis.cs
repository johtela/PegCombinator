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
	}
}
