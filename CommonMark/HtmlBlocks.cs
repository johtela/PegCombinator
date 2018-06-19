namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class HtmlBlocks : TestBase
	{
		/* [Example 116](http://spec.commonmark.org/0.28/#example-116) */
		[TestMethod]
		public void Example116 () => TestParse (
			"<table><tr><td>\n<pre>\n**Hello**,\n\n_world_.\n</pre>\n</td></tr></table>", 
			"<table><tr><td>\n<pre>\n**Hello**,\n<p><em>world</em>.\n</pre></p>\n</td></tr></table>");

		/* [Example 117](http://spec.commonmark.org/0.28/#example-117) */
		[TestMethod]
		public void Example117 () => TestParse (
			"<table>\n  <tr>\n    <td>\n           hi\n    </td>\n  </tr>\n</table>\n\nokay.", 
			"<table>\n  <tr>\n    <td>\n           hi\n    </td>\n  </tr>\n</table>\n<p>okay.</p>");

		/* [Example 118](http://spec.commonmark.org/0.28/#example-118) */
		[TestMethod]
		public void Example118 () => TestParse (
			" <div>\n  *hello*\n         <foo><a>", 
			" <div>\n  *hello*\n         <foo><a>");

		/* [Example 119](http://spec.commonmark.org/0.28/#example-119) */
		[TestMethod]
		public void Example119 () => TestParse (
			"</div>\n*foo*", 
			"</div>\n*foo*");

		/* [Example 120](http://spec.commonmark.org/0.28/#example-120) */
		[TestMethod]
		public void Example120 () => TestParse (
			"<DIV CLASS=\"foo\">\n\n*Markdown*\n\n</DIV>", 
			"<DIV CLASS=\"foo\">\n<p><em>Markdown</em></p>\n</DIV>");

		/* [Example 121](http://spec.commonmark.org/0.28/#example-121) */
		[TestMethod]
		public void Example121 () => TestParse (
			"<div id=\"foo\"\n  class=\"bar\">\n</div>", 
			"<div id=\"foo\"\n  class=\"bar\">\n</div>");

		/* [Example 122](http://spec.commonmark.org/0.28/#example-122) */
		[TestMethod]
		public void Example122 () => TestParse (
			"<div id=\"foo\" class=\"bar\n  baz\">\n</div>", 
			"<div id=\"foo\" class=\"bar\n  baz\">\n</div>");

		/* [Example 123](http://spec.commonmark.org/0.28/#example-123) */
		[TestMethod]
		public void Example123 () => TestParse (
			"<div>\n*foo*\n\n*bar*", 
			"<div>\n*foo*\n<p><em>bar</em></p>");

		/* [Example 124](http://spec.commonmark.org/0.28/#example-124) */
		[TestMethod]
		public void Example124 () => TestParse (
			"<div id=\"foo\"\n*hi*", 
			"<div id=\"foo\"\n*hi*");

		/* [Example 125](http://spec.commonmark.org/0.28/#example-125) */
		[TestMethod]
		public void Example125 () => TestParse (
			"<div class\nfoo", 
			"<div class\nfoo");

		/* [Example 126](http://spec.commonmark.org/0.28/#example-126) */
		[TestMethod]
		public void Example126 () => TestParse (
			"<div *???-&&&-<---\n*foo*", 
			"<div *???-&&&-<---\n*foo*");

		/* [Example 127](http://spec.commonmark.org/0.28/#example-127) */
		[TestMethod]
		public void Example127 () => TestParse (
			"<div><a href=\"bar\">*foo*</a></div>", 
			"<div><a href=\"bar\">*foo*</a></div>");

		/* [Example 128](http://spec.commonmark.org/0.28/#example-128) */
		[TestMethod]
		public void Example128 () => TestParse (
			"<table><tr><td>\nfoo\n</td></tr></table>", 
			"<table><tr><td>\nfoo\n</td></tr></table>");

		/* [Example 129](http://spec.commonmark.org/0.28/#example-129) */
		[TestMethod]
		public void Example129 () => TestParse (
			"<div></div>\n``` c\nint x = 33;\n```", 
			"<div></div>\n``` c\nint x = 33;\n```");

		/* [Example 130](http://spec.commonmark.org/0.28/#example-130) */
		[TestMethod]
		public void Example130 () => TestParse (
			"<a href=\"foo\">\n*bar*\n</a>", 
			"<a href=\"foo\">\n*bar*\n</a>");

		/* [Example 131](http://spec.commonmark.org/0.28/#example-131) */
		[TestMethod]
		public void Example131 () => TestParse (
			"<Warning>\n*bar*\n</Warning>", 
			"<Warning>\n*bar*\n</Warning>");

		/* [Example 132](http://spec.commonmark.org/0.28/#example-132) */
		[TestMethod]
		public void Example132 () => TestParse (
			"<i class=\"foo\">\n*bar*\n</i>", 
			"<i class=\"foo\">\n*bar*\n</i>");

		/* [Example 133](http://spec.commonmark.org/0.28/#example-133) */
		[TestMethod]
		public void Example133 () => TestParse (
			"</ins>\n*bar*", 
			"</ins>\n*bar*");

		/* [Example 134](http://spec.commonmark.org/0.28/#example-134) */
		[TestMethod]
		public void Example134 () => TestParse (
			"<del>\n*foo*\n</del>", 
			"<del>\n*foo*\n</del>");

		/* [Example 135](http://spec.commonmark.org/0.28/#example-135) */
		[TestMethod]
		public void Example135 () => TestParse (
			"<del>\n\n*foo*\n\n</del>", 
			"<del>\n<p><em>foo</em></p>\n</del>");

		/* [Example 136](http://spec.commonmark.org/0.28/#example-136) */
		[TestMethod]
		public void Example136 () => TestParse (
			"<del>*foo*</del>", 
			"<p><del><em>foo</em></del></p>");

		/* [Example 137](http://spec.commonmark.org/0.28/#example-137) */
		[TestMethod]
		public void Example137 () => TestParse (
			"<pre language=\"haskell\"><code>\nimport Text.HTML.TagSoup\n\nmain :: IO ()\nmain = print $ parseTags tags\n</code></pre>\nokay", 
			"<pre language=\"haskell\"><code>\nimport Text.HTML.TagSoup\n\nmain :: IO ()\nmain = print $ parseTags tags\n</code></pre>\n<p>okay</p>");

		/* [Example 138](http://spec.commonmark.org/0.28/#example-138) */
		[TestMethod]
		public void Example138 () => TestParse (
			"<script type=\"text/javascript\">\n// JavaScript example\n\ndocument.getElementById(\"demo\").innerHTML = \"Hello JavaScript!\";\n</script>\nokay", 
			"<script type=\"text/javascript\">\n// JavaScript example\n\ndocument.getElementById(\"demo\").innerHTML = \"Hello JavaScript!\";\n</script>\n<p>okay</p>");

		/* [Example 139](http://spec.commonmark.org/0.28/#example-139) */
		[TestMethod]
		public void Example139 () => TestParse (
			"<style\n  type=\"text/css\">\nh1 {color:red;}\n\np {color:blue;}\n</style>\nokay", 
			"<style\n  type=\"text/css\">\nh1 {color:red;}\n\np {color:blue;}\n</style>\n<p>okay</p>");

		/* [Example 140](http://spec.commonmark.org/0.28/#example-140) */
		[TestMethod]
		public void Example140 () => TestParse (
			"<style\n  type=\"text/css\">\n\nfoo", 
			"<style\n  type=\"text/css\">\n\nfoo");

		/* [Example 141](http://spec.commonmark.org/0.28/#example-141) */
		[TestMethod]
		public void Example141 () => TestParse (
			"> <div>\n> foo\n\nbar", 
			"<blockquote>\n<div>\nfoo\n</blockquote>\n<p>bar</p>");

		/* [Example 142](http://spec.commonmark.org/0.28/#example-142) */
		[TestMethod]
		public void Example142 () => TestParse (
			"- <div>\n- foo", 
			"<ul>\n<li>\n<div>\n</li>\n<li>foo</li>\n</ul>");

		/* [Example 143](http://spec.commonmark.org/0.28/#example-143) */
		[TestMethod]
		public void Example143 () => TestParse (
			"<style>p{color:red;}</style>\n*foo*", 
			"<style>p{color:red;}</style>\n<p><em>foo</em></p>");

		/* [Example 144](http://spec.commonmark.org/0.28/#example-144) */
		[TestMethod]
		public void Example144 () => TestParse (
			"<!-- foo -->*bar*\n*baz*", 
			"<!-- foo -->*bar*\n<p><em>baz</em></p>");

		/* [Example 145](http://spec.commonmark.org/0.28/#example-145) */
		[TestMethod]
		public void Example145 () => TestParse (
			"<script>\nfoo\n</script>1. *bar*", 
			"<script>\nfoo\n</script>1. *bar*");

		/* [Example 146](http://spec.commonmark.org/0.28/#example-146) */
		[TestMethod]
		public void Example146 () => TestParse (
			"<!-- Foo\n\nbar\n   baz -->\nokay", 
			"<!-- Foo\n\nbar\n   baz -->\n<p>okay</p>");

		/* [Example 147](http://spec.commonmark.org/0.28/#example-147) */
		[TestMethod]
		public void Example147 () => TestParse (
			"<?php\n\n  echo '>';\n\n?>\nokay", 
			"<?php\n\n  echo '>';\n\n?>\n<p>okay</p>");

		/* [Example 148](http://spec.commonmark.org/0.28/#example-148) */
		[TestMethod]
		public void Example148 () => TestParse (
			"<!DOCTYPE html>", 
			"<!DOCTYPE html>");

		/* [Example 149](http://spec.commonmark.org/0.28/#example-149) */
		[TestMethod]
		public void Example149 () => TestParse (
			"<![CDATA[\nfunction matchwo(a,b)\n{\n  if (a < b && a < 0) then {\n    return 1;\n\n  } else {\n\n    return 0;\n  }\n}\n]]>\nokay", 
			"<![CDATA[\nfunction matchwo(a,b)\n{\n  if (a < b && a < 0) then {\n    return 1;\n\n  } else {\n\n    return 0;\n  }\n}\n]]>\n<p>okay</p>");

		/* [Example 150](http://spec.commonmark.org/0.28/#example-150) */
		[TestMethod]
		public void Example150 () => TestParse (
			"  <!-- foo -->\n\n    <!-- foo -->", 
			"  <!-- foo -->\n<pre><code>&lt;!-- foo --&gt;\n</code></pre>");

		/* [Example 151](http://spec.commonmark.org/0.28/#example-151) */
		[TestMethod]
		public void Example151 () => TestParse (
			"  <div>\n\n    <div>", 
			"  <div>\n<pre><code>&lt;div&gt;\n</code></pre>");

		/* [Example 152](http://spec.commonmark.org/0.28/#example-152) */
		[TestMethod]
		public void Example152 () => TestParse (
			"Foo\n<div>\nbar\n</div>", 
			"<p>Foo</p>\n<div>\nbar\n</div>");

		/* [Example 153](http://spec.commonmark.org/0.28/#example-153) */
		[TestMethod]
		public void Example153 () => TestParse (
			"<div>\nbar\n</div>\n*foo*", 
			"<div>\nbar\n</div>\n*foo*");

		/* [Example 154](http://spec.commonmark.org/0.28/#example-154) */
		[TestMethod]
		public void Example154 () => TestParse (
			"Foo\n<a href=\"bar\">\nbaz", 
			"<p>Foo\n<a href=\"bar\">\nbaz</p>");

		/* [Example 155](http://spec.commonmark.org/0.28/#example-155) */
		[TestMethod]
		public void Example155 () => TestParse (
			"<div>\n\n*Emphasized* text.\n\n</div>", 
			"<div>\n<p><em>Emphasized</em> text.</p>\n</div>");

		/* [Example 156](http://spec.commonmark.org/0.28/#example-156) */
		[TestMethod]
		public void Example156 () => TestParse (
			"<div>\n*Emphasized* text.\n</div>", 
			"<div>\n*Emphasized* text.\n</div>");

		/* [Example 157](http://spec.commonmark.org/0.28/#example-157) */
		[TestMethod]
		public void Example157 () => TestParse (
			"<table>\n\n<tr>\n\n<td>\nHi\n</td>\n\n</tr>\n\n</table>", 
			"<table>\n<tr>\n<td>\nHi\n</td>\n</tr>\n</table>");

		/* [Example 158](http://spec.commonmark.org/0.28/#example-158) */
		[TestMethod]
		public void Example158 () => TestParse (
			"<table>\n\n  <tr>\n\n    <td>\n      Hi\n    </td>\n\n  </tr>\n\n</table>", 
			"<table>\n  <tr>\n<pre><code>&lt;td&gt;\n  Hi\n&lt;/td&gt;\n</code></pre>\n  </tr>\n</table>");
	}
}
