
namespace CNote
{
    partial class NoteMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NoteMain));
            this.menu_bar2 = new System.Windows.Forms.MenuStrip();
            this.file_tools = new System.Windows.Forms.ToolStripMenuItem();
            this.new_tools = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.edit_tools = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showOutPanel = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.option_tools = new System.Windows.Forms.ToolStripMenuItem();
            this.theme_colorstrip = new System.Windows.Forms.ToolStripMenuItem();
            this.darkm_toggle = new System.Windows.Forms.ToolStripMenuItem();
            this.lightm_toggle = new System.Windows.Forms.ToolStripMenuItem();
            this.fontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.run_tools_parent = new System.Windows.Forms.ToolStripMenuItem();
            this.run_tools = new System.Windows.Forms.ToolStripMenuItem();
            this.fctb_main = new FastColoredTextBoxNS.FastColoredTextBox();
            this.editContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.undoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.rehacerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.compilarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdoutMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmdoutSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdout_cpy = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdout_paste = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdoutHide = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdout_clear = new System.Windows.Forms.ToolStripMenuItem();
            this.statusOptions = new System.Windows.Forms.StatusStrip();
            this.stat_txt = new System.Windows.Forms.ToolStripStatusLabel();
            this.SplitContainer = new System.Windows.Forms.SplitContainer();
            this.cmdout = new FastColoredTextBoxNS.FastColoredTextBox();
            this.AutoCompMenu1 = new AutocompleteMenuNS.AutocompleteMenu();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menu_bar2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fctb_main)).BeginInit();
            this.editContext.SuspendLayout();
            this.cmdoutMenu.SuspendLayout();
            this.statusOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).BeginInit();
            this.SplitContainer.Panel1.SuspendLayout();
            this.SplitContainer.Panel2.SuspendLayout();
            this.SplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmdout)).BeginInit();
            this.SuspendLayout();
            // 
            // menu_bar2
            // 
            this.menu_bar2.BackColor = System.Drawing.Color.White;
            this.menu_bar2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menu_bar2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.file_tools,
            this.edit_tools,
            this.viewToolStripMenuItem,
            this.option_tools,
            this.run_tools_parent});
            this.menu_bar2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menu_bar2.Location = new System.Drawing.Point(0, 0);
            this.menu_bar2.Name = "menu_bar2";
            this.menu_bar2.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menu_bar2.Size = new System.Drawing.Size(941, 25);
            this.menu_bar2.TabIndex = 1;
            this.menu_bar2.Text = "menu_bar";
            // 
            // file_tools
            // 
            this.file_tools.BackColor = System.Drawing.Color.Transparent;
            this.file_tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.new_tools,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.file_tools.Name = "file_tools";
            this.file_tools.Size = new System.Drawing.Size(63, 21);
            this.file_tools.Text = "Archivo";
            // 
            // new_tools
            // 
            this.new_tools.Name = "new_tools";
            this.new_tools.Size = new System.Drawing.Size(180, 22);
            this.new_tools.Text = "Nuevo";
            this.new_tools.Click += new System.EventHandler(this.new_tools_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Abrir";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Guardar";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAsToolStripMenuItem.Text = "Guardar como...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Salir";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // edit_tools
            // 
            this.edit_tools.BackColor = System.Drawing.Color.Transparent;
            this.edit_tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator3,
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator2});
            this.edit_tools.Name = "edit_tools";
            this.edit_tools.Size = new System.Drawing.Size(54, 21);
            this.edit_tools.Text = "Editar";
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.cutToolStripMenuItem.Text = "Cortar";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.copyToolStripMenuItem.Text = "Copiar";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.pasteToolStripMenuItem.Text = "Pegar";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(214, 6);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.undoToolStripMenuItem.Text = "Deshacer";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.redoToolStripMenuItem.Text = "Rehacer";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(214, 6);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showOutPanel,
            this.zoomToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
            this.viewToolStripMenuItem.Text = "Ver";
            // 
            // showOutPanel
            // 
            this.showOutPanel.Name = "showOutPanel";
            this.showOutPanel.Size = new System.Drawing.Size(180, 22);
            this.showOutPanel.Text = "Panel de salida";
            this.showOutPanel.Click += new System.EventHandler(this.showOutPanel_Click);
            // 
            // zoomToolStripMenuItem
            // 
            this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
            this.zoomToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.zoomToolStripMenuItem.Text = "Reiniciar Zoom";
            this.zoomToolStripMenuItem.Click += new System.EventHandler(this.zoomToolStripMenuItem_Click);
            // 
            // option_tools
            // 
            this.option_tools.BackColor = System.Drawing.Color.Transparent;
            this.option_tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.theme_colorstrip,
            this.fontToolStripMenuItem});
            this.option_tools.Name = "option_tools";
            this.option_tools.Size = new System.Drawing.Size(75, 21);
            this.option_tools.Text = "Opciones";
            // 
            // theme_colorstrip
            // 
            this.theme_colorstrip.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.darkm_toggle,
            this.lightm_toggle});
            this.theme_colorstrip.Name = "theme_colorstrip";
            this.theme_colorstrip.Size = new System.Drawing.Size(114, 22);
            this.theme_colorstrip.Text = "Tema";
            // 
            // darkm_toggle
            // 
            this.darkm_toggle.Name = "darkm_toggle";
            this.darkm_toggle.Size = new System.Drawing.Size(118, 22);
            this.darkm_toggle.Text = "Oscuro";
            this.darkm_toggle.Click += new System.EventHandler(this.darkm_toggle_Click);
            // 
            // lightm_toggle
            // 
            this.lightm_toggle.Checked = true;
            this.lightm_toggle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.lightm_toggle.Name = "lightm_toggle";
            this.lightm_toggle.Size = new System.Drawing.Size(118, 22);
            this.lightm_toggle.Text = "Claro";
            this.lightm_toggle.Click += new System.EventHandler(this.lightm_toggle_Click);
            // 
            // fontToolStripMenuItem
            // 
            this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
            this.fontToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.fontToolStripMenuItem.Text = "Fuente";
            this.fontToolStripMenuItem.Click += new System.EventHandler(this.fontToolStripMenuItem_Click);
            // 
            // run_tools_parent
            // 
            this.run_tools_parent.BackColor = System.Drawing.Color.Transparent;
            this.run_tools_parent.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.run_tools});
            this.run_tools_parent.Name = "run_tools_parent";
            this.run_tools_parent.Size = new System.Drawing.Size(73, 21);
            this.run_tools_parent.Text = "Compilar";
            // 
            // run_tools
            // 
            this.run_tools.Name = "run_tools";
            this.run_tools.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.run_tools.Size = new System.Drawing.Size(156, 22);
            this.run_tools.Text = "Iniciar";
            this.run_tools.Click += new System.EventHandler(this.run_tools_Click_1);
            // 
            // fctb_main
            // 
            this.fctb_main.AllowMacroRecording = false;
            this.fctb_main.AllowSeveralTextStyleDrawing = true;
            this.fctb_main.AutoCompleteBrackets = true;
            this.fctb_main.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\'',
        '<',
        '>'};
            this.AutoCompMenu1.SetAutocompleteMenu(this.fctb_main, this.AutoCompMenu1);
            this.fctb_main.AutoIndentCharsPatterns = "\r\n^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;]+);\r\n^\\s*(case|default)\\s*[^:]" +
    "*(?<range>:)\\s*(?<range>[^;]+);\r\n\\b[a-zA-z0-9_]+(:|\\(\\):);\r\n";
            this.fctb_main.AutoScrollMinSize = new System.Drawing.Size(29, 16);
            this.fctb_main.BackBrush = null;
            this.fctb_main.BracketsHighlightStrategy = FastColoredTextBoxNS.BracketsHighlightStrategy.Strategy2;
            this.fctb_main.CaretBlinking = false;
            this.fctb_main.CaretColor = System.Drawing.Color.LightCoral;
            this.fctb_main.CharHeight = 16;
            this.fctb_main.CharWidth = 9;
            this.fctb_main.CommentPrefix = "";
            this.fctb_main.ContextMenuStrip = this.editContext;
            this.fctb_main.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fctb_main.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fctb_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fctb_main.Font = new System.Drawing.Font("Courier New", 11.25F);
            this.fctb_main.Hotkeys = resources.GetString("fctb_main.Hotkeys");
            this.fctb_main.IsReplaceMode = false;
            this.fctb_main.LeftBracket = '(';
            this.fctb_main.LeftBracket2 = '{';
            this.fctb_main.LineNumberColor = System.Drawing.Color.MediumVioletRed;
            this.fctb_main.Location = new System.Drawing.Point(0, 0);
            this.fctb_main.Margin = new System.Windows.Forms.Padding(0);
            this.fctb_main.Name = "fctb_main";
            this.fctb_main.Paddings = new System.Windows.Forms.Padding(0);
            this.fctb_main.RightBracket = ')';
            this.fctb_main.RightBracket2 = '}';
            this.fctb_main.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fctb_main.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctb_main.ServiceColors")));
            this.fctb_main.ShowFoldingLines = true;
            this.fctb_main.Size = new System.Drawing.Size(939, 449);
            this.fctb_main.TabIndex = 2;
            this.fctb_main.ToolTipDelay = 200;
            this.fctb_main.Zoom = 100;
            this.fctb_main.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fctb_main_TextChanged);
            this.fctb_main.KeyPressing += new System.Windows.Forms.KeyPressEventHandler(this.fctb_main_KeyPressing);
            this.fctb_main.KeyPressed += new System.Windows.Forms.KeyPressEventHandler(this.fctb_main_KeyPressed);
            this.fctb_main.ZoomChanged += new System.EventHandler(this.fctb_main_ZoomChanged);
            this.fctb_main.Click += new System.EventHandler(this.fctb_main_Click);
            this.fctb_main.DragDrop += new System.Windows.Forms.DragEventHandler(this.fctb_main_DragDrop);
            this.fctb_main.DragEnter += new System.Windows.Forms.DragEventHandler(this.fctb_main_DragEnter);
            this.fctb_main.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmdout_KeyDown);
            this.fctb_main.KeyUp += new System.Windows.Forms.KeyEventHandler(this.fctb_main_KeyUp);
            // 
            // editContext
            // 
            this.editContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem1,
            this.rehacerToolStripMenuItem,
            this.toolStripSeparator5,
            this.cutToolStripMenuItem1,
            this.copyToolStripMenuItem1,
            this.pasteToolStripMenuItem1,
            this.toolStripSeparator6,
            this.selectAllToolStripMenuItem,
            this.toolStripSeparator7,
            this.compilarToolStripMenuItem});
            this.editContext.Name = "contextMenuStrip1";
            this.editContext.Size = new System.Drawing.Size(163, 176);
            // 
            // undoToolStripMenuItem1
            // 
            this.undoToolStripMenuItem1.Name = "undoToolStripMenuItem1";
            this.undoToolStripMenuItem1.Size = new System.Drawing.Size(162, 22);
            this.undoToolStripMenuItem1.Text = "Deshacer";
            this.undoToolStripMenuItem1.Click += new System.EventHandler(this.undoToolStripMenuItem1_Click);
            // 
            // rehacerToolStripMenuItem
            // 
            this.rehacerToolStripMenuItem.Name = "rehacerToolStripMenuItem";
            this.rehacerToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.rehacerToolStripMenuItem.Text = "Rehacer";
            this.rehacerToolStripMenuItem.Click += new System.EventHandler(this.rehacerToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(159, 6);
            // 
            // cutToolStripMenuItem1
            // 
            this.cutToolStripMenuItem1.Name = "cutToolStripMenuItem1";
            this.cutToolStripMenuItem1.Size = new System.Drawing.Size(162, 22);
            this.cutToolStripMenuItem1.Text = "Cortar";
            this.cutToolStripMenuItem1.Click += new System.EventHandler(this.cutToolStripMenuItem1_Click);
            // 
            // copyToolStripMenuItem1
            // 
            this.copyToolStripMenuItem1.Name = "copyToolStripMenuItem1";
            this.copyToolStripMenuItem1.Size = new System.Drawing.Size(162, 22);
            this.copyToolStripMenuItem1.Text = "Copiar";
            this.copyToolStripMenuItem1.Click += new System.EventHandler(this.copyToolStripMenuItem1_Click);
            // 
            // pasteToolStripMenuItem1
            // 
            this.pasteToolStripMenuItem1.Name = "pasteToolStripMenuItem1";
            this.pasteToolStripMenuItem1.Size = new System.Drawing.Size(162, 22);
            this.pasteToolStripMenuItem1.Text = "Pegar";
            this.pasteToolStripMenuItem1.Click += new System.EventHandler(this.pasteToolStripMenuItem1_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(159, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.selectAllToolStripMenuItem.Text = "Seleccionar todo";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(159, 6);
            // 
            // compilarToolStripMenuItem
            // 
            this.compilarToolStripMenuItem.Name = "compilarToolStripMenuItem";
            this.compilarToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.compilarToolStripMenuItem.Text = "Compilar";
            this.compilarToolStripMenuItem.Click += new System.EventHandler(this.compilarToolStripMenuItem_Click);
            // 
            // cmdoutMenu
            // 
            this.cmdoutMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdoutSelectAll,
            this.cmdout_cpy,
            this.cmdout_paste,
            this.cmdoutHide,
            this.cmdout_clear});
            this.cmdoutMenu.Name = "cmdoutMenu";
            this.cmdoutMenu.Size = new System.Drawing.Size(163, 114);
            // 
            // cmdoutSelectAll
            // 
            this.cmdoutSelectAll.Name = "cmdoutSelectAll";
            this.cmdoutSelectAll.Size = new System.Drawing.Size(162, 22);
            this.cmdoutSelectAll.Text = "Seleccionar todo";
            this.cmdoutSelectAll.Click += new System.EventHandler(this.cmdoutSelectAll_Click);
            // 
            // cmdout_cpy
            // 
            this.cmdout_cpy.Name = "cmdout_cpy";
            this.cmdout_cpy.Size = new System.Drawing.Size(162, 22);
            this.cmdout_cpy.Text = "Copiar";
            this.cmdout_cpy.Click += new System.EventHandler(this.cmdout_cpy_Click);
            // 
            // cmdout_paste
            // 
            this.cmdout_paste.Name = "cmdout_paste";
            this.cmdout_paste.Size = new System.Drawing.Size(162, 22);
            this.cmdout_paste.Text = "Pegar";
            this.cmdout_paste.Click += new System.EventHandler(this.cmdout_paste_Click);
            // 
            // cmdoutHide
            // 
            this.cmdoutHide.Name = "cmdoutHide";
            this.cmdoutHide.Size = new System.Drawing.Size(162, 22);
            this.cmdoutHide.Text = "Ocultar";
            this.cmdoutHide.Click += new System.EventHandler(this.cmdoutHide_Click);
            // 
            // cmdout_clear
            // 
            this.cmdout_clear.Name = "cmdout_clear";
            this.cmdout_clear.Size = new System.Drawing.Size(162, 22);
            this.cmdout_clear.Text = "Limpiar";
            this.cmdout_clear.Click += new System.EventHandler(this.cmdout_clear_Click);
            // 
            // statusOptions
            // 
            this.statusOptions.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.statusOptions.GripMargin = new System.Windows.Forms.Padding(1);
            this.statusOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stat_txt});
            this.statusOptions.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusOptions.Location = new System.Drawing.Point(0, 591);
            this.statusOptions.Name = "statusOptions";
            this.statusOptions.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusOptions.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusOptions.Size = new System.Drawing.Size(941, 22);
            this.statusOptions.Stretch = false;
            this.statusOptions.TabIndex = 5;
            this.statusOptions.Text = "Language";
            // 
            // stat_txt
            // 
            this.stat_txt.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.stat_txt.Margin = new System.Windows.Forms.Padding(10, 4, 0, 2);
            this.stat_txt.Name = "stat_txt";
            this.stat_txt.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.stat_txt.Size = new System.Drawing.Size(34, 16);
            this.stat_txt.Text = "Lines";
            // 
            // SplitContainer
            // 
            this.SplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer.Location = new System.Drawing.Point(0, 25);
            this.SplitContainer.Name = "SplitContainer";
            this.SplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer.Panel1
            // 
            this.SplitContainer.Panel1.Controls.Add(this.fctb_main);
            this.SplitContainer.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SplitContainer.Panel1MinSize = 260;
            // 
            // SplitContainer.Panel2
            // 
            this.SplitContainer.Panel2.Controls.Add(this.cmdout);
            this.SplitContainer.Panel2MinSize = 100;
            this.SplitContainer.Size = new System.Drawing.Size(941, 566);
            this.SplitContainer.SplitterDistance = 451;
            this.SplitContainer.TabIndex = 7;
            // 
            // cmdout
            // 
            this.cmdout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdout.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.AutoCompMenu1.SetAutocompleteMenu(this.cmdout, null);
            this.cmdout.AutoScrollMinSize = new System.Drawing.Size(2, 14);
            this.cmdout.BackBrush = null;
            this.cmdout.CaretBlinking = false;
            this.cmdout.CharHeight = 14;
            this.cmdout.CharWidth = 8;
            this.cmdout.ContextMenuStrip = this.cmdoutMenu;
            this.cmdout.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cmdout.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.cmdout.IsReplaceMode = false;
            this.cmdout.Location = new System.Drawing.Point(3, 3);
            this.cmdout.Name = "cmdout";
            this.cmdout.Paddings = new System.Windows.Forms.Padding(0);
            this.cmdout.ReadOnly = true;
            this.cmdout.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.cmdout.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("cmdout.ServiceColors")));
            this.cmdout.ShowLineNumbers = false;
            this.cmdout.Size = new System.Drawing.Size(933, 103);
            this.cmdout.TabIndex = 1;
            this.cmdout.Zoom = 100;
            this.cmdout.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmdout_KeyDown);
            // 
            // AutoCompMenu1
            // 
            this.AutoCompMenu1.AllowsTabKey = true;
            this.AutoCompMenu1.AppearInterval = 8;
            this.AutoCompMenu1.Colors = ((AutocompleteMenuNS.Colors)(resources.GetObject("AutoCompMenu1.Colors")));
            this.AutoCompMenu1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutoCompMenu1.ImageList = null;
            this.AutoCompMenu1.Items = new string[0];
            this.AutoCompMenu1.LeftPadding = 1;
            this.AutoCompMenu1.MaximumSize = new System.Drawing.Size(200, 200);
            this.AutoCompMenu1.MinFragmentLength = 1;
            this.AutoCompMenu1.SearchPattern = "\\w";
            this.AutoCompMenu1.TargetControlWrapper = null;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(941, 566);
            this.panel1.TabIndex = 8;
            // 
            // NoteMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 613);
            this.Controls.Add(this.SplitContainer);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusOptions);
            this.Controls.Add(this.menu_bar2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menu_bar2;
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "NoteMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MiniCSharp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NoteMain_FormClosing);
            this.Load += new System.EventHandler(this.NoteMain_Load);
            this.menu_bar2.ResumeLayout(false);
            this.menu_bar2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fctb_main)).EndInit();
            this.editContext.ResumeLayout(false);
            this.cmdoutMenu.ResumeLayout(false);
            this.statusOptions.ResumeLayout(false);
            this.statusOptions.PerformLayout();
            this.SplitContainer.Panel1.ResumeLayout(false);
            this.SplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).EndInit();
            this.SplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmdout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menu_bar2;
        private System.Windows.Forms.ToolStripMenuItem edit_tools;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem option_tools;
        private System.Windows.Forms.ToolStripMenuItem theme_colorstrip;
        private System.Windows.Forms.ToolStripMenuItem darkm_toggle;
        private System.Windows.Forms.ToolStripMenuItem lightm_toggle;
        private System.Windows.Forms.ToolStripMenuItem fontToolStripMenuItem;
        private FastColoredTextBoxNS.FastColoredTextBox fctb_main;
        private System.Windows.Forms.ToolStripMenuItem run_tools_parent;
        private System.Windows.Forms.StatusStrip statusOptions;
        private System.Windows.Forms.ToolStripStatusLabel stat_txt;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.SplitContainer SplitContainer;
        private System.Windows.Forms.ContextMenuStrip cmdoutMenu;
        private System.Windows.Forms.ToolStripMenuItem cmdoutSelectAll;
        private System.Windows.Forms.ToolStripMenuItem cmdoutHide;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showOutPanel;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip editContext;
        private System.Windows.Forms.ToolStripMenuItem run_tools;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem1;
        private AutocompleteMenuNS.AutocompleteMenu AutoCompMenu1;
        private System.Windows.Forms.ToolStripMenuItem zoomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cmdout_clear;
        private System.Windows.Forms.ToolStripMenuItem cmdout_cpy;
        private System.Windows.Forms.ToolStripMenuItem cmdout_paste;
        private FastColoredTextBoxNS.FastColoredTextBox cmdout;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem file_tools;
        private System.Windows.Forms.ToolStripMenuItem new_tools;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compilarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rehacerToolStripMenuItem;
    }
}

