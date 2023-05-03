using System;
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
        public int MethodType;
        public AContextual(){
            this.laTabla = new TablaSimbolos();
            this.errorMsgs = new ArrayList<String>();
        }
        
        public Boolean hasErrors()
        {
            return this.errorMsgs.Count > 0;
        }
        
        public String toString()
        {
            if (!hasErrors()) return "0 errores de parse";
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
                case 13: return "new";
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
                if (i == null || i != null && laTabla.buscarNivel(context.IDENTIFIER().GetText(), laTabla.obtenerNivelActual()) == -1)
                {
                    laTabla.insertar(id, idType, false, true, false, id);
                }else{
                    errorMsgs.Add("\n" + "Error de clase, la clase \"" + context.IDENTIFIER().GetText() + "\" ya fue declarada." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            } catch (Exception e){}
            
            for (int i = 0; context.varDecl().Count() > i; i++)
            {
                Visit(context.varDecl(i));
            }
            
            for (int i = 0; context.classDecl().Count() > i; i++)
            {
                laTabla.openScope();
                Visit(context.classDecl(i));
            }

            for (int i = 0; context.methodDecl().Count() > i; i++)
            {
                laTabla.openScope();
                Visit(context.methodDecl(i));
            }
            MessageBox.Show(laTabla.imprimir()+ "\n" + toString());
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

                    if (verificar == -1)
                    {
                        laTabla.insertar(id, idType, false, false, false, id);
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

                        if (verificar == -1)
                        {
                            laTabla.insertar(idN, idType, false, false, false, idN);
                        }
                        
                    }else{
                        errorMsgs.Add("\n" + "Error de variable, variable \"" + context.IDENTIFIER(sum).GetText() + "\" ya fue declarada." + showErrorPosition(context.IDENTIFIER(sum).Symbol));
                    }
                }
            } catch (Exception e){}
            return null;
        }

        public override object VisitClassDeclAST(MiniCSharpParser.ClassDeclASTContext context)
        {
            try {
                IToken id = context.IDENTIFIER().Symbol;
                int idType = 12;
                TablaSimbolos.Ident i = laTabla.buscar(context.IDENTIFIER().GetText());
                if (i == null) {
                    laTabla.insertar(id, idType, false, true, false, id);
                    
                    for (int sum = 0; context.varDecl().Count() > sum; sum++)
                    {
                        Visit(context.varDecl(sum));
                    }
                }else{
                    errorMsgs.Add("\n" + "Error de clase, la clase \"" + context.IDENTIFIER().GetText() + "\" ya fue declarada." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            } catch (Exception e){}
            return null;
        }

        public override object VisitMethodDeclAST(MiniCSharpParser.MethodDeclASTContext context)
        {
            MethodType = -1;
            try {
                IToken id = context.IDENTIFIER().Symbol;
                int idType = 11;
                if (context.VOID() == null)
                {
                    idType = (int)Visit(context.type());
                }
                TablaSimbolos.Ident i = laTabla.buscar(context.IDENTIFIER().GetText());
                if (i == null || i != null && laTabla.buscarNivel(context.IDENTIFIER().GetText(), laTabla.obtenerNivelActual()) == -1) {
                    laTabla.insertar(id, idType, true, false, false, id);
                    
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
                        //MessageBox.Show(subcadena);
                        if (subcadenaReturn == "return")
                        {
                            comprobacion = true;
                        }
                    }

                    if (comprobacion.Equals(false) && idType!=11)
                    {
                        errorMsgs.Add("\n" +"Error de metodo, el metodo " + context.IDENTIFIER().GetText() + " usa el tipo " + showType(idType) + " y debe retornar en el mismo nivel al menos una vez" + "."+ showErrorPosition(context.IDENTIFIER().Symbol));
                    }

                    
                    ///TODO revisar porque no se sabe

                }else{
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
                    laTabla.insertar(id, idType, false, false, true, id);
                }else{
                    errorMsgs.Add("\n" + "Error de variable, variable \"" + context.IDENTIFIER(0).GetText() + "\" ya fue declarada." + showErrorPosition(context.IDENTIFIER(0).Symbol));
                }
                
                for (int sum = 1; context.IDENTIFIER().Count() > sum; sum++)
                {
                    IToken idN = context.IDENTIFIER(sum).Symbol;
                    int idTypeN = (int) Visit(context.type(sum));
                    TablaSimbolos.Ident iN = laTabla.buscar(context.IDENTIFIER(sum).GetText());
                    if (iN == null || iN != null && laTabla.buscarNivel(context.IDENTIFIER(sum).GetText(), laTabla.obtenerNivelActual()) == -1){
                        laTabla.insertar(idN, idTypeN,false, false,true, idN);
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
            if (context.QMARK() == null)
            {
                if (context.IDENTIFIER().GetText().Equals("int"))
                {
                    result = 0;
                    if (context.LESS_THAN() != null)
                    {
                        errorMsgs.Add("\n" + "Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" +
                                      context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." +
                                      showErrorPosition(context.IDENTIFIER().Symbol));
                    }
                }
                else if (context.IDENTIFIER().GetText().Equals("double"))
                {
                    result = 2;

                    if (context.LESS_THAN() != null)
                    {
                        errorMsgs.Add("\n" + "Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" +
                                      context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." +
                                      showErrorPosition(context.IDENTIFIER().Symbol));
                    }
                }
                else if (context.IDENTIFIER().GetText().Equals("char"))
                {
                    result = 4;

                    if (context.LESS_THAN() != null)
                    {
                        errorMsgs.Add("\n" + "Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" +
                                      context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." +
                                      showErrorPosition(context.IDENTIFIER().Symbol));
                    }
                }
                else if (context.IDENTIFIER().GetText().Equals("string"))
                {
                    result = 6;

                    if (context.LESS_THAN() != null)
                    {
                        errorMsgs.Add("\n" + "Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" +
                                      context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." +
                                      showErrorPosition(context.IDENTIFIER().Symbol));
                    }
                }
                else if (context.IDENTIFIER().GetText().Equals("bool"))
                {
                    result = 8;

                    if (context.LESS_THAN() != null)
                    {
                        errorMsgs.Add("\n" + "Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" +
                                      context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." +
                                      showErrorPosition(context.IDENTIFIER().Symbol));
                    }
                }
                else if (context.IDENTIFIER().GetText().Equals("list"))
                {
                    result = 10;
                }
                else
                {
                    errorMsgs.Add("\n" + "Error de tipos, tipo \"" + context.IDENTIFIER().GetText() +
                                  "\" no es un tipo valido." + showErrorPosition(context.IDENTIFIER().Symbol));

                    if (context.LESS_THAN() != null && context.GREATER_THAN() != null)
                    {
                        if (context.IDENTIFIER().GetText().Equals("int"))
                        {
                            errorMsgs.Add("\n" + "Error de tipos, tipo \"" + context.IDENTIFIER().GetText() +
                                          "\" no puede recibir otro tipo \"" + context.IDENTIFIER().GetText() + "\" ." +
                                          showErrorPosition(context.IDENTIFIER().Symbol));
                        }
                    }
                }
            }

            if (context.QMARK() != null)
            {
                if (context.IDENTIFIER().GetText().Equals("int"))
                {
                    result = 1;
                
                    if (context.LESS_THAN() != null)
                    {
                        errorMsgs.Add("\n" +"Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                    }
                }else if (context.IDENTIFIER().GetText().Equals("double"))
                {
                    result = 3;
                
                    if (context.LESS_THAN() != null)
                    {
                        errorMsgs.Add("\n" +"Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                    }
                    
                }else if (context.IDENTIFIER().GetText().Equals("char"))
                {
                    result = 5;
                
                    if (context.LESS_THAN() != null)
                    {
                        errorMsgs.Add("\n" +"Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                    }
                    
                }else if (context.IDENTIFIER().GetText().Equals("string"))
                {
                    result = 7;
                
                    if (context.LESS_THAN() != null)
                    {
                        errorMsgs.Add("\n" +"Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                    }
                    
                }else if (context.IDENTIFIER().GetText().Equals("bool"))
                {
                    result =9;
                
                    if (context.LESS_THAN() != null)
                    {
                        errorMsgs.Add("\n" +"Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                    }
                    
                }
                else
                {
                    errorMsgs.Add("\n" + "Error de tipos, tipo \"" + context.IDENTIFIER().GetText() +
                                  "\" no es un tipo valido." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            return result;
        }

        public override object VisitDesignatorStatementAST(MiniCSharpParser.DesignatorStatementASTContext context)
        {
            try
            {
                //int result = 1;
                //result= (int) Visit(context.designator());
                designatorAssign = (int)Visit(context.designator());
                if (context.ASSIGN() != null)
                {
                    Visit(context.expr());
                    /*
                    int tipoExpr = 1;
                    tipoExpr= (int) Visit(context.expr());
                    if (result != tipoExpr)
                    {
                        errorMsgs.Add("\n" + "Error de tipos, \"" + showType(result) + "\" y \"" + showType(tipoExpr) +
                                      "\" no son compatibles." + showErrorPosition(context.ASSIGN().Symbol));
                    }
                    */
                }

                if (context.INCREMENT() != null)
                {
                    if (designatorAssign != 0 && designatorAssign != 1 && designatorAssign != 2 &&
                        designatorAssign != 3)
                    {
                        errorMsgs.Add("\n" +"Error de tipos, \""+ showType(designatorAssign) + "\" no puede utilizar \"" + context.INCREMENT().GetText() + "\"." + showErrorPosition(context.designator().Start));
                    }
                }
                
                if (context.DECREMENT() != null)
                {
                    if (designatorAssign != 0 && designatorAssign != 1 && designatorAssign != 2 &&
                        designatorAssign != 3)
                    {
                        errorMsgs.Add("\n" +"Error de tipos, \""+ showType(designatorAssign) + "\" no puede utilizar \"" + context.DECREMENT().GetText() + "\"." + showErrorPosition(context.designator().Start));
                    }
                }
            
                if (context.actPars() != null)
                {
                    Visit(context.actPars());
                }


            }
            catch (Exception e) {}
            
            /*
            Visit(context.designator());
            
            if (context.ASSIGN() != null)
            {
                Visit(context.expr());
            }
            
            if (context.actPars().ChildCount > 1)
            {
                Visit(context.actPars());
            }
            return null;
            */
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
                MethodType = tipoID;
                result = (int) Visit(context.expr());

                if ((result == 0 && MethodType == 0) || (result == 1 && MethodType == 1) ||
                    (result == 2 && MethodType == 2) || (result == 3 && MethodType == 3) ||
                    (result == 4 && MethodType == 4) || (result == 5 && MethodType == 5) ||
                    (result == 6 && MethodType == 6) || (result == 7 && MethodType == 7) ||
                    (result == 8 && MethodType == 8) || (result == 9 && MethodType == 9) ||
                    (result == 0 && MethodType == 1) || (result == 2 && MethodType == 3) ||
                    (result == 4 && MethodType == 5) || (result == 6 && MethodType == 7) ||
                    (result == 8 && MethodType == 9) || (result == 0 && MethodType == 2) ||
                    (result == 1 && MethodType == 3) || (result == 0 && MethodType == 3))
                {
                }
                else
                {
                    errorMsgs.Add("\n" +"Error de metodo, el metodo " + i.GetToken().Text + " usa el tipo " + showType(MethodType) + " y se esta retornando el tipo " + showType(result) + "."+ showErrorPosition(context.expr().Start));
                }
            }
            
            //int result = -1;
            //if (context.expr()!= null)
            //{
                //result = (int) Visit(context.expr());
            //}
            MethodType = -1;
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
            Visit(context.expr(0));
            for (int i = 1; context.expr().Count() > i; i++)
            {
                Visit(context.expr(i));
            }
            return null;
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
                }else if (MethodType != -1)
                {
                    if (MethodType != 0 && MethodType != 1 && MethodType != 2 &&
                        MethodType != 3)
                    {
                        errorMsgs.Add("\n" +"Error de tipos, \""+ showType(MethodType) + "\" no puede utilizar \"" + context.MINUS().GetText() + "\"." + showErrorPosition(context.term(0).Start));
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
                }else if (MethodType!=-1)
                {
                    if ((cast == 0 && MethodType == 0) || (cast == 1 && MethodType == 1) ||
                        (cast == 2 && MethodType == 2) || (cast == 3 && MethodType == 3) ||
                        (cast == 4 && MethodType == 4) || (cast == 5 && MethodType == 5) ||
                        (cast == 6 && MethodType == 6) || (cast == 7 && MethodType == 7) ||
                        (cast == 8 && MethodType == 8) || (cast == 9 && MethodType == 9) ||
                        (cast == 0 && MethodType == 1) || (cast == 2 && MethodType == 3) ||
                        (cast == 4 && MethodType == 5) || (cast == 6 && MethodType == 7) ||
                        (cast == 8 && MethodType == 9) || (cast == 0 && MethodType == 2) ||
                        (cast == 1 && MethodType == 3) || (cast == 0 && MethodType == 3))
                    {
                        if (cast == 0)
                        {
                            MethodType = 2;
                        }
                        else if (cast == 1)
                        {
                            MethodType = 3;
                        }
                        else if (cast == 2)
                        {
                            MethodType = 2;
                        }
                        else if (cast == 3)
                        {
                            MethodType = 3;
                        }
                        else if (cast == 4)
                        {
                            MethodType = 4;
                        }
                        else if (cast == 5)
                        {
                            MethodType = 5;
                        }
                        else if (cast == 6)
                        {
                            MethodType = 6;
                        }
                        else if (cast == 7)
                        {
                            MethodType = 7;
                        }
                        else if (cast == 8)
                        {
                            MethodType = 8;
                        }
                        else if (cast == 9)
                        {
                            MethodType = 9;
                        }
                    }
                    else
                    {
                        errorMsgs.Add("\n" + "Error de tipos, " + showType(cast) + " y " + showType(MethodType) +
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
                
                if (MethodType != -1 && MethodType == 0 ||
                    MethodType != -1 && MethodType == 2 ||
                    MethodType != -1 && MethodType == 4 ||
                    MethodType != -1 && MethodType == 6 ||
                    MethodType != -1 && MethodType == 8)
                {
                    errorMsgs.Add("\n" + "Error de tipos, " + showType(MethodType) +
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
                
                if (MethodType != -1 && MethodType == 1 ||
                    MethodType != -1 && MethodType == 3 ||
                    MethodType != -1 && MethodType == 5 ||
                    MethodType != -1 && MethodType == 7 ||
                    MethodType != -1 && MethodType == 9)
                {
                    MethodType--;
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
                    (result == 1 && designatorAssign == 3) || (result == 0 && designatorAssign == 3))
                {
                }
                else
                {
                    errorMsgs.Add("\n" +"Error de tipos: \""+ showType(designatorAssign) + "\" y \"" + showType(result) + "\" no son compatibles." + showErrorPosition(context.term(0).Start));
                }
            }else if (MethodType != -1)
            {
                if ((result == 0 && MethodType == 0) || (result == 1 && MethodType == 1) ||
                    (result == 2 && MethodType == 2) || (result == 3 && MethodType == 3) ||
                    (result == 4 && MethodType == 4) || (result == 5 && MethodType == 5) ||
                    (result == 6 && MethodType == 6) || (result == 7 && MethodType == 7) ||
                    (result == 8 && MethodType == 8) || (result == 9 && MethodType == 9) ||
                    (result == 0 && MethodType == 1) || (result == 2 && MethodType == 3) ||
                    (result == 4 && MethodType == 5) || (result == 6 && MethodType == 7) ||
                    (result == 8 && MethodType == 9) || (result == 0 && MethodType == 2) ||
                    (result == 1 && MethodType == 3) || (result == 0 && MethodType == 3))
                {
                }
                else
                {
                    errorMsgs.Add("\n" +"Error de tipos: \""+ showType(MethodType) + "\" y \"" + showType(result) + "\" no son compatibles." + showErrorPosition(context.term(0).Start));
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
            }else if (MethodType != -1)
            {
                if ((type2 == 0 && MethodType == 0) || (type2 == 1 && MethodType == 1) ||
                    (type2 == 2 && MethodType == 2) || (type2 == 3 && MethodType == 3) ||
                    (type2 == 4 && MethodType == 4) || (type2 == 5 && MethodType == 5) ||
                    (type2 == 6 && MethodType == 6) || (type2 == 7 && MethodType == 7) ||
                    (type2 == 8 && MethodType == 8) || (type2 == 9 && MethodType == 9) ||
                    (type2 == 0 && MethodType == 1) || (type2 == 2 && MethodType == 3) ||
                    (type2 == 4 && MethodType == 5) || (type2 == 6 && MethodType == 7) ||
                    (type2 == 8 && MethodType == 9) || (type2 == 0 && MethodType == 2) ||
                    (type2 == 1 && MethodType == 3) || (type2 == 0 && MethodType == 3))
                {

                }
                else
                {
                    errorMsgs.Add("\n" +"Error de tipos: \""+ showType(MethodType) + "\" y \"" + showType(type2) + "\" no son compatibles." + showErrorPosition(context.term(0).Start));
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

            designatorAssign = -1;
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
                }else if (MethodType != -1)
                {
                    if ((type2 == 0 && MethodType == 0) || (type2 == 1 && MethodType == 1) ||
                        (type2 == 2 && MethodType == 2) || (type2 == 3 && MethodType == 3) ||
                        (type2 == 4 && MethodType == 4) || (type2 == 5 && MethodType == 5) ||
                        (type2 == 6 && MethodType == 6) || (type2 == 7 && MethodType == 7) ||
                        (type2 == 8 && MethodType == 8) || (type2 == 9 && MethodType == 9) ||
                        (type2 == 0 && MethodType == 1) || (type2 == 2 && MethodType == 3) ||
                        (type2 == 4 && MethodType == 5) || (type2 == 6 && MethodType == 7) ||
                        (type2 == 8 && MethodType == 9) || (type2 == 0 && MethodType == 2) ||
                        (type2 == 1 && MethodType == 3) || (type2 == 0 && MethodType == 3))
                    {

                    }
                    else
                    {
                        errorMsgs.Add("\n" +"Error de tipos: \""+ showType(MethodType) + "\" y \"" + showType(type2) + "\" no son compatibles." + showErrorPosition(context.factor(i).Start));
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
            result= (int) Visit(context.designator());
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
            //TODO MODIFICAR
            return context.IDENTIFIER().Symbol;
        }

        public override object VisitExprFactorAST(MiniCSharpParser.ExprFactorASTContext context)
        {
            Visit(context.expr());
            return null;
        }

        public override object VisitNullFactorAST(MiniCSharpParser.NullFactorASTContext context)
        {
            return 21;
        }

        public override object VisitDesignatorAST(MiniCSharpParser.DesignatorASTContext context)
        {
            //TODO MODIFICAR
            int result =-1;
            TablaSimbolos.Ident i = null;

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
            }else if (laTabla.buscarNivelMetodo() != -1)
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
            else
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