namespace SmartCalc.Global.CodeAnalysis
{
    public enum SyntaxKind
    {
        NumberToken,
        WhiteSpaceToken,

        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,

        OpenParenthsisToken,
        CloseParenthsisToken,
        BadToken,
        EndOfFileToken,
        NumberExpression,
        BinaryExpression,
        ParenthesizedExpression
    }
}


