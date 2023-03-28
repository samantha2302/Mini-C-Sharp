using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace generated
{
    public class MySyntaxErrorListener : IAntlrErrorListener<int>
    {
        public ArrayList<String> errorMsgs = new ArrayList<string>();
        
        public MySyntaxErrorListener()
        {
            this.errorMsgs = new ArrayList<String>();
        }
        
        public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            if (recognizer is Scanner)
            {
                errorMsgs.Add("ERROR DE SCANNER - linea " + line + " columna " + charPositionInLine + " " + "("+msg+")");
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
            if (!hasErrors()) return "0 errores de scanner";
            StringBuilder builder = new StringBuilder();
            foreach (String s in errorMsgs)
            {
                builder.Append("\n"+s);
            }
            return builder.ToString();
        }
    }
}