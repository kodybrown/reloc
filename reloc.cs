/*!
	Copyright (C) 2010-2013 Kody Brown (kody@bricksoft.com).
	
	MIT License:
	
	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to
	deal in the Software without restriction, including without limitation the
	rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
	sell copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:
	
	The above copyright notice and this permission notice shall be included in
	all copies or substantial portions of the Software.
	
	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
	FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
	DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Bricksoft.PowerCode;

namespace reloc
{
	public class reloc
	{
		static bool DEBUG = false;

		static string app;
		static int appLen;
		static string appPadding;

		public static int Main( string[] arguments )
		{
			string file;
			Settings settings;
			int value;
			ConsoleUtils.WindowPosition direction = ConsoleUtils.WindowPosition.NotSet;
			int? margin = new int?();
			int? cmargin = new int?();
			int? rmargin = new int?();
			bool writeToConfig = false;

			app = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
			appLen = Math.Max((app + ".exe").Length, 9);
			appPadding = new string(' ', appLen);

			file = Assembly.GetEntryAssembly().Location + ".settings";
			settings = new Settings(file);
			settings.read();

			for (int i = 0; i < arguments.Length; i++) {
				string a = arguments[i].Trim();

				if (int.TryParse(a, out value)) {
					if (Enum.IsDefined(typeof(ConsoleUtils.WindowPosition), value)) {
						direction = (ConsoleUtils.WindowPosition)value;
					} else {
						DisplayAppName();
						DisplayCopyright();
						DisplayError(settings, "invalid value specified.");
						DisplayConfig(settings);
						return 4;
					}

				} else if (a.Equals("restore", StringComparison.CurrentCultureIgnoreCase)) {
					direction = ConsoleUtils.WindowPosition.Restore;
				} else if (a.Equals("n", StringComparison.CurrentCultureIgnoreCase) || a.StartsWith("min", StringComparison.CurrentCultureIgnoreCase)) {
					direction = ConsoleUtils.WindowPosition.Minimized;
				} else if (a.Equals("x", StringComparison.CurrentCultureIgnoreCase) || a.StartsWith("max", StringComparison.CurrentCultureIgnoreCase)) {
					direction = ConsoleUtils.WindowPosition.Maximized;
				} else if (a.Equals("tr", StringComparison.CurrentCultureIgnoreCase) || a.StartsWith("topr", StringComparison.CurrentCultureIgnoreCase)) {
					direction = ConsoleUtils.WindowPosition.TopRight;
				} else if (a.Equals("tl", StringComparison.CurrentCultureIgnoreCase) || a.StartsWith("topl", StringComparison.CurrentCultureIgnoreCase)) {
					direction = ConsoleUtils.WindowPosition.TopLeft;
				} else if (a.StartsWith("u", StringComparison.CurrentCultureIgnoreCase) || a.StartsWith("t", StringComparison.CurrentCultureIgnoreCase)) {
					direction = ConsoleUtils.WindowPosition.Top;
				} else if (a.StartsWith("r", StringComparison.CurrentCultureIgnoreCase) || a.StartsWith("e", StringComparison.CurrentCultureIgnoreCase)) {
					direction = ConsoleUtils.WindowPosition.Right;
				} else if (a.StartsWith("c", StringComparison.CurrentCultureIgnoreCase)) {
					direction = ConsoleUtils.WindowPosition.Center;
				} else if (a.StartsWith("l", StringComparison.CurrentCultureIgnoreCase) || a.StartsWith("w", StringComparison.CurrentCultureIgnoreCase)) {
					direction = ConsoleUtils.WindowPosition.Left;
				} else if (a.Equals("br", StringComparison.CurrentCultureIgnoreCase) || a.StartsWith("bottomr", StringComparison.CurrentCultureIgnoreCase)) {
					direction = ConsoleUtils.WindowPosition.BottomRight;
				} else if (a.Equals("bl", StringComparison.CurrentCultureIgnoreCase) || a.StartsWith("bottoml", StringComparison.CurrentCultureIgnoreCase)) {
					direction = ConsoleUtils.WindowPosition.BottomLeft;
				} else if (a.StartsWith("d", StringComparison.CurrentCultureIgnoreCase) || a.StartsWith("b", StringComparison.CurrentCultureIgnoreCase)) {
					direction = ConsoleUtils.WindowPosition.Bottom;

				} else {
					while (a.StartsWith("-") || a.StartsWith("/")) {
						a = a.TrimStart('-').TrimStart('/');
					}

					if (a.Equals("?") || a.StartsWith("h", StringComparison.CurrentCultureIgnoreCase)) {
						DisplayAppName();
						DisplayCopyright();
						DisplayHelp(settings);
						DisplayConfig(settings);
						return 0;

					} else if (a.Equals("debug", StringComparison.CurrentCultureIgnoreCase)) {
						DEBUG = true;

					} else if (a.Equals("clear", StringComparison.CurrentCultureIgnoreCase)) {
						settings.clear();
						settings.write();

					} else if (a.StartsWith("m", StringComparison.CurrentCultureIgnoreCase)
							|| a.StartsWith("cm", StringComparison.CurrentCultureIgnoreCase)
							|| a.StartsWith("rm", StringComparison.CurrentCultureIgnoreCase)) {
						string[] ar = a.Split(new char[] { ':' }, 2);
						if (ar.Length != 2 || !int.TryParse(ar[1], out value)) {
							DisplayError(settings, "invalid arguments");
							return 5;
						}
						if (a.StartsWith("cm", StringComparison.CurrentCultureIgnoreCase)) {
							cmargin = value;
						} else if (a.StartsWith("rm", StringComparison.CurrentCultureIgnoreCase)) {
							rmargin = value;
						} else if (a.StartsWith("m", StringComparison.CurrentCultureIgnoreCase)) {
							margin = value;
						}

					} else if (a.Equals("config", StringComparison.CurrentCultureIgnoreCase)) {
						writeToConfig = true;
					} else if (a.Equals("!config", StringComparison.CurrentCultureIgnoreCase)) {
						writeToConfig = false;

					} else if (a.Equals("edit", StringComparison.CurrentCultureIgnoreCase)) {
						LaunchUrl("notepad.exe", file);
					} else if (a.Equals("email", StringComparison.CurrentCultureIgnoreCase)) {
						LaunchUrl("mailto:Kody Brown <kody@bricksoft.com>");
					} else if (a.Equals("web", StringComparison.CurrentCultureIgnoreCase)) {
						LaunchUrl("http://bricksoft.com");
					} else if (a.Equals("src", StringComparison.CurrentCultureIgnoreCase) || a.Equals("source", StringComparison.CurrentCultureIgnoreCase)) {
						LaunchUrl("http://github.com/kodybrown/" + app);
					} else if (a.Equals("license", StringComparison.CurrentCultureIgnoreCase)) {
						LaunchUrl("http://opensource.org/licenses/MIT");

					} else {
						DisplayError(settings, "unknown argument.");
						return 1;
					}
				}
			}

			DisplayAppName();

			// If '--config' was specified without any other arguments, 
			// it will only output the current values from config.
			if (writeToConfig && direction == ConsoleUtils.WindowPosition.NotSet && !margin.HasValue && !cmargin.HasValue && !rmargin.HasValue) {
				DisplayAppName();
				DisplayCopyright();
				DisplayConfig(settings);
				return 0;
			}

			// Write config values before they are (possibly) overwritten below.
			if (writeToConfig) {
				if (direction != ConsoleUtils.WindowPosition.NotSet) {
					settings.attr<int>("direction", (int)direction);
				}
				if (margin.HasValue) {
					settings.attr<int>("margin", margin.Value);
				}
				if (cmargin.HasValue) {
					settings.attr<int>("cmargin", cmargin.Value);
				}
				if (rmargin.HasValue) {
					settings.attr<int>("rmargin", rmargin.Value);
				}
				settings.write();
			}

			// If a direction was not specified: use the direction from config,
			// otherwise don't change anything.
			if (direction == ConsoleUtils.WindowPosition.NotSet) {
				if (settings.contains("direction")) {
					direction = (ConsoleUtils.WindowPosition)settings.attr<int>("direction");
				} else {
					DisplayAppName();
					DisplayCopyright();
					DisplayError(settings, "no direction specified");
					DisplayConfig(settings);
					return 3;
				}
			}
			// If a margin was not specified: use the margin from config,
			// otherwise use zero.
			if (!margin.HasValue) {
				margin = settings.contains("margin") ? settings.attr<int>("margin") : 0;
			}
			if (!cmargin.HasValue) {
				cmargin = settings.contains("cmargin") ? settings.attr<int>("cmargin") : 0;
			}
			if (!rmargin.HasValue) {
				rmargin = settings.contains("rmargin") ? settings.attr<int>("rmargin") : 0;
			}

			//
			// Update the console.
			//
			try {
				if (margin.HasValue || (cmargin.Value == 0 && rmargin.Value == 0)) {
					// Specifying a margin overrides col and row margins.
					ConsoleUtils.MoveWindow((ConsoleUtils.WindowPosition)(int)direction, margin.Value, margin.Value);
				} else {
					ConsoleUtils.MoveWindow((ConsoleUtils.WindowPosition)(int)direction, cmargin.Value, rmargin.Value);
				}
			} catch (Exception ex) {
				Console.Write("{0," + appLen + "} | Could not move the window.\n{1}", "** error", ex.Message);
			}

			if (DEBUG) {
				Console.Write("press any key to continue: ");
				Console.ReadKey(true);
				Console.CursorLeft = 0;
				Console.Write("                            ");
				Console.CursorLeft = 0;
			}

			return 0;
		}

		private static void LaunchUrl( string file ) { LaunchUrl(file, null); }

		private static void LaunchUrl( string file, string args )
		{
			ProcessStartInfo info = new ProcessStartInfo();

			info.Verb = "open";
			info.FileName = file;

			if (args != null && args.Length > 0) {
				info.Arguments = args;
			}

			Process.Start(info);
		}

		private static void DisplayAppName()
		{
			Console.WriteLine("{0,-" + appLen + "} | created by kody@bricksoft.com", app + ".exe"); // (--email)
		}

		private static void DisplayCopyright()
		{
			Console.WriteLine("{0} | http://github.com/kodybrown/" + app + " (--src)", appPadding);
			Console.WriteLine("{0} | released under the mit license (--license)", appPadding);
			Console.WriteLine("{0} | display usage information (--help)", appPadding);
		}

		private static void DisplayHelp( Settings settings )
		{
			int w = Console.WindowWidth,
				a = 2,
				b = app.Length + 17;

			Console.WriteLine("\nSYNOPSIS:");
			Console.WriteLine(Text.Wrap(string.Format("\nA simple DOS/CLI utility that resizes and relocates the current command prompt window to various locations on the screen based on the specified arguments (or whatever is saved in its config).", app), w, 4));
			Console.WriteLine("\nUSAGE:");
			Console.WriteLine("\n    {0}.exe [--config][--clear] direction", app);
			Console.WriteLine();
			Console.WriteLine("  Supported Directions:");
			Console.WriteLine(@"
    7|tl            8|up|top        9|tr         
    ▄▄▄▄▄▄▄▄▄▄▄▄▄   ▄▄▄▄▄▄▄▄▄▄▄▄▄   ▄▄▄▄▄▄▄▄▄▄▄▄▄
    ██████      █   █████████████   █      ██████
    ██████      █   █████████████   █      ██████
    █           █   █           █   █           █
    █           █   █           █   █           █
    █           █   █           █   █           █
    ▀▀▀▀▀▀▀▀▀▀▀▀▀   ▀▀▀▀▀▀▀▀▀▀▀▀▀   ▀▀▀▀▀▀▀▀▀▀▀▀▀
    
    4|left|west     5|center        6|right|east
    ▄▄▄▄▄▄▄▄▄▄▄▄▄   ▄▄▄▄▄▄▄▄▄▄▄▄▄   ▄▄▄▄▄▄▄▄▄▄▄▄▄
    ██████      █   █           █   █      ██████
    ██████      █   █   ▄▄▄▄▄   █   █      ██████
    ██████      █   █   █████   █   █      ██████
    ██████      █   █   ▀▀▀▀▀   █   █      ██████
    ██████      █   █           █   █      ██████
    ▀▀▀▀▀▀▀▀▀▀▀▀▀   ▀▀▀▀▀▀▀▀▀▀▀▀▀   ▀▀▀▀▀▀▀▀▀▀▀▀▀
    
    1|bl            2|down|bottom   3|br
    ▄▄▄▄▄▄▄▄▄▄▄▄▄   ▄▄▄▄▄▄▄▄▄▄▄▄▄   ▄▄▄▄▄▄▄▄▄▄▄▄▄
    █           █   █           █   █           █
    █           █   █           █   █           █
    █           █   █           █   █           █
    ██████      █   █████████████   █      ██████
    ██████      █   █████████████   █      ██████
    ▀▀▀▀▀▀▀▀▀▀▀▀▀   ▀▀▀▀▀▀▀▀▀▀▀▀▀   ▀▀▀▀▀▀▀▀▀▀▀▀▀
    
    10|max|x
    ▄▄▄▄▄▄▄▄▄▄▄▄▄
    █████████████
    █████████████
    █████████████
    █████████████
    █████████████
    ▀▀▀▀▀▀▀▀▀▀▀▀▀");

			//Console.WriteLine(Text.Wrap("11|min|n       minimizes the console window."));
			//Console.WriteLine(Text.Wrap("12|restore     restores the console window from being minimized."));
			Console.WriteLine();

			//writeOption("--cmargin:n", "Sets only the column margin between the console window and its specified location.", a, b, w);
			//writeOption("--rmargin:n", "Sets only the row margin between the console window and its specified location.", a, b, w);
			//writeOption("--margin:n", "Sets the column and row margin between the console window and its specified location. The margin values represents the approximate number of columns and rows.", a, b, w);
			//Console.WriteLine();
			writeOption("--clear", "Clears the values in config.", a, b, w);
			writeOption("--config", "When used with direction, the direction saved to config. When used by itself, the config values are displayed.", a, b, w);
			writeOption(" ", "The position of the `--config` option does not matter in relation to the direction.", a, b, w);

			DisplayExamples();
		}

		// promptSymbol
		private static string ps = Path.DirectorySeparatorChar == '\\' ? "> " : "$ ";

		private static void DisplayExamples()
		{
			int w = Console.WindowWidth,
				a = 2,
				b = app.Length + 17;

			Console.WriteLine("\nEXAMPLES:");
			Console.WriteLine();

			writeExample(app + " max", "Maximizes the console window.", a, b, w);
			writeExample(app + " 10", "Maximizes the console window.", a, b, w);
			writeExample(app + " up", "Resizes the console and positions at the top of the screen.", a, b, w);
			writeExample(app + " br", "Resizes the console and positions at the bottom right corner of the screen.", a, b, w);
			Console.WriteLine();
			writeExample(app + " --config", "Displays current config values.", a, b, w);
			writeExample(app + " l", "Relocates to the left edge and saves to config.", a, b, w);
		}

		private static void DisplayConfig( Settings settings )
		{
			int w = Console.WindowWidth,
				a = 2,
				b = 14;

			Console.WriteLine("\nSAVED CONFIG:");
			Console.WriteLine();

			writeExpression("direction", (settings.contains("direction") ? ((ConsoleUtils.WindowPosition)settings.attr<int>("direction")).ToString() : "not set"), a, b, w);
			//writeExpression("margin", (settings.contains("margin") ? settings.attr<int>("margin").ToString() : "not set"), a, b, w);
			//writeExpression("cmargin", (settings.contains("cmargin") ? settings.attr<int>("cmargin").ToString() : "not set"), a, b, w);
			//writeExpression("rmargin", (settings.contains("rmargin") ? settings.attr<int>("rmargin").ToString() : "not set"), a, b, w);
		}

		private static void DisplayError( Settings settings, string message )
		{
			DisplayAppName();
			DisplayCopyright();
			Console.WriteLine();
			Console.WriteLine("{0," + appLen + "} | {1}", "** error", message);
			DisplayHelp(settings);
			DisplayConfig(settings);
		}

		private static void writeExample( string a, string b, int indentCol1, int indentCol2, int width )
		{
			int maxCol1 = indentCol2 - indentCol1 - ps.Length - 1;
			string aa = string.Format("{0,-" + maxCol1 + "}", a);

			if (aa.Length > maxCol1) {
				// Wrap the description onto the next line
				Console.WriteLine(Text.Wrap(aa, width, indentCol1));
				Console.WriteLine(Text.Wrap("» " + b, width, indentCol2 - 1, indentCol2 + 1));
			} else {
				Console.WriteLine(Text.Wrap(ps + aa + "» " + b, width, indentCol1, indentCol2 + 1));
			}
		}

		private static void writeExpression( string a, string b, int indentCol1, int indentCol2, int width )
		{
			int maxCol1 = indentCol2 - indentCol1 - 1;
			string aa = string.Format("{0,-" + maxCol1 + "}", a);

			if (aa.Length > maxCol1) {
				// Wrap the description onto the next line
				Console.WriteLine(Text.Wrap(aa, width, indentCol1));
				Console.WriteLine(Text.Wrap("= " + b, width, indentCol2 - 1, indentCol2 + 1));
			} else {
				Console.WriteLine(Text.Wrap(aa + "= " + b, width, indentCol1, indentCol2 + 1));
			}
		}

		private static void writeOption( string a, string b, int indentCol1, int indentCol2, int width )
		{
			int maxCol1 = indentCol2 - indentCol1 - 1;
			string aa = string.Format("{0,-" + maxCol1 + "}", a);

			if (aa.Length > maxCol1) {
				// Wrap the description onto the next line
				Console.WriteLine(Text.Wrap(aa, width, indentCol1));
				Console.WriteLine(Text.Wrap("» " + b, width, indentCol2 - 1, indentCol2 + 1));
			} else {
				Console.WriteLine(Text.Wrap(aa + "» " + b, width, indentCol1, indentCol2 + 1));
			}
		}
	}
}
