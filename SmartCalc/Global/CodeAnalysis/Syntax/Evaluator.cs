
using System;


namespace SmartCalc.Global.CodeAnalysis.Syntax
{
    public sealed class Evaluator
    {
        private ExpressionSyntax _root;
        public Evaluator(ExpressionSyntax root)
        {
            _root = root;
        }
        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(ExpressionSyntax node)
        {
            // BinaryExpression
            // NumberExpression
            if (node is LiteralExpressionSyntax n)
                return (int)n.LiteralToken.Value;
            if (node is UnaryExpressionSyntax u)
            {
                var operand = EvaluateExpression(u.Operand);
                switch (u.OperatorToken.Kind)
                {
                    case SyntaxKind.PlusToken:
                        return operand;
                    case SyntaxKind.MinusToken:
                        return -operand;
                    default:
                        throw new Exception($"Unexpected unary operator '{u.OperatorToken.Kind}'.");
                }
            }
            if (node is BinaryExpressionSyntax b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                switch (b.OperatorToken.Kind)
                {
                    case SyntaxKind.PlusToken:
                        return left + right;
                    case SyntaxKind.MinusToken:
                        return left - right;
                    case SyntaxKind.StarToken:
                        return left * right;
                    case SyntaxKind.SlashToken:
                        return left / right;
                    default:
                        throw new Exception($"Unexpected binary operator '{b.OperatorToken.Kind}'.");
                }
            }
            if (node is ParenthesizedExpressionSyntax p)
                return EvaluateExpression(p.Expression);

            throw new Exception($"Unexpected node '{node.Kind}'.");

        }
    }
}


