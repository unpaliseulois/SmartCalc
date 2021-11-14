using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SmartCalc.Global.CodeAnalysis.Text;
using SmartCalc.Global.Compilation;
using static System.Console;
using static System.ConsoleColor;
using static SmartCalc.Global.Common.Global;

namespace SmartCalc.Global.CodeAnalysis.Binding
{
    internal abstract class BoundNode
    {
        private static List<ConsoleColor> _colors = new List<ConsoleColor>();
        public abstract BoundNodeKind Kind { get; }


        //////////////
        public IEnumerable<BoundNode> GetChildren()
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (typeof(BoundNode).IsAssignableFrom(property.PropertyType))
                {
                    var child = (BoundNode)property.GetValue(this);
                    if (child != null)
                        yield return child;
                }
                else if (typeof(IEnumerable<BoundNode>).IsAssignableFrom(property.PropertyType))
                {
                    var children = (IEnumerable<BoundNode>)property.GetValue(this);
                    foreach (var child in children)
                    {
                        if (child != null)
                            yield return child;
                    }
                }
            }
        }
        public IEnumerable<(string Name, object Value)> GetProperties()
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.Name == nameof(Kind) ||
                    property.Name == nameof(BoundBinaryExpression.Op))
                    continue;
                if (typeof(BoundNode).IsAssignableFrom(property.PropertyType) ||
                    typeof(IEnumerable<BoundNode>).IsAssignableFrom(property.PropertyType))
                    continue;

                var value = property.GetValue(this);
                if (value != null)
                    yield return (property.Name, value);

            }
        }

        public void WriteTo(TextWriter writer)
        {
            PrettyPrint(writer, this);
        }
        private static void PrettyPrint(TextWriter writer, BoundNode node, string indent = "", bool isLast = true)
        {
            var isToConsole = writer == Out;
            var marker = isLast ? "└──" : "├──";

            if (isToConsole)
                ForegroundColor = DarkGray;

            writer.Write(indent);
            writer.Write(marker);

            if (isToConsole)
                ForegroundColor = GetColor();

            var text = GetText(node);
            writer.Write(text);

            var isFirstProperty = true;

            foreach (var p in node.GetProperties())
            {
                if (isFirstProperty)
                    isFirstProperty = false;
                else
                {
                    if (isToConsole)
                        ForegroundColor = DarkGray;
                    writer.Write(",");
                }
                writer.Write(" ");
                if (isToConsole)
                    ForegroundColor = DarkYellow;
                writer.Write(p.Name);
                if (isToConsole)
                    ForegroundColor = DarkGray;
                writer.Write(" = ");
                if (isToConsole)
                    ForegroundColor = DarkYellow;
                if(p.Value.ToString().Contains("System"))
                {
                    var value = p.Value.ToString().Substring(7);
                    writer.Write(value);
                }
                else
                    writer.Write(p.Value);
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
        private static string GetText(BoundNode node)
        {
            if (node is BoundBinaryExpression b)
                return b.Op.Kind.ToString() + "Expression";
            if (node is BoundUnaryExpression u)
                return u.Op.Kind.ToString() + "Expression";
            else
                return node.Kind.ToString();
        }

        private static ConsoleColor GetColor()
        {
            var randomColor = GetRandColor();
            while (_colors.Contains(randomColor))
            {
                randomColor = GetRandColor();
                _colors.Add(randomColor);
            }
            if (_colors.Count == 16)
                _colors.Clear();

            return randomColor;
        }

        public override string ToString()
        {
            using (var writer = new StringWriter())
            {
                WriteTo(writer);
                return writer.ToString();
            }
        }

        //////////////

    }

}