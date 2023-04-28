namespace MiniCSharp.Interfaz
{
    partial class Prueba
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Prueba));
            this.cmdout = new FastColoredTextBoxNS.FastColoredTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.cmdout)).BeginInit();
            this.SuspendLayout();
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
            this.cmdout.AutoScrollMinSize = new System.Drawing.Size(2, 14);
            this.cmdout.BackBrush = null;
            this.cmdout.CaretBlinking = false;
            this.cmdout.CharHeight = 14;
            this.cmdout.CharWidth = 8;
            this.cmdout.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cmdout.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.cmdout.IsReplaceMode = false;
            this.cmdout.Location = new System.Drawing.Point(14, 12);
            this.cmdout.Name = "cmdout";
            this.cmdout.Paddings = new System.Windows.Forms.Padding(0);
            this.cmdout.ReadOnly = true;
            this.cmdout.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.cmdout.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("cmdout.ServiceColors")));
            this.cmdout.ShowLineNumbers = false;
            this.cmdout.Size = new System.Drawing.Size(933, 557);
            this.cmdout.TabIndex = 2;
            this.cmdout.Zoom = 100;
            // 
            // Prueba
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 581);
            this.Controls.Add(this.cmdout);
            this.Name = "Prueba";
            this.Text = "Prueba";
            this.Load += new System.EventHandler(this.Prueba_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmdout)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FastColoredTextBoxNS.FastColoredTextBox cmdout;
    }
}