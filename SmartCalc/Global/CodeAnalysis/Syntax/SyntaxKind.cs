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
        BangEqualsToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        IdentifierToken,
        OpenPraceToken,
        ClosePraceToken,
        //ComaToken,        

        // Keywords
        FalseKeyword,
        LetKeyword,
        TrueKeyword,
        VarKeyword,

        // Nodes        
        CompilationUnit,

        // Statements
        BlockStatement,        
        VariableDeclaration,
        ExpressionStatement,

        // Expressions
        LiteralExpression,
        NameExpression,
        AssignmentExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression
    }
}


