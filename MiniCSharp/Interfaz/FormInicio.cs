//Todos los metodos de esta clase fueron declarados para manejar ciertas funciones integradas dentro del editor grafico,
//son necesarios para que funcione correctamente, fue creado por Fus3n: https://github.com/Fus3n/cnote
//Ademas, aqui se implemento la logica que utilizara ANTLR4 para manejar archivos de texto ingresados por el usuario y se puedan compilar.
using ColorsNew;
using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using generated;
using MiniCSharp.ANTLR4;

namespace CNote
{

    public partial class NoteMain : Form
    {
        private bool TextChangedFCTB;
        private bool isOpenFirstTime = false;
        CNoteUtils util;
        private string currFileName;
        private string currFilePath;
        private string[] args;
        

        private Keys lastkeycode;

        //Algunos metodos que cargaran antes de iniciar el programa.
        public NoteMain()
        {
            InitializeComponent();
            util = new CNoteUtils();

            args = Environment.GetCommandLineArgs();
            SplitContainer.Panel2Collapsed = false;
        }

        //Metodo para remover cosas del editor grafico.
        public string[] removeDup(string[] arr)
        {
            string[] q = arr.Distinct().ToArray();

            return q;
        }
        //Algunos metodos que cargaran cuando se arranca el programa.
        private void NoteMain_Load(object sender, EventArgs e)
        {
            util.cshapr_items = removeDup(util.cshapr_items);

            cmdout.Zoom = 122;

            if (args.Length >= 2 && !string.IsNullOrEmpty(args[1]))
            {
                FileOpener(args[1]);
            }

            menu_bar2.Renderer = new ToolBarTheme_Light();

            fctb_main.AllowDrop = true;

        

            SplitContainer.Panel2Collapsed = false;

            InitSettings();
        }

        //Metodo para cargar configuracion visual del programa.
        private void InitSettings()
        {
            string pyPath = util.GetPythonPath();

            if (util.GetSettings("zoom") != null)
                fctb_main.Zoom = Int32.Parse(util.GetSettings("zoom"));


            if (util.GetSettings("theme") == null)
            {
                util.AOUSettings("theme", "light");
            }
            else
            {
                if (util.GetSettings("theme") == "dark")
                {
                    DarkMode();
                }
                else
                {
                    LightMode();
                }
            }

            
        }

        //Metodo para crear un nuevo archivo.
        private void new_tools_Click(object sender, EventArgs e)
        {
            fctb_main.Clear();
            currFileName = "Desconocido";
            currFilePath = "";
            TextChangedFCTB = false;
            Text = currFileName + " - MiniCSharp";
        }

        //Metodo para buscar un archivo dentro de la computadora.
        private void OpenDlg()
        {
            System.Windows.Forms.OpenFileDialog of = new System.Windows.Forms.OpenFileDialog();

            of.Filter = "Documentos de texto|*.txt|Todos los archivos|*.*";

            if (of.ShowDialog() == DialogResult.OK)
            {

                StreamReader sr = new StreamReader(of.FileName);
                currFilePath = of.FileName;
                currFileName = Path.GetFileName(currFilePath);
                TextChangedFCTB = false;
                fctb_main.Text = sr.ReadToEnd();
                sr.Close();
                FileSetup();

            }
        }

        //Metodo para cargar archivos.
        private void FileSetup()
        {
            isOpenFirstTime = true;

            this.Text = currFileName + " - MiniCSharp";

            fctb_main.SelectionStart = 1;
        }


        //Metodo para abrir archivos.
        private void FileOpener(string filename)
        {
            try
            {
                StreamReader sr = new StreamReader(filename);

                fctb_main.Text = sr.ReadToEnd();
                sr.Close();
                currFilePath = filename;
                currFileName = Path.GetFileName(filename);
                sr.Close();
                TextChangedFCTB = false;
                FileSetup();
            }
            catch
            {
            }

        }


        //Metodo para leer archivos de texto.
        private string FileReader(string path)
        {
            StreamReader sr = new StreamReader(path);
            var content = sr.ReadToEnd();
            sr.Close();
            return content;
        }



