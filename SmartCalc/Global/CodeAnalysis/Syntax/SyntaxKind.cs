namespace SmartCalc.Global.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        // Tokens
        BadToken,
        EndOfFileToken,        
        WhiteSpaceToken,
        NumberToken,
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
        //ComaToken,        

        // Keywords
        FalseKeyword,
        TrueKeyword,

        // Expressions
        LiteralExpression,
        NameExpression,
        AssignmentExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression        
    }
}


