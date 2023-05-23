﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Windows.Forms;
using generated;
using Label = System.Reflection.Emit.Label;

namespace MiniCSharp.ANTLR4
{
    public class CodeGen : MiniCSharpParserBaseVisitor<object>
    {
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
        
        public CodeGen(string txt)
        {
            metodosGlobales = new List<MethodBuilder>();
            
            myAsmName.Name = txt;
            myAsmBldr = currentDom.DefineDynamicAssembly(myAsmName, AssemblyBuilderAccess.RunAndSave);
            myModuleBldr = myAsmBldr.DefineDynamicModule(asmFileName);
            myTypeBldr = myModuleBldr.DefineType(txt+"Class");
            
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
            }if (tipo.Equals("void"))
            {
                return typeof(void);
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
            
            pointType = myTypeBldr.CreateType(); //crea la clase para ser luego instanciada
            myAsmBldr.SetEntryPoint(pointMainBldr);
            myAsmBldr.Save(asmFileName);
            return pointType;
        }

        public override object VisitUsingAST(MiniCSharpParser.UsingASTContext context)
        {
            return null;
        }

        public override object VisitVarDeclAST(MiniCSharpParser.VarDeclASTContext context)
        {
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();

            //Visit(context.type());
            
            currentIL.DeclareLocal((Type)Visit(context.type()));

            for (int i = 1; context.IDENTIFIER().Count() > i; i++)
            {
                currentIL.DeclareLocal((Type)Visit(context.type()));
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
            Type tipoMetodo = null;
            if (context.VOID() != null)
            {
                tipoMetodo = verificarTipoRetorno("void");
            }else if (context.VOID() == null)
            {
                tipoMetodo = verificarTipoRetorno((string) Visit(context.type()));
            }
            

            //se declara el método actual utilizando los datos optenidos en las visitas 
            currentMethodBldr = myTypeBldr.DefineMethod(context.IDENTIFIER().GetText(),
                MethodAttributes.Public |
                MethodAttributes.Static,
                tipoMetodo,
                null);//los parámetros son null porque se tiene que visitar despues de declarar el método... se cambiará luego

            //se visitan los parámetros para definir el arreglo de tipos de cada uno de los parámetros formales... si es que hay (not null)
            Type[] parameters = null;
            if (context.formPars() != null)
            {
                parameters = (Type[]) Visit(context.formPars());
                //Visit(context.formPars());
            }
            
            //después de visitar los parámetros, se cambia el signatura que requiere la definición del método
            currentMethodBldr.SetParameters(parameters);

            Visit(context.block());
            
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            currentIL.Emit(OpCodes.Ret); 
            
            //Se agrega el método recién creado a la lista de mpetodos globales para no perder su referencia cuando se creen más métodos
            metodosGlobales.Add(currentMethodBldr);
            if (context.IDENTIFIER().GetText().Equals("Main")) {
                //el puntero al metodo principal se setea cuando es el Main quien se declara
                pointMainBldr = currentMethodBldr;
            }
            
            return null;
        }

        public override object VisitFormParsAST(MiniCSharpParser.FormParsASTContext context)
        {
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            isArgument = true;
            
            //se construye el arreglo de tipos necesario para enviarle a la definición de métodos
            Type[] result = new Type[context.type().Length];

            for (int i = 0; context.type().Count() > i; i++)
            {
                result[i] = (Type)Visit(context.type(i));
                currentIL.Emit(OpCodes.Ldarg, i);
                currentIL.DeclareLocal(result[i]);
                currentIL.Emit(OpCodes.Stloc, i); //TODO se debería llevar una lista de argumentos para saber cual es cual cuando se deban llamar
                //currentIL.Emit(OpCodes.Ldloc, 0);//solo para la prueba, el 0 es el que se va a llamar
            }
            return null;
        }

        public override object VisitTypeAST(MiniCSharpParser.TypeASTContext context)
        {
            //PENSAAARf

            if (context.QMARK(0) == null && context.LESS_THAN() == null && context.LBRACK() == null)
            {
                return context.IDENTIFIER(0).GetText();
                //return verificarTipoRetorno(context.IDENTIFIER(0).GetText());
            }
            if (context.QMARK(0) != null && context.LESS_THAN() == null && context.LBRACK() == null)
            {
                return context.IDENTIFIER(0).GetText()+"?";
                //return verificarTipoRetorno(context.IDENTIFIER(0).GetText()+"?");
            }
            if (context.QMARK(0) == null && context.LESS_THAN() != null && context.LBRACK() == null)
            {
                return "List"+"<"+context.IDENTIFIER(0).GetText()+">";
                //return verificarTipoRetorno("List"+"<"+context.IDENTIFIER(0).GetText()+">");
            }
            if (context.QMARK(0) != null && context.LESS_THAN() != null && context.LBRACK() == null)
            {
                return "List"+"<"+context.IDENTIFIER(0).GetText()+"?"+">";
                //return verificarTipoRetorno("List"+"<"+context.IDENTIFIER(0).GetText()+"?"+">");
            }
            if (context.QMARK(0) == null && context.LESS_THAN() == null && context.LBRACK() != null)
            {
                return context.IDENTIFIER(0).GetText()+"[]";
                //return verificarTipoRetorno(context.IDENTIFIER(0).GetText()+"[]");
            }
            if (context.QMARK(0) != null && context.LESS_THAN() == null && context.LBRACK() != null)
            {
                return context.IDENTIFIER(0).GetText()+"?"+"[]";
                //return verificarTipoRetorno(context.IDENTIFIER(0).GetText()+"?"+"[]");
            }
            
            return null;
        }

        public override object VisitDesignatorStatementAST(MiniCSharpParser.DesignatorStatementASTContext context)
        {
            Visit(context.designator());

            if (context.ASSIGN() != null)
            {
                Visit(context.expr());
                
                //se asigna el valor a la variable
                //TODO hay que discriminar si es local o global porque la instrucción a generar es distinta según el caso
                ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                currentIL.Emit(OpCodes.Stloc,0); //TODO: e debe utilizar el índice que corresponde a la variable y no 0 siempre
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
            
            //definir etiqueta
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            Label labelFalse = currentIL.DefineLabel();
            
            //saltar if false
            currentIL.Emit(OpCodes.Brfalse,labelFalse);

            Visit(context.statement(0));
            
            //marcar etiqueta
            currentIL.MarkLabel(labelFalse);

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
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            
            // se debe visitar a los parámetros reales para generar el código que corresponda
            Visit(context.expr());
            
            //TODO: debe conocerse el tipo de la expresión para saber a cual write llamar
            currentIL.EmitCall(OpCodes.Call, writeMI/*OJO... EL QUE CORRESPONDA SEGUN TIPO*/, null);
            //Visit(context.expr());
            
            //if (context.COMMA() != null)
            //{
               ///PENSAR
            //}
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
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            try
            {
                currentIL.Emit(OpCodes.Ldc_I4 , Int32.Parse(context.NUMBER().GetText()));
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse the number expression!!!");
            }
            return null;
        }

        public override object VisitCharFactorAST(MiniCSharpParser.CharFactorASTContext context)
        {
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            try
            {
                currentIL.Emit(OpCodes.Ldc_I4 , char.Parse(context.CHAR_CONSTANT().GetText()));
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse the number expression!!!");
            }
            return null;
        }

        public override object VisitStringFactorAST(MiniCSharpParser.StringFactorASTContext context)
        {
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            try
            {
                currentIL.Emit(OpCodes.Ldstr, (context.STRING_CONSTANT().GetText()));
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse the number expression!!!");
            }
            return null;
        }

        public override object VisitDoubleFactorAST(MiniCSharpParser.DoubleFactorASTContext context)
        {
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            try
            {
                currentIL.Emit(OpCodes.Ldc_R8 , float.Parse(context.DOUBLE_CONST().GetText()));
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse the number expression!!!");
            }
            return null;
        }

        public override object VisitBoolFactorAST(MiniCSharpParser.BoolFactorASTContext context)
        {
            if (context.TRUE() != null)
            {
                ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                try
                {
                    currentIL.Emit(OpCodes.Ldc_I4, (context.TRUE().GetText()));
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Unable to parse the number expression!!!");
                }
            }
            else if (context.FALSE() != null)
            {
                ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                try
                {
                    currentIL.Emit(OpCodes.Ldc_I4, (context.FALSE().GetText()));
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Unable to parse the number expression!!!");
                }
            }
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