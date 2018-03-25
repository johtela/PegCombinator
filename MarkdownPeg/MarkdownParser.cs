namespace MarkdownPeg
{
	using PegCombinator;
	using SP = PegCombinator.StringParser;

	public enum MdElem
	{
		Heading,
		Raw
	}

    public class MarkdownParser
    {

		protected virtual void Heading (object start, object end, string headingText) { }

		public Parser<string, char> AtxStart ()
		{
			return SP.String ("######")
				.Or (SP.String ("#####"))
				.Or (SP.String ("####"))
				.Or (SP.String ("###"))
				.Or (SP.String ("##"))
				.Or (SP.String ("#"));
		}
	}
}
