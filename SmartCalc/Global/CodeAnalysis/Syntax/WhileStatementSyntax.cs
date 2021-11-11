using SmartCalc.Global.CodeAnalysis.Syntax;

public sealed class WhileStatementSyntax:StatementSyntax
{
    public WhileStatementSyntax(SyntaxToken whileKeword, 
                                ExpressionSyntax condition, 
                                StatementSyntax body
                                )
    {
        WhileKeword = whileKeword;
        Condition = condition;
        Body = body;
    }

    public override SyntaxKind Kind => SyntaxKind.WhileStatement;

    public SyntaxToken WhileKeword { get; }
    public ExpressionSyntax Condition { get; }
    public StatementSyntax Body { get; }
}


