using Antlr4.Runtime;

namespace MiniCSharp.ANTLR4
{
    public class MyToken : IToken
    {
        public int Type { get; set; }
        public string Text { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public int Channel { get; }
        public int TokenIndex { get; }
        public int StartIndex { get; }
        public int StopIndex { get; }
        public ITokenSource TokenSource { get; }
        public ICharStream InputStream { get; }

        public MyToken(string text)
        {
            Text = text;
        }
    }
}