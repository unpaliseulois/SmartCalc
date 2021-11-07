using System;
using System.Linq;
using static System.Console;
using static System.ConsoleColor;
using static System.Environment;
using SmartCalc.Global.CodeAnalysis.Syntax;
using System.Collections.Generic;
using SmartCalc.Global.CodeAnalysis;
using SmartCalc.Global.Compilation;
using System.IO;
using System.Text;

namespace SmartCalc.Main
{
    internal static class Program
    {
        private static void Main()
        {
            var displayTree = false;
            var variables = new Dictionary<VariableSymbol, object>();

            while (true)
            {
                var appName = "SmartCalc";
                var userName = UserName;
                userName = userName[0].ToString().ToUpper() + userName.Substring(1);

                Write($"{userName}@{appName} :: ");
                var line = ReadLine();

                if (string.IsNullOrWhiteSpace(line))
                    return;

                else if (line.ToLower() == "dt")
                {
                    var msg = string.Empty;
                    if (displayTree == false)
                    {
                        displayTree = true;
                        ForegroundColor = DarkGreen;
                        msg = "Display trees is enabled.";

                    }
                    else
                    {
                        ForegroundColor = DarkYellow;
                        msg = "Display trees is already enabled.";
                    }
                    WriteLine(msg);
                    ResetColor();
                    continue;
                }
                else if (line.ToLower() == "ht")
                {
                    var msg = string.Empty;
                    if (displayTree == true)
                    {
                        displayTree = false;
                        ForegroundColor = DarkYellow;
                        msg = "Display trees is disabled.";

                    }
                    else
                    {
                        ForegroundColor = DarkCyan;
                        msg = "Display trees is already disabled.";
                    }
                    WriteLine(msg);
                    ResetColor();
                    continue;
                }
                // Clear
                string[] clearCmmands = { "cc", "cls", "clear", "wipe", "wc", "clean", "cs" };
                if (clearCmmands.Contains(line.ToLower()))
                {
                    Clear();
                    ResetColor();
                    continue;
                }
                // Exit
                string[] clearCommands = { "e", "exit", "terminate", "close", "bye" };
                if (clearCommands.Contains(line.ToLower()))
                {
                    Environment.Exit(0);
                }

                var syntaxTree = SyntaxTree.Parse(line);
                var compilation = new Compilation(syntaxTree);
                var result = compilation.Evaluate(variables);

                var diagnostics = result.Diagnostics;

                if (displayTree)
                {
                    ForegroundColor = DarkCyan;
                    syntaxTree.Root.WriteTo(Console.Out);                  
                    ResetColor();
                }

                if (!diagnostics.Any())
                {

                    ForegroundColor = Gray;
                    WriteLine(result.Value);
                    ResetColor();
                }
                else
                {
                    foreach (var diagnostic in diagnostics)
                    {
                        ForegroundColor = DarkRed;
                        WriteLine($"\n{diagnostic}\n");
                        ResetColor();

                        var prefix = line.Substring(0, diagnostic.Span.Start);
                        var error = line.Substring(diagnostic.Span.Start, diagnostic.Span.Length);
                        var sufix = line.Substring(diagnostic.Span.End);

                        Write($"    {prefix}");
                        ForegroundColor = DarkRed;
                        Write(error);
                        ResetColor();
                        WriteLine($"{sufix}\n");
                    }
                    ResetColor();
                }
            }
        }
        /*
        static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
        {
            var marker = isLast ? "└──" : "├──";
            Write(indent);
            Write(marker);
            Write(node.Kind);
            if (node is SyntaxToken t && t.Value != null)
            {
                Write(" ");
                Write(t.Value);
            }
            WriteLine();
            indent += isLast ? "   " : "│  ";

            var lastChild = node.GetChildren().LastOrDefault();
            foreach (var child in node.GetChildren())
                PrettyPrint(child, indent, child == lastChild);
        }
        //*/
    }
}
