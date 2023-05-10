using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Interop;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using generated;

namespace MiniCSharp.ANTLR4
{
    public class AContextual : MiniCSharpParserBaseVisitor<object>
    {
        private TablaSimbolos laTabla;
        public ArrayList<String> errorMsgs = new ArrayList<String>();
        public int designatorAssign;
        public int methodType;
        public bool isList;
        public bool isArray;
        public TablaSimbolos.Ident instanceToken;
        public bool isVarInClass;
        
        public AContextual(){
            this.laTabla = new TablaSimbolos();
            this.errorMsgs = new ArrayList<String>();
        }


        public void variablesPredeclaradas()
        {
            //1) null; valor nulo de una clase o arreglo variable. 
            laTabla.insertar(new MyToken("null"), 21,-1, false, false, false, null);
        }

        public void metodosPredeclarados()
        {
            //2) chr(i); convierte el entero “i” a carácter.
            laTabla.openScope();
            laTabla.insertar(new MyToken("chr"), 4,-1, true, false, false, null);
            laTabla.insertar(new MyToken("i"), 0,-1, false, false, true, null);
            
            //3) ord(ch); convierte el carácter ch en entero.
            laTabla.openScope();
            laTabla.insertar(new MyToken("ord"), 0,-1, true, false, false, null);
            laTabla.insertar(new MyToken("ch"), 4,-1, false, false, true, null);
            
            //4) len(a); retorna el número de elementos de un arreglo/lista..
            laTabla.openScope();
            laTabla.insertar(new MyToken("len"), 0,-1, true, false, false, null);
            laTabla.insertar(new MyToken("a"), 15,-1, false, false, true, null);
            
            //5) add(a,e); agrega un elemento "e" a una lista/arreglo "a"..
            laTabla.openScope();
            laTabla.insertar(new MyToken("add"), 11,-1, true, false, false, null);
            laTabla.insertar(new MyToken("a"), 15,-1, false, false, true, null);
            laTabla.insertar(new MyToken("e"), 15,-1, false, false, true, null);
            
            //6) del(a,i) elimina el elemento del index "i" de la lista/arreglo "a", si existe..
            laTabla.openScope();
            laTabla.insertar(new MyToken("del"), 11,-1, true, false, false, null);
            laTabla.insertar(new MyToken("a"), 15,-1, false, false, true, null);
            laTabla.insertar(new MyToken("i"), 0,-1, false, false, true, null);
        }
        
        public Boolean hasErrors()
        {
            return this.errorMsgs.Count > 0;
        }
        
        public String toString()
        {
            if (!hasErrors()) return "0 errores";
            StringBuilder builder = new StringBuilder();
            foreach (String s in errorMsgs)
            {
                builder.Append("\n"+s);
            }
            return builder.ToString();
        }
        
        private String showType(int type){
            switch(type){
                case 0: return "int";
                case 1: return "int?";
                case 2: return "double";
                case 3: return "double?";
                case 4: return "char";
                case 5: return "char?";
                case 6: return "string";
                case 7: return "string?";
                case 8: return "bool";
                case 9: return "bool?";
                case 10: return "list";
                case 11: return "void";
                case 12: return "class";
                case 13: return "array";
                case 14: return "inst";
                case 15: return "var";
                case 21: return "null";
                default: return "none";
            }
        }
        
