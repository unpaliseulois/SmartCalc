
using System;

namespace SmartCalc.Global.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryExpression : BoundExpression
    {
        public BoundBinaryExpression(BoundExpression left, BoundBinaryOperatorKind operatorKind, BoundExpression right)
        {
            Left = left;
            OperatorKind = operatorKind;
            Right = right;
        }

        public override BoundNodeKine Kind => BoundNodeKine.BinaryExpression;
        public override Type Type => Left.Type;

        public BoundExpression Left { get; }
        public BoundBinaryOperatorKind OperatorKind { get; }
        public BoundExpression Right { get; }

    }

}