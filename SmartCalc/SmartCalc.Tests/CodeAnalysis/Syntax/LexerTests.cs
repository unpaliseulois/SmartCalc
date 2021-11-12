using System;
using System.Collections.Generic;
using System.Linq;
using SmartCalc.Global.CodeAnalysis.Syntax;
using Xunit;

namespace SmartCalc.Tests.CodeAnalysis.Syntax
{
    public class LexerTests
    {
        [Fact]
        [MemberData(nameof(GetTokensData))]
        public void Lexer_Tests_AllTokens()
        {

            var tokenKinds = Enum.GetValues(typeof(SyntaxKind))
                                 .Cast<SyntaxKind>()
                                 .Where(k => k.ToString().EndsWith("Keyword") ||
                                              k.ToString().EndsWith("Token"))
                                 .ToList();

            var testedTokenKinds = GetTokens().Concat(GetSeparators())
                                              .Select(t => t.kind);

            var untestedTokenKinds = new SortedSet<SyntaxKind>(tokenKinds);
            untestedTokenKinds.Remove(SyntaxKind.BadToken);
            untestedTokenKinds.Remove(SyntaxKind.EndOfFileToken);
            untestedTokenKinds.ExceptWith(testedTokenKinds);

            Assert.Empty(untestedTokenKinds);

        }
        [Theory]
        [MemberData(nameof(GetTokensData))]
        public void Lexer_Lexes_Token(SyntaxKind kind, string text)
        {
            var tokens = SyntaxTree.ParseTokens(text);
            var token = Assert.Single(tokens);
            Assert.Equal(kind, token.Kind);
            Assert.Equal(text, token.Text);
        }
        [Theory]
        [MemberData(nameof(GetTokenPairsData))]
        public void Lexer_Lexes_TokenPairs(SyntaxKind t1Kind, string t1Text,
                                          SyntaxKind t2Kind, string t2Text)
        {
            var text = t1Text + t2Text;
            var tokens = SyntaxTree.ParseTokens(text).ToArray();

            Assert.Equal(2, tokens.Length);
            /*/
            Assert.Equal(tokens[0].Kind, t1Kind);
            Assert.Equal(tokens[0].Text, t1Text);

            Assert.Equal(tokens[1].Kind, t2Kind);
            Assert.Equal(tokens[1].Text, t2Text);
            //*/

            Assert.Equal(t1Kind, tokens[0].Kind);
            Assert.Equal(t1Text, tokens[0].Text);

            Assert.Equal(t2Kind, tokens[1].Kind);
            Assert.Equal(t2Text, tokens[1].Text);
        }
        [Theory]
        [MemberData(nameof(GetTokenPairsWithSeparatorData))]
        public void Lexer_Lexes_TokenPairs_WithSeparator(SyntaxKind t1Kind, string t1Text,
                                                         SyntaxKind separatorKind, string separatorText,
                                                         SyntaxKind t2Kind, string t2Text)
        {
            var text = t1Text + separatorText + t2Text;
            var tokens = SyntaxTree.ParseTokens(text).ToArray();

            Assert.Equal(3, tokens.Length);
            /*/
            Assert.Equal(tokens[0].Kind, t1Kind);
            Assert.Equal(tokens[0].Text, t1Text);

            Assert.Equal(tokens[1].Kind, separatorKind);
            Assert.Equal(tokens[1].Text, separatorText);

            Assert.Equal(tokens[2].Kind, t2Kind);
            Assert.Equal(tokens[2].Text, t2Text);
            //*/
            Assert.Equal(t1Kind, tokens[0].Kind);
            Assert.Equal(t1Text, tokens[0].Text);

            Assert.Equal(separatorKind, tokens[1].Kind);
            Assert.Equal(separatorText, tokens[1].Text);

            Assert.Equal(t2Kind, tokens[2].Kind);
            Assert.Equal(t2Text, tokens[2].Text);
        }
        public static IEnumerable<Object[]> GetTokensData()
        {
            foreach (var t in GetTokens().Concat(GetSeparators()))
            {
                yield return new object[] { t.kind, t.text };
            }
        }
        public static IEnumerable<Object[]> GetTokenPairsData()
        {
            foreach (var t in GetTokenPairs())
            {
                yield return new object[] { t.t1Kind, t.t1Text, t.t2Kind, t.t2Text };
            }
        }
        public static IEnumerable<Object[]> GetTokenPairsWithSeparatorData()
        {
            foreach (var t in GetTokenPairsWithSeparator())
            {
                yield return new object[] { t.t1Kind, t.t1Text, t.separatorKind, t.separatorText, t.t2Kind, t.t2Text };
            }
        }
        private static IEnumerable<(SyntaxKind kind, string text)> GetTokens()
        {
            var fixedTokens = Enum.GetValues(typeof(SyntaxKind))
                                  .Cast<SyntaxKind>()
                                  .Select(k => (kind: k, text: SyntaxFacts.GetText(k)))
                                  .Where(t => t.text != null);

            var dynamicTokens = new[]
            {
                (SyntaxKind.NumberToken, "1"),
                (SyntaxKind.NumberToken, "123"),
                (SyntaxKind.IdentifierToken, "a"),
                (SyntaxKind.IdentifierToken,"abc")
            };
            return fixedTokens.Concat(dynamicTokens);
        }
        private static IEnumerable<(SyntaxKind kind, string text)> GetSeparators()
        {
            return new[]
            {
                (SyntaxKind.WhiteSpaceToken, " "),
                (SyntaxKind.WhiteSpaceToken, "  "),
                (SyntaxKind.WhiteSpaceToken, "\r"),
                (SyntaxKind.WhiteSpaceToken, "\n"),
                (SyntaxKind.WhiteSpaceToken, "\r\n")
            };
        }
        private static bool RequiresSeparator(SyntaxKind t1Kind, SyntaxKind t2Kind)
        {
            var t1IsKeayword = t1Kind.ToString().EndsWith("Keyword");
            var t2IsKeayword = t2Kind.ToString().EndsWith("Keyword");

            if (t1Kind == SyntaxKind.IdentifierToken && t2Kind == SyntaxKind.IdentifierToken)
                return true;
            if (t1IsKeayword && t2IsKeayword)
                return true;
            if (t1IsKeayword && t2Kind == SyntaxKind.IdentifierToken)
                return true;
            if (t1Kind == SyntaxKind.IdentifierToken && t2IsKeayword)
                return true;
            if (t1Kind == SyntaxKind.NumberToken && t2Kind == SyntaxKind.NumberToken)
                return true;
            if (t1Kind == SyntaxKind.StarToken && t2Kind == SyntaxKind.StarToken)
                return true;
            if (t1Kind == SyntaxKind.PipeToken && t2Kind == SyntaxKind.PipeToken)
                return true;
            if (t1Kind == SyntaxKind.PipeToken && t2Kind == SyntaxKind.PipePipeToken)
                return true;
            if (t1Kind == SyntaxKind.AmpersandToken && t2Kind == SyntaxKind.AmpersandToken)
                return true;
            if (t1Kind == SyntaxKind.AmpersandToken && t2Kind == SyntaxKind.AmpersandAmpersandToken)
                return true;
            if (t1Kind == SyntaxKind.EqualsToken && t2Kind == SyntaxKind.EqualsEqualsToken)
                return true;
            if (t1Kind == SyntaxKind.EqualsToken && t2Kind == SyntaxKind.EqualsToken)
                return true;
            if (t1Kind == SyntaxKind.BangToken && t2Kind == SyntaxKind.EqualsEqualsToken)
                return true;
            if (t1Kind == SyntaxKind.BangToken && t2Kind == SyntaxKind.EqualsToken)
                return true;
            if (t1Kind == SyntaxKind.StarToken && t2Kind == SyntaxKind.StarStarToken)
                return true;
            if (t1Kind == SyntaxKind.LessToken && t2Kind == SyntaxKind.EqualsToken)
                return true;
            if (t1Kind == SyntaxKind.LessToken && t2Kind == SyntaxKind.EqualsEqualsToken)
                return true;
            if (t1Kind == SyntaxKind.GreaterToken && t2Kind == SyntaxKind.EqualsToken)
                return true;
            if (t1Kind == SyntaxKind.GreaterToken && t2Kind == SyntaxKind.EqualsEqualsToken)
                return true;
            // TODO: More cases
            return false;
        }

        private static IEnumerable<(SyntaxKind t1Kind, string t1Text, SyntaxKind t2Kind, string t2Text)> GetTokenPairs()
        {
            foreach (var t1 in GetTokens())
            {
                foreach (var t2 in GetTokens())
                {
                    if (!RequiresSeparator(t1.kind, t2.kind))
                        yield return (t1.kind, t1.text, t2.kind, t2.text);
                }
            }
        }
        private static IEnumerable<(SyntaxKind t1Kind, string t1Text,
                                    SyntaxKind separatorKind, string separatorText,
                                    SyntaxKind t2Kind, string t2Text)> GetTokenPairsWithSeparator()
        {
            foreach (var t1 in GetTokens())
            {
                foreach (var t2 in GetTokens())
                {
                    if (!RequiresSeparator(t1.kind, t2.kind))
                    {
                        foreach (var s in GetSeparators())
                            yield return (t1.kind, t1.text, s.kind, s.text, t2.kind, t2.text);
                    }
                }
            }
        }

    }
}
