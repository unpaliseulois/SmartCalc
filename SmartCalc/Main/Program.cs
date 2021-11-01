using System;
using System.Linq;
using static System.Console;
using static System.ConsoleColor;
using static System.Environment;
using SmartCalc.Global.CodeAnalysis;

namespace SmartCalc.Main
{
    internal static class Program
    {
        private static void Main()
        {
            var displayTree = false;

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
                        ForegroundColor=DarkGreen;
                        msg = "Display trees is enabled.";

                    }
                    else
                    {
                        ForegroundColor=DarkYellow;
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
                string[] clearCmmands = { "cc", "cls", "clear", "wipe", "wc","clean", "cs" };
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

                if (displayTree)
                {
                    ForegroundColor = DarkCyan;
                    PrettyPrint(syntaxTree.Root);
                    ResetColor();
                }

                if (!syntaxTree.Diagnostics.Any())
                {
                    var e = new Evaluator(syntaxTree.Root);
                    var result = e.Evaluate();
                    ForegroundColor = Gray;
                    WriteLine(result);
                    ResetColor();
                }
                else
                {
                    ForegroundColor = DarkRed;
                    foreach (var diagnostic in syntaxTree.Diagnostics)
                        WriteLine(diagnostic);
                        ResetColor();
                }
                
            }
        }
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
    }
}
