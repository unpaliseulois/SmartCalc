namespace SmartCalc.Global.CodeAnalysis
{
    public enum SyntaxKind
    {
        // Tokens
        BadToken,
        EndOfFileToken,
        NumberToken,
        WhiteSpaceToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenParenthsisToken,
        CloseParenthsisToken,

        // Expressions
        LiteralExpression,
        BinaryExpression,
        ParenthesizedExpression        
    }
}


