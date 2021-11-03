namespace SmartCalc.Global.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        // Tokens
        BadToken,
        EndOfFileToken,
        NumberToken,
        WhiteSpaceToken,
        HatToken,
        StarStarToken,
        StarToken,
        SlashToken,
        PlusToken,
        MinusToken,
        BangToken,
        AmpersandAmpersandToken,
        PipePipeToken,
        PipeToken,
        AmpersandToken,
        EqualsEqualsToken,
        EqualsToken,
        BangEqualsToken   ,     
        OpenParenthsisToken,
        CloseParenthsisToken,
        IdentifierToken,        

        // Keywords
        FalseKeyword,
        TrueKeyword,

        // Expressions
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression        
    }
}