        private String showErrorPosition(IToken t){
            return " Fila: "+t.Line + " - Columna: " + (t.Column+1);
        }
        private bool isMultitype(String op){
            switch (op){
                case "==": return true;
                case "!=": return true;
                default:  return false;
            }
        }
        public override object VisitProgramAST(MiniCSharpParser.ProgramASTContext context)
        {
            for (int i = 0; context.@using().Count() > i; i++)
            {
                Visit(context.@using(i));
            }
            laTabla.openScope();
            try {
                IToken id = context.IDENTIFIER().Symbol;
                int idType = 12;
                TablaSimbolos.Ident i = laTabla.buscar(context.IDENTIFIER().GetText());
                if (i == null && laTabla.buscarNivel(context.IDENTIFIER().GetText(), laTabla.obtenerNivelActual()) == -1)
                {
                    if (context.IDENTIFIER().GetText() != "chr" && context.IDENTIFIER().GetText() != "ord" && context.IDENTIFIER().GetText() != "len" &&
                        context.IDENTIFIER().GetText() != "add" && context.IDENTIFIER().GetText() != "del")
                    {
                        laTabla.insertar(id, idType,-1, false, true, false, null);
                    }
                    else
                    {
                        errorMsgs.Add("\n" + "Error de clase, identificador \"" + context.IDENTIFIER().GetText() + "\" ya fue declarado." + showErrorPosition(context.IDENTIFIER().Symbol));
                    }
                }else{
                    errorMsgs.Add("\n" + "Error de clase, identificador \"" + context.IDENTIFIER().GetText() + "\" ya fue declarado." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            } catch (Exception e){}
            
            variablesPredeclaradas();
            
            for (int i = 0; context.varDecl().Count() > i; i++)
            {
                Visit(context.varDecl(i));
            }
            
            for (int i = 0; context.classDecl().Count() > i; i++)
            {
                laTabla.openScope();
                Visit(context.classDecl(i));
            }
            
            metodosPredeclarados();

            for (int i = 0; context.methodDecl().Count() > i; i++)
            {
                laTabla.openScope();
                Visit(context.methodDecl(i));
            }
            //MessageBox.Show(laTabla.imprimir()+ "\n" + toString());
            MessageBox.Show(laTabla.imprimir());
            MessageBox.Show(toString());
            laTabla.closeScope();
            return null;
        }

        //No es necesario.
        public override object VisitUsingAST(MiniCSharpParser.UsingASTContext context)
        {
            return null;
        }

        public override object VisitVarDeclAST(MiniCSharpParser.VarDeclASTContext context)
        {
            try {
                IToken id = context.IDENTIFIER(0).Symbol;
                int idType = (int) Visit(context.type());
                TablaSimbolos.Ident i = laTabla.buscar(context.IDENTIFIER(0).GetText());
                
                if (isVarInClass.Equals(true) && idType==14)
                {
                    errorMsgs.Add("\n" + "Error de variable, variable \"" + context.IDENTIFIER(0).GetText() + "\" no puede utilizar el tipo \"" + showType(14) + "\" en una clase." + showErrorPosition(context.IDENTIFIER(0).Symbol));
                    return null;
                }

                if (i == null || i != null && laTabla.buscarNivel(context.IDENTIFIER(0).GetText(), laTabla.obtenerNivelActual()) == -1)
                {
                    int verificar = -1;
                    if (laTabla.buscarNivelMetodo() != -1)
                    {
                        for (int p = laTabla.buscarNivelMetodo(); laTabla.obtenerNivelActual() > p; p++)
                        {
                            if (laTabla.buscarNivel(context.IDENTIFIER(0).GetText(), p) != -1)
                            {
                                errorMsgs.Add("\n" + "Error de variable, variable \"" + context.IDENTIFIER(0).GetText() + "\" ya fue declarada." + showErrorPosition(context.IDENTIFIER(0).Symbol));
                                verificar = 1;
                            }
                        }
                    }

                    if (verificar == -1 && isList.Equals(false)&& isArray.Equals(false) && idType != -1)
                    {
                        laTabla.insertar(id, idType,-1, false, false, false, null);
                    }
                    
                    if (verificar == -1 && isArray.Equals(true)&& idType != -1)
                    {
                        laTabla.insertar(id, 13,idType, false, false, false, null);
                    }

                    if (verificar == -1 && isList.Equals(true) && idType != -1)
                    {
                        laTabla.insertar(id, 10,idType, false, false, false, null);
                    }
                    
                }else{
                    errorMsgs.Add("\n" + "Error de variable, variable \"" + context.IDENTIFIER(0).GetText() + "\" ya fue declarada." + showErrorPosition(context.IDENTIFIER(0).Symbol));
                }
                
                for (int sum = 1; context.IDENTIFIER().Count() > sum; sum++)
                {
                    IToken idN = context.IDENTIFIER(sum).Symbol;
                    TablaSimbolos.Ident iN = laTabla.buscar(context.IDENTIFIER(sum).GetText());
                    if (iN == null || iN != null && laTabla.buscarNivel(context.IDENTIFIER(sum).GetText(), laTabla.obtenerNivelActual()) == -1) {
                        
                        int verificar = -1;
                        if (laTabla.buscarNivelMetodo() != -1)
                        {
                            for (int p = laTabla.buscarNivelMetodo(); laTabla.obtenerNivelActual() > p; p++)
                            {
                                if (laTabla.buscarNivel(context.IDENTIFIER(sum).GetText(), p) != -1)
                                {
                                    errorMsgs.Add("\n" + "Error de variable, variable \"" + context.IDENTIFIER(sum).GetText() + "\" ya fue declarada." + showErrorPosition(context.IDENTIFIER(0).Symbol));
                                    verificar = 1;
                                }
                            }
                        }

                        if (verificar == -1 && isList.Equals(false)&& isArray.Equals(false) && idType != -1)
                        {
                            laTabla.insertar(idN, idType,-1, false, false, false, null);
                        }
                        
                        if (verificar == -1 && isArray.Equals(true)&& idType != -1)
                        {
                            laTabla.insertar(idN, 13,idType, false, false, false, null);
                        }
                        
                        if (verificar == -1 && isList.Equals(true)&& idType != -1)
                        {
                            laTabla.insertar(idN, 10,idType, false, false, false, null);
                        }
                        
                    }else{
                        errorMsgs.Add("\n" + "Error de variable, variable \"" + context.IDENTIFIER(sum).GetText() + "\" ya fue declarada." + showErrorPosition(context.IDENTIFIER(sum).Symbol));
                    }
                }
            } catch (Exception e){}

            isList = false;
            return null;
        }

        public override object VisitClassDeclAST(MiniCSharpParser.ClassDeclASTContext context)
        {
            try {
                IToken id = context.IDENTIFIER().Symbol;
                int idType = 12;
                TablaSimbolos.Ident i = laTabla.buscar(context.IDENTIFIER().GetText());
                if (i == null) {
                    laTabla.insertar(id, idType,-1, false, true, false, null);

                    isVarInClass = true;
                    for (int sum = 0; context.varDecl().Count() > sum; sum++)
                    {
                        Visit(context.varDecl(sum));
                    }
                    isVarInClass = false;
                }else{
                    errorMsgs.Add("\n" + "Error de clase, identificador \"" + context.IDENTIFIER().GetText() + "\" ya fue declarado." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            } catch (Exception e){}
            return null;
        }

        public override object VisitMethodDeclAST(MiniCSharpParser.MethodDeclASTContext context)
        {
            methodType = -1;
            try {
                IToken id = context.IDENTIFIER().Symbol;
                int idType = 11;
                if (context.VOID() == null)
                {
                    idType = (int)Visit(context.type());
                }
                TablaSimbolos.Ident i = laTabla.buscar(context.IDENTIFIER().GetText());
                if (i == null && laTabla.buscarNivel(context.IDENTIFIER().GetText(), laTabla.obtenerNivelActual()) == -1) {
                    laTabla.insertar(id, idType,-1, true, false, false, null);
                    
                    if (context.formPars() != null)
                    {
                        Visit(context.formPars());
                    }
                    
                    laTabla.DecrementarNivel();
                    Visit(context.block());

                    bool comprobacion=false;

                    for (int sum1 = 0; context.block().ChildCount > sum1; sum1++)
                    {
                        string subcadenaReturn = new string(context.block().GetChild(sum1).GetText().Take(6).ToArray());
                        if (subcadenaReturn == "return")
                        {
                            comprobacion = true;
                        }
                    }

                    if (comprobacion.Equals(false) && idType!=11)
                    {
                        errorMsgs.Add("\n" +"Error de metodo, el metodo " + context.IDENTIFIER().GetText() + " usa el tipo " + showType(idType) + " y debe retornar en el mismo nivel al menos una vez" + "."+ showErrorPosition(context.IDENTIFIER().Symbol));
                    }

                }else
                {
                    errorMsgs.Add("\n" + "Error de metodo, metodo \"" + context.IDENTIFIER().GetText() + "\" ya fue declarado." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            } catch (Exception e){}
            return null;
        }

        public override object VisitFormParsAST(MiniCSharpParser.FormParsASTContext context)
        {
            try {
                IToken id = context.IDENTIFIER(0).Symbol;
                int idType = (int) Visit(context.type(0));
                TablaSimbolos.Ident i = laTabla.buscar(context.IDENTIFIER(0).GetText());
                if (i == null || i != null && laTabla.buscarNivel(context.IDENTIFIER(0).GetText(), laTabla.obtenerNivelActual()) == -1)
                {
                    laTabla.insertar(id, idType, -1,false, false, true, null);
                }else{
                    errorMsgs.Add("\n" + "Error de variable, variable \"" + context.IDENTIFIER(0).GetText() + "\" ya fue declarada." + showErrorPosition(context.IDENTIFIER(0).Symbol));
                }
                
                for (int sum = 1; context.IDENTIFIER().Count() > sum; sum++)
                {
                    IToken idN = context.IDENTIFIER(sum).Symbol;
                    int idTypeN = (int) Visit(context.type(sum));
                    TablaSimbolos.Ident iN = laTabla.buscar(context.IDENTIFIER(sum).GetText());
                    if (iN == null || iN != null && laTabla.buscarNivel(context.IDENTIFIER(sum).GetText(), laTabla.obtenerNivelActual()) == -1){
                        laTabla.insertar(idN, idTypeN,-1,false, false,true, null);
                    }else{
                        errorMsgs.Add("\n" + "Error de variable, variable \"" + context.IDENTIFIER(sum).GetText() + "\" ya fue declarada." + showErrorPosition(context.IDENTIFIER(sum).Symbol));
                    }
                }
            } catch (Exception e){}
            return null;
        }

        public override object VisitTypeAST(MiniCSharpParser.TypeASTContext context)
        {
            int result=-1;
            isList = false;
            isArray = false;

            if (context.QMARK(0) == null && context.IDENTIFIER(0).GetText() != "list" && 
                context.LBRACK() == null && context.RBRACK() == null && context.LESS_THAN()== null &&
                context.GREATER_THAN() == null)
            {
                if (context.IDENTIFIER(0).GetText().Equals("int"))
                {
                    result = 0;
                }
                else if (context.IDENTIFIER(0).GetText().Equals("double"))
                {
                    result = 2;
                }
                else if (context.IDENTIFIER(0).GetText().Equals("char"))
                {
                    result = 4;
                }
                else if (context.IDENTIFIER(0).GetText().Equals("string"))
                {
                    result = 6;
                }
                else if (context.IDENTIFIER(0).GetText().Equals("bool"))
                {
                    result = 8;
                } else if (context.IDENTIFIER(0).GetText().Equals("inst"))
                {
                    result = 14;
                }
                else
                {
                    errorMsgs.Add("\n" + "Error de tipos, tipo \"" + context.IDENTIFIER(0).GetText() +
                                  "\" no es un tipo valido." + showErrorPosition(context.IDENTIFIER(0).Symbol));
                }
            }

            else if (context.QMARK(0) != null && context.IDENTIFIER(0).GetText() != "list" && 
                     context.LBRACK() == null && context.RBRACK() == null && context.LESS_THAN()== null &&
                     context.GREATER_THAN() == null)
            {
                if (context.IDENTIFIER(0).GetText().Equals("int"))
                {
                    result = 1;
                }else if (context.IDENTIFIER(0).GetText().Equals("double"))
                {
                    result = 3;

                }else if (context.IDENTIFIER(0).GetText().Equals("char"))
                {
                    result = 5;

                }else if (context.IDENTIFIER(0).GetText().Equals("string"))
                {
                    result = 7;

                    
                }else if (context.IDENTIFIER(0).GetText().Equals("bool"))
                {
                    result =9;
                }
                else
                {
                    errorMsgs.Add("\n" + "Error de tipos, tipo \"" + context.IDENTIFIER(0).GetText() +
                                  "\" no es un tipo valido." + showErrorPosition(context.IDENTIFIER(0).Symbol));
                }
            }
            
            else if (context.QMARK(0) == null && context.QMARK(1) == null && context.IDENTIFIER(0).GetText() == "list" && 
                     context.LBRACK() == null && context.RBRACK() == null && context.LESS_THAN()!= null &&
                     context.GREATER_THAN() != null)
            {
                if (context.IDENTIFIER(1).GetText().Equals("int"))
                {
                    result = 0;
                    isList = true;
                }
                else if (context.IDENTIFIER(1).GetText().Equals("double"))
                {
                    result = 2;
                    isList = true;
                }
                else if (context.IDENTIFIER(1).GetText().Equals("char"))
                {
                    result = 4;
                    isList = true;
                }
                else if (context.IDENTIFIER(1).GetText().Equals("string"))
                {
                    result = 6;
                    isList = true;
                }
                else if (context.IDENTIFIER(1).GetText().Equals("bool"))
                {
                    result = 8;
                    isList = true;
                }
                else
                {
                    errorMsgs.Add("\n" + "Error de tipos, tipo \"" + context.IDENTIFIER(1).GetText() +
                                  "\" no es un tipo valido." + showErrorPosition(context.IDENTIFIER(1).Symbol));
                    
                }
            }
            
            else if (context.QMARK(0) != null && context.QMARK(1) == null && context.IDENTIFIER(0).GetText() == "list" && 
                     context.LBRACK() == null && context.RBRACK() == null && context.LESS_THAN()!= null &&
                     context.GREATER_THAN() != null)
            {
                if (context.QMARK(0).Symbol.Column > context.LESS_THAN().Symbol.Column)
                {

                    if (context.IDENTIFIER(1).GetText().Equals("int"))
                    {

                        result = 1;
                        isList = true;
                    }
                    else if (context.IDENTIFIER(1).GetText().Equals("double"))
                    {

                        result = 3;
                        isList = true;

                    }
                    else if (context.IDENTIFIER(1).GetText().Equals("char"))
                    {

                        result = 5;
                        isList = true;

                    }
                    else if (context.IDENTIFIER(1).GetText().Equals("string"))
                    {

                        result = 7;
                        isList = true;

                    }
                    else if (context.IDENTIFIER(1).GetText().Equals("bool"))
                    {

                        result = 9;
                        isList = true;

                    }
                    else
                    {
                        errorMsgs.Add("\n" + "Error de tipos, tipo \"" + context.IDENTIFIER(1).GetText() +
                                      "\" no es un tipo valido." + showErrorPosition(context.IDENTIFIER(1).Symbol));
                    }
                }
                else
                {
                    errorMsgs.Add("\n" + "Error de tipos, tipo \"" + context.IDENTIFIER(1).GetText() +
                                  "\" si desea usar \"" + context.QMARK(0).GetText() +
                                  "\" debe hacerlo dentro de \"" + context.LESS_THAN().GetText() + "\" y el \"" +
                                  context.GREATER_THAN().GetText() + "\" ." + showErrorPosition(context.IDENTIFIER(1).Symbol));
                }
            }else if (context.QMARK(0) == null && 
                      context.LBRACK() != null && context.RBRACK() != null && context.LESS_THAN()== null &&
                      context.GREATER_THAN() == null)
            {
                if (context.IDENTIFIER(0).GetText().Equals("int"))
                {
                    result = 0;
                    isArray = true;
                }
                else if (context.IDENTIFIER(0).GetText().Equals("double"))
                {
                    result = 2;
                    isArray = true;
                }
                else if (context.IDENTIFIER(0).GetText().Equals("char"))
                {
                    result = 4;
                    isArray = true;
                }
                else if (context.IDENTIFIER(0).GetText().Equals("string"))
                {
                    result = 6;
                    isArray = true;
                }
                else if (context.IDENTIFIER(0).GetText().Equals("bool"))
                {
                    result = 8;
                    isArray = true;
                }
                else
                {
                    errorMsgs.Add("\n" + "Error de tipos, tipo \"" + context.IDENTIFIER(0).GetText() +
                                  "\" no es un tipo valido." + showErrorPosition(context.IDENTIFIER(0).Symbol));
                }
                
            }else if (context.QMARK(0) != null &&
                      context.LBRACK() != null && context.RBRACK() != null && context.LESS_THAN() == null &&
                      context.GREATER_THAN() == null)
            {
                if (context.IDENTIFIER(0).GetText().Equals("int"))
                {

                    result = 1;
                    isArray = true;
                }
                else if (context.IDENTIFIER(0).GetText().Equals("double"))
                {

                    result = 3;
                    isArray = true;

                }
                else if (context.IDENTIFIER(0).GetText().Equals("char"))
                {

                    result = 5;
                    isArray = true;

                }
                else if (context.IDENTIFIER(0).GetText().Equals("string"))
                {

                    result = 7;
                    isArray = true;

                }
                else if (context.IDENTIFIER(0).GetText().Equals("bool"))
                {

                    result = 9;
                    isArray = true;

                }
                else
                {
                    errorMsgs.Add("\n" + "Error de tipos, tipo \"" + context.IDENTIFIER(1).GetText() +
                                  "\" no es un tipo valido." + showErrorPosition(context.IDENTIFIER(1).Symbol));
                }


            }
            else
            {
                errorMsgs.Add("\n" + "Error de tipos, tipo \"" + context.IDENTIFIER(0).GetText() +
                              "\" no es un tipo valido." + showErrorPosition(context.IDENTIFIER(1).Symbol));
            }
            
            return result;
        }

        public override object VisitDesignatorStatementAST(MiniCSharpParser.DesignatorStatementASTContext context)
        {
            try
            {
                if (context.ASSIGN() != null)
                {
                    designatorAssign = (int)Visit(context.designator());

                    if (designatorAssign == 14)
                    {
                        int i = -1;
                        i= (int)Visit(context.expr());
                        
                        if (i != -1)
                        {

                            if (laTabla.buscarNivel(context.designator().GetText(), laTabla.obtenerNivelActual()) !=
                                -1 ||
                                laTabla.buscarNivel(context.designator().GetText(), laTabla.buscarNivelMetodo()) !=
                                -1 ||
                                laTabla.buscarNivel(context.designator().GetText(), 0) != -1)
                            {
                                TablaSimbolos.Ident designator = null;
                                if (laTabla.buscarNivel(context.designator().GetText(), laTabla.obtenerNivelActual()) != -1)
                                {
                                    designator = laTabla.buscarToken(context.designator().GetText(), laTabla.obtenerNivelActual());
                                }
                                else if (laTabla.buscarNivel(context.designator().GetText(), laTabla.buscarNivelMetodo()) != -1)
                                {
                                    designator = laTabla.buscarToken(context.designator().GetText(), laTabla.buscarNivelMetodo());
                                }
                                else if (laTabla.buscarNivel(context.designator().GetText(), 0) != -1)
                                {
                                    designator = laTabla.buscarToken(context.designator().GetText(), 0);
                                }

                                TablaSimbolos.Ident verificacion = null;
                                
                                verificacion= laTabla.ModificarTokenInstancia(designator.GetToken().Text, designator.GetNivel(), instanceToken.GetToken());

                                if (verificacion == null)
                                {
                                    errorMsgs.Add("\n" +"Error de instancia, la instancia \""+ context.designator().GetText() + "\" ya se le asigno una clase." + showErrorPosition(context.designator().Start));
                                }

                                instanceToken = null;

                            }
                            else
                            {
                                errorMsgs.Add("\n" +"Error de instancia, la instancia \""+ context.designator().GetText() + "\" no se encuentra." + showErrorPosition(context.designator().Start));
                            }
                        }

                    }
                    else
                    {
                        Visit(context.expr());
                    }
                    
                    designatorAssign = -1;
                    
                    
                    
                }

                if (context.INCREMENT() != null)
                {
                    designatorAssign = (int)Visit(context.designator());
                    if (designatorAssign != 0 && designatorAssign != 1 && designatorAssign != 2 &&
                        designatorAssign != 3)
                    {
                        errorMsgs.Add("\n" +"Error de tipos, \""+ showType(designatorAssign) + "\" no puede utilizar \"" + context.INCREMENT().GetText() + "\"." + showErrorPosition(context.designator().Start));
                    }
                    
                    designatorAssign = -1;
                }
                
                if (context.DECREMENT() != null)
                {
                    designatorAssign = (int)Visit(context.designator());
                    if (designatorAssign != 0 && designatorAssign != 1 && designatorAssign != 2 &&
                        designatorAssign != 3)
                    {
                        errorMsgs.Add("\n" +"Error de tipos, \""+ showType(designatorAssign) + "\" no puede utilizar \"" + context.DECREMENT().GetText() + "\"." + showErrorPosition(context.designator().Start));
                    }
                    
                    designatorAssign = -1;
                }
            
                if (context.LPAREN() != null)
                {
                    TablaSimbolos.Ident i = laTabla.buscarTokenMetodoNombre(context.designator().GetText());
                    TablaSimbolos.Ident metodoActual = laTabla.buscarTokenMetodo();

                    if (i==null)
                    {
                        errorMsgs.Add("\n" +"Error de metodo, el identificador \""+ context.designator().GetText() + "\" no puede utilizar \"" + context.LPAREN().GetText() + "\" \""+ context.RPAREN().GetText() + "\", no es un metodo." + showErrorPosition(context.designator().Start));
                    }

                    if (i.GetToken().ToString() == metodoActual.GetToken().ToString())
                    {
                        errorMsgs.Add("\n" +"Error de metodo, el metodo \""+ context.designator().GetText() + "\" no puede utilizarse dentro del mismo metodo." + showErrorPosition(context.designator().Start));
                    }else if (i.GetToken().ToString() != metodoActual.GetToken().ToString())
                    {
                        List<int> resul = laTabla.obtenerTiposMetodosVariables(i.GetNivel());
                        resul.Reverse();

                        int cantidad = 0;
                        
                        for (int sum2 = 0; context.actPars().ChildCount > sum2; sum2++)
                        {
                            if (context.actPars().GetChild(sum2).GetText() != ",")
                            {
                                cantidad++;
                            }

                        }

                        if (cantidad != resul.Count())
                        {
                            errorMsgs.Add("\n" +"Error de metodo, el identificador \""+ i.GetToken().Text + "\" recibio menos o mas parametros de los solicitados." + showErrorPosition(context.designator().Start));
                        }
                        else
                        {
                            if (i.GetToken().Text != "len" && i.GetToken().Text != "add" && i.GetToken().Text != "del")
                            { 
                                int suma=0;
                                for (int sum2 = 0; context.actPars().ChildCount > sum2; sum2++)
                                {

                                    if (context.actPars().GetChild(sum2).GetText() != ",")
                                    {
                                        int result;
                                        designatorAssign = resul[suma];
                                        result = (int)Visit(context.actPars().GetChild(sum2));
                                        if ((result == 0 && designatorAssign == 0) || (result == 1 && designatorAssign == 1) ||
                                            (result == 2 && designatorAssign == 2) || (result == 3 && designatorAssign == 3) ||
                                            (result == 4 && designatorAssign == 4) || (result == 5 && designatorAssign == 5) ||
                                            (result == 6 && designatorAssign == 6) || (result == 7 && designatorAssign == 7) ||
                                            (result == 8 && designatorAssign == 8) || (result == 9 && designatorAssign == 9) ||
                                            (result == 0 && designatorAssign == 1) || (result == 2 && designatorAssign == 3) ||
                                            (result == 4 && designatorAssign == 5) || (result == 6 && designatorAssign == 7) ||
                                            (result == 8 && designatorAssign == 9) || (result == 0 && designatorAssign == 2) ||
                                            (result == 1 && designatorAssign == 3) || (result == 0 && designatorAssign == 3))
                                        {
                                        }
                                        else
                                        {
                                            errorMsgs.Add("\n" +"Error de metodo, uno de los parametros requiere \""+ showType(designatorAssign) + "\" y se recibio \"" + showType(result) + "\"." + showErrorPosition(context.designator().Start));
                                        }

                                        suma++;
                                    }
                                    
                                }
                            }else if (i.GetToken().Text == "len")
                            {
                                int result;
                                result = (int)Visit(context.actPars().GetChild(0));

                                if (result != 10 && result != 13)
                                {
                                    errorMsgs.Add("\n" +"Error de metodo, este metodo requiere el tipo \""+ showType(10) + "\" o el \"" + showType(13) + "\"." + showErrorPosition(context.designator().Start)); 
                                }
                                
                            }else if (i.GetToken().Text == "add")
                            {
                                int result;
                                result = (int)Visit(context.actPars().GetChild(0));

                                if (result != 10 && result != 13)
                                {
                                    errorMsgs.Add("\n" +"Error de metodo, este metodo requiere el tipo \""+ showType(10) + "\" o el \"" + showType(13) + "\"." + showErrorPosition(context.designator().Start)); 
                                }
                                else if (result == 10 || result == 13)
                                {
                                    
                                    TablaSimbolos.Ident designator = null;

                                    if (laTabla.buscarNivel(context.actPars().GetChild(0).GetText(), laTabla.obtenerNivelActual()) != -1)
                                    {
                                        designator = laTabla.buscarToken(context.actPars().GetChild(0).GetText(), laTabla.obtenerNivelActual());
                                    }
                                    else if (laTabla.buscarNivel(context.actPars().GetChild(0).GetText(), laTabla.buscarNivelMetodo()) != -1)
                                    {
                                        designator = laTabla.buscarToken(context.actPars().GetChild(0).GetText(), laTabla.buscarNivelMetodo());
                                    }
                                    else if (laTabla.buscarNivel(context.actPars().GetChild(0).GetText(), 0) != -1)
                                    {
                                        designator = laTabla.buscarToken(context.actPars().GetChild(0).GetText(), 0);
                                    }

                                    designatorAssign = designator.GetSecondType();

                                    result = (int)Visit(context.actPars().GetChild(2));

                                    if ((result == 0 && designatorAssign == 0) || (result == 1 && designatorAssign == 1) ||
                                        (result == 2 && designatorAssign == 2) || (result == 3 && designatorAssign == 3) ||
                                        (result == 4 && designatorAssign == 4) || (result == 5 && designatorAssign == 5) ||
                                        (result == 6 && designatorAssign == 6) || (result == 7 && designatorAssign == 7) ||
                                        (result == 8 && designatorAssign == 8) || (result == 9 && designatorAssign == 9) ||
                                        (result == 0 && designatorAssign == 1) || (result == 2 && designatorAssign == 3) ||
                                        (result == 4 && designatorAssign == 5) || (result == 6 && designatorAssign == 7) ||
                                        (result == 8 && designatorAssign == 9) || (result == 0 && designatorAssign == 2) ||
                                        (result == 1 && designatorAssign == 3) || (result == 0 && designatorAssign == 3))
                                    {
                                    }
                                    else
                                    {
                                        errorMsgs.Add("\n" +"Error de metodo, uno de los parametros requiere \""+ showType(designatorAssign) + "\" y se recibio \"" + showType(result) + "\"." + showErrorPosition(context.designator().Start));
                                    }

                                    
                                }
                                
                            }else if (i.GetToken().Text == "del")
                            {
                                int result;
                                result = (int)Visit(context.actPars().GetChild(0));

                                if (result != 10 && result != 13)
                                {
                                    errorMsgs.Add("\n" +"Error de metodo, este metodo requiere el tipo \""+ showType(10) + "\" o el \"" + showType(13) + "\"." + showErrorPosition(context.designator().Start)); 
                                }
                                else if (result == 10 || result == 13)
                                {
                                    designatorAssign = 0;
                                    result = (int)Visit(context.actPars().GetChild(2));
                                    if ((result == 0 && designatorAssign == 0) || (result == 1 && designatorAssign == 1) ||
                                        (result == 2 && designatorAssign == 2) || (result == 3 && designatorAssign == 3) ||
                                        (result == 4 && designatorAssign == 4) || (result == 5 && designatorAssign == 5) ||
                                        (result == 6 && designatorAssign == 6) || (result == 7 && designatorAssign == 7) ||
                                        (result == 8 && designatorAssign == 8) || (result == 9 && designatorAssign == 9) ||
                                        (result == 0 && designatorAssign == 1) || (result == 2 && designatorAssign == 3) ||
                                        (result == 4 && designatorAssign == 5) || (result == 6 && designatorAssign == 7) ||
                                        (result == 8 && designatorAssign == 9) || (result == 0 && designatorAssign == 2) ||
                                        (result == 1 && designatorAssign == 3) || (result == 0 && designatorAssign == 3))
                                    {
                                    }
                                    else
                                    {
                                        errorMsgs.Add("\n" +"Error de metodo, uno de los parametros requiere \""+ showType(designatorAssign) + "\" y se recibio \"" + showType(result) + "\"." + showErrorPosition(context.designator().Start));
                                    }

                                    
                                }
                                
                            }
                        }

                    }
                    designatorAssign = -1;
                }


            }
            catch (Exception e) {}
            return null;
        }

        public override object VisitIfStatementAST(MiniCSharpParser.IfStatementASTContext context)
        {
            Visit(context.condition());
            Visit(context.statement(0));
            if (context.ELSE() != null)
            {
                Visit(context.statement(1));
            }
            return null;
        }

        public override object VisitForStatementAST(MiniCSharpParser.ForStatementASTContext context)
        {
            Visit(context.expr());
            
            if (context.condition() != null)
            {
                Visit(context.condition());
            }
            
            if (context.statement().Count() > 2)
            {
                Visit(context.statement(1));
            }
            Visit(context.statement(0));
            return null;
        }

        public override object VisitWhileStatementAST(MiniCSharpParser.WhileStatementASTContext context)
        {
            Visit(context.condition());
            Visit(context.statement());
            return null;
        }

        public override object VisitBreakStatementAST(MiniCSharpParser.BreakStatementASTContext context)
        {
            var currentContext = context.Parent;
            int verificar = -1;
            
            while (currentContext != null)
            {
                if (currentContext is MiniCSharpParser.ForStatementASTContext || currentContext is MiniCSharpParser.WhileStatementASTContext)
                {
                    verificar = 1;
                }
                currentContext = currentContext.Parent;
            }
            
            if (verificar == -1)
            {
                errorMsgs.Add("\n" + "Error de break, el \"" + context.BREAK().GetText() + "\" solo se puede usar en while o for." +
                              showErrorPosition(context.BREAK().Symbol));
            }

            return null;
        }

        public override object VisitReturnStatementAST(MiniCSharpParser.ReturnStatementASTContext context)
        {
            int result = -1;
            
            TablaSimbolos.Ident i = laTabla.buscarTokenMetodo();

            int tipoID = i.GetType();

            if (context.expr()!= null)
            {
                methodType = tipoID;
                result = (int) Visit(context.expr());

                if ((result == 0 && methodType == 0) || (result == 1 && methodType == 1) ||
                    (result == 2 && methodType == 2) || (result == 3 && methodType == 3) ||
                    (result == 4 && methodType == 4) || (result == 5 && methodType == 5) ||
                    (result == 6 && methodType == 6) || (result == 7 && methodType == 7) ||
                    (result == 8 && methodType == 8) || (result == 9 && methodType == 9) ||
                    (result == 0 && methodType == 1) || (result == 2 && methodType == 3) ||
                    (result == 4 && methodType == 5) || (result == 6 && methodType == 7) ||
                    (result == 8 && methodType == 9) || (result == 0 && methodType == 2) ||
                    (result == 1 && methodType == 3) || (result == 0 && methodType == 3))
                {
                }
                else
                {
                    errorMsgs.Add("\n" +"Error de metodo, el metodo " + i.GetToken().Text + " usa el tipo " + showType(methodType) + " y se esta retornando el tipo " + showType(result) + "."+ showErrorPosition(context.expr().Start));
                }
            }
            
            //int result = -1;
            //if (context.expr()!= null)
            //{
                //result = (int) Visit(context.expr());
            //}
            methodType = -1;
            return result;
        }

        public override object VisitReadStatementAST(MiniCSharpParser.ReadStatementASTContext context)
        {
            Visit(context.designator());
            return null;
        }

        public override object VisitWriteStatementAST(MiniCSharpParser.WriteStatementASTContext context)
        {
            Visit(context.expr());
            return null;
        }

        public override object VisitBlockStatementAST(MiniCSharpParser.BlockStatementASTContext context)
        {
            Visit(context.block());
            return null;
        }

        public override object VisitSemicolonStatementAST(MiniCSharpParser.SemicolonStatementASTContext context)
        {
            return null;
        }

        public override object VisitBlockAST(MiniCSharpParser.BlockASTContext context)
        {
            laTabla.openScope();
            
            for (int i = 0; context.varDecl().Count() > i; i++)
            {
                Visit(context.varDecl(i));
            }
            
            for (int sum = 0; context.statement().Count() > sum; sum++)
            {
                Visit(context.statement(sum));
            }
            return null;
        }

        public override object VisitActParsAST(MiniCSharpParser.ActParsASTContext context)
        {
            int result = -1;
            Visit(context.expr(0));
            for (int i = 1; context.expr().Count() > i; i++)
            {
                result= (int) Visit(context.expr(i));
            }
            return result;
        }

        public override object VisitConditionAST(MiniCSharpParser.ConditionASTContext context)
        {
            Visit(context.condTerm(0));
            for (int i = 1; context.condTerm().Count() > i; i++)
            {
                Visit(context.condTerm(i));
            }
            return null;
        }

        public override object VisitCondTermAST(MiniCSharpParser.CondTermASTContext context)
        {
            Visit(context.condFact(0));
            for (int i = 1; context.condFact().Count() > i; i++)
            {
                Visit(context.condFact(i));
            }
            return null;
        }

        public override object VisitCondFactAST(MiniCSharpParser.CondFactASTContext context)
        {
            int result = -1;
            string op = "";
            int type2 = -1; 
            
            result= (int) Visit(context.expr(0));
            
            
            op = (String) Visit(context.relop());
            
            
            type2= (int) Visit(context.expr(1));
            
            if (isMultitype(op)){ //el operador es multitipo (char, int ...)
                if ((result==0&&type2==0) || (result==1&&type2==1) ||
                    (result==2&&type2==2) || (result==3&&type2==3) ||
                    (result==4&&type2==4) || (result==5&&type2==5) || 
                    (result==6&&type2==6) || (result==7&&type2==7) ||
                    (result==8&&type2==8) || (result==9&&type2==9) ||
                    (result==10&type2==10) ||
                    (result==0&&type2==1) || (result==1&&type2==0) ||
                    (result==2&&type2==3) || (result==3&&type2==2) ||
                    (result==4&&type2==5) || (result==5&&type2==4) ||
                    (result==6&&type2==7) || (result==7&&type2==6) ||
                    (result==8&&type2==9) || (result==9&&type2==8)||
                    (result==0&&type2==2) || (result==2&&type2==0)||
                    (result==1&&type2==3) || (result==3&&type2==1)||
                    (result==0&&type2==3) || (result==3&&type2==0)||
                    (result==1&&type2==2) || (result==2&&type2==1) || 
                    (result==21&&type2==21)){
                    result = type2; //si el operador recibiera dos tipos iguales pero devolviera otro, debe de cambiarse
                }else {
                    errorMsgs.Add("\n" +"Error de tipos, " + showType(result) + " y " + showType(type2) + " no son compatibles para el operador " + op + "." + showErrorPosition(context.relop().Start));
                }
            }
            else { //el operador es solo para int
                if (result==0&&type2==0 || result==1&&type2==1 ||
                    result==2&&type2==2 || result==3&&type2==3 ||
                    result==0&&type2==1 || result==0&&type2==2 ||
                    result==0&&type2==3  || result==1&&type2==0 ||
                    result==1&&type2==1 || result==1&&type2==2  ||
                    result==1&&type2==3 || result==2&&type2==0 ||
                    result==2&&type2==1 || result==2&&type2==2 ||
                    result==2&&type2==3 || result==3&&type2==0 ||
                    result==3&&type2==1 || result==3&&type2==2) {
                    result = type2;
                }else {
                    errorMsgs.Add("\n" +"Error de tipos, " + showType(result) + " y " + showType(type2) + " no son compatibles para el operador " + op + "." + showErrorPosition(context.relop().Start));
                }
            }
            
            
            
            return result;
        }

        public override object VisitCastAST(MiniCSharpParser.CastASTContext context)
        {
            int result = -1;
            
            result= (int)Visit(context.type());
            return result;
        }

        public override object VisitExprAST(MiniCSharpParser.ExprASTContext context)
        {
            
            String op="";

            int result = -1;

            result = (int) Visit(context.term(0));
            

            if (context.MINUS() != null)
            {
                if (designatorAssign != -1)
                {
                    if (designatorAssign != 0 && designatorAssign != 1 && designatorAssign != 2 &&
                        designatorAssign != 3)
                    {
                        errorMsgs.Add("\n" +"Error de tipos, \""+ showType(designatorAssign) + "\" no puede utilizar \"" + context.MINUS().GetText() + "\"." + showErrorPosition(context.term(0).Start));
                    }
                }else if (result != 0 && result != 1 && result != 2 &&
                          result != 3)
                {
                    errorMsgs.Add("\n" +"Error de tipos, \""+ showType(result) + "\" no puede utilizar \"" + context.MINUS().GetText() + "\"." + showErrorPosition(context.term(0).Start));
                }else if (methodType != -1)
                {
                    if (methodType != 0 && methodType != 1 && methodType != 2 &&
                        methodType != 3)
                    {
                        errorMsgs.Add("\n" +"Error de tipos, \""+ showType(methodType) + "\" no puede utilizar \"" + context.MINUS().GetText() + "\"." + showErrorPosition(context.term(0).Start));
                    }
                }

            }

            if (context.cast() != null)
            {
                int cast = -1;
                cast = (int) Visit(context.cast());

                if (designatorAssign != -1)
                {
                    //el operador es multitipo (char, int ...)
                    if ((cast == 0 && designatorAssign == 0) || (cast == 1 && designatorAssign == 1) ||
                        (cast == 2 && designatorAssign == 2) || (cast == 3 && designatorAssign == 3) ||
                        (cast == 4 && designatorAssign == 4) || (cast == 5 && designatorAssign == 5) ||
                        (cast == 6 && designatorAssign == 6) || (cast == 7 && designatorAssign == 7) ||
                        (cast == 8 && designatorAssign == 8) || (cast == 9 && designatorAssign == 9) ||
                        (cast == 0 && designatorAssign == 1) || (cast == 2 && designatorAssign == 3) ||
                        (cast == 4 && designatorAssign == 5) || (cast == 6 && designatorAssign == 7) ||
                        (cast == 8 && designatorAssign == 9) || (cast == 0 && designatorAssign == 2) ||
                        (cast == 1 && designatorAssign == 3) || (cast == 0 && designatorAssign == 3))
                    {
                        if (cast == 0)
                        {
                            designatorAssign = 2;
                        }
                        else if (cast == 1)
                        {
                            designatorAssign = 3;
                        }
                        else if (cast == 2)
                        {
                            designatorAssign = 2;
                        }
                        else if (cast == 3)
                        {
                            designatorAssign = 3;
                        }
                        else if (cast == 4)
                        {
                            designatorAssign = 4;
                        }
                        else if (cast == 5)
                        {
                            designatorAssign = 5;
                        }
                        else if (cast == 6)
                        {
                            designatorAssign = 6;
                        }
                        else if (cast == 7)
                        {
                            designatorAssign = 7;
                        }
                        else if (cast == 8)
                        {
                            designatorAssign = 8;
                        }
                        else if (cast == 9)
                        {
                            designatorAssign = 9;
                        }
                    }
                    else
                    {
                        errorMsgs.Add("\n" + "Error de tipos, " + showType(cast) + " y " + showType(designatorAssign) +
                                      " no son compatibles para casting" + "." +
                                      showErrorPosition(context.term(0).Start));
                    }
                }else if (methodType!=-1)
                {
                    if ((cast == 0 && methodType == 0) || (cast == 1 && methodType == 1) ||
                        (cast == 2 && methodType == 2) || (cast == 3 && methodType == 3) ||
                        (cast == 4 && methodType == 4) || (cast == 5 && methodType == 5) ||
                        (cast == 6 && methodType == 6) || (cast == 7 && methodType == 7) ||
                        (cast == 8 && methodType == 8) || (cast == 9 && methodType == 9) ||
                        (cast == 0 && methodType == 1) || (cast == 2 && methodType == 3) ||
                        (cast == 4 && methodType == 5) || (cast == 6 && methodType == 7) ||
                        (cast == 8 && methodType == 9) || (cast == 0 && methodType == 2) ||
                        (cast == 1 && methodType == 3) || (cast == 0 && methodType == 3))
                    {
                        if (cast == 0)
                        {
                            methodType = 2;
                        }
                        else if (cast == 1)
                        {
                            methodType = 3;
                        }
                        else if (cast == 2)
                        {
                            methodType = 2;
                        }
                        else if (cast == 3)
                        {
                            methodType = 3;
                        }
                        else if (cast == 4)
                        {
                            methodType = 4;
                        }
                        else if (cast == 5)
                        {
                            methodType = 5;
                        }
                        else if (cast == 6)
                        {
                            methodType = 6;
                        }
                        else if (cast == 7)
                        {
                            methodType = 7;
                        }
                        else if (cast == 8)
                        {
                            methodType = 8;
                        }
                        else if (cast == 9)
                        {
                            methodType = 9;
                        }
                    }
                    else
                    {
                        errorMsgs.Add("\n" + "Error de tipos, " + showType(cast) + " y " + showType(methodType) +
                                      " no son compatibles para casting" + "." +
                                      showErrorPosition(context.term(0).Start));
                    }
                }
                else
                { 
                    if ((cast == 0 && result == 0) || (cast == 1 && result == 1) ||
                      (cast == 2 && result == 2) || (cast == 3 && result == 3) ||
                      (cast == 4 && result == 4) || (cast == 5 && result == 5) ||
                      (cast == 6 && result == 6) || (cast == 7 && result == 7) ||
                      (cast == 8 && result == 8) || (cast == 9 && result == 9) ||
                      (cast == 0 && result == 1) || (cast == 2 && result == 3) ||
                      (cast == 4 && result == 5) || (cast == 6 && result == 7) ||
                      (cast == 8 && result == 9) || (cast == 0 && result == 2) ||
                      (cast == 1 && result == 3) || (cast == 0 && result == 3))
                    {
                        if (cast == 0)
                        {
                            result = 2;
                        }
                        else if (cast == 1)
                        {
                            result = 3;
                        }
                        else if (cast == 2)
                        {
                            result = 2;
                        }
                        else if (cast == 3)
                        {
                            result = 3;
                        }
                        else if (cast == 4)
                        {
                            result = 4;
                        }
                        else if (cast == 5)
                        {
                            result = 5;
                        }
                        else if (cast == 6)
                        {
                            result = 6;
                        }
                        else if (cast == 7)
                        {
                            result = 7;
                        }
                        else if (cast == 8)
                        {
                            result = 8;
                        }
                        else if (cast == 9)
                        {
                            result = 9;
                        }
                    }
                    else
                    {
                        errorMsgs.Add("\n" + "Error de tipos, " + showType(cast) + " y " + showType(result) +
                                      " no son compatibles para casting" + "." +
                                      showErrorPosition(context.term(0).Start));
                    }
                }
            }

            if (result ==21)
            {
                if (designatorAssign != -1 && designatorAssign == 0 ||
                    designatorAssign != -1 && designatorAssign == 2 ||
                    designatorAssign != -1 && designatorAssign == 4 ||
                    designatorAssign != -1 && designatorAssign == 6 ||
                    designatorAssign != -1 && designatorAssign == 8)
                {
                    errorMsgs.Add("\n" + "Error de tipos, " + showType(designatorAssign) +
                                  " no puede asignarse con null" + "." +
                                  showErrorPosition(context.term(0).Start));
                }
                
                if (methodType != -1 && methodType == 0 ||
                    methodType != -1 && methodType == 2 ||
                    methodType != -1 && methodType == 4 ||
                    methodType != -1 && methodType == 6 ||
                    methodType != -1 && methodType == 8)
                {
                    errorMsgs.Add("\n" + "Error de tipos, " + showType(methodType) +
                                  " no puede asignarse con null" + "." +
                                  showErrorPosition(context.term(0).Start));
                }
            }
            if (result !=21)
            {
                if (designatorAssign != -1 && designatorAssign == 1 ||
                    designatorAssign != -1 && designatorAssign == 3 ||
                    designatorAssign != -1 && designatorAssign == 5 ||
                    designatorAssign != -1 && designatorAssign == 7 ||
                    designatorAssign != -1 && designatorAssign == 9)
                {
                    designatorAssign--;
                }
                
                if (methodType != -1 && methodType == 1 ||
                    methodType != -1 && methodType == 3 ||
                    methodType != -1 && methodType == 5 ||
                    methodType != -1 && methodType == 7 ||
                    methodType != -1 && methodType == 9)
                {
                    methodType--;
                }
            }

            if (designatorAssign != -1)
            {
                if ((result == 0 && designatorAssign == 0) || (result == 1 && designatorAssign == 1) ||
                    (result == 2 && designatorAssign == 2) || (result == 3 && designatorAssign == 3) ||
                    (result == 4 && designatorAssign == 4) || (result == 5 && designatorAssign == 5) ||
                    (result == 6 && designatorAssign == 6) || (result == 7 && designatorAssign == 7) ||
                    (result == 8 && designatorAssign == 8) || (result == 9 && designatorAssign == 9) ||
                    (result == 0 && designatorAssign == 1) || (result == 2 && designatorAssign == 3) ||
                    (result == 4 && designatorAssign == 5) || (result == 6 && designatorAssign == 7) ||
                    (result == 8 && designatorAssign == 9) || (result == 0 && designatorAssign == 2) ||
                    (result == 1 && designatorAssign == 3) || (result == 0 && designatorAssign == 3) ||
                    (result == 21 && designatorAssign == 1) || (result == 21 && designatorAssign == 3) || 
                    (result == 21 && designatorAssign == 5) || (result == 21 && designatorAssign == 7) ||
                    (result == 21 && designatorAssign == 9) || (result == 12 && designatorAssign == 14))
                {
                }
                else
                {
                    errorMsgs.Add("\n" +"Error de tipos: \""+ showType(designatorAssign) + "\" y \"" + showType(result) + "\" no son compatibles." + showErrorPosition(context.term(0).Start));
                }
            }else if (methodType != -1)
            {
                if ((result == 0 && methodType == 0) || (result == 1 && methodType == 1) ||
                    (result == 2 && methodType == 2) || (result == 3 && methodType == 3) ||
                    (result == 4 && methodType == 4) || (result == 5 && methodType == 5) ||
                    (result == 6 && methodType == 6) || (result == 7 && methodType == 7) ||
                    (result == 8 && methodType == 8) || (result == 9 && methodType == 9) ||
                    (result == 0 && methodType == 1) || (result == 2 && methodType == 3) ||
                    (result == 4 && methodType == 5) || (result == 6 && methodType == 7) ||
                    (result == 8 && methodType == 9) || (result == 0 && methodType == 2) ||
                    (result == 1 && methodType == 3) || (result == 0 && methodType == 3))
                {
                }
                else
                {
                    errorMsgs.Add("\n" +"Error de tipos: \""+ showType(methodType) + "\" y \"" + showType(result) + "\" no son compatibles." + showErrorPosition(context.term(0).Start));
                }
            }



            for (int i = 1; context.term().Count() > i; i++) {
                op = (String) Visit(context.addop(i-1));
                int type2 = (int) Visit(context.term(i));
            if (designatorAssign != -1)
            {
                if ((type2 == 0 && designatorAssign == 0) || (type2 == 1 && designatorAssign == 1) ||
                    (type2 == 2 && designatorAssign == 2) || (type2 == 3 && designatorAssign == 3) ||
                    (type2 == 4 && designatorAssign == 4) || (type2 == 5 && designatorAssign == 5) ||
                    (type2 == 6 && designatorAssign == 6) || (type2 == 7 && designatorAssign == 7) ||
                    (type2 == 8 && designatorAssign == 8) || (type2 == 9 && designatorAssign == 9) ||
                    (type2 == 0 && designatorAssign == 1) || (type2 == 2 && designatorAssign == 3) ||
                    (type2 == 4 && designatorAssign == 5) || (type2 == 6 && designatorAssign == 7) ||
                    (type2 == 8 && designatorAssign == 9) || (type2 == 0 && designatorAssign == 2) ||
                    (type2 == 1 && designatorAssign == 3) || (type2 == 0 && designatorAssign == 3))
                {

                }
                else
                {
                    errorMsgs.Add("\n" +"Error de tipos: \""+ showType(designatorAssign) + "\" y \"" + showType(type2) + "\" no son compatibles." + showErrorPosition(context.term(0).Start));
                }
            }else if (methodType != -1)
            {
                if ((type2 == 0 && methodType == 0) || (type2 == 1 && methodType == 1) ||
                    (type2 == 2 && methodType == 2) || (type2 == 3 && methodType == 3) ||
                    (type2 == 4 && methodType == 4) || (type2 == 5 && methodType == 5) ||
                    (type2 == 6 && methodType == 6) || (type2 == 7 && methodType == 7) ||
                    (type2 == 8 && methodType == 8) || (type2 == 9 && methodType == 9) ||
                    (type2 == 0 && methodType == 1) || (type2 == 2 && methodType == 3) ||
                    (type2 == 4 && methodType == 5) || (type2 == 6 && methodType == 7) ||
                    (type2 == 8 && methodType == 9) || (type2 == 0 && methodType == 2) ||
                    (type2 == 1 && methodType == 3) || (type2 == 0 && methodType == 3))
                {

                }
                else
                {
                    errorMsgs.Add("\n" +"Error de tipos: \""+ showType(methodType) + "\" y \"" + showType(type2) + "\" no son compatibles." + showErrorPosition(context.term(0).Start));
                }
            }
                if (isMultitype(op)){ //el operador es multitipo (char, int ...)
                    if ((result==0&&type2==0) || (result==1&&type2==1)) {
                        result = type2; //si el operador recibiera dos tipos iguales pero devolviera otro, debe de cambiarse
                    }else {
                        errorMsgs.Add("\n" +"Error de tipos: " + showType(result) + " y " + showType(type2) + " no son compatibles para el operador " + op + "." + showErrorPosition(context.addop(i - 1).Start));
                    }
                }
                else { //el operador es solo para int
                    if (result==0&&type2==0 || result==1&&type2==1 ||
                        result==2&&type2==2 || result==3&&type2==3 ||
                        result==0&&type2==1 || result==0&&type2==2 ||
                        result==0&&type2==3  || result==1&&type2==0 ||
                        result==1&&type2==1 || result==1&&type2==2  ||
                        result==1&&type2==3 || result==2&&type2==0 ||
                        result==2&&type2==1 || result==2&&type2==2 ||
                        result==2&&type2==3 || result==3&&type2==0 ||
                        result==3&&type2==1 || result==3&&type2==2 ||
                        result==3&&type2==3) {
                        result = type2;
                    }else {
                        errorMsgs.Add("\n" +"Error de tipos: " + showType(result) + " y " + showType(type2) + " no son compatibles para el operador " + op + "." + showErrorPosition(context.addop(i - 1).Start));
                    }
                }
            }
            return result;
            
            /*
            if (context.cast().ChildCount > 1)
            {
                Visit(context.cast());
            }

            Visit(context.term(0));
            
            for (int i = 0; context.addop().Count() > i; i++)
            {
                Visit(context.addop(i));
            }
            
            for (int i = 1; context.term().Count() > i; i++)
            {
                Visit(context.term(i));
            }
            */
            return null;
        }

        public override object VisitTermAST(MiniCSharpParser.TermASTContext context)
        {
            int result = -1;
            String op="";
            result = (int) Visit(context.factor(0));

            for (int i = 1; context.factor().Count() > i; i++) {
                op = (String) Visit(context.mulop(i-1));
                int type2 = (int) Visit(context.factor(i));

                if (designatorAssign != -1)
                {
                    if ((type2 == 0 && designatorAssign == 0) || (type2 == 1 && designatorAssign == 1) ||
                        (type2 == 2 && designatorAssign == 2) || (type2 == 3 && designatorAssign == 3) ||
                        (type2 == 4 && designatorAssign == 4) || (type2 == 5 && designatorAssign == 5) ||
                        (type2 == 6 && designatorAssign == 6) || (type2 == 7 && designatorAssign == 7) ||
                        (type2 == 8 && designatorAssign == 8) || (type2 == 9 && designatorAssign == 9) ||
                        (type2 == 0 && designatorAssign == 1) || (type2 == 2 && designatorAssign == 3) ||
                        (type2 == 4 && designatorAssign == 5) || (type2 == 6 && designatorAssign == 7) ||
                        (type2 == 8 && designatorAssign == 9) || (type2 == 0 && designatorAssign == 2) ||
                        (type2 == 1 && designatorAssign == 3) || (type2 == 0 && designatorAssign == 3))
                    {

                    }
                    else
                    {
                        errorMsgs.Add("\n" +"Error de tipos: \""+ showType(designatorAssign) + "\" y \"" + showType(type2) + "\" no son compatibles." + showErrorPosition(context.factor(i).Start));
                    }
                }else if (methodType != -1)
                {
                    if ((type2 == 0 && methodType == 0) || (type2 == 1 && methodType == 1) ||
                        (type2 == 2 && methodType == 2) || (type2 == 3 && methodType == 3) ||
                        (type2 == 4 && methodType == 4) || (type2 == 5 && methodType == 5) ||
                        (type2 == 6 && methodType == 6) || (type2 == 7 && methodType == 7) ||
                        (type2 == 8 && methodType == 8) || (type2 == 9 && methodType == 9) ||
                        (type2 == 0 && methodType == 1) || (type2 == 2 && methodType == 3) ||
                        (type2 == 4 && methodType == 5) || (type2 == 6 && methodType == 7) ||
                        (type2 == 8 && methodType == 9) || (type2 == 0 && methodType == 2) ||
                        (type2 == 1 && methodType == 3) || (type2 == 0 && methodType == 3))
                    {

                    }
                    else
                    {
                        errorMsgs.Add("\n" +"Error de tipos: \""+ showType(methodType) + "\" y \"" + showType(type2) + "\" no son compatibles." + showErrorPosition(context.factor(i).Start));
                    }
                }

                if (isMultitype(op)){ //el operador es multitipo (char, int ...)
                    if ((result==0&&type2==0) || (result==1&&type2==1)) {
                        result = type2; //si el operador recibiera dos tipos iguales pero devolviera otro, debe de cambiarse
                    }else {
                        errorMsgs.Add("\n" +"Error de tipos, " + showType(result) + " y " + showType(type2) + " no son compatibles para el operador " + op + "." + showErrorPosition(context.mulop(i - 1).Start));
                    }
                }
                else { //el operador es solo para int
                    if (result==0&&type2==0 || result==1&&type2==1 ||
                        result==2&&type2==2 || result==3&&type2==3 ||
                        result==0&&type2==1 || result==0&&type2==2 ||
                        result==0&&type2==3  || result==1&&type2==0 ||
                        result==1&&type2==1 || result==1&&type2==2  ||
                        result==1&&type2==3 || result==2&&type2==0 ||
                        result==2&&type2==1 || result==2&&type2==2 ||
                        result==2&&type2==3 || result==3&&type2==0 ||
                        result==3&&type2==1 || result==3&&type2==2 ||
                        result==3&&type2==3) {
                        result = type2;
                    }else {
                        errorMsgs.Add("\n" +"Error de tipos: " + showType(result) + " y " + showType(type2) + " no son compatibles para el operador " + op + "." + showErrorPosition(context.mulop(i - 1).Start));
                    }
                }
            }
            return result;
            /*
            Visit(context.factor(0));
            
            for (int i = 1; context.mulop().Count() > i; i++)
            {
                Visit(context.mulop(i));
            }
            
            for (int sum = 1; context.factor().Count() > sum; sum++)
            {
                Visit(context.factor(sum));
            }

            return null;
            */
        }

        public override object VisitDesignatorFactorAST(MiniCSharpParser.DesignatorFactorASTContext context)
        {
            int result = -1;
            
            if (context.LPAREN() == null)
            {
                result= (int) Visit(context.designator());
            }

            if (context.LPAREN() != null)
            {
                int guardar = designatorAssign;
                designatorAssign = -1;
                TablaSimbolos.Ident i = laTabla.buscarTokenMetodoNombre(context.designator().GetText());
                TablaSimbolos.Ident metodoActual = laTabla.buscarTokenMetodo();
                
                if (i==null)
                {
                    errorMsgs.Add("\n" +"Error de metodo, el identificador \""+ context.designator().GetText() + "\" no puede utilizar \"" + context.LPAREN().GetText() + "\" \""+ context.RPAREN().GetText() + "\", no es un metodo." + showErrorPosition(context.designator().Start));
                }

                if (i.GetToken().ToString() == metodoActual.GetToken().ToString())
                {
                    errorMsgs.Add("\n" +"Error de metodo, el metodo \""+ context.designator().GetText() + "\" no puede utilizarse dentro del mismo metodo." + showErrorPosition(context.designator().Start));
                }else if (i.GetToken().ToString() != metodoActual.GetToken().ToString())
                {
                    List<int> resul = laTabla.obtenerTiposMetodosVariables(i.GetNivel());
                    resul.Reverse();

                    int cantidad = 0;
                    
                    for (int sum2 = 0; context.actPars().ChildCount > sum2; sum2++)
                    {
                        if (context.actPars().GetChild(sum2).GetText() != ",")
                        {
                            cantidad++;
                        }

                    }

                    if (cantidad != resul.Count())
                    {
                        errorMsgs.Add("\n" +"Error de metodo, el identificador \""+ i.GetToken().Text + "\" recibio menos o mas parametros de los solicitados." + showErrorPosition(context.designator().Start));
                    }
                    else
                    {
                        if (i.GetToken().Text != "len" && i.GetToken().Text != "add" && i.GetToken().Text != "del")
                        {
                            int suma = 0;
                            for (int sum2 = 0; context.actPars().ChildCount > sum2; sum2++)
                            {

                                if (context.actPars().GetChild(sum2).GetText() != ",")
                                {
                                    int result2;
                                    methodType = resul[suma];
                                    result2 = (int)Visit(context.actPars().GetChild(sum2));
                                    if ((result2 == 0 && methodType == 0) || (result2 == 1 && methodType == 1) ||
                                        (result2 == 2 && methodType == 2) || (result2 == 3 && methodType == 3) ||
                                        (result2 == 4 && methodType == 4) || (result2 == 5 && methodType == 5) ||
                                        (result2 == 6 && methodType == 6) || (result2 == 7 && methodType == 7) ||
                                        (result2 == 8 && methodType == 8) || (result2 == 9 && methodType == 9) ||
                                        (result2 == 0 && methodType == 1) || (result2 == 2 && methodType == 3) ||
                                        (result2 == 4 && methodType == 5) || (result2 == 6 && methodType == 7) ||
                                        (result2 == 8 && methodType == 9) || (result2 == 0 && methodType == 2) ||
                                        (result2 == 1 && methodType == 3) || (result2 == 0 && methodType == 3))
                                    {
                                    }
                                    else
                                    {
                                        errorMsgs.Add("\n" + "Error de metodo, uno de los parametros requiere \"" +
                                                      showType(methodType) + "\" y se recibio \"" + showType(result2) +
                                                      "\"." + showErrorPosition(context.designator().Start));
                                    }

                                    suma++;
                                }

                            }
                        }else if (i.GetToken().Text == "len")
                        {
                            int verificar;
                            verificar = (int)Visit(context.actPars().GetChild(0));

                            if (verificar != 10 && verificar != 13)
                            {
                                errorMsgs.Add("\n" +"Error de metodo, este metodo requiere el tipo \""+ showType(10) + "\" o el \"" + showType(13) + "\"." + showErrorPosition(context.designator().Start)); 
                            }

                        }else if (i.GetToken().Text == "add")
                        {
                            int verificar;
                            verificar = (int)Visit(context.actPars().GetChild(0));

                                if (verificar != 10 && verificar != 13)
                                {
                                    errorMsgs.Add("\n" +"Error de metodo, este metodo requiere el tipo \""+ showType(10) + "\" o el \"" + showType(13) + "\"." + showErrorPosition(context.designator().Start)); 
                                }
                                else if (verificar == 10 || verificar == 13)
                                {
                                    
                                    TablaSimbolos.Ident designator = null;

                                    if (laTabla.buscarNivel(context.actPars().GetChild(0).GetText(), laTabla.obtenerNivelActual()) != -1)
                                    {
                                        designator = laTabla.buscarToken(context.actPars().GetChild(0).GetText(), laTabla.obtenerNivelActual());
                                    }
                                    else if (laTabla.buscarNivel(context.actPars().GetChild(0).GetText(), laTabla.buscarNivelMetodo()) != -1)
                                    {
                                        designator = laTabla.buscarToken(context.actPars().GetChild(0).GetText(), laTabla.buscarNivelMetodo());
                                    }
                                    else if (laTabla.buscarNivel(context.actPars().GetChild(0).GetText(), 0) != -1)
                                    {
                                        designator = laTabla.buscarToken(context.actPars().GetChild(0).GetText(), 0);
                                    }

                                    methodType = designator.GetSecondType();

                                    result = (int)Visit(context.actPars().GetChild(2));

                                    if ((result == 0 && methodType == 0) || (result == 1 && methodType == 1) ||
                                        (result == 2 && methodType == 2) || (result == 3 && methodType == 3) ||
                                        (result == 4 && methodType == 4) || (result == 5 && methodType == 5) ||
                                        (result == 6 && methodType == 6) || (result == 7 && methodType == 7) ||
                                        (result == 8 && methodType == 8) || (result == 9 && methodType == 9) ||
                                        (result == 0 && methodType == 1) || (result == 2 && methodType == 3) ||
                                        (result == 4 && methodType == 5) || (result == 6 && methodType == 7) ||
                                        (result == 8 && methodType == 9) || (result == 0 && methodType == 2) ||
                                        (result == 1 && methodType == 3) || (result == 0 && methodType == 3))
                                    {
                                    }
                                    else
                                    {
                                        errorMsgs.Add("\n" +"Error de metodo, uno de los parametros requiere \""+ showType(methodType) + "\" y se recibio \"" + showType(result) + "\"." + showErrorPosition(context.designator().Start));
                                    }

                                    
                                }

                        }else if (i.GetToken().Text == "del")
                        {
                            int verificar;
                            verificar = (int)Visit(context.actPars().GetChild(0));

                                if (verificar != 10 && verificar != 13)
                                {
                                    errorMsgs.Add("\n" +"Error de metodo, este metodo requiere el tipo \""+ showType(10) + "\" o el \"" + showType(13) + "\"." + showErrorPosition(context.designator().Start)); 
                                }
                                else if (verificar == 10 || verificar == 13)
                                {
                                    methodType = 0;

                                    result = (int)Visit(context.actPars().GetChild(2));

                                    if ((result == 0 && methodType == 0) || (result == 1 && methodType == 1) ||
                                        (result == 2 && methodType == 2) || (result == 3 && methodType == 3) ||
                                        (result == 4 && methodType == 4) || (result == 5 && methodType == 5) ||
                                        (result == 6 && methodType == 6) || (result == 7 && methodType == 7) ||
                                        (result == 8 && methodType == 8) || (result == 9 && methodType == 9) ||
                                        (result == 0 && methodType == 1) || (result == 2 && methodType == 3) ||
                                        (result == 4 && methodType == 5) || (result == 6 && methodType == 7) ||
                                        (result == 8 && methodType == 9) || (result == 0 && methodType == 2) ||
                                        (result == 1 && methodType == 3) || (result == 0 && methodType == 3))
                                    {
                                    }
                                    else
                                    {
                                        errorMsgs.Add("\n" +"Error de metodo, uno de los parametros requiere \""+ showType(methodType) + "\" y se recibio \"" + showType(result) + "\"." + showErrorPosition(context.designator().Start));
                                    }

                                    
                                }

                        }

                    }


                }
                
                result = i.GetType();
                methodType = -1;
                designatorAssign = guardar;
            }
            return result;
        }

        public override object VisitNumberFactorAST(MiniCSharpParser.NumberFactorASTContext context)
        {
            return 0;
        }

        public override object VisitCharFactorAST(MiniCSharpParser.CharFactorASTContext context)
        {
            return 4;
        }

        public override object VisitStringFactorAST(MiniCSharpParser.StringFactorASTContext context)
        {
            return 6;
        }

        public override object VisitDoubleFactorAST(MiniCSharpParser.DoubleFactorASTContext context)
        {
            return 2;
        }

        public override object VisitBoolFactorAST(MiniCSharpParser.BoolFactorASTContext context)
        {
            return 8;
        }

        public override object VisitNewFactorAST(MiniCSharpParser.NewFactorASTContext context)
        {
            int result = -1;
            try {
                TablaSimbolos.Ident i = laTabla.buscarTokenClaseNombre(context.IDENTIFIER().GetText());
                if (i != null)
                {
                    instanceToken = i;
                    result = i.GetType();
                }
                else
                {
                    errorMsgs.Add("\n" +"Error de clase, identificador \"" + context.IDENTIFIER().GetText() + "\" no es una clase." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            } catch (Exception e){}
            return result;
        }

        public override object VisitExprFactorAST(MiniCSharpParser.ExprFactorASTContext context)
        {
            int result = -1;
            result= (int) Visit(context.expr());
            return result;
        }

        public override object VisitDesignatorAST(MiniCSharpParser.DesignatorASTContext context)
        {
            int result =-1;
            TablaSimbolos.Ident i = null;

            if (context.DOT(0) == null && context.LBRACK(0) == null && context.RBRACK(0) == null)
            {
                if (laTabla.buscarNivel(context.IDENTIFIER(0).GetText(), laTabla.obtenerNivelActual()) != -1 ||
                laTabla.buscarNivel(context.IDENTIFIER(0).GetText(), laTabla.buscarNivelMetodo()) != -1 ||
                laTabla.buscarNivel(context.IDENTIFIER(0).GetText(), 0) != -1)
                {
                    if (laTabla.buscarNivel(context.IDENTIFIER(0).GetText(), laTabla.obtenerNivelActual()) != -1)
                    {
                        i = laTabla.buscarToken(context.IDENTIFIER(0).GetText(), laTabla.obtenerNivelActual());
                    }
                    else if (laTabla.buscarNivel(context.IDENTIFIER(0).GetText(), laTabla.buscarNivelMetodo()) != -1)
                    {
                        i = laTabla.buscarToken(context.IDENTIFIER(0).GetText(), laTabla.buscarNivelMetodo());
                    }
                    else if (laTabla.buscarNivel(context.IDENTIFIER(0).GetText(), 0) != -1)
                    {
                        i = laTabla.buscarToken(context.IDENTIFIER(0).GetText(), 0);
                    }

                    result = i.GetType();
                }else if (context.DOT() == null && context.LBRACK() == null && context.RBRACK() == null && 
                          laTabla.buscarNivelMetodo() != -1)
                {
                    int verificarNivel =-1;

                    int nivel = -1;
                    
                    for (int p = laTabla.buscarNivelMetodo(); laTabla.obtenerNivelActual() > p; p++)
                    {
                        if (laTabla.buscarNivel(context.IDENTIFIER(0).GetText(), p) != -1)
                        {
                            nivel = p;
                            break;
                        }
                    }
                    
                    if (nivel !=-1)
                    {
                        i = laTabla.buscarToken(context.IDENTIFIER(0).GetText(), nivel);
                        result = i.GetType();
                    }
                    else
                    {
                        errorMsgs.Add("\n" +"Error de asignacion, identificador \"" + context.IDENTIFIER(0).GetText() + "\" no declarado." + showErrorPosition(context.IDENTIFIER(0).Symbol));
                    }

                }
            
            }else if (context.DOT(0) == null && context.LBRACK(0) != null && context.RBRACK(0) != null)
            {
                if (laTabla.buscarSegundoTipoArray(context.IDENTIFIER(0).GetText(), laTabla.obtenerNivelActual()) !=
                    -1 ||
                    laTabla.buscarSegundoTipoArray(context.IDENTIFIER(0).GetText(), laTabla.buscarNivelMetodo()) !=
                    -1 ||
                    laTabla.buscarSegundoTipoArray(context.IDENTIFIER(0).GetText(), 0) != -1)
                {
                    if (laTabla.buscarSegundoTipoArray(context.IDENTIFIER(0).GetText(), laTabla.obtenerNivelActual()) != -1 )
                    {
                        i = laTabla.buscarToken(context.IDENTIFIER(0).GetText(), laTabla.obtenerNivelActual());
                    }
                    else if (laTabla.buscarSegundoTipoArray(context.IDENTIFIER(0).GetText(), laTabla.buscarNivelMetodo()) != -1)
                    {
                        i = laTabla.buscarToken(context.IDENTIFIER(0).GetText(), laTabla.buscarNivelMetodo());
                    }
                    else if (laTabla.buscarSegundoTipoArray(context.IDENTIFIER(0).GetText(), 0) != -1)
                    {
                        i = laTabla.buscarToken(context.IDENTIFIER(0).GetText(), 0);
                    }

                    int resultExpr;

                    for (int sum = 0; context.expr().Count() > sum; sum++)
                    {
                        resultExpr = (int) Visit(context.expr(sum));

                        if (resultExpr != 0 && resultExpr != 2)
                        {
                            errorMsgs.Add("\n" +"Error de array, el tipo \"" + showType(resultExpr) + "\" no puede ser usado para un indice." + showErrorPosition(context.expr(sum).Start));
                        }
                    }
                
                    result = i.GetSecondType();
                    
                }
                else if (laTabla.buscarSegundoTipoList(context.IDENTIFIER(0).GetText(), laTabla.obtenerNivelActual()) !=
                         -1 ||
                         laTabla.buscarSegundoTipoList(context.IDENTIFIER(0).GetText(), laTabla.buscarNivelMetodo()) !=
                         -1 ||
                         laTabla.buscarSegundoTipoList(context.IDENTIFIER(0).GetText(), 0) != -1)
                {
                    if (laTabla.buscarSegundoTipoList(context.IDENTIFIER(0).GetText(), laTabla.obtenerNivelActual()) != -1 )
                    {
                        i = laTabla.buscarToken(context.IDENTIFIER(0).GetText(), laTabla.obtenerNivelActual());
                    }
                    else if (laTabla.buscarSegundoTipoList(context.IDENTIFIER(0).GetText(), laTabla.buscarNivelMetodo()) != -1)
                    {
                        i = laTabla.buscarToken(context.IDENTIFIER(0).GetText(), laTabla.buscarNivelMetodo());
                    }
                    else if (laTabla.buscarSegundoTipoList(context.IDENTIFIER(0).GetText(), 0) != -1)
                    {
                        i = laTabla.buscarToken(context.IDENTIFIER(0).GetText(), 0);
                    }
                    
                    int resultExpr;

                    if (context.expr().Count() == 1)
                    {
                        
                        resultExpr = (int) Visit(context.expr(0));

                        if (resultExpr != 0 && resultExpr != 2)
                        {
                            errorMsgs.Add("\n" +"Error de list, el tipo \"" + showType(resultExpr) + "\" no puede ser usado para un indice." + showErrorPosition(context.expr(0).Start));
                        }
                        else
                        {
                            result = i.GetSecondType();
                        }
                        
                    }
                    else
                    {
                        errorMsgs.Add("\n" +"Error de list, se necesita un solo indice para utilizar." + showErrorPosition(context.IDENTIFIER(0).Symbol));
                    }

                }
            }else if (context.DOT(0) != null && context.LBRACK(0) == null && context.RBRACK(0) == null)
            {
                TablaSimbolos.Ident designator = null;
                if (laTabla.buscarNivel(context.IDENTIFIER(0).GetText(), laTabla.obtenerNivelActual()) != -1)
                {
                    designator = laTabla.buscarToken(context.IDENTIFIER(0).GetText(), laTabla.obtenerNivelActual());
                }
                else if (laTabla.buscarNivel(context.IDENTIFIER(0).GetText(), laTabla.buscarNivelMetodo()) != -1)
                {
                    designator = laTabla.buscarToken(context.IDENTIFIER(0).GetText(), laTabla.buscarNivelMetodo());
                }
                else if (laTabla.buscarNivel(context.IDENTIFIER(0).GetText(), 0) != -1)
                {
                    designator = laTabla.buscarToken(context.IDENTIFIER(0).GetText(), 0);
                }

                if (designator.GetInstanceToken() != null)
                {
                    TablaSimbolos.Ident instanceDesignatorClass = laTabla.buscarTokenClaseNombre(designator.GetInstanceToken().Text);
                    TablaSimbolos.Ident designatorClass = laTabla.buscarToken(context.IDENTIFIER(1).GetText(), instanceDesignatorClass.GetNivel());

                    if (designatorClass != null)
                    {

                        if (designatorClass.GetType() == 13)
                        {
                            errorMsgs.Add("\n" +"Error de clase, identificador \"" + context.IDENTIFIER(1).GetText() + "\" es de tipo \"" + showType(13) + "\" debe utilizar: \"" + "[" + "\" y \"" + "]" + "\"." + showErrorPosition(context.IDENTIFIER(1).Symbol));
                        }
                        else
                        {
                                                    
                            result = designatorClass.GetType();
                        }
                    }
                    else
                    {
                        errorMsgs.Add("\n" +"Error de clase, identificador \"" + context.IDENTIFIER(1).GetText() + "\" no existe en la clase." + showErrorPosition(context.IDENTIFIER(1).Symbol));
                    }

                }
                else
                {
                    errorMsgs.Add("\n" +"Error de instancia, identificador \"" + context.IDENTIFIER(0).GetText() + "\" no posee ninguna clase." + showErrorPosition(context.IDENTIFIER(0).Symbol));
                }
                
                
            }else if (context.DOT(0) != null && context.LBRACK(0) != null && context.RBRACK(0) != null)
            {
                TablaSimbolos.Ident designator = null;
                if (laTabla.buscarNivel(context.IDENTIFIER(0).GetText(), laTabla.obtenerNivelActual()) != -1)
                {
                    designator = laTabla.buscarToken(context.IDENTIFIER(0).GetText(), laTabla.obtenerNivelActual());
                }
                else if (laTabla.buscarNivel(context.IDENTIFIER(0).GetText(), laTabla.buscarNivelMetodo()) != -1)
                {
                    designator = laTabla.buscarToken(context.IDENTIFIER(0).GetText(), laTabla.buscarNivelMetodo());
                }
                else if (laTabla.buscarNivel(context.IDENTIFIER(0).GetText(), 0) != -1)
                {
                    designator = laTabla.buscarToken(context.IDENTIFIER(0).GetText(), 0);
                }

                if (designator.GetInstanceToken() != null)
                {
                    TablaSimbolos.Ident instanceDesignatorClass = laTabla.buscarTokenClaseNombre(designator.GetInstanceToken().Text);
                    TablaSimbolos.Ident designatorClass = laTabla.buscarToken(context.IDENTIFIER(1).GetText(), instanceDesignatorClass.GetNivel());

                    if (designatorClass != null)
                    {

                        if (designatorClass.GetType() != 13 && designatorClass.GetType() != 10)
                        {
                            errorMsgs.Add("\n" +"Error de clase, identificador \"" + context.IDENTIFIER(1).GetText() + "\" es de tipo \"" + showType(designatorClass.GetType()) + "\" los: \"" + "[" + "\" y \"" + "]" + "\" solo los puede utilizar el tipo \"" + showType(13)  + "\" y \"" + showType(10)  + "\"." + showErrorPosition(context.IDENTIFIER(1).Symbol));
                        }
                        else
                        {

                            if (designatorClass.GetType() == 10)
                            {
                                int resultExpr;
                                if (context.expr().Count() == 1)
                                {
                        
                                    resultExpr = (int) Visit(context.expr(0));

                                    if (resultExpr != 0 && resultExpr != 2)
                                    {
                                        errorMsgs.Add("\n" +"Error de list, el tipo \"" + showType(resultExpr) + "\" no puede ser usado para un indice." + showErrorPosition(context.expr(0).Start));
                                    }
                                    else
                                    {
                                        result = i.GetSecondType();
                                    }
                        
                                }
                                else
                                {
                                    errorMsgs.Add("\n" +"Error de list, se necesita un solo indice para utilizar." + showErrorPosition(context.IDENTIFIER(0).Symbol));
                                }
                            }else if (designatorClass.GetType() == 13)
                            {
                                int resultExpr;

                                for (int sum = 0; context.expr().Count() > sum; sum++)
                                {
                                    resultExpr = (int) Visit(context.expr(sum));

                                    if (resultExpr != 0 && resultExpr != 2)
                                    {
                                        errorMsgs.Add("\n" +"Error de array, el tipo \"" + showType(resultExpr) + "\" no puede ser usado para un indice." + showErrorPosition(context.expr(sum).Start));
                                    }
                                }
                                result = designatorClass.GetSecondType();
                            }
                        }
                    }
                    else
                    {
                        errorMsgs.Add("\n" +"Error de clase, identificador \"" + context.IDENTIFIER(1).GetText() + "\" no existe en la clase." + showErrorPosition(context.IDENTIFIER(1).Symbol));
                    }

                }
                else
                {
                    errorMsgs.Add("\n" +"Error de instancia, identificador \"" + context.IDENTIFIER(0).GetText() + "\" no posee ninguna clase." + showErrorPosition(context.IDENTIFIER(0).Symbol));
                }
                
                
            }else
            {
                errorMsgs.Add("\n" +"Error de asignacion, identificador \"" + context.IDENTIFIER(0).GetText() + "\" no declarado." + showErrorPosition(context.IDENTIFIER(0).Symbol));
            }

            //for (int i = 0; context.expr().Count() > i; i++)
            //{
                //Visit(context.expr(i));
            //}
            //return context.IDENTIFIER(0).Symbol;
            return result;
        }

        public override object VisitRelop(MiniCSharpParser.RelopContext context)
        {
            return context.GetChild(0).GetText();
        }

        public override object VisitAddop(MiniCSharpParser.AddopContext context)
        {
            return context.GetChild(0).GetText();
        }

        public override object VisitMulop(MiniCSharpParser.MulopContext context)
        {
            return context.GetChild(0).GetText();
        }
    }
}