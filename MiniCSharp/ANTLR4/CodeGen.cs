using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using generated;

namespace MiniCSharp.ANTLR4
{
    public class CodeGen : MiniCSharpParserBaseVisitor<object>
    {
        private string nombreTxt = "";
        public void cambiarNombreTxt(string nombre)
        {
            nombreTxt = nombre;
        }
        
        private Type pointType = null;
        private string asmFileName = "result.exe";
        private AssemblyName myAsmName = new AssemblyName();

        private AppDomain currentDom = Thread.GetDomain();
        private AssemblyBuilder myAsmBldr;

        private ModuleBuilder myModuleBldr;

        private TypeBuilder myTypeBldr;
        private ConstructorInfo objCtor=null;

        private MethodInfo writeMI, writeMS;

        private MethodBuilder pointMainBldr, currentMethodBldr;

        private List<MethodBuilder> metodosGlobales; 

        private bool isArgument = false;
        
        public CodeGen()
        {
            metodosGlobales = new List<MethodBuilder>();
            
            myAsmName.Name = nombreTxt;
            myAsmBldr = currentDom.DefineDynamicAssembly(myAsmName, AssemblyBuilderAccess.RunAndSave);
            myModuleBldr = myAsmBldr.DefineDynamicModule(asmFileName);
            myTypeBldr = myModuleBldr.DefineType(nombreTxt+"Class");
            
            Type objType = Type.GetType("System.Object");
            objCtor = objType.GetConstructor(new Type[0]);
            
            Type[] ctorParams = new Type[0];
            ConstructorBuilder pointCtor = myTypeBldr.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                ctorParams);
            ILGenerator ctorIL = pointCtor.GetILGenerator();
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Call, objCtor);
            ctorIL.Emit(OpCodes.Ret);
            
            //inicializar writeline para string
            
