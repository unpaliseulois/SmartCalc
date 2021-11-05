using System.Collections.Generic;


namespace SmartCalc.Global.CodeAnalysis.Syntax
{
    internal sealed class Lexer
    {
        private DiagnosticBag _diagnostics = new DiagnosticBag();
        private readonly string _text;
        private int _position;
        private int _start;
        public Lexer(string text)
        {
            _text = text;
        }
        public DiagnosticBag Diagnostics => _diagnostics;
        private char Current => Peek(0);
        //private char Lookahead => Peek(1);


        private char Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _text.Length)
                return '\0';
            return _text[index];
        }

        public SyntaxToken Lex()
        {
            _start = _position;

            if (_position >= _text.Length)
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);

            if (char.IsDigit(Current))
            {
                while (char.IsDigit(Current))
                    _position++;

                var length = _position - _start;
                var text = _text.Substring(_start, length);

                if (!int.TryParse(text, out var value))
                {
                    var span = new TextSpan(_start, length);
                    _diagnostics.ReportInvalidNumber(span, text, typeof(int));
                }
                return new SyntaxToken(SyntaxKind.NumberToken, _start, text, value);
            }

            if (char.IsWhiteSpace(Current))
            {
                while (char.IsWhiteSpace(Current))
                    _position++;

                var length = _position - _start;
                var text = _text.Substring(_start, length);
                int.TryParse(text, out var value);
                return new SyntaxToken(SyntaxKind.WhiteSpaceToken, _start, text, value);
            }

            // true - false
            if (char.IsLetter(Current))
            {
                while (char.IsLetter(Current))
                    _position++;

                var length = _position - _start;
                var text = _text.Substring(_start, length);
                var kind = SyntaxFacts.GetKeywordKind(text);
                return new SyntaxToken(kind, _start, text, null);
            }

            switch (Current)
            {
                case '+':
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.PlusToken, _start, "+", null);
                    }
                case '-':
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.MinusToken, _start, "-", null);
                    }
                case '^':
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.HatToken, _start, "^", null);
                    }
                case '*':
                    _position++;
                    if (Current == '*')
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.StarStarToken, _start, "**", null);
                    }
                    else
                    {
                        return new SyntaxToken(SyntaxKind.StarToken, _start, "*", null);
                    }
                case '/':
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.SlashToken, _start, "/", null);
                    }
                case '(':
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.OpenParenthsisToken, _start, "(", null);
                    }
                case ')':
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.CloseParenthsisToken, _start, ")", null);
                    }
                case '!':
                    _position++;
                    if (Current == '=')
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.BangEqualsToken, _start, "!=", null);
                    }
                    else
                    {
                        return new SyntaxToken(SyntaxKind.BangToken, _start, "!", null);
                    }
                case '&':
                    _position++;
                    if (Current == '&')
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.AmpersandAmpersandToken, _start, "&&", null);
                    }
                    else
                    {
                        return new SyntaxToken(SyntaxKind.AmpersandToken, _start, "&", null);
                    }

                case '|':
                    _position++;
                    if (Current == '|')
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.PipePipeToken, _start, "||", null);
                    }
                    else
                    {
                        return new SyntaxToken(SyntaxKind.PipeToken, _start, "|", null);
                    }
                case '=':
                    _position++;
                    if (Current == '=')
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.EqualsEqualsToken, _start, "==", null);
                    }
                    else
                    {
                        return new SyntaxToken(SyntaxKind.EqualsToken, _start, "=", null);
                    }
                default:
                    {
                        _position++;
                        _diagnostics.ReportBadCharacter(_position, Current);
                        return new SyntaxToken(SyntaxKind.BadToken, _start, _text.Substring(_position - 1, 1), null);
                    }
            }

        }
    }
}


