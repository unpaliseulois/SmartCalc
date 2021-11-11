
using System;

namespace SmartCalc.Global.CodeAnalysis.Binding
{
    internal sealed class BoundLiteralExpression : BoundExpression
    {
        public BoundLiteralExpression(object value)
        {
            Value = value;
        }

        public override BoundNodeKine Kind => BoundNodeKine.LiteralExpression;
        public override Type Type => Value.GetType();
        public object Value { get; }
    }

}