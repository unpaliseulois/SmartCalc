using System.Collections.Generic;


namespace SmartCalc.Global.CodeAnalysis.Syntax
{
    internal sealed class Lexer
    {
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private readonly string _text;
        private int _position;
        private int _start;
        private SyntaxKind _kind;
        private object _value;
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
            _kind = SyntaxKind.BadToken;
            _value = null;

            if (char.IsDigit(Current))
            {
                ReadNumberToken();
            }
            else if (char.IsWhiteSpace(Current))
            {
                ReadWhiteSpaceToken();
            }
            else if (char.IsLetter(Current))
            {                
                ReadIdentifierKeyword();
            }
            else
            {

                switch (Current)
                {
                    case '\0':
                        {
                            return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);
                        }
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
                            _diagnostics.ReportBadCharacter(_position, Current);
                            _position++;
                        }
                        break;
                }
            }
            var length = _position-_start;
            var text = SyntaxFacts.GetText(_kind);
            if(text == null)
                text = _text.Substring(_start,length);
            return new SyntaxToken(_kind, _start, text, _value);
        }

        private void ReadIdentifierKeyword()
        {
            while (char.IsLetter(Current))
                _position++;

            var length = _position - _start;
            var text = _text.Substring(_start, length);
            _kind = SyntaxFacts.GetKeywordKind(text);
        }

        private void ReadWhiteSpaceToken()
        {
            while (char.IsWhiteSpace(Current))
                _position++;

            _kind = SyntaxKind.WhiteSpaceToken;
        }

        private void ReadNumberToken()
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
            _value = value;
            _kind = SyntaxKind.NumberToken;
        }
    }
}


