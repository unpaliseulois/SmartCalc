namespace SmartCalc.Global.CodeAnalysis.Binding
{
    internal enum BoundNodeKine
    {
        // Statements        
        BlockStatement,
        VariableDeclaration,
        ExpressionStatement,
        //Expressions        
        LiteralExpression,
        VariableExpression,
        AssignmentExpression,
        UnaryExpression,
        BinaryExpression
    }

}