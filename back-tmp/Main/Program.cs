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
            Compilation previous = null;
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
                        WriteLine();
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
                        WriteLine();
                        continue;
                    }
                    else if (clearCmmands.Contains(input.ToLower()))
                    {
                        Clear();
                        ResetColor();
                        continue;
                    }
                    else if (input.ToLower() == "vmr")
                    {
                        WriteLine();
                        if (previous != null)
                        {
                            previous = null;
                            ForegroundColor = DarkYellow;
                            WriteLine("The variables memory reset Successfully.");
                        }
                        else
                        {
                            ForegroundColor = DarkCyan;
                            WriteLine("The variables memory is already reset.");
                        }
                        WriteLine();
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
                var compilation = previous == null ? new Compilation(syntaxTree)
                                                   : previous.ContinueWith(syntaxTree);

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
                    Write($"\n    ");
                    FormatResult(textToDisplay);
                    ForegroundColor = Green;
                    Write($"{result.Value}\n");
                    ResetColor();
                    previous = compilation;
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
                        Write($"    [{prefix.Trim()}");
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

        private static void FormatResult(string input)
        {
            char[] operators = { '+', '-', '*', '/', '^' };
            char[] parenthesis = { '(', ')' };
            if (!int.TryParse(input, out var result))
            {                
                foreach (char c in input)
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
                ForegroundColor = DarkYellow;
                Write(" = ");
            }
            ResetColor();
        }
    }
}
