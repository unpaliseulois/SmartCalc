using System.Collections.Generic;
using SmartCalc.Global.CodeAnalysis.Text;
using SmartCalc.Global.Compilation;

namespace SmartCalc.Global.CodeAnalysis.Syntax
{
    internal sealed class Lexer
    {
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private readonly SourceText _text;
        private int _position;
        private int _start;
        private SyntaxKind _kind;
        private object _value;
        public Lexer(SourceText text)
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

            switch (Current)
            {
                case '\0':
                    {
                        _kind = SyntaxKind.EndOfFileToken;
                    }
                    break;
                case '+':
                    {

                        _kind = SyntaxKind.PlusToken;
                        _position++;
                    }
                    break;
                case '-':
                    {
                        _kind = SyntaxKind.MinusToken;
                        _position++;
                    }
                    break;
                case '^':
                    {
                        _kind = SyntaxKind.HatToken;
                        _position++;
                    }
                    break;
                case '*':
                    _position++;
                    if (Current == '*')
                    {
                        _kind = SyntaxKind.StarStarToken;
                        _position++;
                    }
                    else
                    {
                        _kind = SyntaxKind.StarToken;
                    }
                    break;
                case '/':
                    {
                        _kind = SyntaxKind.SlashToken;
                        _position++;
                    }
                    break;
                case '(':
                    {
                        _kind = SyntaxKind.OpenParenthesisToken;
                        _position++;
                    }
                    break;
                case ')':
                    {
                        _kind = SyntaxKind.CloseParenthesisToken;
                        _position++;
                    }
                    break;
                case '{':
                    {
                        _kind = SyntaxKind.OpenPraceToken;
                        _position++;
                    }
                    break;
                case '}':
                    {
                        _kind = SyntaxKind.ClosePraceToken;
                        _position++;
                    }
                    break;
                case '!':
                    _position++;
                    if (Current != '=')
                    {
                        _kind = SyntaxKind.BangToken;
                    }
                    else
                    {
                        _kind = SyntaxKind.BangEqualsToken;
                        _position++;
                    }
                    break;
                    case '<':
                    _position++;
                    if (Current != '=')
                    {
                        _kind = SyntaxKind.LessToken;
                    }
                    else
                    {
                        _kind = SyntaxKind.LessOrEqualsToken;
                        _position++;
                    }
                    break;
                     case '>':
                    _position++;
                    if (Current != '=')
                    {
                        _kind = SyntaxKind.GreaterToken;
                    }
                    else
                    {
                        _kind = SyntaxKind.GreaterOrEqualsToken;
                        _position++;
                    }
                    break;
                case '&':
                    _position++;
                    if (Current == '&')
                    {
                        _kind = SyntaxKind.AmpersandAmpersandToken;
                        _position++;
                    }
                    else
                    {
                        _kind = SyntaxKind.AmpersandToken;
                    }
                    break;

                case '|':
                    _position++;
                    if (Current == '|')
                    {
                        _kind = SyntaxKind.PipePipeToken;
                        _position++;
                    }
                    else
                    {
                        _kind = SyntaxKind.PipeToken;
                    }
                    break;
                case '=':
                    _position++;
                    if (Current == '=')
                    {
                        _kind = SyntaxKind.EqualsEqualsToken;
                        _position++;
                    }
                    else
                    {
                        _kind = SyntaxKind.EqualsToken;
                    }
                    break;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    {
                        ReadNumberToken();
                    }
                    break;
                case ' ':
                case '\t':
                case '\n':
                case '\r':

                    {
                        ReadWhiteSpaceToken();
                    }
                    break;
                default:
                    {
                        if (char.IsWhiteSpace(Current))
                        {
                            ReadWhiteSpaceToken();
                        }
                        else if (char.IsLetter(Current))
                        {
                            ReadIdentifierKeyword();
                        }
                        else
                        {
                            _diagnostics.ReportBadCharacter(_position, Current);
                            _position++;
                        }
                    }
                    break;
            }

            var length = _position - _start;
            var text = SyntaxFacts.GetText(_kind);
            if (text == null)
                text = _text.ToString(_start, length);
            return new SyntaxToken(_kind, _start, text, _value);
        }

        private void ReadIdentifierKeyword()
        {
            while (char.IsLetter(Current))
                _position++;

            var length = _position - _start;
            var text = _text.ToString(_start, length);
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
            var text = _text.ToString(_start, length);

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


