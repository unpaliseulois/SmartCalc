using System;

namespace SmartCalc.Global.CodeAnalysis.Syntax
{
    internal static class SyntaxFacts{
        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {                
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 4;                
                default:
                    return 0;
            }
        }
        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.HatToken:
                case SyntaxKind.StarStarToken:
                    return 3;
                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                    return 2;
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 1;                
                default:
                    return 0;
            }
        }
        
        public static SyntaxKind GetKeywordKind(string text)
        {
            switch (text)
            {
                case "false":
                return SyntaxKind.FalseKeyword;
                case "true":
                return SyntaxKind.TrueKeyword;
                default:
                    return SyntaxKind.IdentifierToken;
            }
        }
    }
}


