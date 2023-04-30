using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Antlr4.Runtime;

namespace MiniCSharp.ANTLR4
{
    public class TablaSimbolos {
        LinkedList<Object> tabla;

        private static int nivelActual;

        public class Ident{
            IToken tok;
            int type; //esto probablemente cambie a un tipo más estructurado
            int nivel;
            int valor;

            bool isMethod;

            bool isInstance;
            
            IToken instanceToken;

            public Ident(IToken t, int tp, bool ism, bool isI, IToken iT){
                tok = t;
                type = tp;
                nivel=nivelActual;
                valor = 0;
                isMethod=ism;
                isInstance = isI;
                instanceToken = iT;
            }
            
            public IToken GetToken()
            {
                return tok;
            }
            
            public bool GetIsMethod()
            {
                return isMethod;
            }
            
            public bool GetIsInstance()
            {
                return isInstance;
            }
            
            public int GetType()
            {
                return type;
            }

            public int GetNivelActual()
            {
                return nivelActual;
            }

            public void SetNivelActual(int nivel_actual)
            {
                nivelActual = nivel_actual;
            }
            
            public int GetNivel()
            {
                return nivel;
            }

            public void setValue(int v){
                valor = v;
            }

        }

        public TablaSimbolos() {
            tabla = new LinkedList<Object>();
            nivelActual = -1;
        }

        public void insertar(IToken t, int tp, bool ism, bool isI, IToken iT)
        {
            Ident i = new Ident(t,tp,ism, isI, iT);
            tabla.AddFirst(i);
        }

        public Ident buscar(String nombre)
        {
            foreach (Object id in tabla)
            {
                if (((Ident)id).GetToken().Text.Equals(nombre))
                {
                    return (Ident)id;
                }
            }
            return null;
        }

        public int buscarNivel(String nombre, int nivel){
            foreach (Object id in tabla){
                if (((Ident)id).GetToken().Text.Equals(nombre)){
                    if (((Ident)id).GetNivel().Equals(nivel))
                    {
                        return ((Ident)id).GetNivel();
                    }
                }
            }
            return -1;
        }
        
        public Ident buscarToken(String nombre, int nivel){
            foreach (Object id in tabla){
                if (((Ident)id).GetToken().Text.Equals(nombre)){
                    if (((Ident)id).GetNivel().Equals(nivel))
                    {
                        return (Ident)id;
                    }
                }
            }
            return null;
        }
        
        public int buscarNivelMetodo(){
            foreach (Object id in tabla){
                if (((Ident)id).GetIsMethod().Equals(true))
                {
                    //MessageBox.Show(((Ident)id).GetToken().Text);
                    return ((Ident)id).GetNivel();
                }
            }
            return -1;
        }

        public int buscarMetodo(string nombre, int tipo)
        {
            foreach (Object id in tabla)
            {
                if (((Ident)id).GetToken().Text.Equals(nombre))
                {
                    if (((Ident)id).GetType().Equals(tipo))
                    {
                        return ((Ident)id).GetNivel();
                    }
                }
            }
            return -1;
        }
        
        public IToken buscarMetodoToken(string nombre, int tipo)
        {
            foreach (Object id in tabla)
            {
                if (((Ident)id).GetToken().Text.Equals(nombre))
                {
                    if (((Ident)id).GetType().Equals(tipo))
                    {
                        return ((Ident)id).GetToken();
                    }
                }
            }
            return null;
        }

        public int obtenerNivelActual(){
            return nivelActual;
        }


        public void openScope(){
            nivelActual++;
        }

        public void closeScope(){
            for (int i = tabla.Count - 1; i >= 0; i--)
            {
                object n = tabla.ElementAt(i);
                if (((Ident)n).GetNivel() == nivelActual)
                {
                    tabla.Remove(n);
                }
            }
            nivelActual--;
        }

        public String imprimir() {
            StringBuilder builder = new StringBuilder();
            builder.Append("\n" +"----- INICIO TABLA ------");
            for (int i = 0; i < tabla.Count(); i++) {
                IToken s = (IToken) ((Ident) tabla.ElementAt(i)).GetToken();
                builder.Append("\n" + "Nombre: " + s.Text + " - " + ((Ident) tabla.ElementAt(i)).GetNivel() + " - " + ((Ident) tabla.ElementAt(i)).GetType() + " - " + ((Ident) tabla.ElementAt(i)).GetIsMethod());
            }
            builder.Append("\n" +"----- FIN TABLA ------");
            return builder.ToString();
        }
    }

    
    
}