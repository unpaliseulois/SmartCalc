using System;
using System.Linq;
using static System.Console;
using static System.ConsoleColor;
using static System.Environment;
using SmartCalc.Global.CodeAnalysis.Syntax;
using System.Collections.Generic;
using SmartCalc.Global.Compilation;
using System.Text;
using SmartCalc.Global.CodeAnalysis.Text;

namespace SmartCalc.Main
{
    internal static class Program
    {
        private static void Main()
        {
            var displayTree = false;
            var variables = new Dictionary<VariableSymbol, object>();
            var textBuilder = new StringBuilder();
            var textBuilderToDisplay = new StringBuilder();


            while (true)
            {
                var appName = "SmartCalc";
                var userName = UserName;
                userName = userName[0].ToString().ToUpper() + userName.Substring(1);

                Title = appName;

                if (textBuilder.Length == 0)
                {
                    ForegroundColor = DarkCyan;
                    Write($"{userName}");
                    ForegroundColor = Magenta;
                    Write("@");
                    ForegroundColor = Yellow;
                    Write($"{appName}");
                    ForegroundColor = Green;
                    Write(" :: ");
                    ResetColor();
                }
                else
                {
                    ForegroundColor = DarkRed;
                    Write("> ");
                    ResetColor();
                }

                var input = ReadLine();
                var isBlank = string.IsNullOrWhiteSpace(input);

                if (textBuilder.Length == 0)
                {
                    // Clear
                    string[] clearCmmands = { "cc", "cls", "clear", "wipe", "wc", "clean", "cs" };
                    // Exit
                    string[] exitCommands = { "e", "exit", "terminate", "close", "bye" };
                    if (isBlank)
                    {
                        continue;
                    }
                    else if (input.ToLower() == "dt")
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
                    else if (input.ToLower() == "ht")
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
                    else if (clearCmmands.Contains(input.ToLower()))
                    {
                        Clear();
                        ResetColor();
                        continue;
                    }
                    else if (exitCommands.Contains(input.ToLower()))
                    {
                        Environment.Exit(0);
                    }
                }

                //error here
                textBuilder.AppendLine(input);
                textBuilderToDisplay.Append(input);
                var text = textBuilder.ToString();
                var textToDisplay = textBuilderToDisplay.ToString();
                var syntaxTree = SyntaxTree.Parse(text);

                if (!isBlank && syntaxTree.Diagnostics.Any())
                    continue;
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
                    /*                   
                    ForegroundColor = Gray;
                    Write($"    \n{textToDisplay}");
                    ForegroundColor = DarkYellow;
                    Write(" = ");
                    ForegroundColor = Green;
                    Write($"{result.Value}\n");
                    ResetColor();
                    //*/
                    Write($"\n    ");
                    FormatResult(textToDisplay);
                    ForegroundColor = DarkYellow;
                    Write(" = ");
                    ForegroundColor = Green;
                    Write($"{result.Value}\n");
                    ResetColor();
                }
                else
                {
                    WriteLine();
                    foreach (var diagnostic in diagnostics)
                    {
                        var lineIndex = syntaxTree.Text.GetLineIndex(diagnostic.Span.Start);
                        var line = syntaxTree.Text.Lines[lineIndex];
                        var rowNumber = lineIndex + 1;
                        var columnNumber = diagnostic.Span.Start - line.Start + 1;

                        var prefixSpan = TextSpan.FromBounds(line.Start, diagnostic.Span.Start);
                        var sufixSpan = TextSpan.FromBounds(diagnostic.Span.End, line.End);

                        var prefix = syntaxTree.Text.ToString(prefixSpan);
                        var error = syntaxTree.Text.ToString(diagnostic.Span);
                        var sufix = syntaxTree.Text.ToString(sufixSpan);

                        ForegroundColor = DarkYellow;
                        Write($"    [{prefix}");
                        ForegroundColor = DarkRed;
                        Write($"{error}");
                        ForegroundColor = DarkYellow;
                        Write($"{sufix}] ");
                        ResetColor();

                        ForegroundColor = DarkRed;
                        Write($"    [Row:{rowNumber} - Col:{columnNumber}] :: ");
                        WriteLine($"{diagnostic}");
                        ResetColor();


                    }
                    //WriteLine();                    
                }
                WriteLine();
                textBuilder.Clear();
                textBuilderToDisplay.Clear();
            }
        }

        private static void FormatResult(string output)
        {
            char[] operators = { '+', '-', '*', '/', '^' };
            char[] parenthesis = { '(', ')' };
            foreach (char c in output)
            {
                if (operators.Contains(c))
                {
                    ForegroundColor = DarkYellow;
                }
                else if (parenthesis.Contains(c))
                {
                    ForegroundColor = DarkCyan;
                }
                else
                {
                    ForegroundColor = Cyan;
                }
                Write(c);
            }
            ResetColor();

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