            writeMI = typeof(Console).GetMethod(
                "WriteLine",
                new Type[] { typeof(int) });
            writeMS = typeof(Console).GetMethod(
                "WriteLine",
                new Type[] { typeof(string) });
            
        }

        private Type verificarTipoRetorno(string tipo)
        {
            if (tipo.Equals("int"))
            {
                return typeof(int);
            }
            if (tipo.Equals("int?"))
            {
                return typeof(int?);
            }
            if (tipo.Equals("double"))
            {
                return typeof(double);
            }
            if (tipo.Equals("double?"))
            {
                return typeof(double?);
            }
            if (tipo.Equals("char"))
            {
                return typeof(char);
            }
            if (tipo.Equals("char?"))
            {
                return typeof(char?);
            }
            if (tipo.Equals("string"))
            {
                return typeof(string);
            }
            if (tipo.Equals("string?"))
            {
                return typeof(string);
            }
            if (tipo.Equals("bool"))
            {
                return typeof(char);
            }
            if (tipo.Equals("bool?"))
            {
                return typeof(bool?);
            }
            if (tipo.Equals("inst"))
            {
                return null;/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
            
            
            if (tipo.Equals("List<int>"))
            {
                return typeof(List<int>);
            }
            if (tipo.Equals("List<int?>"))
            {
                return typeof(List<int?>);
            }
            if (tipo.Equals("List<double>"))
            {
                return typeof(List<double>);
            }
            if (tipo.Equals("List<double?>"))
            {
                return typeof(List<double?>);
            }
            if (tipo.Equals("List<char>"))
            {
                return typeof(List<char>);
            }
            if (tipo.Equals("List<char?>"))
            {
                return typeof(List<char?>);
            }
            if (tipo.Equals("List<string>"))
            {
                return typeof(List<string>);
            }
            if (tipo.Equals("List<string?>"))
            {
                return typeof(List<string>);
            }
            if (tipo.Equals("List<bool>"))
            {
                return typeof(List<bool>);
            }
            if (tipo.Equals("List<bool?>"))
            {
                return typeof(List<bool?>);
            }
            
            

            if (tipo.Equals("int[]"))
            {
                return typeof(int[]);
            }
            if (tipo.Equals("int?[]"))
            {
                return typeof(int?[]);
            }
            if (tipo.Equals("double[]"))
            {
                return typeof(double[]);
            }
            if (tipo.Equals("double?[]"))
            {
                return typeof(double?[]);
            }
            if (tipo.Equals("char[]"))
            {
                return typeof(char[]);
            }
            if (tipo.Equals("char?[]"))
            {
                return typeof(char?[]);
            }
            if (tipo.Equals("string[]"))
            {
                return typeof(string[]);
            }
            if (tipo.Equals("string?[]"))
            {
                return typeof(string[]);
            }
            if (tipo.Equals("bool[]"))
            {
                return typeof(bool[]);
            }
            if (tipo.Equals("bool?[]"))
            {
                return typeof(bool?[]);
            }
            else
            {
                return typeof(void);
            }
        }


        public override object VisitProgramAST(MiniCSharpParser.ProgramASTContext context)
        {
            for (int a = 0; context.@using().Count() > a; a++)
            {
                Visit(context.@using(a));
            }
            
            pointType = myTypeBldr.CreateType(); //crea la clase para ser luego instanciada
            myAsmBldr.SetEntryPoint(pointMainBldr);
            myAsmBldr.Save(asmFileName);

            for (int i = 0; context.varDecl().Count() > i; i++)
            {
                Visit(context.varDecl(i));
            }
            
            for (int i = 0; context.classDecl().Count() > i; i++)
            {
                Visit(context.classDecl(i));
            }
            
            for (int i = 0; context.methodDecl().Count() > i; i++)
            {
                Visit(context.methodDecl(i));
            }

            return pointType;
        }

        public override object VisitUsingAST(MiniCSharpParser.UsingASTContext context)
        {
            return null;
        }

        public override object VisitVarDeclAST(MiniCSharpParser.VarDeclASTContext context)
        {
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();

            Visit(context.type());
            
            currentIL.DeclareLocal((Type)Visit(context.IDENTIFIER(0)));

            for (int i = 1; context.IDENTIFIER().Count() > i; i++)
            {
                currentIL.DeclareLocal((Type)Visit(context.IDENTIFIER(i)));
            }
            
            return null;
        }

        public override object VisitClassDeclAST(MiniCSharpParser.ClassDeclASTContext context)
        {
            for (int i = 0; context.varDecl().Count() > i; i++)
            {
                Visit(context.varDecl(i));
            }
            return null;
        }

        public override object VisitMethodDeclAST(MiniCSharpParser.MethodDeclASTContext context)
        {
            if (context.VOID() == null)
            {
                Visit(context.type());
            }

            if (context.formPars() != null)
            {
                Visit(context.formPars());
            }

            Visit(context.block());
            
            return null;
        }

        public override object VisitFormParsAST(MiniCSharpParser.FormParsASTContext context)
        {
            Visit(context.type(0));
            
            for (int i = 1; context.type().Count() > i; i++)
            {
                Visit(context.type(i));
            }
            return null;
        }

        public override object VisitTypeAST(MiniCSharpParser.TypeASTContext context)
        {
            //PENSAAAR

            if (context.QMARK() == null && context.LESS_THAN() == null && context.LBRACK() == null)
            {
                return context.IDENTIFIER(0).GetText();
            }
            if (context.QMARK() != null && context.LESS_THAN() == null && context.LBRACK() == null)
            {
                return context.IDENTIFIER(0).GetText()+"?";
            }
            if (context.QMARK() == null && context.LESS_THAN() != null && context.LBRACK() == null)
            {
                return "List"+"<"+context.IDENTIFIER(0).GetText()+">";
            }
            if (context.QMARK() != null && context.LESS_THAN() != null && context.LBRACK() == null)
            {
                return "List"+"<"+context.IDENTIFIER(0).GetText()+"?"+">";
            }
            if (context.QMARK() == null && context.LESS_THAN() == null && context.LBRACK() != null)
            {
                return context.IDENTIFIER(0).GetText()+"[]";
            }
            if (context.QMARK() != null && context.LESS_THAN() == null && context.LBRACK() != null)
            {
                return context.IDENTIFIER(0).GetText()+"?"+"[]";
            }
            
            return null;
        }

        public override object VisitDesignatorStatementAST(MiniCSharpParser.DesignatorStatementASTContext context)
        {
            Visit(context.designator());

            if (context.ASSIGN() != null)
            {
                Visit(context.expr());
            }else if (context.LPAREN() != null)
            {
                Visit(context.actPars());
            }else if (context.INCREMENT() != null)
            {
                /////
            }else if (context.DECREMENT() != null)
            {
                /////
            }

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
            ///PENSAR
            return null;
        }

        public override object VisitReturnStatementAST(MiniCSharpParser.ReturnStatementASTContext context)
        {
            ///PENSAR
            if (context.expr() != null)
            {
                Visit(context.expr());
            }
            return null;
        }

        public override object VisitReadStatementAST(MiniCSharpParser.ReadStatementASTContext context)
        {
            ///PENSAR
            Visit(context.designator());
            return null;
        }

        public override object VisitWriteStatementAST(MiniCSharpParser.WriteStatementASTContext context)
        {
            Visit(context.expr());
            
            if (context.COMMA() != null)
            {
               ///PENSAR
            }
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
            
            for (int i = 0; context.statement().Count() > i; i++)
            {
                Visit(context.statement(i));
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
            if (context.MINUS() != null)
            {
                ///
            }
            
            if (context.cast() != null)
            {
                Visit(context.cast());
            }

            Visit(context.term(0));
            
            for (int i = 1; context.term().Count() > i; i++)
            {
                Visit(context.addop(i-1));
                Visit(context.term(i));
            }
            
            return null;
        }

        public override object VisitTermAST(MiniCSharpParser.TermASTContext context)
        {
            Visit(context.factor(0));
            
            for (int i = 1; context.factor().Count() > i; i++)
            {
                Visit(context.mulop(i-1));
                Visit(context.factor(i));
            }
            return null;
        }

        public override object VisitDesignatorFactorAST(MiniCSharpParser.DesignatorFactorASTContext context)
        {
            Visit(context.designator());

            if (context.LPAREN() != null)
            {
                Visit(context.actPars());
            }
            return null;
        }

        public override object VisitNumberFactorAST(MiniCSharpParser.NumberFactorASTContext context)
        {
            ///PENSAR
            return null;
        }

        public override object VisitCharFactorAST(MiniCSharpParser.CharFactorASTContext context)
        {
            ///PENSAR
            return null;
        }

        public override object VisitStringFactorAST(MiniCSharpParser.StringFactorASTContext context)
        {
            ///PENSAR
            return null;
        }

        public override object VisitDoubleFactorAST(MiniCSharpParser.DoubleFactorASTContext context)
        {
            ///PENSAR
            return null;
        }

        public override object VisitBoolFactorAST(MiniCSharpParser.BoolFactorASTContext context)
        {
            ///PENSAR
            return null;
        }

        public override object VisitNewFactorAST(MiniCSharpParser.NewFactorASTContext context)
        {
            ///PENSAR
            
            if (context.LBRACK() != null)
            {
                Visit(context.expr());
            }
            
            return null;
        }

        public override object VisitExprFactorAST(MiniCSharpParser.ExprFactorASTContext context)
        {
            Visit(context.expr());
            return null;
        }

        public override object VisitDesignatorAST(MiniCSharpParser.DesignatorASTContext context)
        {
            if (context.DOT() != null)
            {
                ///
            }
            
            for (int i = 0; context.expr().Count() > i; i++)
            {
                Visit(context.expr(i));
            }
            return null;
        }

        public override object VisitRelop(MiniCSharpParser.RelopContext context)
        {
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            if (context.EQUALS() != null)
            {
                currentIL.Emit(OpCodes.Ceq);
            }
            else if (context.NOT_EQUALS() != null)
            {
                currentIL.Emit(OpCodes.Cgt_Un);
            }
            else if (context.GREATER_THAN() != null)
            {
                currentIL.Emit(OpCodes.Cgt);
            }
            else if (context.GREATER_EQUALS() != null)
            {
                currentIL.Emit(OpCodes.Clt);
                ///
                currentIL.Emit(OpCodes.Ceq);
            }
            else if (context.LESS_THAN() != null)
            {
                currentIL.Emit(OpCodes.Clt);
            }
            else if (context.LESS_EQUALS() != null)
            {
                currentIL.Emit(OpCodes.Cgt);
                ///
                currentIL.Emit(OpCodes.Ceq);
            }
            return null;
        }

        public override object VisitAddop(MiniCSharpParser.AddopContext context)
        {
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            if (context.PLUS() != null)
            {
                currentIL.Emit(OpCodes.Add);
            }
            else if (context.MINUS() != null)
            {
                currentIL.Emit(OpCodes.Sub);
            }

            return null;
        }

        public override object VisitMulop(MiniCSharpParser.MulopContext context)
        {
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            if (context.MULT() != null)
            {
                currentIL.Emit(OpCodes.Mul);
            }
            else if (context.DIV() != null)
            {
                currentIL.Emit(OpCodes.Div);
            }
            else if (context.MOD() != null)
            {
                currentIL.Emit(OpCodes.Rem);
            }
            return null;
        }
    }
}