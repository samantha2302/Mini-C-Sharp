/*
 Copyright 2021 Fusen
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
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

        //total all files 1775 lines

        private bool TextChangedFCTB; //check if text changed used to add * in the title 
        private bool isOpenFirstTime = false;
        CNoteUtils util;
        private string currFileName;
        private string currFilePath;
        private string[] args;
        

        private Keys lastkeycode;
        public NoteMain()
        {
            InitializeComponent();
            util = new CNoteUtils();

            args = Environment.GetCommandLineArgs();
            SplitContainer.Panel2Collapsed = false;
        }

        public string[] removeDup(string[] arr)
        {
            string[] q = arr.Distinct().ToArray();

            return q;
        }
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

        //init some settings
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

        //Create New File
        private void new_tools_Click(object sender, EventArgs e)
        {
            fctb_main.Clear();
            currFileName = "Desconocido";
            currFilePath = "";
            TextChangedFCTB = false;
            Text = currFileName + " - MiniCSharp";
        }

        //Open File Dialog Depen
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

        private void FileSetup()
        {
            isOpenFirstTime = true;

            this.Text = currFileName + " - MiniCSharp";

            fctb_main.SelectionStart = 1;
            //SaveFileAll();
        }


        //opens files....
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
                //pass
            }

        }


        //reads files......maybe?
        private string FileReader(string path)
        {
            StreamReader sr = new StreamReader(path);
            var content = sr.ReadToEnd();
            sr.Close();
            return content;
        }



        //opens new file 
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


        //saves file :)
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileAll();
        }

        //function to save all file if cant then open save file dialog
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


        //yup and this one writes files
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


        //SaveDlg
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


        //saves as file dialog
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDlg();

        }


        //exit..
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //uh
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.Cut();
        }

        //hm
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.Copy();
        }

        //yea 
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.Paste();
        }

        //about that..
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.Undo();
        }

        // redo
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.Redo();
        }


        //Theme Options

        private void lightm_toggle_Click(object sender, EventArgs e)
        {
            LightMode();
        }

        private void darkm_toggle_Click(object sender, EventArgs e)
        {
            DarkMode();
        }

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

        //temporary font selection
        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                fctb_main.Font = fd.Font;//change current font and size

            }
        }


        //Context Menu Items
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.SelectAll();
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fctb_main.Cut();

        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fctb_main.Copy();
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fctb_main.Paste();
        }

        //Get basic info for statusStrip
        private string GetEditInfo()
        {
            return $"Linea: {fctb_main.Selection.End.iLine +1} Columna: {fctb_main.Selection.End.iChar + 1} Zoom {fctb_main.Zoom}";
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
                //string inputFilePath = @"..\..\..\MiniCSharp\ANTLR4\test.txt";
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

                // Configuramos el TreeView
                treeView.Dock = DockStyle.Fill;
                treeView.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                // Convertimos el árbol de análisis en un árbol de nodos del TreeView
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


        //Run File is Runnable
        private void run_tools_Click_1(object sender, EventArgs e)
        {
            stat_txt.Text = GetEditInfo();
            compilarStart();


        }
        
        // Convierte un IParseTree en un TreeNode para ser mostrado en un TreeView
        private TreeNode GenerateTreeNode(IParseTree parseTree, Parser parser)
        {
            if (parseTree is TerminalNodeImpl terminal)
            {
                // Si es un nodo terminal, creamos un nuevo TreeNode con el nombre del token
                return new TreeNode($"{parser.Vocabulary.GetSymbolicName(terminal.Symbol.Type)}: {terminal.Symbol.Text}");
            }
            else
            {
                // Si es un nodo no terminal, creamos un nuevo TreeNode con el nombre de la regla
                TreeNode node = new TreeNode(parser.RuleNames[((ParserRuleContext)parseTree).RuleIndex]);

                // Para cada hijo, generamos un nuevo TreeNode y lo agregamos como hijo del nodo actual
                for (int i = 0; i < parseTree.ChildCount; i++)
                {
                    node.Nodes.Add(GenerateTreeNode(parseTree.GetChild(i), parser));
                }

                return node;
            }
        }

        //Run Other Files

        // Get Info in StatusTool
        private void fctb_main_Click(object sender, EventArgs e)
        {
            stat_txt.Text = GetEditInfo();
        }


        private void fctb_main_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fctb_main.IsChanged)
            {
                // put '*' in title if file not saved after change

                TextChangedFCTB = true;
                if (string.IsNullOrEmpty(currFilePath)) //check if file is saved or opend if not then adds untitled
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
                    this.Text = "*" + currFileName + " - MiniCSharp"; //if opened add * before filename
                }

            }
            else
            {
                this.Text = currFileName + " - MiniCSharp"; //if already saved add normal title
            }

        }

        //save zoom value on zoom change
        private void fctb_main_ZoomChanged(object sender, EventArgs e)
        {
            stat_txt.Text = GetEditInfo();
            util.AOUSettings("zoom", $"{fctb_main.Zoom}");
        }

        private void cmdoutHide_Click(object sender, EventArgs e)
        {
            SplitContainer.Panel2Collapsed = true;
        }

        private void cmdoutSelectAll_Click(object sender, EventArgs e)
        {
            cmdout.SelectAll();
        }

        //Show Cmd output panel
        private void showOutPanel_Click(object sender, EventArgs e)
        {
            SplitContainer.Panel2Collapsed = SplitContainer.Panel2Collapsed == true ? false : true;
        }


        //Drag files 
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

        private void fctb_main_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }


        // save dialog if any contenet has changed without saving
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


        //reset zoom and save it
        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.Zoom = 122;
            util.AOUSettings("zoom", "122");

        }

        private void cmdout_clear_Click(object sender, EventArgs e)
        {
            cmdout.Clear();
        }

        private void cmdout_paste_Click(object sender, EventArgs e)
        {
            cmdout.Paste();
        }

        private void cmdout_cpy_Click(object sender, EventArgs e)
        {
            cmdout.Copy();
        }

        private void undoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fctb_main.Undo();
        }


        //Auto indent for Python and info
        private void fctb_main_KeyPressed(object sender, KeyPressEventArgs e)
        {
            stat_txt.Text = GetEditInfo();
        }

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

        private void compilarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compilarStart();
        }

        private void rehacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.Redo();
        }

        private void fctb_main_KeyPressing(object sender, KeyPressEventArgs e)
        {

        }
    }

}

