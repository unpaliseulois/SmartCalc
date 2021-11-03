using System.Collections;

namespace SmartCalc.Global.CodeAnalysis.Syntax
{
    public sealed class Diagnostic{
        public Diagnostic(TextSpan span, string message)
        {
            Span = span;
            Message = message;
        }

        public TextSpan Span { get; }
        public string Message { get; }

        public override string ToString() => Message;       
    }

}


