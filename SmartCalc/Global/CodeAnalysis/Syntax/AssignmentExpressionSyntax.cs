namespace SmartCalc.Global.CodeAnalysis.Syntax
{
    public sealed class AssignmentExpressionSyntax : ExpressionSyntax
    {
        public AssignmentExpressionSyntax(SyntaxToken identifierToken, SyntaxToken eqaulsToken, ExpressionSyntax expression)
        {
            IdentifierToken = identifierToken;
            EqaulsToken = eqaulsToken;
            Expression = expression;
        }
        public override SyntaxKind Kind => SyntaxKind.AssignmentExpression;
        public SyntaxToken IdentifierToken { get; }
        public SyntaxToken EqaulsToken { get; }
        public ExpressionSyntax Expression { get; }
    }
}


