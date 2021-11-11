using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SmartCalc.Global.CodeAnalysis.Text;
using static System.Console;
using static System.ConsoleColor;

namespace SmartCalc.Global.CodeAnalysis.Syntax
{
    public abstract class SyntaxNode
    {
        public abstract SyntaxKind Kind { get; }
        public virtual TextSpan Span
        {
            get
            {
                var first = GetChildren().First().Span;
                var last = GetChildren().Last().Span;

                return TextSpan.FromBounds(first.Start, last.End);
            }
        }
        public IEnumerable<SyntaxNode> GetChildren()
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (typeof(SyntaxNode).IsAssignableFrom(property.PropertyType))
                {
                    var child = (SyntaxNode)property.GetValue(this);
                    if(child != null)
                        yield return child;
                }
                else if (typeof(IEnumerable<SyntaxNode>).IsAssignableFrom(property.PropertyType))
                {
                    var children = (IEnumerable<SyntaxNode>)property.GetValue(this);
                    foreach (var child in children)
                        if(child != null)
                            yield return child;
                }
            }
        }
        public void WriteTo(TextWriter writer)
        {
            PrettyPrint(writer, this);
        }
        private static void PrettyPrint(TextWriter writer, SyntaxNode node, string indent = "", bool isLast = true)
        {
            var isToConsole = writer == Out;
            var marker = isLast ? "└──" : "├──";

            writer.Write(indent);

            if (isToConsole)
                ForegroundColor = DarkGray;
            writer.Write(marker);


            if (isToConsole)
            {
                switch (node.Kind)
                {
                    case SyntaxKind.BadToken:
                        ForegroundColor = DarkRed;
                        break;
                    case SyntaxKind.ParenthesizedExpression:
                        ForegroundColor = DarkMagenta;
                        break;
                    case SyntaxKind.OpenParenthesisToken:
                    case SyntaxKind.CloseParenthesisToken:
                        ForegroundColor = Magenta;
                        break;
                    case SyntaxKind.PlusToken:
                    case SyntaxKind.MinusToken:
                    case SyntaxKind.StarToken:
                    case SyntaxKind.SlashToken:
                    case SyntaxKind.StarStarToken:
                    case SyntaxKind.HatToken:
                        ForegroundColor = DarkYellow;
                        break;
                    case SyntaxKind.IdentifierToken:
                        ForegroundColor = DarkGreen;
                        break;
                    case SyntaxKind.AmpersandToken:
                    case SyntaxKind.AmpersandAmpersandToken:
                    case SyntaxKind.EqualsToken:
                    case SyntaxKind.EqualsEqualsToken:
                    case SyntaxKind.PipeToken:
                    case SyntaxKind.PipePipeToken:
                    case SyntaxKind.BangEqualsToken:
                        ForegroundColor = Yellow;
                        break;
                    case SyntaxKind.NumberToken:
                        ForegroundColor = DarkBlue;
                        break;
                    case SyntaxKind.AssignmentExpression:
                    case SyntaxKind.LiteralExpression:
                    case SyntaxKind.UnaryExpression:
                    case SyntaxKind.BinaryExpression:
                    case SyntaxKind.NameExpression:
                        ForegroundColor = Blue;
                        break;
                    default:
                        ResetColor();
                        break;
                }

            }


            writer.Write(node.Kind);

            if (node is SyntaxToken t && t.Value != null)
            {
                writer.Write(" ");
                writer.Write(t.Value);
            }

            if (isToConsole)
                ResetColor();
            writer.WriteLine();

            indent += isLast ? "   " : "│  ";


            var lastChild = node.GetChildren().LastOrDefault();
            foreach (var child in node.GetChildren())
                PrettyPrint(writer, child, indent, child == lastChild);
            //ResetColor();
        }
        public override string ToString()
        {
            using (var writer = new StringWriter())
            {
                WriteTo(writer);
                return writer.ToString();
            }
        }
    }
}


