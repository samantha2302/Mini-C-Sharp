using System.Drawing;
using System.Windows.Forms;

namespace ColorsNew
{

    //Todos los metodos de esta clase fueron declarados para manejar los colores del editor grafico de textos, son necesarios para que funcione
    //correctamente, fue creado por Fus3n: https://github.com/Fus3n/cnote

    public class ToolBarTheme_Light : ToolStripProfessionalRenderer
        {
            public ToolBarTheme_Light() : base(new MyColors()) { }
        }

        public class MyColors : ProfessionalColorTable
        { 
            public override Color MenuItemPressedGradientBegin
            {
                get { return Color.Transparent; }
            }

            public override Color MenuItemPressedGradientEnd
            {
                get { return Color.Transparent; }
            }

            public override Color MenuItemSelected
            {
                get { return Color.LightGray; }
            }

            public override Color MenuItemSelectedGradientBegin
            {
                get { return Color.Transparent; }
            }
            public override Color MenuItemSelectedGradientEnd
            {
                get { return Color.Transparent; }
            }
        }


    public class ToolBarTheme_Dark : ToolStripProfessionalRenderer
    {
        public ToolBarTheme_Dark() : base(new MyColorsDark()) { }
    }


    public class MyColorsDark : ProfessionalColorTable
    {
        public override Color MenuItemPressedGradientBegin
        {
            get { return Color.Transparent; }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get { return Color.Transparent; }
        }

        public override Color MenuItemSelected
        {
            get { return Color.DimGray; }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get { return Color.Transparent; }
        }
        public override Color MenuItemSelectedGradientEnd
        {
            get { return Color.Transparent; }
        }
    }

}

