namespace PegCombinator
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public abstract class StringTree
	{
		protected abstract void Output (StringBuilder sb);

		internal class Leaf : StringTree
		{
			public readonly string Value;

			public Leaf (string value) => 
				Value = value;

			protected override void Output (StringBuilder sb)
			{
				sb.Append (Value);
			}
		}

		internal class ListNode : StringTree
		{
			public readonly IEnumerable<StringTree> Values;

			public ListNode (IEnumerable<StringTree> values)
			{
				Values = values;
			}

			protected override void Output (StringBuilder sb)
			{
				foreach (var value in Values)
					value.Output (sb);
			}
		}

		internal class TagNode : StringTree
		{
			public readonly string Tag;
			public readonly StringTree Value;

			public TagNode (string tag, StringTree value)
			{
				Tag = tag;
				Value = value;
			}

			protected override void Output (StringBuilder sb) => 
				Value.Output (sb);
		}

		internal class LazyNode : StringTree
		{
			public readonly Func<StringTree> GetValue;
			public StringTree Value;

			public LazyNode (Func<StringTree> getValue)
			{
				GetValue = getValue;
			}

			protected override void Output (StringBuilder sb)
			{
				if (Value == null)
					Value = GetValue ();
				Value.Output (sb);
			}
		}

		public bool IsLeaf () =>
			this is Leaf;

		public string LeafValue () =>
			IsLeaf () ? (this as Leaf).Value : null;

		public bool IsList () =>
			this is ListNode;

		public IEnumerable<StringTree> ListValues () =>
			IsList () ? (this as ListNode).Values : null;

		public override string ToString ()
		{
			var sb = new StringBuilder ();
			Output (sb);
			return sb.ToString ();
		}

		public static readonly StringTree Empty = new Leaf (string.Empty);

		public static implicit operator StringTree (string value) =>
			new Leaf (value);

		public static implicit operator StringTree (int value) =>
			new Leaf (value.ToString ());

		public static implicit operator StringTree (char value) =>
			new Leaf (new string (value, 1));

		public static StringTree operator + (StringTree left, StringTree right) =>
			From (left, right);

		public static StringTree From (params StringTree[] values) =>
			StringTreeHelpers.FromEnumerable (values);

		public static StringTree Lazy (Func<StringTree> getValue) => 
			new LazyNode (getValue);
	}

	public static class StringTreeHelpers
	{
		public static StringTree FromEnumerable (this IEnumerable<StringTree> values) =>
			new StringTree.ListNode (values);

		public static StringTree FromEnumerable (this IEnumerable<string> values) =>
			new StringTree.ListNode (values.Select (v => new StringTree.Leaf (v)));

		public static StringTree Tag (this StringTree value, string tag) =>
			new StringTree.TagNode (tag, value);
	}
}
