namespace SmartCalc.Global.CodeAnalysis.Binding
{
    internal sealed class BoundExpressionStatement : BoundStatement
    {
        public BoundExpressionStatement(BoundExpression expression)
        {
            Expression = expression;
        }

        public override BoundNodeKine Kind => BoundNodeKine.ExpressionStatement;

        public BoundExpression Expression { get; }
    }
}