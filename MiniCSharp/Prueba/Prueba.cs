using System;
using System.Windows.Forms;
using System.IO;
using Antlr4.Runtime;
using generated;
using MiniCSharp.ANTLR4;
using Antlr4.Runtime.Tree;

namespace MiniCSharp.Interfaz
{
    public partial class Prueba : Form
    {
        public Prueba()
        {
            InitializeComponent();
        }

        public void antlr4Start()
        {

            Scanner inst = null;
            MiniCSharpParser parser = null;
            ICharStream input = null;
            CommonTokenStream tokens = null;
            MyErrorListener errorListener = null;
            MySyntaxErrorListener syntaxErrorListener = null;
            var defaultErrorStrategy = new MyDefaultErrorStrategy();
            IParseTree tree;
            try
            {
                string inputFilePath = @"../../Prueba/Prueba.txt"; ;
                string txt = File.ReadAllText(inputFilePath);
                AntlrInputStream inputStream = new AntlrInputStream(txt);
                inst = new Scanner(inputStream);
                tokens = new CommonTokenStream(inst);
                parser = new MiniCSharpParser(tokens);

                errorListener = new MyErrorListener();
                syntaxErrorListener = new MySyntaxErrorListener();

                inst.RemoveErrorListeners();
                inst.AddErrorListener(syntaxErrorListener);

                parser.RemoveErrorListeners();
                parser.AddErrorListener(errorListener);
                parser.ErrorHandler = defaultErrorStrategy;
                tree = parser.program();

                if (errorListener.hasErrors() == false && syntaxErrorListener.hasErrors() == false)
                {
                    cmdout.Text = "Compilacion exitosa\n";
                }
                else
                {
                    cmdout.Text = "";
                    cmdout.Text = "Compilacion fallida\n";
                    if (errorListener.hasErrors())
                    {
                        cmdout.Text += errorListener.toString();
                    }
                    if (syntaxErrorListener.hasErrors())
                    {
                        cmdout.Text += syntaxErrorListener.toString();
                    }
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine("No hay archivo");
                throw;
            }

        }

        private void Prueba_Load(object sender, EventArgs e)
        {
            antlr4Start();
        }
    }
}
