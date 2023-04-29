using System;
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
                int idType = 6;
                TablaSimbolos.Ident i = laTabla.buscar(context.IDENTIFIER().GetText());
                if (i == null || i != null && laTabla.buscarNivel(context.IDENTIFIER().GetText()) != laTabla.obtenerNivelActual()) {
                    laTabla.insertar(id, idType, true, 0);
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
                Visit(context.methodDecl(i));
            }
            MessageBox.Show(laTabla.imprimir()+ "\n" + toString());
            laTabla.closeScope();
            //Visit(context.@using(0));
            
            //Visit(context.classDecl(0));
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
                if (i == null || i != null && laTabla.buscarNivel(context.IDENTIFIER(0).GetText()) != laTabla.obtenerNivelActual()) {
                    laTabla.insertar(id, idType,false, 0);
                }else{
                    errorMsgs.Add("\n" + "Error de variable, variable \"" + context.IDENTIFIER(0).GetText() + "\" ya fue declarada." + showErrorPosition(context.IDENTIFIER(0).Symbol));
                }
                
                for (int sum = 1; context.IDENTIFIER().Count() > sum; sum++)
                {
                    IToken idN = context.IDENTIFIER(sum).Symbol;
                    TablaSimbolos.Ident iN = laTabla.buscar(context.IDENTIFIER(sum).GetText());
                    if (iN == null || iN != null && laTabla.buscarNivel(context.IDENTIFIER(sum).GetText()) != laTabla.obtenerNivelActual()) {
                        laTabla.insertar(idN, idType,false, 0);
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
                int idType = 6;
                TablaSimbolos.Ident i = laTabla.buscar(context.IDENTIFIER().GetText());
                if (i == null) {
                    laTabla.insertar(id, idType, true, 0);
                    
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
                if (i == null || i != null && laTabla.buscarNivel(context.IDENTIFIER().GetText()) != laTabla.obtenerNivelActual()) {
                    laTabla.insertar(id, idType, true, 1);
                    if (context.formPars().ChildCount > 1)
                    {
                        Visit(context.formPars());
                    }
                    Visit(context.block());
                    
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
                if (i == null || i != null && laTabla.buscarNivel(context.IDENTIFIER(0).GetText()) != laTabla.obtenerNivelActual()) {
                    laTabla.insertar(id, idType,true, 0);
                }else{
                    errorMsgs.Add("\n" + "Error de variable, variable \"" + context.IDENTIFIER(0).GetText() + "\" ya fue declarada." + showErrorPosition(context.IDENTIFIER(0).Symbol));
                }
                
                for (int sum = 1; context.IDENTIFIER().Count() > sum; sum++)
                {
                    IToken idN = context.IDENTIFIER(sum).Symbol;
                    int idTypeN = (int) Visit(context.type(sum));
                    TablaSimbolos.Ident iN = laTabla.buscar(context.IDENTIFIER(sum).GetText());
                    if (iN == null || iN != null && laTabla.buscarNivel(context.IDENTIFIER(sum).GetText()) != laTabla.obtenerNivelActual()) {
                        laTabla.insertar(idN, idTypeN,true, 0);
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
                    errorMsgs.Add("Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            else if (context.IDENTIFIER().GetText().Equals("int?"))
            {
                result = 1;
                
                if (context.LESS_THAN() != null)
                {
                    errorMsgs.Add("Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            else if (context.IDENTIFIER().GetText().Equals("double"))
            {
                result = 2;
                
                if (context.LESS_THAN() != null)
                {
                    errorMsgs.Add("Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            else if (context.IDENTIFIER().GetText().Equals("double?"))
            {
                result = 3;
                
                if (context.LESS_THAN() != null)
                {
                    errorMsgs.Add("Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            else if (context.IDENTIFIER().GetText().Equals("char"))
            {
                result = 4;
                
                if (context.LESS_THAN() != null)
                {
                    errorMsgs.Add("Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            else if (context.IDENTIFIER().GetText().Equals("char?"))
            {
                result = 5;
                
                if (context.LESS_THAN() != null)
                {
                    errorMsgs.Add("Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            else if (context.IDENTIFIER().GetText().Equals("string"))
            {
                result = 6;
                
                if (context.LESS_THAN() != null)
                {
                    errorMsgs.Add("Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            else if (context.IDENTIFIER().GetText().Equals("string?"))
            {
                result = 7;
                
                if (context.LESS_THAN() != null)
                {
                    errorMsgs.Add("Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            else if (context.IDENTIFIER().GetText().Equals("bool"))
            {
                result = 8;
                
                if (context.LESS_THAN() != null)
                {
                    errorMsgs.Add("Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            else if (context.IDENTIFIER().GetText().Equals("bool?"))
            {
                result = 9;
                
                if (context.LESS_THAN() != null)
                {
                    errorMsgs.Add("Error de tipos, el \"" + context.LESS_THAN().GetText() + "\" y el \"" + context.GREATER_THAN().GetText() + "\"solo se puede usar en el tipo list." + showErrorPosition(context.IDENTIFIER().Symbol));
                }
            }
            else if (context.IDENTIFIER().GetText().Equals("list"))
            {
                result = 10;
            }
            else {
                errorMsgs.Add("Error de tipos, tipo \"" + context.IDENTIFIER().GetText() + "\" no es un tipo valido." + showErrorPosition(context.IDENTIFIER().Symbol));
                
                if (context.LESS_THAN() != null && context.GREATER_THAN() != null)
                {
                    if (context.IDENTIFIER().GetText().Equals("int"))
                    {
                        errorMsgs.Add("Error de tipos, tipo \"" + context.IDENTIFIER().GetText() + "\" no puede recibir otro tipo \"" + context.IDENTIFIER().GetText() + "\" ." + showErrorPosition(context.IDENTIFIER().Symbol));
                    }
                }
            }
            return result;
        }

        public override object VisitDesignatorStatementAST(MiniCSharpParser.DesignatorStatementASTContext context)
        {
            /*
try {
TablaSimbolos.Ident i = laTabla.buscar((string) Visit(context.designator()));

if (i == null)
    System.out.println("Error de alcances, identificador \"" + ctx.ID().getText() + "\" no declarado en asignación." + showErrorPosition(ctx.ID().getSymbol()));
else {
    int tipoID = i.type;
    int variableID = i.variable;
    if (tipoID==2){
        System.out.println("Error de tipos: identificador \"" + ctx.ID().getText() + "\" es de tipo void, no se puede usar en asignacion." + showErrorPosition(ctx.ID().getSymbol()));
    }
    if (variableID==0){
        System.out.println("Error de tipos: identificador \"" + ctx.ID().getText() + "\" es una constante, no se puede usar en asignacion." + showErrorPosition(ctx.ID().getSymbol()));
    }
    int tipoExpr = (int) visit(ctx.expression());
    if (tipoID != tipoExpr)//o al menos compatibles
        System.out.println("Error de tipos: \""+ showType(tipoID) + "\" y \"" + showType(tipoExpr) + "\" no son compatibles." + showErrorPosition(ctx.Assign().getSymbol()));
}

}
catch (RuntimeException e) {}
*/
        return null;
        }

        public override object VisitIfStatementAST(MiniCSharpParser.IfStatementASTContext context)
        {
            return null;
        }

        public override object VisitForStatementAST(MiniCSharpParser.ForStatementASTContext context)
        {
            return null;
        }

        public override object VisitWhileStatementAST(MiniCSharpParser.WhileStatementASTContext context)
        {
            return null;
        }

        public override object VisitBreakStatementAST(MiniCSharpParser.BreakStatementASTContext context)
        {
            return null;
        }

        public override object VisitReturnStatementAST(MiniCSharpParser.ReturnStatementASTContext context)
        {
            return null;
        }

        public override object VisitReadStatementAST(MiniCSharpParser.ReadStatementASTContext context)
        {
            return null;
        }

        public override object VisitWriteStatementAST(MiniCSharpParser.WriteStatementASTContext context)
        {
            return null;
        }

        public override object VisitBlockStatementAST(MiniCSharpParser.BlockStatementASTContext context)
        {
            return null;
        }

        public override object VisitSemicolonStatementAST(MiniCSharpParser.SemicolonStatementASTContext context)
        {
            return null;
        }

        public override object VisitBlockAST(MiniCSharpParser.BlockASTContext context)
        {
            return null;
        }

        public override object VisitActParsAST(MiniCSharpParser.ActParsASTContext context)
        {
            return null;
        }

        public override object VisitConditionAST(MiniCSharpParser.ConditionASTContext context)
        {
            return null;
        }

        public override object VisitCondTermAST(MiniCSharpParser.CondTermASTContext context)
        {
            return null;
        }

        public override object VisitCondFactAST(MiniCSharpParser.CondFactASTContext context)
        {
            return null;
        }

        public override object VisitCastAST(MiniCSharpParser.CastASTContext context)
        {
            return null;
        }

        public override object VisitExprAST(MiniCSharpParser.ExprASTContext context)
        {
            return null;
        }

        public override object VisitTermAST(MiniCSharpParser.TermASTContext context)
        {
            return null;
        }

        public override object VisitDesignatorFactorAST(MiniCSharpParser.DesignatorFactorASTContext context)
        {
            return null;
        }

        public override object VisitNumberFactorAST(MiniCSharpParser.NumberFactorASTContext context)
        {
            return null;
        }

        public override object VisitCharFactorAST(MiniCSharpParser.CharFactorASTContext context)
        {
            return null;
        }

        public override object VisitStringFactorAST(MiniCSharpParser.StringFactorASTContext context)
        {
            return null;
        }

        public override object VisitDoubleFactorAST(MiniCSharpParser.DoubleFactorASTContext context)
        {
            return null;
        }

        public override object VisitBoolFactorAST(MiniCSharpParser.BoolFactorASTContext context)
        {
            return null;
        }

        public override object VisitNewFactorAST(MiniCSharpParser.NewFactorASTContext context)
        {
            return null;
        }

        public override object VisitExprFactorAST(MiniCSharpParser.ExprFactorASTContext context)
        {
            return null;
        }

        public override object VisitDesignatorAST(MiniCSharpParser.DesignatorASTContext context)
        {
            return null;
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