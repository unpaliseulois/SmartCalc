
using System;

namespace SmartCalc.Global.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryExpression : BoundExpression
    {
        public BoundUnaryExpression(BoundUnaryOperator op, BoundExpression operand)
        {
            Op = op;
            Operand = operand;
        }

        public override BoundNodeKine Kind => BoundNodeKine.UnaryExpression;
        public override Type Type => Operand.Type;
        public BoundUnaryOperator Op { get; }
        public BoundExpression Operand { get; }

    }

}