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
                case ">": return true;
                case ">=": return true;
                case "<": return true;
                case "<=": return true;
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
                if (i == null || i != null && laTabla.buscarNivel(context.IDENTIFIER().GetText(), laTabla.obtenerNivelActual()) == -1) {
                    laTabla.insertar(id, idType, true, false, id);
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
                if (i == null || i != null && laTabla.buscarNivel(context.IDENTIFIER(0).GetText(), laTabla.obtenerNivelActual()) == -1) {
                    
                    laTabla.insertar(id, idType,false, false, id);
                }else{
                    errorMsgs.Add("\n" + "Error de variable, variable \"" + context.IDENTIFIER(0).GetText() + "\" ya fue declarada." + showErrorPosition(context.IDENTIFIER(0).Symbol));
                }
                
                for (int sum = 1; context.IDENTIFIER().Count() > sum; sum++)
                {
                    IToken idN = context.IDENTIFIER(sum).Symbol;
                    TablaSimbolos.Ident iN = laTabla.buscar(context.IDENTIFIER(sum).GetText());
                    if (iN == null || iN != null && laTabla.buscarNivel(context.IDENTIFIER(sum).GetText(), laTabla.obtenerNivelActual()) == -1) {
                        laTabla.insertar(idN, idType,false, false, idN);
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
                    laTabla.insertar(id, idType, true, false, id);
                    
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
            try {
                IToken id = context.IDENTIFIER().Symbol;
                int idType = 11;
                if (context.VOID() == null)
                {
                    idType = (int)Visit(context.type());
                }
                TablaSimbolos.Ident i = laTabla.buscar(context.IDENTIFIER().GetText());
                if (i == null || i != null && laTabla.buscarNivel(context.IDENTIFIER().GetText(), laTabla.obtenerNivelActual()) == -1) {
                    laTabla.insertar(id, idType, true, false, id);
                    Visit(context.block());
                    
                    ///TODO revisar porque no se sabe
                    if (context.formPars().ChildCount > 1)
                    {
                        Visit(context.formPars());
                    }

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
                if (i == null || i != null && laTabla.buscarNivel(context.IDENTIFIER(0).GetText(), laTabla.obtenerNivelActual()) == -1) {
                    laTabla.insertar(id, idType,true, false, id);
                }else{
                    errorMsgs.Add("\n" + "Error de variable, variable \"" + context.IDENTIFIER(0).GetText() + "\" ya fue declarada." + showErrorPosition(context.IDENTIFIER(0).Symbol));
                }
                
                for (int sum = 1; context.IDENTIFIER().Count() > sum; sum++)
                {
                    IToken idN = context.IDENTIFIER(sum).Symbol;
                    int idTypeN = (int) Visit(context.type(sum));
                    TablaSimbolos.Ident iN = laTabla.buscar(context.IDENTIFIER(sum).GetText());
                    if (iN == null || iN != null && laTabla.buscarNivel(context.IDENTIFIER(sum).GetText(), laTabla.obtenerNivelActual()) == -1){
                        laTabla.insertar(idN, idTypeN,true, false, idN);
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
            if (context.IDENTIFIER().GetText().Equals("int"))
            {
                result = 0;
                if (context.LESS_THAN() != null)
                {
                    errorMsgs.Add("\n" +"Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            else if (context.IDENTIFIER().GetText().Equals("int?"))
            {
                result = 1;
                
                if (context.LESS_THAN() != null)
                {
                    errorMsgs.Add("\n" +"Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            else if (context.IDENTIFIER().GetText().Equals("double"))
            {
                result = 2;
                
                if (context.LESS_THAN() != null)
                {
                    errorMsgs.Add("\n" +"Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            else if (context.IDENTIFIER().GetText().Equals("double?"))
            {
                result = 3;
                
                if (context.LESS_THAN() != null)
                {
                    errorMsgs.Add("\n" +"Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            else if (context.IDENTIFIER().GetText().Equals("char"))
            {
                result = 4;
                
                if (context.LESS_THAN() != null)
                {
                    errorMsgs.Add("\n" +"Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            else if (context.IDENTIFIER().GetText().Equals("char?"))
            {
                result = 5;
                
                if (context.LESS_THAN() != null)
                {
                    errorMsgs.Add("\n" +"Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            else if (context.IDENTIFIER().GetText().Equals("string"))
            {
                result = 6;
                
                if (context.LESS_THAN() != null)
                {
                    errorMsgs.Add("\n" +"Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            else if (context.IDENTIFIER().GetText().Equals("string?"))
            {
                result = 7;
                
                if (context.LESS_THAN() != null)
                {
                    errorMsgs.Add("\n" +"Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            else if (context.IDENTIFIER().GetText().Equals("bool"))
            {
                result = 8;
                
                if (context.LESS_THAN() != null)
                {
                    errorMsgs.Add("\n" +"Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            else if (context.IDENTIFIER().GetText().Equals("bool?"))
            {
                result = 9;
                
                if (context.LESS_THAN() != null)
                {
                    errorMsgs.Add("\n" +"Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            else if (context.IDENTIFIER().GetText().Equals("list"))
            {
                result = 10;
            }
            else {
                errorMsgs.Add("\n" +"Error de tipos, tipo \"" + context.IDENTIFIER().GetText() + "\" no es un tipo valido." + showErrorPosition(context.IDENTIFIER().Symbol));
                
                if (context.LESS_THAN() != null && context.GREATER_THAN() != null)
                {
                    if (context.IDENTIFIER().GetText().Equals("int"))
                    {
                        errorMsgs.Add("\n" +"Error de tipos, tipo \"" + context.IDENTIFIER().GetText() + "\" no puede recibir otro tipo \"" + context.IDENTIFIER().GetText() + "\" ." + showErrorPosition(context.IDENTIFIER().Symbol));
                    }
                }
            }
            return result;
        }

        public override object VisitDesignatorStatementAST(MiniCSharpParser.DesignatorStatementASTContext context)
        {
            try {
                if (context.expr().GetChild(0).GetText().Contains("new"))
                {
                    string cadena = context.expr().GetChild(0).GetText();
                    cadena = cadena.Replace("new", "");
                    IToken id = (IToken) Visit(context.designator());
                    TablaSimbolos.Ident idExpr = laTabla.buscar(cadena);
                    int idType = 13;
                    if (laTabla.buscarMetodo(context.expr().GetChild(1).GetText(), idType) != -1) {
                        laTabla.insertar(id, idType, false, true, idExpr);
                    }else{
                        errorMsgs.Add("\n" + "Error de instancia, la clase o metodo \"" + context.expr().GetChild(1).GetText() + "\" no ha sido creada." + showErrorPosition(id));
                    }
                }
                
                //MessageBox.Show(context.designator().GetText());
                if (laTabla.buscarNivel(context.designator().GetText(), laTabla.obtenerNivelActual()) != -1)
                {

                }else if (laTabla.buscarNivel(context.designator().GetText(), laTabla.buscarNivelMetodo()) != -1)
                {
                    //int nivelMetodo = laTabla.buscarNivelMetodo();
                }else if (laTabla.buscarNivel(context.designator().GetText(), 0) != -1)
                {

                }
                else
                {
                    errorMsgs.Add("\n" +"Error de alcances, identificador \"" + context.designator().GetText() + "\" no declarado en asignación." + showErrorPosition(context.designator().Start));
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
            if (context.statement().Count() > 1)
            {
                Visit(context.statement(1));
            }

            return null;
        }

        public override object VisitForStatementAST(MiniCSharpParser.ForStatementASTContext context)
        {
            Visit(context.expr());
            
            if (context.condition().ChildCount > 1)
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
            //esto debe hacer algo
            return null;
        }

        public override object VisitReturnStatementAST(MiniCSharpParser.ReturnStatementASTContext context)
        {
            if (context.expr().ChildCount > 1)
            {
                Visit(context.expr());
            }
            return null;
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
            Visit(context.expr(0));
            Visit(context.relop());
            Visit(context.expr(1));
            return null;
        }

        public override object VisitCastAST(MiniCSharpParser.CastASTContext context)
        {
            Visit(context.type());
            return null;
        }

        public override object VisitExprAST(MiniCSharpParser.ExprASTContext context)
        {
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

            return null;
        }

        public override object VisitTermAST(MiniCSharpParser.TermASTContext context)
        {
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
        }

        public override object VisitDesignatorFactorAST(MiniCSharpParser.DesignatorFactorASTContext context)
        {
            Visit(context.designator());
            return null;
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
            
            return context.IDENTIFIER().Symbol;
        }

        public override object VisitExprFactorAST(MiniCSharpParser.ExprFactorASTContext context)
        {
            Visit(context.expr());
            return null;
        }

        public override object VisitDesignatorAST(MiniCSharpParser.DesignatorASTContext context)
        {
            for (int i = 0; context.expr().Count() > i; i++)
            {
                Visit(context.expr(i));
            }
            return context.IDENTIFIER(0).Symbol;
        }

        public override object VisitRelop(MiniCSharpParser.RelopContext context)
        {
            return null;
        }

        public override object VisitAddop(MiniCSharpParser.AddopContext context)
        {
            return null;
        }

        public override object VisitMulop(MiniCSharpParser.MulopContext context)
        {
            return null;
        }
    }
}