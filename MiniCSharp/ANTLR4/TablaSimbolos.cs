using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Windows.Forms;
using Antlr4.Runtime;

namespace MiniCSharp.ANTLR4
{
    public class TablaSimbolos {
        LinkedList<Object> tabla;

        private static int nivelActual;

        /*
         -Tok almacena el token del identificador,
         -Type se guia por medio de un numero que representara el tipo del identificador,
         -SecondType es utilizado para tipos list y array, ya que, a pesar de que son de este tipo, estos mismos pueden
         tener otros tipos.
         -Nivel almacena a que nivel se encuentra el identificador.
         -IsMethod se marcara como true unicamente si el identificador es un metodo.
         -IsClass se marcara como true unicamente si el identificador es una clase.
         -IsVarMethod se marcara como true unicamente si el identificador es utilizado como un parametro de un metodo.
         -InstanceToken almacena el token de un identificador de tipo clase.
        */
        public class Ident{
            IToken tok;
            
            int type;
            
            int secondType;
            
            int nivel;

            bool isMethod;

            bool isClass;

            bool isVarMethod;
            
            IToken instanceToken;

            public Ident(IToken t, int tp, int stp, bool ism, bool isc, bool isvm, IToken iT){
                tok = t;
                type = tp;
                secondType = stp;
                nivel=nivelActual;
                isMethod=ism;
                isClass = isc;
                isVarMethod = isvm;
                instanceToken = iT;
            }
            
            public IToken GetToken()
            {
                return tok;
            }
            
            public int GetType()
            {
                return type;
            }
            
            public int GetSecondType()
            {
                return secondType;
            }
            
            
            public int GetNivel()
            {
                return nivel;
            }
            
            public bool GetIsMethod()
            {
                return isMethod;
            }
            
            public bool GetIsClass()
            {
                return isClass;
            }
            
            public bool GetIsVarMethod()
            {
                return isVarMethod;
            }
            
            public IToken GetInstanceToken()
            {
                return instanceToken;
            }
            
            public void SetInstanceToken(IToken token)
            {
                instanceToken=token;
            }

        }

        public TablaSimbolos() {
            tabla = new LinkedList<Object>();
            nivelActual = -1;
        }

        public void insertar(IToken t, int tp,int stp, bool ism, bool isc, bool isvm, IToken iT)
        {
            Ident i = new Ident(t,tp,stp,ism,isc, isvm, iT);
            tabla.AddFirst(i);
        }

        /*
         Este metodo permite buscar un token por medio de un nombre devolviendo el Ident de la tabla.
         */
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

        /*
         Este metodo permite buscar el nivel por medio del nombre y el nivel devolviendo el nivel del Ident buscado.
         */
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
        
        /*
         Este metodo permite buscar el segundo tipo de un array por medio del nombre y el nivel devolviendo el segundo tipo del Ident buscado.
        */
        public int buscarSegundoTipoArray(String nombre, int nivel){
            foreach (Object id in tabla){
                if (((Ident)id).GetToken().Text.Equals(nombre)){
                    if (((Ident)id).GetNivel().Equals(nivel))
                    {
                        if (((Ident)id).GetType().Equals(13))
                        {
                            return ((Ident)id).GetSecondType();
                        }
                    }
                }
            }
            return -1;
        }
        
        /*
         Este metodo permite buscar el segundo tipo de un list por medio del nombre y el nivel devolviendo el segundo tipo del Ident buscado.
        */
        public int buscarSegundoTipoList(String nombre, int nivel){
            foreach (Object id in tabla){
                if (((Ident)id).GetToken().Text.Equals(nombre)){
                    if (((Ident)id).GetNivel().Equals(nivel))
                    {
                        if (((Ident)id).GetType().Equals(10))
                        {
                            return ((Ident)id).GetSecondType();
                        }
                    }
                }
            }
            return -1;
        }
        
        /*
         Este metodo permite buscar el token por medio del nombre y el nivel devolviendo el Ident buscado.
        */
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
        
