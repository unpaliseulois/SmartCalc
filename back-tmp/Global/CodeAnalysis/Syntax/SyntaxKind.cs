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
        AmpersandAmpersandToken,
        PipePipeToken,
        PipeToken,
        AmpersandToken,
        EqualsEqualsToken,
        EqualsToken,
        BangToken,
        BangEqualsToken,
        LessToken,
        LessOrEqualsToken,
        GreaterToken,
        GreaterOrEqualsToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        IdentifierToken,
        OpenBraceToken,
        CloseBraceToken,
        //ComaToken,        

        // Keywords        
        ElseKeyword,
        FalseKeyword,
        IfKeyword,
        LetKeyword,
        TrueKeyword,
        VarKeyword,

        // Nodes        
        CompilationUnit,
        ElseClause,

        // Statements
        BlockStatement,
        VariableDeclaration,
        IfStatement,
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


