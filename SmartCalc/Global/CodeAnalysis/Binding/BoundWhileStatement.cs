namespace SmartCalc.Global.CodeAnalysis.Binding
{
    internal sealed class BoundWhileStatement:BoundStatement
    {
        public BoundWhileStatement(BoundExpression condition,
                                   BoundStatement body)
        {
            Condition = condition;
            Body = body;
        }
        public override BoundNodeKine Kind => BoundNodeKine.WhileStatement;
        public BoundExpression Condition { get; }
        public BoundStatement Body { get; }
    }
}