
using System;
using System.Collections.Generic;
using System.Linq;
using SmartCalc.Global.CodeAnalysis.Binding;
using SmartCalc.Global.CodeAnalysis.Syntax;
using System.Collections.Immutable;


namespace SmartCalc.Global.Compilation
{
    public sealed class Compilation{
        public Compilation(SyntaxTree syntax)
        {
            Syntax = syntax;
        }
        public SyntaxTree Syntax { get; }
        public EvaluationResult Evaluate(Dictionary<VariableSymbol, object> variables){
            var binder = new Binder(variables);
            var boundExpression = binder.BindExpression(Syntax.Root);
            //var evaluator = new Evaluator(boundExpression,variables);
            //evaluator.Evaluate();

            var diagnostics = Syntax.Diagnostics.Concat(binder.Diagnostics).ToImmutableArray();
            //var diagnostics = Syntax.Diagnostics.Concat(binder.Diagnostics).Concat(evaluator.Diagnostics).ToArray();
            if(diagnostics.Any())
                return new EvaluationResult(diagnostics.ToImmutableArray(), null);

            var evaluator = new Evaluator(boundExpression, variables);
            var value = evaluator.Evaluate();
            return new EvaluationResult(ImmutableArray<Diagnostic>.Empty,value);
        }        
    }
}


