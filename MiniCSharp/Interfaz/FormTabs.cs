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
    public partial class FormTabs : Form
    {
        int contador;
        public FormTabs()
        {
            InitializeComponent();
            AdjustTabControlSize();
        }

        // Método para agregar una nueva ventana al TabControl
        private void AddTabPage()
        {
            CNote.NoteMain form = new CNote.NoteMain();
            var tabPage = new TabPage("Ventana " + contador++);
            // Crear una nueva pestaña con el título dado
            //TabPage tabPage = new TabPage(text);

            // Agregar la ventana al contenido de la pestaña
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            tabPage.Controls.Add(form);
            form.Show();

            // Agregar la pestaña al TabControl
            tabControl1.TabPages.Add(tabPage);

            // Asignar la pestaña recién creada como la pestaña activa
            //tabControl1.SelectedTab = tabPage;

            form.FormClosing += new FormClosingEventHandler(FormClosingEventHandler);
        }

        private void FormClosingEventHandler(object sender, FormClosingEventArgs e)
        {
            // Obtener la pestaña correspondiente a la ventana que se está cerrando
            TabPage tabPage = null;
            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Controls.Contains((Control)sender))
                {
                    tabPage = tab;
                    break;
                }
            }

            // Si se encontró la pestaña, cerrar la ventana y eliminar la pestaña del TabControl
            if (tabPage != null)
            {
                //MessageBox.Show("dxd");
                //tabPage.Controls.Remove((Control)sender);
                //tabPage.Dispose();
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


        private void FormTabs_FormClosing(object sender, FormClosingEventArgs e)
        {
            TabPage tabPage = this.Parent as TabPage;

            // Si existe la pestaña, eliminarla del TabControl
            if (tabPage != null)
            {
                tabControl1.TabPages.Remove(tabPage);
            }

            if (e.CloseReason == CloseReason.UserClosing)
            {
                foreach (TabPage tabPage1 in tabControl1.TabPages)
                {
                    //Obtener el formulario de la pestaña actual
                    //Form form = tabPage1.Controls[0] as Form;

                    // Verificar si el formulario actual puede cerrar
                    //if (!VerificarCierreFormulario(form))
                    //{
                    // Cancelar el cierre del formulario principal
                    //e.Cancel = true;
                    //return;
                    ////}

                    if (tabPage1.Controls.Count > 0 && tabPage1.Controls[0] is Form form)
                    {
                        form.Close();
                        if (form.IsHandleCreated)
                        {
                            // Si el formulario no se cerró correctamente, cancelar el cierre del programa
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }
        }

        private bool VerificarCierreFormulario(Form form)
        {
            // Realizar validaciones necesarias en el formulario
            // ...

            // Si el formulario no puede cerrar, devolver false
            if (2 +2 ==2)
            {
                return false;
            }

            // Si el formulario puede cerrar, devolver true
            return true;
        }

        private void AdjustTabControlSize()
        {
            var workingArea = Screen.PrimaryScreen.WorkingArea;

            this.Size = workingArea.Size;
            this.Location = workingArea.Location;

            // Calcular la nueva posición y tamaño para el TabControl
            var newLocation = new Point(0, 0);
            var newWidth = workingArea.Width;
            var newHeight = workingArea.Height - tabControl1.Top;

            // Establecer la nueva posición y tamaño para el TabControl
            tabControl1.Location = newLocation;
            tabControl1.Size = new Size(newWidth, newHeight-20);
        }

        private void FormTabs_SizeChanged(object sender, EventArgs e)
        {
            // establecer el tamaño y la posición del formulario
            //AdjustTabControlSize();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == tabControl1.TabCount - 1)
            {
                // Agregar una nueva pestaña y seleccionarla
                AddTabPage();
            }
        }

        private void FormTabs_Load(object sender, EventArgs e)
        {
            //TabPage newTabPage = new TabPage("Nueva pestaña");
           // tabControl1.TabPages.Add(newTabPage);
            AddTabPage();
            AddTabPage();
            //AdjustTabControlSize();
        }

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
