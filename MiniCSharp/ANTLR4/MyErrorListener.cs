using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using generated;

namespace MiniCSharp.ANTLR4
{
    public class MyErrorListener : BaseErrorListener
    {

        public ArrayList<String> errorMsgs = new ArrayList<String>();

        public MyErrorListener()
        {
            this.errorMsgs = new ArrayList<String>();
        }

        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol,
            int charPositionInLine, int i, string msg, RecognitionException recognitionException)
        {
            if (recognizer is MiniCSharpParser)
            {
                errorMsgs.Add("ERROR DE PARSE - linea " + charPositionInLine + " columna " + i + " " + "("+msg+")");
            } 
            else
            {
                errorMsgs.Add("OTRO ERROR");
            }
                
        }

        public Boolean hasErrors()
        {
            return this.errorMsgs.Count > 0;
        }

        public String toString()
        {
            if (!hasErrors()) return "0 errores de parse";
            StringBuilder builder = new StringBuilder();
            foreach (String s in errorMsgs)
            {
                builder.Append("\n"+s);
            }
            return builder.ToString();
        }

    }
}