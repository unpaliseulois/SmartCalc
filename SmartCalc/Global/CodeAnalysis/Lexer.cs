using System.Collections.Generic;


namespace SmartCalc.Global.CodeAnalysis
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
        private char Current
        {
            get
            {
                if (_position >= _text.Length)
                    return '\0';
                return _text[_position];
            }
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

            switch (Current)
            {
                case '+':
                    _position++;
                    return new SyntaxToken(SyntaxKind.PlusToken, start, "+", null);
                case '-':
                    _position++;
                    return new SyntaxToken(SyntaxKind.MinusToken, start, "-", null);
                case '*':
                    _position++;
                    return new SyntaxToken(SyntaxKind.StarToken, start, "*", null);
                case '/':
                    _position++;
                    return new SyntaxToken(SyntaxKind.SlashToken, start, "/", null);
                case '(':
                    _position++;
                    return new SyntaxToken(SyntaxKind.OpenParenthsisToken, start, "(", null);
                case ')':
                    _position++;
                    return new SyntaxToken(SyntaxKind.CloseParenthsisToken, start, ")", null);
                default:
                    _diagnostics.Add($"ERROR: bad character input: '{Current}'.");
                    _position++;
                    return new SyntaxToken(SyntaxKind.BadToken, start, _text.Substring(_position - 1, 1), null);
            }

        }
    }
}