        //Se abre un nuevo archivo desde el menu.
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TextChangedFCTB && !string.IsNullOrEmpty(fctb_main.Text))
            {
                if (TextChangedFCTB && !string.IsNullOrEmpty(fctb_main.Text))
                {
                    DialogResult res = MessageBox.Show($"Quieres guardar los cambios en el archivo {currFileName}?", "MiniCSharp", MessageBoxButtons.YesNoCancel);
                    if (res == DialogResult.No)
                    {
                        OpenDlg();
                    }
                    else if (res == DialogResult.Yes)
                    {
                        FileWriter();
                        OpenDlg();
                    }

                }
            }
            else
            {
                OpenDlg();
            }

        }


        //Se guarda un archivo desde el menu.
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileAll();
        }

        //Metodo para guardar archivos si no se puede.
        private void SaveFileAll()
        {
            try
            {
                StreamWriter sw = new StreamWriter(currFilePath);
                sw.Write(fctb_main.Text);
                this.Text = currFileName + " - MiniCSharp";
                TextChangedFCTB = false;
                sw.Close();
                isOpenFirstTime = false;
            }
            catch { SaveDlg(); }
        }


        //Metodo para escribir en los archivos.
        void FileWriter()
        {
            try
            {
                StreamWriter sw = new StreamWriter(currFilePath);
                sw.Write(fctb_main.Text);
                sw.Close();
            }
            catch
            {
                SaveDlg();
            }
        }


        //Metodo para guardar un archivo buscando donde en la computadora.
        private void SaveDlg()
        {
            System.Windows.Forms.SaveFileDialog of = new System.Windows.Forms.SaveFileDialog();
            of.Filter = "Documentos de texto|*.txt|Todos los archivos|*.*";

            if (of.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(of.FileName);
                sw.Write(fctb_main.Text);
                sw.Close();
                currFilePath = of.FileName;
                currFileName = Path.GetFileName(of.FileName);
                TextChangedFCTB = false;
                FileSetup();
                isOpenFirstTime = false;
            }
        }


        //Se guarda un archivo desde el menu.
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDlg();

        }


        //Se sale del programa desde el menu.
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Cortar texto desde el menu.
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.Cut();
        }

        //Copiar texto desde el menu.
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.Copy();
        }

        //Pegar texto desde el menu.
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.Paste();
        }

        //Deshacer texto desde el menu.
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.Undo();
        }

        //Rehacer texto desde el menu.
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.Redo();
        }


        //Opciones del tema del programa desde el menu.

        private void lightm_toggle_Click(object sender, EventArgs e)
        {
            LightMode();
        }

        //Opciones del tema del programa desde el menu.
        private void darkm_toggle_Click(object sender, EventArgs e)
        {
            DarkMode();
        }

        //Metodo para que el programa este en modo claro.
        private void LightMode()
        {
            lightm_toggle.Checked = true;
            darkm_toggle.Checked = false;
            fctb_main.BackColor = Color.FromArgb(254, 251, 243);
            fctb_main.ForeColor = Color.FromArgb(0, 0, 0);
            fctb_main.IndentBackColor = Color.WhiteSmoke;
            cmdout.BackColor = Color.WhiteSmoke;
            AutoCompMenu1.Colors.BackColor = Color.WhiteSmoke;

            menu_bar2.BackColor = Color.FromArgb(254, 251, 243);
            menu_bar2.ForeColor = Color.FromArgb(0, 0, 0);
            cmdout.ForeColor = Color.Black;

            AutoCompMenu1.Colors.ForeColor = Color.Black;
            AutoCompMenu1.Colors.HighlightingColor = Color.DarkOrange;



            foreach (ToolStripMenuItem item in menu_bar2.Items)
            {
                foreach (ToolStripItem a in item.DropDownItems)
                {
                    a.BackColor = Color.White;
                    a.ForeColor = Color.Black;
                }
            }


            foreach (ToolStripItem item in theme_colorstrip.DropDownItems)
            {
                item.BackColor = Color.White;
                item.ForeColor = Color.Black;
            }

            menu_bar2.Renderer = new ToolBarTheme_Light();
            menu_bar2.Refresh();
            util.AOUSettings("theme", "light");

        }

        //Metodo para que el programa este en modo oscuro.
        private void DarkMode()
        {

            darkm_toggle.Checked = true;
            lightm_toggle.Checked = false;
            fctb_main.BackColor = Color.FromArgb(23, 16, 16);
            fctb_main.IndentBackColor = Color.FromArgb(23, 16, 16);
            menu_bar2.BackColor = Color.FromArgb(23, 16, 16);
            cmdout.BackColor = Color.FromArgb(23, 16, 16);
            AutoCompMenu1.Colors.BackColor = Color.FromArgb(44, 57, 75);

            fctb_main.ForeColor = Color.FromArgb(254, 251, 243);
            menu_bar2.ForeColor = Color.FromArgb(254, 251, 243);
            cmdout.ForeColor = Color.LightSteelBlue;

            AutoCompMenu1.Colors.ForeColor = Color.LightSteelBlue;
            AutoCompMenu1.Colors.HighlightingColor = Color.White;





            foreach (ToolStripMenuItem item in menu_bar2.Items)
            {
                foreach (ToolStripItem a in item.DropDownItems)
                {
                    a.BackColor = Color.FromArgb(23, 16, 16);
                    a.ForeColor = Color.FromArgb(254, 251, 243);


                }

            }

            foreach (ToolStripItem item in theme_colorstrip.DropDownItems)
            {
                item.BackColor = Color.FromArgb(23, 16, 16);
                item.ForeColor = Color.FromArgb(254, 251, 243);
            }

            menu_bar2.Renderer = new ToolBarTheme_Dark();
            menu_bar2.Refresh();
            util.AOUSettings("theme", "dark");
        }

        //Opciones de la fuente del programa desde el menu.
        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                fctb_main.Font = fd.Font;

            }
        }


        //Seleccionar todo el texto desde el menu.
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.SelectAll();
        }

        //Cortar texto desde el menu.
        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fctb_main.Cut();

        }

        //Copiar texto desde el menu.
        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fctb_main.Copy();
        }

        //Pegar texto desde el menu.
        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fctb_main.Paste();
        }

        //Metodo para obtener informacion del cursor respecto a la linea, columna y zoom.
        private string GetEditInfo()
        {
            return $"Linea: {fctb_main.Selection.End.iLine +1} Columna: {fctb_main.Selection.End.iChar + 1} Zoom {fctb_main.Zoom}";
        }

        //Metodo para llamar la logica de la compilacion de ANTLR4
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
                string inputFilePath = currFilePath;
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
                    form.ShowDialog();
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

        //Metodo que tiene la logica para compilar.
        public void compilarStart()
        {
            if (string.IsNullOrEmpty(currFilePath) && !string.IsNullOrEmpty(fctb_main.Text))
            {
                MessageBox.Show("Se debe guardar el archivo antes de compilar.");
                SaveFileAll();
                return;
            }
            else if (string.IsNullOrEmpty(fctb_main.Text))
            {
                MessageBox.Show("No se puede compilar una entrada vacia.");
                return;
            }
            else if (!string.IsNullOrEmpty(currFilePath) && !string.IsNullOrEmpty(fctb_main.Text))
            {
                SaveFileAll();
                antlr4Start();
                return;
            }
        }


        //Compilar desde el menu.
        private void run_tools_Click_1(object sender, EventArgs e)
        {
            stat_txt.Text = GetEditInfo();
            compilarStart();


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

        //Obtener datos del cursor en el inicio
        private void fctb_main_Click(object sender, EventArgs e)
        {
            stat_txt.Text = GetEditInfo();
        }

        //Verificar cuando se modifique el texto en el editor grafico.
        private void fctb_main_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fctb_main.IsChanged)
            {

                TextChangedFCTB = true;
                if (string.IsNullOrEmpty(currFilePath))
                {
                    if (string.IsNullOrEmpty(currFileName))
                    {
                        currFileName = "Desconocido";
                        this.Text = currFileName + " - MiniCSharp";
                    }
                    else
                    {
                        currFileName = "Desconocido";
                        this.Text = "*"+ currFileName + " - MiniCSharp";
                    }
                   
                }
                else
                {
                    this.Text = "*" + currFileName + " - MiniCSharp";
                }

            }
            else
            {
                this.Text = currFileName + " - MiniCSharp";
            }

        }

        //Modificar el zoom desde el menu.
        private void fctb_main_ZoomChanged(object sender, EventArgs e)
        {
            stat_txt.Text = GetEditInfo();
            util.AOUSettings("zoom", $"{fctb_main.Zoom}");
        }

        //Mostrar ventana de depuracion.
        private void cmdoutHide_Click(object sender, EventArgs e)
        {
            SplitContainer.Panel2Collapsed = true;
        }

        //Seleccionar todo desde el menu.
        private void cmdoutSelectAll_Click(object sender, EventArgs e)
        {
            cmdout.SelectAll();
        }

        //Seleccionar la ventana de depuracion desde el menu.
        private void showOutPanel_Click(object sender, EventArgs e)
        {
            SplitContainer.Panel2Collapsed = SplitContainer.Panel2Collapsed == true ? false : true;
        }


        //Mover archivos al editor grafico.
        private void fctb_main_DragDrop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(DataFormats.FileDrop);
            if (data != null)
            {
                var filenames = data as string[];
                if (filenames.Length > 0)
                {
                    currFilePath = filenames[0];
                    currFileName = Path.GetFileName(filenames[0]);
                    fctb_main.Text = FileReader(filenames[0]);
                    this.Text = currFileName + " - MiniCSharp";
                    fctb_main.ClearStylesBuffer();
                    FileSetup();
                }

            }
        }

        //Mover archivos al editor grafico.
        private void fctb_main_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }


        //Verificar si se quiere guardar la informacion en un archivo si no lo hizo anteriormente.
        private void NoteMain_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (TextChangedFCTB && !string.IsNullOrEmpty(fctb_main.Text) && isOpenFirstTime == false)
            {
                DialogResult res = MessageBox.Show($"Quieres guardar los cambios en el archivo {currFileName}?", "MiniCSharp", MessageBoxButtons.YesNoCancel);
                if (res == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (res == DialogResult.Yes)
                {
                    FileWriter();
                }

            }


        }


        //Reiniciar zoom desde el menu.
        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.Zoom = 122;
            util.AOUSettings("zoom", "122");

        }

        //Limpiar texto desde el menu.
        private void cmdout_clear_Click(object sender, EventArgs e)
        {
            cmdout.Clear();
        }

        //Pegar texto desde el menu.
        private void cmdout_paste_Click(object sender, EventArgs e)
        {
            cmdout.Paste();
        }

        //Copiar texto desde el menu.
        private void cmdout_cpy_Click(object sender, EventArgs e)
        {
            cmdout.Copy();
        }

        //Deshacer texto desde el menu.
        private void undoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fctb_main.Undo();
        }


        //Verificar la posicion del cursor cuando se escribe.
        private void fctb_main_KeyPressed(object sender, KeyPressEventArgs e)
        {
            stat_txt.Text = GetEditInfo();
        }

        //Verificar la posicion del cursor cuando se mueve con las flechas del teclado.
        private void fctb_main_KeyUp(object sender, KeyEventArgs e)
        {
            lastkeycode = e.KeyCode;

            if (e.Control)
            {
                stat_txt.Text = GetEditInfo();
            }

            if(e.KeyCode == Keys.Up | e.KeyCode == Keys.Down | e.KeyCode == Keys.Left | e.KeyCode == Keys.Right)
            {
                stat_txt.Text = GetEditInfo();
            }
        }

        private void cmdout_KeyDown(object sender, KeyEventArgs e)
        {
        }

        //Compilar desde el menu.
        private void compilarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compilarStart();
        }

        //Rehacer texto desde el menu.
        private void rehacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.Redo();
        }

        private void fctb_main_KeyPressing(object sender, KeyPressEventArgs e)
        {

        }
    }

}

