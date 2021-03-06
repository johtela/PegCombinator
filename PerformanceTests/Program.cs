﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MarkdownPeg;
using System.Diagnostics;
using Markdig;

namespace PerformanceTests
{
	class Program
	{
		public static MarkdownToHtml Parser;
		public static Stopwatch Timing;
		public static MarkdownPipeline Pipeline;

		static Program ()
		{
			PegCombinator.Parser.Debugging = false;
			Parser = new MarkdownToHtml ("\n");
			Timing = new Stopwatch ();
			Pipeline = new MarkdownPipelineBuilder ().Build ();
		}

		static void Main (string[] args)
		{
			Console.WriteLine ("Reading file...");
			var file1 = File.ReadAllText (@"..\..\syntax.md");
			var file2 = File.ReadAllText (@"..\..\syntax2.md");
			for (int i = 0; i < 100; i++)
			{
				//Parse (file1, Parser.Run, "MarkdownPeg");
				//Parse (file2, Parser.Run, "MarkdownPeg");
				Parse (file1, s => Markdown.ToHtml (s, Pipeline), "Markdig");
				Parse (file2, s => Markdown.ToHtml (s, Pipeline), "Markdig");
			}
			//Console.WriteLine ("Writing output...");
			//File.WriteAllText (@"..\..\syntax1.html", parsed1);
			//File.WriteAllText (@"..\..\syntax2.html", parsed2);
		}

		private static string Parse (string file, Func<string, string> parser, string name)
		{
			Timing.Reset ();
			Console.WriteLine ("Parsing text with {0}...", name);
			Timing.Start ();
			var parsed = parser (file);
			Timing.Stop ();
			Console.WriteLine ("Parsed in {0} ms.", Timing.ElapsedMilliseconds);
			return parsed;
		}
	}
}
