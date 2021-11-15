namespace SmartCalc.Global.CodeAnalysis.Binding
{
    internal enum BoundNodeKind
    {
        // Statements       
        BlockStatement,
        VariableDeclaration,
        IfStatement,
        WhileStatement,
        ForStatement,        
        GotoStatement,
        LabelStatement,
        ConditionalGotoStatement,
        ExpressionStatement,
        
        //Expressions        
        LiteralExpression,
        VariableExpression,
        AssignmentExpression,
        UnaryExpression,
        BinaryExpression,
    }
}