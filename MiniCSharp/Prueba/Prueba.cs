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
        
        //Metodo que convierte un IParseTree en un TreeNode para ser mostrado en un TreeView
        private TreeNode GenerateTreeNode(IParseTree parseTree, Parser parser)
        {
            if (parseTree is TerminalNodeImpl terminal)
            {
                return new TreeNode($"{parser.Vocabulary.GetSymbolicName(terminal.Symbol.Type)}: {terminal.Symbol.Text}");
            }
            else
            {
                TreeNode node = new TreeNode(parser.RuleNames[((ParserRuleContext)parseTree).RuleIndex]);

                for (int i = 0; i < parseTree.ChildCount; i++)
                {
                    node.Nodes.Add(GenerateTreeNode(parseTree.GetChild(i), parser));
                }

                return node;
            }
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
                string inputFilePath = @"../../Prueba/Prueba.txt";
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
                TreeView treeView = new TreeView();

                treeView.Dock = DockStyle.Fill;
                treeView.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                TreeNode rootNode = GenerateTreeNode(tree, parser);
                treeView.Nodes.Add(rootNode);

                if (errorListener.hasErrors() == false && syntaxErrorListener.hasErrors() == false)
                {
                    cmdout.Text = "Compilacion exitosa\n";
                    Form form = new Form();
                    form.Text = "Arbol MiniCSharp";
                    form.Controls.Add(treeView);
                    form.Width = 400;
                    form.Height = 500;
                    //form.ShowDialog();
                    AContextual mv = new AContextual();
                    mv.Visit(tree);
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
