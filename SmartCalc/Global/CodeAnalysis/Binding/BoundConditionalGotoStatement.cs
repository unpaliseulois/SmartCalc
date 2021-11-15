using SmartCalc.Global.Compilation;

namespace SmartCalc.Global.CodeAnalysis.Binding
{
    internal sealed class BoundConditionalGotoStatement : BoundStatement
    {
        public BoundConditionalGotoStatement(LabelSymbol label, BoundExpression condition, bool jumpIfTrue = false)
        {
            Label = label;
            Condition = condition;
            JumpIfTrue = jumpIfTrue;
        }
        public override BoundNodeKind Kind => BoundNodeKind.ConditionalGotoStatement;
        public LabelSymbol Label { get; }
        public BoundExpression Condition { get; }
        public bool JumpIfTrue { get; }
    }
}