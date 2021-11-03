
using System;

namespace SmartCalc.Global.CodeAnalysis.Binding
{
    internal sealed class BoundVariableExpression : BoundExpression
    {
        public BoundVariableExpression(string name, Type type)
        {
            Name = name;
            Type = type;
        }
        public override BoundNodeKine Kind => BoundNodeKine.VariableExpression;
        public override Type Type { get; }
        public string Name { get; }
    }
}