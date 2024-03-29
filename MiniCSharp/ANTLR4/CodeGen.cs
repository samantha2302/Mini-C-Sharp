﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Windows.Forms;
using Antlr4.Runtime.Misc;
using generated;
using Label = System.Reflection.Emit.Label;

namespace MiniCSharp.ANTLR4
{
    public class Instancia
    {
        private string variable;
        private int nivel;
        private string clase;

        public Instancia(string variable, int nivel, string clase)
        {
            this.variable = variable;
            this.nivel = nivel;
            this.clase = clase;
        }

        public string Variable
        {
            get => variable;
            set => variable = value;
        }

        public int Nivel
        {
            get => nivel;
            set => nivel = value;
        }

        public string Clase
        {
            get => clase;
            set => clase = value;
        }
    }
    public class CodeGen : MiniCSharpParserBaseVisitor<object>
    {
        private Type pointType = null;
        private string asmFileName = "result.exe";
        private AssemblyName myAsmName = new AssemblyName();

        private AppDomain currentDom = Thread.GetDomain();
        private AssemblyBuilder myAsmBldr;

        private ModuleBuilder myModuleBldr;

        private TypeBuilder myTypeBldr, classBuilder;
        private ConstructorInfo objCtor=null;

        private MethodInfo writeInt, writeDouble, writeChar, writeString, writeBool;

        private MethodBuilder pointMainBldr, currentMethodBldr;

        private List<MethodBuilder> metodosGlobales; 
        
        private List<FieldBuilder> variablesGlobales;

        private bool isArgument = false;
        
        private int nivelActual = -1;
        
        public int decSumBlock;

        public int totalSumBlock;

        private FieldBuilder currentFieldBldr, currentFieldBldrClass;
        
        private Dictionary<string, LocalBuilder> variablesLocales;

        private Label loopExitLabel;

        private List<Type> clasesGlobales;
        
        private ModuleBuilder currentModuleBldr;
        
        private bool inClass=false;

        private bool inClassVar = false;
        
        private bool inNewFactor = false;
        
        private string className = "";
        
        private List<Instancia> listaInstancia;
        
        private bool inAssign = false;

        public CodeGen(string txt)
        {
            variablesLocales = new Dictionary<string, LocalBuilder>();
            metodosGlobales = new List<MethodBuilder>();
            variablesGlobales = new List<FieldBuilder>();
            clasesGlobales = new List<Type>();
            listaInstancia= new List<Instancia>();
            
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
            
            writeInt = typeof(Console).GetMethod(
                "WriteLine",
                new Type[] { typeof(int) });
            writeDouble = typeof(Console).GetMethod(
                "WriteLine",
                new Type[] { typeof(double) });
            writeChar = typeof(Console).GetMethod(
                "WriteLine",
                new Type[] { typeof(char) });
            writeString = typeof(Console).GetMethod(
                "WriteLine",
                new Type[] { typeof(string) });
            writeBool= typeof(Console).GetMethod(
                "WriteLine",
                new Type[] { typeof(bool) });
        }
        
        public void declararVariableLocal(string nombreVariable, LocalBuilder localBuilder)
        {
            variablesLocales[nombreVariable] = localBuilder;
        }
        
        private MethodBuilder buscarMetodo(String name)
        {
            foreach (var method in metodosGlobales)
            {
                if (method.Name.Equals(name))
                    return method;
            }

            return null;
        }

        private FieldBuilder buscarVariableGlobal(String name)
        {
            foreach (var global in variablesGlobales)
            {
                if (global.Name.Equals(name))
                    return global;
            }

            return null;
        }
        
        private Type buscarClase(String name)
        {
            foreach (var classN in clasesGlobales)
            {
                if (classN.Name.Equals(name))
                    return classN;
            }

            return null;
        }

