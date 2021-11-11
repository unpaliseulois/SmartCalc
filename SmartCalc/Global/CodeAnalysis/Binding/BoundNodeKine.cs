namespace SmartCalc.Global.CodeAnalysis.Binding
{
    internal enum BoundNodeKine
    {
        // Statements       
        ExpressionStatement,
        BlockStatement,
        IfStatement,
        WhileStatement,
        VariableDeclaration,
        //Expressions        
        LiteralExpression,
        VariableExpression,
        AssignmentExpression,
        UnaryExpression,
        BinaryExpression        
    }
}