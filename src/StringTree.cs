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

		internal class ConcatNode : StringTree
		{
			public readonly StringTree Left;
			public readonly StringTree Right;

			public ConcatNode (StringTree left, StringTree right)
			{
				Left = left;
				Right = right;
			}

			protected override void Output (StringBuilder sb)
			{
				Left.Output (sb);
				Right.Output (sb);
			}
		}

		internal class ListNode : StringTree
		{
			public readonly IEnumerable<StringTree> Values;
			public readonly StringTree OpenDelimiter;
			public readonly StringTree CloseDelimiter;
			public readonly StringTree Separator;

			public ListNode (IEnumerable<StringTree> values, StringTree open,
				StringTree close, StringTree separator)
			{
				Values = values;
				OpenDelimiter = open;
				CloseDelimiter = close;
				Separator = separator;
			}

			protected override void Output (StringBuilder sb)
			{
				if (OpenDelimiter != null)
					OpenDelimiter.Output (sb);
				if (Separator == null)
					foreach (var value in Values)
						value.Output (sb);
				else
				{
					var first = Values.FirstOrDefault ();
					if (first != null)
					{
						first.Output (sb);
						foreach (var value in Values.Skip (1))
						{
							Separator.Output (sb);
							value.Output (sb);
						}
					}
				}
				if (CloseDelimiter != null)
					CloseDelimiter.Output (sb);
			}
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
			new ConcatNode (left, right);

		public static StringTree From (params StringTree[] values) =>
			StringTreeHelpers.FromEnumerable (values);

		public static StringTree Lazy (Func<StringTree> getValue) => 
			new LazyNode (getValue);
	}

	public static class StringTreeHelpers
	{
		public static StringTree FromEnumerable (this IEnumerable<StringTree> values,
			StringTree open = null, StringTree close = null, StringTree separator = null) =>
			new StringTree.ListNode (values, open, close, separator);

		public static StringTree FromEnumerable (this IEnumerable<string> values,
			StringTree open = null, StringTree close = null, StringTree separator = null) =>
			new StringTree.ListNode (values.Select (v => new StringTree.Leaf (v)), 
				open, close, separator);
	}
}