        public int buscarIndiceListaInstancia (string variable, int nivel)
        {
            for (int i = 0; i < listaInstancia.Count(); i++) 
            {
                if (listaInstancia[i].Variable.Equals(variable) && listaInstancia[i].Nivel.Equals(nivel))
                {
                    return i;
                }
            }
            return -1;
        }

        private Type verificarTipoRetorno(string tipo)
        {
            if (tipo.Equals("int"))
            {
                return typeof(double);
            }
            if (tipo.Equals("int?"))
            {
                return typeof(double);
            }
            if (tipo.Equals("double"))
            {
                return typeof(double);
            }
            if (tipo.Equals("double?"))
            {
                return typeof(double);
            }
            if (tipo.Equals("char"))
            {
                return typeof(char);
            }
            if (tipo.Equals("char?"))
            {
                return typeof(char);
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
                return typeof(bool);
            }
            if (tipo.Equals("bool?"))
            {
                return typeof(bool);
            }
            if (tipo.Equals("inst"))
            {
                return typeof(object);
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
                return typeof(List<int>);
            }
            if (tipo.Equals("List<double>"))
            {
                return typeof(List<double>);
            }
            if (tipo.Equals("List<double?>"))
            {
                return typeof(List<double>);
            }
            if (tipo.Equals("List<char>"))
            {
                return typeof(List<char>);
            }
            if (tipo.Equals("List<char?>"))
            {
                return typeof(List<char>);
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
                return typeof(List<bool>);
            }
            
            

            if (tipo.Equals("int[]"))
            {
                return typeof(int[]);
            }
            if (tipo.Equals("int?[]"))
            {
                return typeof(int[]);
            }
            if (tipo.Equals("double[]"))
            {
                return typeof(double[]);
            }
            if (tipo.Equals("double?[]"))
            {
                return typeof(double[]);
            }
            if (tipo.Equals("char[]"))
            {
                return typeof(char[]);
            }
            if (tipo.Equals("char?[]"))
            {
                return typeof(char[]);
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
                return typeof(bool[]);
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

            nivelActual++;

            for (int i = 0; context.varDecl().Count() > i; i++)
            {
                Visit(context.varDecl(i));
            }
            
            for (int i = 0; context.classDecl().Count() > i; i++)
            {
                nivelActual++;
                Visit(context.classDecl(i));
            }
            
            for (int i = 0; context.methodDecl().Count() > i; i++)
            {
                nivelActual++;
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
            if (inClass.Equals(true))
            {
                for (int i = 0; i < context.IDENTIFIER().Length; i++)
                {
                    string variableName = context.IDENTIFIER(i).GetText();
                    Type variableType = verificarTipoRetorno((string)Visit(context.type()));

                    FieldBuilder fieldBuilder = classBuilder.DefineField(variableName, variableType, FieldAttributes.Public);
                }
            }
            
            else if (nivelActual == 0)
            {
                for (int i = 0; context.IDENTIFIER().Count() > i; i++)
                {
                    string variableName = context.IDENTIFIER(i).GetText();
                    Type variableType = verificarTipoRetorno((string)Visit(context.type()));

                    currentFieldBldr = myTypeBldr.DefineField(variableName, variableType, FieldAttributes.Public | FieldAttributes.Static);
                    variablesGlobales.Add(currentFieldBldr);
                }

            }

            else if (nivelActual != 0)
            {
                ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                for (int i = 0; context.IDENTIFIER().Count() > i; i++)
                {
                    declararVariableLocal(context.IDENTIFIER(i).GetText(),currentIL.DeclareLocal(verificarTipoRetorno((string) Visit(context.type()))));
                }
                
            }

            return null;
        }

        public override object VisitClassDeclAST(MiniCSharpParser.ClassDeclASTContext context)
        {
            inClass = true;
            classBuilder = myModuleBldr.DefineType(context.IDENTIFIER().GetText(), TypeAttributes.Public);
            for (int i = 0; context.varDecl().Count() > i; i++)
            {
                Visit(context.varDecl(i));
            }
            Type generatedType = classBuilder.CreateType();
            clasesGlobales.Add(generatedType);
            inClass = false;
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
            
            currentMethodBldr = myTypeBldr.DefineMethod(context.IDENTIFIER().GetText(),
                MethodAttributes.Public |
                MethodAttributes.Static,
                tipoMetodo,
                null);
            
            Type[] parameters = null;
            if (context.formPars() != null)
            {
                parameters = (Type[]) Visit(context.formPars());
            }
            
            currentMethodBldr.SetParameters(parameters);

            nivelActual--;
            Visit(context.block());

            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            currentIL.Emit(OpCodes.Ret);
            
            metodosGlobales.Add(currentMethodBldr);
            
            if (context.IDENTIFIER().GetText().Equals("Main")) {
                pointMainBldr = currentMethodBldr;
            }
            
            return null;
        }

        public override object VisitFormParsAST(MiniCSharpParser.FormParsASTContext context)
        {
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            isArgument = true;
            
            Type[] result = new Type[context.type().Length];
            
            List<Type> parametros = new List<Type>();
            for (int i = 0; context.type().Count() > i; i++)
            {
                result[i] = verificarTipoRetorno((string)Visit(context.type(i)));
                parametros.Add(result[i]);

                declararVariableLocal(context.IDENTIFIER(i).GetText(),currentIL.DeclareLocal(result[i]));
                currentIL.Emit(OpCodes.Ldarg, i);
                currentIL.Emit(OpCodes.Stloc, i);
            }

            isArgument = false;
            return result;
        }

        public override object VisitTypeAST(MiniCSharpParser.TypeASTContext context)
        {
            if (context.QMARK(0) == null && context.LESS_THAN() == null && context.LBRACK() == null)
            {
                return context.IDENTIFIER(0).GetText();
            }
            if (context.QMARK(0) != null && context.LESS_THAN() == null && context.LBRACK() == null)
            {
                return context.IDENTIFIER(0).GetText()+"?";
            }
            if (context.QMARK(0) == null && context.LESS_THAN() != null && context.LBRACK() == null)
            {
                return "List"+"<"+context.IDENTIFIER(0).GetText()+">";
            }
            if (context.QMARK(0) != null && context.LESS_THAN() != null && context.LBRACK() == null)
            {
                return "List"+"<"+context.IDENTIFIER(0).GetText()+"?"+">";
            }
            if (context.QMARK(0) == null && context.LESS_THAN() == null && context.LBRACK() != null)
            {
                return context.IDENTIFIER(0).GetText()+"[]";
            }
            if (context.QMARK(0) != null && context.LESS_THAN() == null && context.LBRACK() != null)
            {
                return context.IDENTIFIER(0).GetText()+"?"+"[]";
            }
            
            return null;
        }

        public override object VisitDesignatorStatementAST(MiniCSharpParser.DesignatorStatementASTContext context)
        {
            if (context.ASSIGN() != null)
            {

                if (!variablesLocales.ContainsKey(context.designator().GetText()))
                {
                    inAssign = true;
                    Visit(context.designator());
                    inAssign = false;
                }
                
                Visit(context.expr());

                ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                if (buscarVariableGlobal(context.designator().GetText()) != null)
                {
                    currentIL.Emit(OpCodes.Stsfld,buscarVariableGlobal(context.designator().GetText()));
                }
                else if(variablesLocales.ContainsKey(context.designator().GetText()))
                {
                    currentIL.Emit(OpCodes.Stloc,variablesLocales[context.designator().GetText()]);
                    if (inNewFactor.Equals(true))
                    {
                        listaInstancia.Add(new Instancia(context.designator().GetText(), nivelActual, className));
                        inNewFactor = false;
                        className = "";
                    }
                }
                else
                {
                    string designator = context.designator().GetText();
                    int index = designator.IndexOf('.');
                    string salidaVariable = designator.Substring(0, index);
                    
                    string salidaVariableClase = designator.Substring(index+1);
                    
                    int indice = buscarIndiceListaInstancia(salidaVariable, nivelActual);
                    currentIL.Emit(OpCodes.Stfld,buscarClase(listaInstancia[indice].Clase).GetField(salidaVariableClase));
                }
            }else if (context.LPAREN() != null)
            {
                ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                if(!context.designator().GetText().Equals("Main"))
                {
                    if (context.actPars() != null)
                    {
                        Visit(context.actPars());
                    }
                    currentIL.Emit(OpCodes.Call, buscarMetodo(context.designator().GetText()));
                    currentIL.Emit(OpCodes.Pop);
                }
            }else if (context.INCREMENT() != null)
            {
                ILGenerator currentIL = currentMethodBldr.GetILGenerator();

                if (!variablesLocales.ContainsKey(context.designator().GetText()))
                {
                    inClassVar = true; 
                }

                Visit(context.designator());
                currentIL.Emit(OpCodes.Ldc_R8 , 1.0);
                currentIL.Emit(OpCodes.Add);
                if (buscarVariableGlobal(context.designator().GetText()) != null)
                {
                    currentIL.Emit(OpCodes.Stsfld,buscarVariableGlobal(context.designator().GetText()));
                }
                else if (variablesLocales.ContainsKey(context.designator().GetText()))
                {
                    currentIL.Emit(OpCodes.Stloc,variablesLocales[context.designator().GetText()]);
                }
                else
                {
                    string designator = context.designator().GetText();
                    int index = designator.IndexOf('.');
                    string salidaVariable = designator.Substring(0, index);
                    
                    string salidaVariableClase = designator.Substring(index+1);
                    
                    int indice = buscarIndiceListaInstancia(salidaVariable, nivelActual);
                    currentIL.Emit(OpCodes.Stfld,buscarClase(listaInstancia[indice].Clase).GetField(salidaVariableClase));
                }

                inClassVar = false;

            }else if (context.DECREMENT() != null)
            {
                ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                if (!variablesLocales.ContainsKey(context.designator().GetText()))
                {
                    inClassVar = true; 
                }
                Visit(context.designator());
                currentIL.Emit(OpCodes.Ldc_R8 , 1.0);
                currentIL.Emit(OpCodes.Sub);
                if (buscarVariableGlobal(context.designator().GetText()) != null)
                {
                    currentIL.Emit(OpCodes.Stsfld,buscarVariableGlobal(context.designator().GetText()));
                }
                else if (variablesLocales.ContainsKey(context.designator().GetText()))
                {
                    currentIL.Emit(OpCodes.Stloc,variablesLocales[context.designator().GetText()]);
                }
                else
                {
                    string designator = context.designator().GetText();
                    int index = designator.IndexOf('.');
                    string salidaVariable = designator.Substring(0, index);
                    
                    string salidaVariableClase = designator.Substring(index+1);
                    
                    int indice = buscarIndiceListaInstancia(salidaVariable, nivelActual);
                    
                    currentIL.Emit(OpCodes.Stfld,buscarClase(listaInstancia[indice].Clase).GetField(salidaVariableClase));
                }
                inClassVar = false;
            }

            return null;
        }

        public override object VisitIfStatementAST(MiniCSharpParser.IfStatementASTContext context)
        {
            Visit(context.condition());
            
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            Label labelFalse = currentIL.DefineLabel();
            Label labelEnd = currentIL.DefineLabel();

            currentIL.Emit(OpCodes.Brfalse,labelFalse);

            Visit(context.statement(0));
            
            currentIL.Emit(OpCodes.Br, labelEnd);
            currentIL.MarkLabel(labelFalse);
            if (context.ELSE() != null)
            {
                Visit(context.statement(1));
            }
            currentIL.MarkLabel(labelEnd);
            return null;
        }

        public override object VisitForStatementAST(MiniCSharpParser.ForStatementASTContext context)
        {
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();

            currentIL = currentMethodBldr.GetILGenerator();
            Label loopStartLabel = currentIL.DefineLabel();
            loopExitLabel = currentIL.DefineLabel();
            
            currentIL.MarkLabel(loopStartLabel);
            
            Visit(context.condition());
            Visit(context.statement(0));
            
            currentIL.Emit(OpCodes.Brfalse, loopExitLabel);
            
            Label breakLabel = loopExitLabel;
            
            Visit(context.statement(1));
            
            currentIL.Emit(OpCodes.Br, loopStartLabel);
            
            currentIL.MarkLabel(loopExitLabel);

            return null;
        }

        public override object VisitWhileStatementAST(MiniCSharpParser.WhileStatementASTContext context)
        {
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            Label loopStartLabel = currentIL.DefineLabel();
            loopExitLabel = currentIL.DefineLabel();
            
            currentIL.MarkLabel(loopStartLabel);
            
            Visit(context.condition());
            
            currentIL.Emit(OpCodes.Brfalse, loopExitLabel);
            
            Visit(context.statement());
            
            currentIL.Emit(OpCodes.Br, loopStartLabel);
            
            currentIL.MarkLabel(loopExitLabel);

            return null;
        }

        public override object VisitBreakStatementAST(MiniCSharpParser.BreakStatementASTContext context)
        {
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            
            currentIL.Emit(OpCodes.Br, loopExitLabel);
            return null;
        }

        public override object VisitReturnStatementAST(MiniCSharpParser.ReturnStatementASTContext context)
        {
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            if (context.expr() != null)
            {
                Visit(context.expr());
            }
            currentIL.Emit(OpCodes.Ret);
            return null;
        }

        public override object VisitReadStatementAST(MiniCSharpParser.ReadStatementASTContext context)
        {
            Visit(context.designator());
            return null;
        }

        public override object VisitWriteStatementAST(MiniCSharpParser.WriteStatementASTContext context)
        {
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();

            Type tipo = (Type)Visit(context.expr());

            if (tipo == typeof(int))
            {
                currentIL.EmitCall(OpCodes.Call, writeInt, null); 
            }else if (tipo == typeof(double))
            {
                currentIL.EmitCall(OpCodes.Call, writeDouble, null); 
            }else if (tipo == typeof(char))
            {
                currentIL.EmitCall(OpCodes.Call, writeChar, null); 
            }else if (tipo == typeof(string))
            {
                currentIL.EmitCall(OpCodes.Call, writeString, null); 
            }else if (tipo == typeof(bool))
            {
                currentIL.EmitCall(OpCodes.Call, writeBool, null); 
            }
            return null;
        }

        public override object VisitBlockStatementAST(MiniCSharpParser.BlockStatementASTContext context)
        {
            decSumBlock++;
            totalSumBlock++;
            Visit(context.block());
            decSumBlock--;

            if (decSumBlock == 0)
            {
                for (int i = 0; totalSumBlock > i; i++)
                {
                    nivelActual--;
                }
            }
            
            return null;
        }

        public override object VisitSemicolonStatementAST(MiniCSharpParser.SemicolonStatementASTContext context)
        {
            return null;
        }

        public override object VisitBlockAST(MiniCSharpParser.BlockASTContext context)
        {
            nivelActual++;
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
            for (int i = 0; context.expr().Count() > i; i++)
            {
                Visit(context.expr(i));
            }
            isArgument = false;
            return null;
        }

        public override object VisitConditionAST(MiniCSharpParser.ConditionASTContext context)
        {
            for (int i = 0; i < context.condTerm().Length; i++)
            {
                
                Visit(context.condTerm(i));
            }
            return null;
        }

        public override object VisitCondTermAST(MiniCSharpParser.CondTermASTContext context)
        {
            for (int i = 0; context.condFact().Count() > i; i++)
            {
                Visit(context.condFact(i));
            }
            return null;
        }

        public override object VisitCondFactAST(MiniCSharpParser.CondFactASTContext context)
        {
            Visit(context.expr(0));
            Visit(context.expr(1));
            Visit(context.relop());

            return null;
        }

        public override object VisitCastAST(MiniCSharpParser.CastASTContext context)
        {
            Visit(context.type());

            return null;
        }

        public override object VisitExprAST(MiniCSharpParser.ExprASTContext context)
        {
            Type tipo = null;
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            
            if (context.cast() != null)
            {

            }

            tipo= (Type)Visit(context.term(0));
            
            if (context.MINUS() != null)
            {
                currentIL.Emit(OpCodes.Neg);
            }

            for (int i = 1; context.term().Count() > i; i++)
            {
                tipo= (Type)Visit(context.term(i));
                Visit(context.addop(i-1));
            }
            isArgument = false;

            return tipo;
        }

        public override object VisitTermAST(MiniCSharpParser.TermASTContext context)
        {
            Type tipo = null;
            tipo= (Type)Visit(context.factor(0));

            for (int i = 1; context.factor().Count() > i; i++)
            {
                tipo= (Type)Visit(context.factor(i));
                Visit(context.mulop(i-1));
            }
            isArgument = false;
            return tipo;
        }

        public override object VisitDesignatorFactorAST(MiniCSharpParser.DesignatorFactorASTContext context)
        {
            Type tipo = null;
            if (context.LPAREN() == null)
            {
                tipo= (Type)Visit(context.designator());
            }

            if (context.LPAREN() != null)
            {
                ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                if(!context.designator().GetText().Equals("Main"))
                {
                    if (context.actPars() != null)
                    {
                        Visit(context.actPars());
                    }
                    currentIL.Emit(OpCodes.Call, buscarMetodo(context.designator().GetText()));

                    tipo = buscarMetodo(context.designator().GetText()).ReturnType;

                }
            }
            return tipo;
        }

        public override object VisitNumberFactorAST(MiniCSharpParser.NumberFactorASTContext context)
        {
            Type tipo = null;
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();

            try
            {
                string doubleText = context.NUMBER().GetText() +".0";
                doubleText= doubleText.Replace(".", ",");
                double OutVal;
                double.TryParse(doubleText, out OutVal);
                currentIL.Emit(OpCodes.Ldc_R8 , OutVal);
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse the number expression!!!");
            }
            tipo= typeof(double);
            return tipo;
        }

        public override object VisitCharFactorAST(MiniCSharpParser.CharFactorASTContext context)
        {
            Type tipo = null;
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            try
            {
                string charText = context.CHAR_CONSTANT().GetText();
                char elChar = charText[1];
                int casteoFinal = Convert.ToInt32(elChar);
                currentIL.Emit(OpCodes.Ldc_I4 , casteoFinal);
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse the number expression!!!");
            }
            tipo= typeof(char);
            return tipo;
        }

        public override object VisitStringFactorAST(MiniCSharpParser.StringFactorASTContext context)
        {
            Type tipo = null;
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            try
            {
                currentIL.Emit(OpCodes.Ldstr, (context.STRING_CONSTANT().GetText()));
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse the number expression!!!");
            }
            tipo= typeof(string);
            return tipo;
        }

        public override object VisitDoubleFactorAST(MiniCSharpParser.DoubleFactorASTContext context)
        {
            Type tipo = null;
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            try
            {

                string doubleText = context.DOUBLE_CONST().GetText();
                doubleText= doubleText.Replace(".", ",");
                double OutVal;
                double.TryParse(doubleText, out OutVal);
                currentIL.Emit(OpCodes.Ldc_R8 , OutVal);
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse the number expression!!!");
            }
            tipo= typeof(double);
            return tipo;
        }

        public override object VisitBoolFactorAST(MiniCSharpParser.BoolFactorASTContext context)
        {
            Type tipo = null;
            if (context.TRUE() != null)
            {
                ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                try
                {
                    currentIL.Emit(OpCodes.Ldc_I4, 1);
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
                    currentIL.Emit(OpCodes.Ldc_I4, 0);
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Unable to parse the number expression!!!");
                }
            }
            tipo= typeof(bool);
            return tipo;
        }

        public override object VisitNewFactorAST(MiniCSharpParser.NewFactorASTContext context)
        {
            inNewFactor = true;
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            Type classType = buscarClase(context.IDENTIFIER().GetText());
            ConstructorInfo constructor = classType.GetConstructor(Type.EmptyTypes);

            currentIL.Emit(OpCodes.Newobj, constructor);
            className = classType.ToString();
            if (context.LBRACK() != null)
            {
                Visit(context.expr());
            }
            
            return null;
        }

        public override object VisitExprFactorAST(MiniCSharpParser.ExprFactorASTContext context)
        {
            Type tipo = null;
            tipo= (Type)Visit(context.expr());
            return tipo;
        }

        public override object VisitDesignatorAST(MiniCSharpParser.DesignatorASTContext context)
        {
            Type tipo = null;
            if (context.DOT(0) == null && context.LBRACK(0) == null)
            {
                if (context.IDENTIFIER(0).GetText().Equals("null"))
                {
                    ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                    currentIL.Emit(OpCodes.Ldnull);
                }else if (buscarVariableGlobal(context.IDENTIFIER(0).GetText()) != null)
                {
                    ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                    if (isArgument)
                    {
                        currentIL.Emit(OpCodes.Ldsfld, buscarVariableGlobal(context.IDENTIFIER(0).GetText()));
                    }
                    else
                    {
                        currentIL.Emit(OpCodes.Ldsfld, buscarVariableGlobal(context.IDENTIFIER(0).GetText()));
                    }

                    tipo = buscarVariableGlobal(context.IDENTIFIER(0).GetText()).FieldType;
                }
                else
                {
                    ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                    if (isArgument)
                    {
                        currentIL.Emit(OpCodes.Ldarg, variablesLocales[context.IDENTIFIER(0).GetText()]);
                    }
                    else
                    {
                        currentIL.Emit(OpCodes.Ldloc, variablesLocales[context.IDENTIFIER(0).GetText()]);
                    }
                    tipo = variablesLocales[context.IDENTIFIER(0).GetText()].LocalType;
                }
            }
            if (context.DOT(0) != null)
            {
                ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                int indice = buscarIndiceListaInstancia(context.IDENTIFIER(0).GetText(), nivelActual);
                if (isArgument)
                {
                    currentIL.Emit(OpCodes.Ldarg, variablesLocales[context.IDENTIFIER(0).GetText()]);
                }
                else
                {
                    currentIL.Emit(OpCodes.Ldloc, variablesLocales[context.IDENTIFIER(0).GetText()]);

                    if (inClassVar.Equals(true))
                    {
                        currentIL.Emit(OpCodes.Dup);
                    }

                    if (inAssign.Equals(false))
                    {
                        currentIL.Emit(OpCodes.Ldfld, buscarClase(listaInstancia[indice].Clase).GetField(context.IDENTIFIER(1).GetText()));
                    }
                }
                tipo = buscarClase(listaInstancia[indice].Clase).GetField(context.IDENTIFIER(1).GetText()).FieldType;
            }
            
            for (int i = 0; context.expr().Count() > i; i++)
            {
                Visit(context.expr(i));
            }
            return tipo;
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
                currentIL.Emit(OpCodes.Ldc_I4 , Int32.Parse("0"));
                currentIL.Emit(OpCodes.Ceq);
            }
            else if (context.LESS_THAN() != null)
            {
                currentIL.Emit(OpCodes.Clt);
            }
            else if (context.LESS_EQUALS() != null)
            {
                currentIL.Emit(OpCodes.Cgt);
                currentIL.Emit(OpCodes.Ldc_I4 , Int32.Parse("0"));
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