using System.Collections.Generic;


namespace SmartCalc.Global.CodeAnalysis.Syntax
{
    internal sealed class Lexer
    {
        private readonly string _text;
        private int _position;
        private List<string> _diagnostics = new List<string>();
        public Lexer(string text)
        {
            _text = text;
        }
        public IEnumerable<string> Diagnostics => _diagnostics;
        private char Current => Peek(0);
        private char Lookahead => Peek(1);


        private char Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _text.Length)
                return '\0';
            return _text[index];
        }

        private void Next()
        {
            _position++;
        }

        public SyntaxToken Lex()
        {
            // <numbers>
            // + - * ( ) ^ ** .

            var start = _position;

            if (_position >= _text.Length)
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);

            if (char.IsDigit(Current))
            {
                while (char.IsDigit(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);

                if (!int.TryParse(text, out var value))
                    _diagnostics.Add($"The number {_text} isn't a valid Int32.");
                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            if (char.IsWhiteSpace(Current))
            {
                while (char.IsWhiteSpace(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                int.TryParse(text, out var value);
                return new SyntaxToken(SyntaxKind.WhiteSpaceToken, start, text, value);
            }

            // true - false
            if (char.IsLetter(Current))
            {
                while (char.IsLetter(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                var kind = SyntaxFacts.GetKeywordKind(text);
                return new SyntaxToken(kind, start, text, null);
            }

            switch (Current)
            {
                case '+':
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.PlusToken, start, "+", null);
                    }
                case '-':
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.MinusToken, start, "-", null);
                    }
                case '^':
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.HatToken, start, "^", null);
                    }
                case '*':
                    if (Lookahead == '*')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.StarStarToken, start, "**", null);
                    }
                    else
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.StarToken, start, "*", null);
                    }
                case '/':
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.SlashToken, start, "/", null);
                    }
                case '(':
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.OpenParenthsisToken, start, "(", null);
                    }
                case ')':
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.CloseParenthsisToken, start, ")", null);
                    }
                case '!':
                    if (Lookahead == '=')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.BangEqualsToken, start, "!=", null);
                    }
                    else
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.BangToken, start, "!", null);
                    }
                case '&':
                    if (Lookahead == '&')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.AmpersandAmpersandToken, start, "&&", null);
                    }
                    else

                    {
                        _position += 1;
                        return new SyntaxToken(SyntaxKind.AmpersandToken, start, "&", null);
                    }

                case '|':
                    if (Lookahead == '|')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.PipePipeToken, start, "||", null);
                    }
                    else
                    {
                        _position += 1;
                        return new SyntaxToken(SyntaxKind.PipeToken, start, "|", null);
                    }
                case '=':
                    if (Lookahead == '=')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.EqualsEqualsToken, start, "==", null);
                    }
                    else
                    {
                        _position += 1;
                        return new SyntaxToken(SyntaxKind.EqualsToken, start, "=", null);
                    }
                default:
                    {
                        _position++;
                        _diagnostics.Add($"ERROR: bad character input: '{Current}'.");
                        return new SyntaxToken(SyntaxKind.BadToken, start, _text.Substring(_position - 1, 1), null);
                    }
            }

        }
    }
}


