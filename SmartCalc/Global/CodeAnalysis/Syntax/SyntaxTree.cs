using System.Collections.Generic;
using System.Collections.Immutable;
using SmartCalc.Global.CodeAnalysis.Text;
using SmartCalc.Global.Compilation;
namespace SmartCalc.Global.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(SourceText text, ImmutableArray<Diagnostic> diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken, bool enabled = false)
        {
            Text = text;
            Diagnostics = diagnostics;
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public SourceText Text { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }
        public static SyntaxTree Parse(string text)
        {
            var sourceText = SourceText.From(text);
            return Parse(sourceText);
        }
        public static SyntaxTree Parse(SourceText text)
        {
            var parser = new Parser(text);
            return parser.Parse();
        }
        public static IEnumerable<SyntaxToken> ParseTokens(string text)
        {
            var sourceText = SourceText.From(text);
            return ParseTokens(sourceText);
        }
        public static IEnumerable<SyntaxToken> ParseTokens(SourceText text)
        {
            var lexer = new Lexer(text);
            while (true)
            {
                var token = lexer.Lex();
                if (token.Kind == SyntaxKind.EndOfFileToken)
                    break;
                yield return token;
            }
        }
    }
}


