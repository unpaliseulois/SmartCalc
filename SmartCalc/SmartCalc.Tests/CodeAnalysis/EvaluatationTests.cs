using System;
using System.Collections.Generic;
using SmartCalc.Global.CodeAnalysis.Syntax;
using SmartCalc.Global.Compilation;
using Xunit;

namespace SmartCalc.Tests.CodeAnalysis
{
    public class EvaluatationTests
    {
        [Theory]
        [InlineData("1", 1)]
        [InlineData("+1", 1)]
        [InlineData("-1", -1)]

        [InlineData("~-1", 0)]

        [InlineData("14 + 12 ", 26)]
        [InlineData("12 - 3", 9)]
        [InlineData("4 * 2", 8)]
        [InlineData("9 / 3", 3)]
        [InlineData("2 ** 8", 256)]
        [InlineData("(10)", 10)]

        [InlineData("12 == 3", false)]
        [InlineData("3 == 3", true)]

        [InlineData("12 != 3", true)]
        [InlineData("3 != 3", false)]

        [InlineData("3 < 4", true)]
        [InlineData("5 < 4", false)]
        [InlineData("4 <= 4", true)]
        [InlineData("4 <= 5", true)]
        [InlineData("5 <= 4", false)]

        [InlineData("4 > 3", true)]
        [InlineData("4 > 5", false)]
        [InlineData("4 >= 4", true)]
        [InlineData("5 >= 5", true)]
        [InlineData("4 >= 5", false)]

        [InlineData("1 | 2", 3)]
        [InlineData("1 | 0", 1)]
        [InlineData("1 & 3", 1)]
        [InlineData("1 & 0", 0)]
        [InlineData("1 ^ 0", 1)]
        [InlineData("0 ^ 1", 1)]
        [InlineData("1 ^ 3", 2)]

        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("!true", false)]
        [InlineData("!false", true)]

        [InlineData("false == false", true)]
        [InlineData("true == false", false)]
        [InlineData("false != false", false)]
        [InlineData("true != false", true)]        

        [InlineData("true && false", false)]
        [InlineData("true && true", true)]        

        [InlineData("true || false", true)]
        [InlineData("false || false", false)]

        [InlineData("false | false", false)]
        [InlineData("false | true", true)]
        [InlineData("true | false", true)]
        [InlineData("true | true", true)]

        [InlineData("false & false", false)]
        [InlineData("false & true", false)]
        [InlineData("true & false", false)]
        [InlineData("true & true", true)]

        [InlineData("false ^ false", false)]
        [InlineData("false ^ true", true)]
        [InlineData("true ^ false", true)]
        [InlineData("true ^ true", false)]

        [InlineData("{ var a = 0 ( a = 10 ) * a }", 100)]

        [InlineData("{ var a = 0 if a == 0 a = 10 a }", 10)]
        [InlineData("{ var a = 0 if a == 4 a = 10 a }", 0)]

        [InlineData("{ var a = 0 if a == 0 a = 10 else a = 5 a }", 10)]
        [InlineData("{ var a = 0 if a == 4 a = 10 else a = 5 a }", 5)]

        [InlineData("{ var i = 10 var result = 0 while i > 0 { result = result + i i = i - 1 } result}", 55)]
        [InlineData("{ var result = 0 for i = 1 to 10 { result = result + i} result }", 55)]
        [InlineData("{ var a = 10 for i = 1 to (a = a - 1) { } a }", 9)]

