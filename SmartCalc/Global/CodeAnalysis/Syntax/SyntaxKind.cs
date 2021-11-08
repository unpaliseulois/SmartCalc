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
        OpenParenthesisToken,
        CloseParenthesisToken,
        IdentifierToken,
        //ComaToken,        

        // Keywords
        FalseKeyword,
        TrueKeyword,

        // Nodes
        
        CompilationUnit,

        // Expressions
        LiteralExpression,
        NameExpression,
        AssignmentExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression
    }
}


