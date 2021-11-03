
using System;

namespace SmartCalc.Global.CodeAnalysis.Binding
{
    internal abstract class BoundExpression : BoundNode
    {
        public abstract Type Type { get; }

    }

}