using System;
using System.Collections;
using System.Collections.Generic;
using SmartCalc.Global.CodeAnalysis.Syntax;
using SmartCalc.Global.CodeAnalysis.Text;

namespace SmartCalc.Global.Compilation
{
    internal sealed class DiagnosticBag : IEnumerable<Diagnostic>
    {
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public void AddRange(DiagnosticBag diagnostics)
        {
            _diagnostics.AddRange(diagnostics._diagnostics);

        }
        public void Report(TextSpan span, string message)
        {
            var diagnostic = new Diagnostic(span, message);
            _diagnostics.Add(diagnostic);
        }
        public void ReportInvalidNumber(TextSpan span, string operatorText, Type type)
        {
            var _type = type.ToString().Split('.')[1];
            var message = $"The number '{operatorText}' isn't a valid '{_type}'.";
            Report(span, message);
        }
        public void ReportBadCharacter(int position, char character)
        {
            var span = new TextSpan(position, 1);
            var message = $"Bad character input: '{character}'.";
            Report(span, message);
        }

        public void ReportUnexpectedToken(TextSpan span, SyntaxKind actualKind, SyntaxKind expectedKind)
        {
            var message = $"Unexpected token '{actualKind}', expected '{expectedKind}'.";
            Report(span, message);
        }

        public void RepoetUndifinedUnaryOperator(TextSpan span, string operatorText, Type operandType)
        {
            var _type = operandType.ToString().Split('.')[1];
            var message = $"Unary operator '{operatorText}' is not defined for type '{_type}'.";
            Report(span, message);
        }

        public void RepoetUndifinedBinaryOperator(TextSpan span, string operatorText, Type leftType, Type rightType)
        {
            var _leftType = leftType.ToString().Split('.')[1];
            var _rightType = rightType.ToString().Split('.')[1];

            var message = $"Binary operator '{operatorText}' is not defined for types '{_leftType}' and '{_rightType}'.";
            Report(span, message);

        }

        public void ReportUndefinedName(TextSpan span, string name)
        {
            var message = $"Variable '{name}' doesn't exist.";
            Report(span, message);
        }

        public void ReportDivisionByZero(TextSpan span, string message)
        {
            Report(span, message);
        }        

        public void ReportCannotConvert(TextSpan span, Type fromType, Type toType)
        {
            var fType = fromType.ToString().Split('.')[1];
            var tType = toType.ToString().Split('.')[1];
            var message = $"Cannot convert '{fType}' type to '{tType}' type.";
            Report(span, message);

        }
    }
}


