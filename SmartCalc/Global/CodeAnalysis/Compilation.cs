
using System;
using System.Collections.Generic;
using System.Linq;
using SmartCalc.Global.CodeAnalysis.Binding;
using SmartCalc.Global.CodeAnalysis.Syntax;

namespace SmartCalc.Global.CodeAnalysis
{
    public sealed class Compilation{
        public Compilation(SyntaxTree syntax)
        {
            Syntax = syntax;
        }
        public SyntaxTree Syntax { get; }
        public EvaluationResult Evaluate(Dictionary<string, object> variables){
            var binder = new Binder(variables);
            var boundExpression = binder.BindExpression(Syntax.Root);

            var diagnostics = Syntax.Diagnostics.Concat(binder.Diagnostics).ToArray();
            if(diagnostics.Any())
                return new EvaluationResult(diagnostics, null);

            var evaluator = new Evaluator(boundExpression, variables);
            var value = evaluator.Evaluate();
            return new EvaluationResult(Array.Empty<Diagnostic>(),value);
        }        
    }
}


