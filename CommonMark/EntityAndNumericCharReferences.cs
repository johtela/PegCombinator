namespace CommonMark
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class EntityAndNumericCharReferences : TestBase
	{
		/* [Example 302](http://spec.commonmark.org/0.28/#example-302) */
		[TestMethod]
		public void Example302 () => TestParse (
			"&nbsp; &amp; &copy; &AElig; &Dcaron;\n&frac34; &HilbertSpace; &DifferentialD;\n&ClockwiseContourIntegral; &ngE;", 
			"<p>  &amp; © Æ Ď\n¾ ℋ ⅆ\n∲ ≧̸</p>");
	}
}
