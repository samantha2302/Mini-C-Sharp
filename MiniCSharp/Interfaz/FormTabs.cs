//Esta clase permite manejar multiples pestanas del editor grafico.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniCSharp.Interfaz
{
    //Antes de iniciar se ajusta el tamano de la ventana.
    public partial class FormTabs : Form
    {
        int contador;
        public FormTabs()
        {
            InitializeComponent();
            AdjustTabControlSize();
        }

        //Cada vez que la persona haga click al la ultima pestana se agregara una nueva ventana.
        private void AddTabPage()
        {
            CNote.NoteMain form = new CNote.NoteMain();
            var tabPage = new TabPage("Ventana " + contador++);

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            tabPage.Controls.Add(form);
            form.Show();

            tabControl1.TabPages.Add(tabPage);


            form.FormClosing += new FormClosingEventHandler(FormClosingEventHandler);
        }

        //Se verificara cuando se cierra una pestana individual se pueda o no eliminarla en el caso de que tenga informacion.

        private void FormClosingEventHandler(object sender, FormClosingEventArgs e)
        {
            TabPage tabPage = null;
            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Controls.Contains((Control)sender))
                {
                    tabPage = tab;
                    break;
                }
            }
            if (tabPage != null)
            {
                foreach (TabPage tabPage1 in tabControl1.TabPages)
                {
                    if (e.Cancel==false)
                    {
                        tabPage.Controls.Remove((Control)sender);
                        tabPage.Dispose();
                    }
                }
            }

        }

        //Se verificara antes de salir si todas las pestanas se pueden cerrar en el caso de que alguna no pueda se evitara el cierre.
        private void FormTabs_FormClosing(object sender, FormClosingEventArgs e)
        {
            TabPage tabPage = this.Parent as TabPage;

            if (tabPage != null)
            {
                tabControl1.TabPages.Remove(tabPage);
            }

            if (e.CloseReason == CloseReason.UserClosing)
            {
                foreach (TabPage tabPage1 in tabControl1.TabPages)
                {

                    if (tabPage1.Controls.Count > 0 && tabPage1.Controls[0] is Form form)
                    {
                        form.Close();
                        if (form.IsHandleCreated)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }
        }

        //Se ajustara el tamano de la ventana.
        private void AdjustTabControlSize()
        {
            var workingArea = Screen.PrimaryScreen.WorkingArea;

            this.Size = workingArea.Size;
            this.Location = workingArea.Location;

            var newLocation = new Point(0, 0);
            var newWidth = workingArea.Width;
            var newHeight = workingArea.Height - tabControl1.Top;

            tabControl1.Location = newLocation;
            tabControl1.Size = new Size(newWidth, newHeight-20);
        }

        private void FormTabs_SizeChanged(object sender, EventArgs e)
        {
        }

        //Se agregara una pestana nueva al final cuando se manipulen.
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == tabControl1.TabCount - 1)
            {
                AddTabPage();
            }
        }

        //Se agregaran dos pestanas al momento de iniciar, en el momento de que se toque la ultima aparecera una nueva.
        private void FormTabs_Load(object sender, EventArgs e)
        {
            AddTabPage();
            AddTabPage();
        }

        //Se verificara el estado de la ventana para modificar su tamano.
        private void FormTabs_Resize(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Maximized)
            {
                tabControl1.Size = this.ClientSize;
            }

            if (WindowState == FormWindowState.Maximized)
            {
                AdjustTabControlSize();
            }
        }
    }
    }