        public void Evaluator_Computes_CorrectValues(string text, object expectedValue)
        {
            AssertValue(text, expectedValue);
        }
        [Fact]
        private void Evaluator_VariableDeclaration_Reports_Redeclaration()
        {
            var text = @"
                {
                    var x = 10
                    var y = 100
                    {
                        var x = 10
                    }
                    var [x] = 5
                }
            ";

            var diagnostics = @"
                Variable 'x' is already declared.
            ";

            AssertDiagnostics(text, diagnostics);
        }
        [Fact]
        private void Evaluator_NameExpression_Reports_Undifined()
        {
            var text = @"[x] * 10";
            var diagnostics = @"
                Variable 'x' doesn't exist.
            ";

            AssertDiagnostics(text, diagnostics);
        }
        [Fact]
        private void Evaluator_NameExpression_Reports_NoErrorForInsertedToken()
        {
            var text = @"[]";
            var diagnostics = @"
                Unexpected token 'EndOfFileToken', expected 'IdentifierToken'.
            ";

            AssertDiagnostics(text, diagnostics);
        }
        [Fact]
        private void Evaluator_UnaryExpression_Reports_Undefined()
        {
            var text = @"[+]true";


            var diagnostics = @"
                Unary operator '+' is not defined for type 'Boolean'.
            ";

            AssertDiagnostics(text, diagnostics);
        }
        [Fact]
        private void Evaluator_BinaryExpression_Reports_Undefined()
        {
            var text = @"true [*] true";


            var diagnostics = @"
                Binary operator '*' is not defined for types 'Boolean' and 'Boolean'.
            ";

            AssertDiagnostics(text, diagnostics);
        }
        [Fact]
        private void Evaluator_AssignmentExpression_Reports_Undifined()
        {
            var text = @"[x] = 10";
            var diagnostics = @"
                Variable 'x' doesn't exist.
            ";

            AssertDiagnostics(text, diagnostics);
        }
        [Fact]
        private void Evaluator_AssignmentExpression_Reports_CannotAssign()
        {
            var text = @"
                {
                    let x = 10
                    x [=] 0
                }
            ";
            var diagnostics = @"
                Variable 'x' is read-only and cannot be assigned to.
            ";

            AssertDiagnostics(text, diagnostics);
        }
        [Fact]
        private void Evaluator_AssignmentExpression_Reports_CannotConvert()
        {
            var text = @"
                {
                    var x = 10
                    x =  [true]
                }
            ";

            var diagnostics = @"
                Cannot convert 'Boolean' type to 'Int32' type.
            ";

            AssertDiagnostics(text, diagnostics);
        }
        [Fact]
        private void Evaluator_BlockStatement_NoInfinitLoop()
        {
            var text = @"{
                [)][]
            ";

            var diagnostics = @"
                Unexpected token 'CloseParenthesisToken', expected 'IdentifierToken'.
                Unexpected token 'EndOfFileToken', expected 'CloseBraceToken'.
            ";

            AssertDiagnostics(text, diagnostics);
        }
        [Fact]
        private void Evaluator_IfStatement_Reports_CannotConvert()
        {
            var text = @"
                {
                    var x = 0
                    if [10]
                        x = 10
                }
            ";

            var diagnostics = @"
                Cannot convert 'Int32' type to 'Boolean' type.
            ";

            AssertDiagnostics(text, diagnostics);
        }
        [Fact]
        private void Evaluator_WhileStatement_Reports_CannotConvert()
        {
            var text = @"
                {
                    var x = 0
                    while [10]
                        x = 10
                }
            ";

            var diagnostics = @"
                Cannot convert 'Int32' type to 'Boolean' type.
            ";

            AssertDiagnostics(text, diagnostics);
        }
        [Fact]
        private void Evaluator_ForStatement_Reports_CannotConvert_LowerBound()
        {
            var text = @"
                {
                    var result = 0
                    for i = [false] to 10
                        result = result + i
                }
            ";

            var diagnostics = @"
                Cannot convert 'Boolean' type to 'Int32' type.
            ";

            AssertDiagnostics(text, diagnostics);
        }
        private void Evaluator_ForStatement_Reports_CannotConvert_UpperBound()
        {
            var text = @"
                {
                    var result = 0
                    for i = 0 to [true]
                        result = result + i
                }
            ";

            var diagnostics = @"
                Cannot convert 'Boolean' type to 'Int32' type.
            ";

            AssertDiagnostics(text, diagnostics);
        }
        private static void AssertValue(string text, object expectedValue)
        {
            var syntaxTree = SyntaxTree.Parse(text);
            var compliation = new Compilation(syntaxTree);
            var variables = new Dictionary<VariableSymbol, object>();
            var result = compliation.Evaluate(variables);

            Assert.Empty(result.Diagnostics);
            Assert.Equal(expectedValue, result.Value);
        }
        private void AssertDiagnostics(string text, string diagnosticText)
        {
            var annotadedText = AnnotadedText.Parse(text);
            var syntaxTree = SyntaxTree.Parse(annotadedText.Text);
            var compilation = new Compilation(syntaxTree);
            var result = compilation.Evaluate(new Dictionary<VariableSymbol, object>());

            var expectedDiagnostics = AnnotadedText.UnindentLines(diagnosticText);

            if (annotadedText.Spans.Length != expectedDiagnostics.Length)
                throw new Exception("ERROR: Must mark as many spans as there are expected diagnostics.");

            Assert.Equal(expectedDiagnostics.Length, result.Diagnostics.Length);

            for (int i = 0; i < expectedDiagnostics.Length; i++)
            {
                var expectedMessage = expectedDiagnostics[i];
                var actualMessage = result.Diagnostics[i].Message;
                Assert.Equal(expectedMessage, actualMessage);

                var expectedSpan = annotadedText.Spans[i];
                var actualSpan = result.Diagnostics[i].Span;
                Assert.Equal(expectedSpan, actualSpan);
            }
        }
    }
}