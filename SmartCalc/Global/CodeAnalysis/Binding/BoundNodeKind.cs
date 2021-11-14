namespace SmartCalc.Global.CodeAnalysis.Binding
{
    internal enum BoundNodeKind
    {
        // Statements       
        ExpressionStatement,
        BlockStatement,
        IfStatement,
        WhileStatement,
        ForStatement,
        VariableDeclaration,
        //Expressions        
        LiteralExpression,
        VariableExpression,
        AssignmentExpression,
        UnaryExpression,
        BinaryExpression
    }
}