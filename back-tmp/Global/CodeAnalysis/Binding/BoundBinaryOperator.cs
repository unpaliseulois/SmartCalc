
using System;
using SmartCalc.Global.CodeAnalysis.Syntax;

namespace SmartCalc.Global.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryOperator
    {
        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type type)
         : this(syntaxKind, kind, type, type, type)
        {
        }
        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type operandType, Type type)
         : this(syntaxKind, kind, operandType, operandType, type)
        {
        }
        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type leftType, Type rightType, Type type)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            LeftType = leftType;
            RightType = rightType;
            Type = type;
        }

        public SyntaxKind SyntaxKind { get; }
        public BoundBinaryOperatorKind Kind { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type Type { get; }

        private static BoundBinaryOperator[] _operators =
        {
            // '^' | '**' 
            new BoundBinaryOperator(SyntaxKind.HatToken,
                BoundBinaryOperatorKind.Power,typeof(int)),
            new BoundBinaryOperator(SyntaxKind.StarStarToken,
                BoundBinaryOperatorKind.Power,typeof(int)),
            // '*' | '/' | '+' | '-'
            new BoundBinaryOperator(SyntaxKind.StarToken,
                BoundBinaryOperatorKind.Multiplication,typeof(int)),
            new BoundBinaryOperator(SyntaxKind.SlashToken,
                BoundBinaryOperatorKind.Division,typeof(int)),
            new BoundBinaryOperator(SyntaxKind.PlusToken,
                BoundBinaryOperatorKind.Addition,typeof(int)),
            new BoundBinaryOperator(SyntaxKind.MinusToken,
                BoundBinaryOperatorKind.Substraction,typeof(int)),
            // '==' | '!='
            new BoundBinaryOperator(SyntaxKind.EqualsEqualsToken,
                BoundBinaryOperatorKind.Equals,typeof(int),typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.BangEqualsToken,
                BoundBinaryOperatorKind.NotEquals,typeof(int),typeof(bool)),
            // '<' | '<=' | '>' | '>='
            new BoundBinaryOperator(SyntaxKind.LessToken,
                BoundBinaryOperatorKind.Less,typeof(int),typeof(bool)),

            new BoundBinaryOperator(SyntaxKind.LessOrEqualsToken,
                BoundBinaryOperatorKind.LessOrEquals,typeof(int),typeof(bool)),

            new BoundBinaryOperator(SyntaxKind.GreaterToken,
                BoundBinaryOperatorKind.Greater,typeof(int),typeof(bool)),

            new BoundBinaryOperator(SyntaxKind.GreaterOrEqualsToken,
                BoundBinaryOperatorKind.GreaterOrEquals,typeof(int),typeof(bool)),
            // & | '|' | '&&' | '||' | '==' | '!='
            new BoundBinaryOperator(SyntaxKind.AmpersandToken,
                BoundBinaryOperatorKind.LogicalAnd,typeof(bool)),

            new BoundBinaryOperator(SyntaxKind.PipeToken,
                BoundBinaryOperatorKind.LogicalOr,typeof(bool)),

            new BoundBinaryOperator(SyntaxKind.AmpersandAmpersandToken,
                BoundBinaryOperatorKind.LogicalAnd,typeof(bool)),

            new BoundBinaryOperator(SyntaxKind.PipePipeToken,
                BoundBinaryOperatorKind.LogicalOr,typeof(bool)),

            new BoundBinaryOperator(SyntaxKind.EqualsEqualsToken,
                BoundBinaryOperatorKind.Equals,typeof(bool)),

            new BoundBinaryOperator(SyntaxKind.BangEqualsToken,
                BoundBinaryOperatorKind.NotEquals,typeof(bool)),
        };
        
        public static BoundBinaryOperator Bind(SyntaxKind syntaxKind, Type leftType, Type rightType)
        {
            foreach (var op in _operators)
            {
                if (op.SyntaxKind == syntaxKind && op.LeftType == leftType && op.RightType == rightType)
                    return op;
            }
            return null;
        }
    }

}