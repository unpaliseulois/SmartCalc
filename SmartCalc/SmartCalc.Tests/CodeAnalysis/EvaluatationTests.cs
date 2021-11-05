using System;
using System.Collections.Generic;
using SmartCalc.Global.CodeAnalysis;
using SmartCalc.Global.CodeAnalysis.Syntax;
using Xunit;

namespace SmartCalc.Tests.CodeAnalysis
{
    public class EvaluatationTests
    {
        [Theory]
        [InlineData("1", 1)]
        [InlineData("+1", 1)]
        [InlineData("-1", -1)]
        [InlineData("14 + 12 ", 26)]
        [InlineData("12 - 3", 9)]
        [InlineData("4 * 2", 8)]
        [InlineData("9 / 3", 3)]
        [InlineData("2 ** 8", 256)]
        [InlineData("2 ^ 7", 128)]
        [InlineData("(10)", 10)]

        [InlineData("12 == 3", false)]
        [InlineData("3 == 3", true)]

        [InlineData("12 != 3", true)]
        [InlineData("3 != 3", false)]

        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("!true", false)]
        [InlineData("!false", true)]

        [InlineData("false == false", true)]
        [InlineData("true == false", false)]
        [InlineData("false != false", false)]
        [InlineData("true != false", true)]

        [InlineData("(a=10) * a", 100)]        
        public void SyntaxFact_GetText_RoundTrips(string text, object expectedResult)
        {
            var syntaxTree = SyntaxTree.Parse(text);
            var compliation = new Compilation(syntaxTree);
            var variables = new Dictionary<VariableSymbol, object>();
            var result = compliation.Evaluate(variables);

            Assert.Empty(result.Diagnostics);
            Assert.Equal(expectedResult, result.Value);
        }
    }
}
