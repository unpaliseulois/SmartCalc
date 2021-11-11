using System.Collections.Immutable;

namespace SmartCalc.Global.CodeAnalysis.Binding
{
    internal sealed class BoundBlockStatement : BoundStatement
    {
        public BoundBlockStatement(ImmutableArray<BoundStatement> statements)
        {
            Statements = statements;
        }

        public override BoundNodeKine Kind => BoundNodeKine.BlockStatement;

        public ImmutableArray<BoundStatement> Statements { get; }
    }
}