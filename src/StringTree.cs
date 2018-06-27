namespace PegCombinator
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using ExtensionCord;

	public abstract class StringTree
	{
		protected abstract void Output (StringBuilder sb);

		public abstract StringTree FindTag (object tag);

		public bool HasTag (object tag) => FindTag (tag) != null;

		internal class Leaf : StringTree
		{
			public readonly string Value;

			public Leaf (string value) => 
				Value = value;

			protected override void Output (StringBuilder sb)
			{
				sb.Append (Value);
			}

			public override StringTree FindTag (object tag) => null;
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

			public override StringTree FindTag (object tag) => 
				Values.FirstOrDefault (v => v.FindTag (tag) != null);
		}

		internal class TagNode : StringTree
		{
			public readonly StringTree Target;
			public readonly object Tag;

			public TagNode (StringTree target, object tag)
			{
				Target = target;
				Tag = tag;
			}

			protected override void Output (StringBuilder sb) => 
				Target.Output (sb);

			public override StringTree FindTag (object tag) => 
				Tag == tag ? this : Target.FindTag (tag);
		}

		internal class LazyNode : StringTree
		{
			public readonly Func<StringTree> GetValue;
			public StringTree Value;

			public LazyNode (Func<StringTree> getValue)
			{
				GetValue = getValue;
			}

			private void ForceValue ()
			{
				if (Value == null)
					Value = GetValue ();
			}

			protected override void Output (StringBuilder sb)
			{
				ForceValue ();
				Value.Output (sb);
			}

			public override StringTree FindTag (object tag)
			{
				ForceValue ();
				return Value.FindTag (tag);
			}
		}

		public bool IsLeaf => this is Leaf;

		public string LeafValue => (this as Leaf)?.Value;

		public bool IsList => this is ListNode;

		public IEnumerable<StringTree> ListValues =>
			(this as ListNode)?.Values;

		public bool IsTag => this is TagNode;

		public object TagValue => (this as TagNode)?.Tag;

		public StringTree TagTarget => (this as TagNode)?.Target;

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
			StringTreeHelpers.ToStringTree (values);

		public static StringTree Lazy (Func<StringTree> getValue) => 
			new LazyNode (getValue);
	}

	public static class StringTreeHelpers
	{
		public static StringTree ToStringTree (this IEnumerable<StringTree> values) => 
			values.None () ?
				StringTree.Empty :
				new StringTree.ListNode (values);

		public static StringTree ToStringTree (this IEnumerable<string> values) =>
			ToStringTree (values.Select (v => new StringTree.Leaf (v)));

		public static StringTree Tag (this StringTree target, object tag) =>
			new StringTree.TagNode (target, tag);
	}
}
