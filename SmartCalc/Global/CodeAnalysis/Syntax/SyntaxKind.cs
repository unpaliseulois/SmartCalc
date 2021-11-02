namespace SmartCalc.Global.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        // Tokens
        BadToken,
        EndOfFileToken,
        NumberToken,
        WhiteSpaceToken,
        StarStarToken,
        StarToken,
        SlashToken,
        PlusToken,
        MinusToken,        
        OpenParenthsisToken,
        CloseParenthsisToken,

        // Expressions
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression,
        HatToken
    }
}


