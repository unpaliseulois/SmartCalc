namespace SmartCalc.Global.CodeAnalysis.Binding
{
    internal enum BoundNodeKine
    {
        // Statements        
        BlockStatement,
        ExpressionStatement,

        //Expressions        
        LiteralExpression,
        VariableExpression,
        AssignmentExpression,
        UnaryExpression,
        BinaryExpression,
    }

}