        /*
         Este metodo permite buscar el token de un metodo por medio del nombre devolviendo el Ident buscado.
        */        
        public Ident buscarTokenMetodoNombre(String nombre){
            foreach (Object id in tabla){
                if (((Ident)id).GetToken().Text.Equals(nombre)){
                    if (((Ident)id).GetIsMethod().Equals(true))
                    {
                        return (Ident)id;
                    }
                }
            }
            return null;
        }
        
        /*
         Este metodo permite obtener los tipos de los identificadores de los metodos por medio del nivel devolviendo una lista
         de todos los tipos que tienen los identificadores.
        */        
        public List<int> obtenerTiposMetodosVariables(int nivel)
        {
            List<int> resul = new List<int>();
            foreach (Object id in tabla){
                if (((Ident)id).GetNivel().Equals(nivel)){
                    if (((Ident)id).GetIsVarMethod().Equals(true))
                    {
                        resul.Add(((Ident)id).GetType());
                    }
                }
            }

            return resul;
        }
        
        /*
         Este metodo permite buscar el nivel del metodo actual devolviendo el nivel del Ident.
        */
        public int buscarNivelMetodo(){
            foreach (Object id in tabla){
                if (((Ident)id).GetIsMethod().Equals(true))
                {
                    return ((Ident)id).GetNivel();
                }
            }
            return -1;
        }
        
        /*
         Este metodo permite buscar el token del metodo actual devolviendo el Ident.
        */
        public Ident buscarTokenMetodo(){
            foreach (Object id in tabla){
                if (((Ident)id).GetIsMethod().Equals(true))
                {
                    return (Ident)id;
                }
            }
            return null;
        }
        
        /*
         Este metodo permite buscar el token de una clase por medio de un nombre devolviendo el Ident buscado.
        */
        public Ident buscarTokenClaseNombre(String nombre){
            foreach (Object id in tabla){
                if (((Ident)id).GetToken().Text.Equals(nombre)){
                    if (((Ident)id).GetIsClass().Equals(true))
                    {
                        return (Ident)id;
                    }
                }
            }
            return null;
        }
        
        /*
         Este metodo permite modificar el token de una instancia por medio del nombre, el nivel y el token instancia
         devolviendo el Ident buscado.
        */
        public Ident ModificarTokenInstancia(String nombre, int nivel, IToken instancia)
        {
            foreach (Object id in tabla)
            {
                if (((Ident)id).GetToken().Text.Equals(nombre)){
                    if (((Ident)id).GetNivel().Equals(nivel))
                    {
                        if (((Ident)id).GetInstanceToken() == null)
                        {
                            ((Ident)id).SetInstanceToken(instancia);
                            return (Ident)id;
                        }
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
        
        public void DecrementarNivel(){
            nivelActual--;
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

                string textoInstancia;
                
                if (((Ident)tabla.ElementAt(i)).GetInstanceToken() != null)
                {
                    textoInstancia = ((Ident)tabla.ElementAt(i)).GetInstanceToken().Text;
                }
                else
                {
                    textoInstancia = "NO";
                }
                
                builder.Append("\n" + "Nombre: " + s.Text + " -- Tipo: " + ((Ident) tabla.ElementAt(i)).GetType() + " -- SegundoTipo: " + ((Ident) tabla.ElementAt(i)).GetSecondType() + " -- Nivel: " + ((Ident) tabla.ElementAt(i)).GetNivel() + " -- EsMetodo: " + ((Ident) tabla.ElementAt(i)).GetIsMethod());
                builder.Append("\n" + "EsClase: " + ((Ident) tabla.ElementAt(i)).GetIsClass()+ " -- EsVarMetodo: " + ((Ident) tabla.ElementAt(i)).GetIsVarMethod() + " -- Nombre Instancia : " + textoInstancia + "\n");
            }
            builder.Append("\n" +"----- FIN TABLA ------");
            return builder.ToString();
        }
    }

    
    
}