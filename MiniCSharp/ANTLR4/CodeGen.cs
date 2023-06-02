using System;
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
        private List<FieldBuilder> variablesClase; 

        private bool isArgument = false;
        
        private int nivelActual = -1;
        
        public int decSumBlock;

        public int totalSumBlock;

        private FieldBuilder currentFieldBldr, currentFieldBldrClass;
        
        private Dictionary<string, LocalBuilder> variablesLocales;
        
        List<List<Type>> tiposMetodos = new List<List<Type>>();

        private Type tipoDesignator;
        
        private Type tipoMethod;
        
        private int indiceMethod;
        
        private bool entradaMethod = false;
        
        private bool entradaMethodNull = false;
        
        private Label loopExitLabel;

        private List<Type> clasesGlobales;
        
        private ModuleBuilder currentModuleBldr;
        
        private bool inClass=false;

        private bool inClassVar = false;

        public CodeGen(string txt)
        {
            variablesLocales = new Dictionary<string, LocalBuilder>();
            metodosGlobales = new List<MethodBuilder>();
            variablesGlobales = new List<FieldBuilder>();
            clasesGlobales = new List<Type>();
            variablesClase= new List<FieldBuilder>();
            
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
        
        private int buscarMetodoIndice(String name)
        {
            int i = 0;
            foreach (var method in metodosGlobales)
            {
                if (method.Name.Equals(name))
                    return i;
                i++;
            }

            return -1;
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
        
        private FieldBuilder buscarVariableClase(String name)
        {
            foreach (var classN in variablesClase)
            {
                if (classN.Name.Equals(name))
                    return classN;
            }

            return null;
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
                return typeof(object);/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

            //Visit(context.type());
            
            //currentIL.DeclareLocal(verificarTipoRetorno((string) Visit(context.type())));
            //currentIL.DeclareLocal((Type)Visit(context.type()));
            if (inClass.Equals(true))
            {
                for (int i = 0; i < context.IDENTIFIER().Length; i++)
                {
                    string variableName = context.IDENTIFIER(i).GetText();
                    Type variableType = verificarTipoRetorno((string)Visit(context.type()));

                    FieldBuilder fieldBuilder = classBuilder.DefineField(variableName, variableType, FieldAttributes.Public);

                    // Puedes realizar otras acciones con el fieldBuilder según sea necesario

                    variablesClase.Add(fieldBuilder);
                }
            }
            
            else if (nivelActual == 0)
            {
                //ILGenerator currentIL = pointMainBldr.GetILGenerator();
                for (int i = 0; context.IDENTIFIER().Count() > i; i++)
                {
                    string variableName = context.IDENTIFIER(i).GetText();
                    Type variableType = verificarTipoRetorno((string)Visit(context.type()));

                   // FieldBuilder fieldBuilder = myTypeBldr.DefineField(variableName, variableType, FieldAttributes.Public | FieldAttributes.Static);

                   currentFieldBldr = myTypeBldr.DefineField(variableName, variableType, FieldAttributes.Public | FieldAttributes.Static);
                   variablesGlobales.Add(currentFieldBldr);
                    //fieldBuilder.SetValue(null, null); // Asignar un valor nulo a la variable global
                }

            }

            else if (nivelActual != 0)
            {
                ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                for (int i = 0; context.IDENTIFIER().Count() > i; i++)
                {
                    //currentIL.DeclareLocal(verificarTipoRetorno((string) Visit(context.type())));
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
            //MessageBox.Show(buscarClase(context.IDENTIFIER().GetText()).GetField("pos").FieldType.ToString());
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

            nivelActual--;
            tipoMethod = tipoMetodo;
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
            //POSIBLE ERROR POR TEXTO
            
            //currentIL.DeclareLocal(verificarTipoRetorno((string) Visit(context.type())));
            //declararVariableLocal(context.IDENTIFIER(i).GetText(),currentIL.DeclareLocal(verificarTipoRetorno((string) Visit(context.type()))));
            List<Type> parametros = new List<Type>();
            for (int i = 0; context.type().Count() > i; i++)
            {
                //result[i] = verificarTipoRetorno((string)Visit(context.type(i)));
                //currentIL.Emit(OpCodes.Ldarg, i);
                //currentIL.DeclareLocal(result[i]);
                //currentIL.Emit(OpCodes.Stloc, i); //TODO se debería llevar una lista de argumentos para saber cual es cual cuando se deban llamar
                //currentIL.Emit(OpCodes.Ldloc, 0);//solo para la prueba, el 0 es el que se va a llamar
                
                result[i] = verificarTipoRetorno((string)Visit(context.type(i)));
                parametros.Add(result[i]);
                //currentIL.DeclareLocal(result[i]);
                declararVariableLocal(context.IDENTIFIER(i).GetText(),currentIL.DeclareLocal(result[i]));
                currentIL.Emit(OpCodes.Ldarg, i);
                currentIL.Emit(OpCodes.Stloc, i);
                //currentIL.Emit(OpCodes.Ldloc, 0);

                //currentIL.DeclareLocal(verificarTipoRetorno((string) Visit(context.type())));
                //declararVariableLocal(context.IDENTIFIER(i).GetText(),currentIL.DeclareLocal(verificarTipoRetorno((string) Visit(context.type()))));
            }
            
            tiposMetodos.Add(parametros);
            
            isArgument = false;
            return result;
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
           // Visit(context.designator());

            if (context.ASSIGN() != null)
            {
                if (buscarVariableGlobal(context.designator().GetText()) != null)
                {
                    if (buscarVariableGlobal(context.designator().GetText()).FieldType == typeof(double))
                    {
                        tipoDesignator = typeof(double);
                    } else if (buscarVariableGlobal(context.designator().GetText()).FieldType == typeof(double?))
                    {
                        tipoDesignator = typeof(double?);
                    }
                }
                else if (variablesLocales[context.designator().GetText()] != null)
                {
                    if (variablesLocales[context.designator().GetText()].LocalType == typeof(double))
                    {
                        tipoDesignator = typeof(double);
                    } else if (variablesLocales[context.designator().GetText()].LocalType == typeof(double?))
                    {
                        tipoDesignator = typeof(double?);
                    }
                }
                Visit(context.expr());
                tipoDesignator = null;
                entradaMethod = false;
                entradaMethodNull = false;

                ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                if (buscarVariableGlobal(context.designator().GetText()) != null)
                {
                    currentIL.Emit(OpCodes.Stsfld,buscarVariableGlobal(context.designator().GetText())); //TODO: e debe utilizar el índice que corresponde a la variable y no 0 siempre
                }
                else if(variablesLocales[context.designator().GetText()]!= null)
                {
                    currentIL.Emit(OpCodes.Stloc,variablesLocales[context.designator().GetText()]);
                    //MessageBox.Show(variablesLocales[context.designator().GetText()].LocalType.Name);
                }
            }else if (context.LPAREN() != null)
            {
                ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                if(!context.designator().GetText().Equals("Main"))
                {
                    indiceMethod = buscarMetodoIndice(context.designator().GetText());
                    //MessageBox.Show(buscarMetodo("resul").ToString());
                    // se debe visitar a los parámetros reales para generar el código que corresponda
                    if (context.actPars() != null)
                    {
                        Visit(context.actPars());
                    }
                    //currentIL.Emit(OpCodes.Ldarg, 2);
                    //currentIL.Emit(OpCodes.Stloc, 2);
                    //se busca el método en la lista de métodos globales para referenciarlo
                    currentIL.Emit(OpCodes.Call, buscarMetodo(context.designator().GetText()));
                    currentIL.Emit(OpCodes.Pop);
                }
            }else if (context.INCREMENT() != null)
            {
                ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                inClassVar = true;
                Visit(context.designator());
                currentIL.Emit(OpCodes.Ldc_R8 , 1.0);
                currentIL.Emit(OpCodes.Add);
                if (buscarVariableGlobal(context.designator().GetText()) != null)
                {
                    currentIL.Emit(OpCodes.Stsfld,buscarVariableGlobal(context.designator().GetText())); //TODO: e debe utilizar el índice que corresponde a la variable y no 0 siempre
                }
                else
                {
                    //currentIL.Emit(OpCodes.Stloc,variablesLocales[context.designator().GetText()]);
                    currentIL.Emit(OpCodes.Stfld,buscarClase("miclase").GetField("neg"));
                }

                inClassVar = false;

            }else if (context.DECREMENT() != null)
            {
                ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                inClassVar = true;
                Visit(context.designator());
                currentIL.Emit(OpCodes.Ldc_R8 , 1.0);
                currentIL.Emit(OpCodes.Sub);
                if (buscarVariableGlobal(context.designator().GetText()) != null)
                {
                    currentIL.Emit(OpCodes.Stsfld,buscarVariableGlobal(context.designator().GetText())); //TODO: e debe utilizar el índice que corresponde a la variable y no 0 siempre
                }
                else
                {
                    currentIL.Emit(OpCodes.Stloc,variablesLocales[context.designator().GetText()]);
                }
                inClassVar = false;
            }

            return null;
        }

        public override object VisitIfStatementAST(MiniCSharpParser.IfStatementASTContext context)
        {
            Visit(context.condition());
            
            //definir etiqueta
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            Label labelFalse = currentIL.DefineLabel();
            Label labelEnd = currentIL.DefineLabel();
            //saltar if false
            currentIL.Emit(OpCodes.Brfalse,labelFalse);

            Visit(context.statement(0));
            
            //marcar etiqueta
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
            // Generar la etiqueta de inicio del bucle
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            // Generar las etiquetas del bucle
            currentIL = currentMethodBldr.GetILGenerator();
            Label loopStartLabel = currentIL.DefineLabel();
            loopExitLabel = currentIL.DefineLabel();

            // Marcar el inicio del bucle
            currentIL.MarkLabel(loopStartLabel);

            // Visitar la expresión del bucle
            Visit(context.condition());
            Visit(context.statement(0));

            // Salto condicional al punto de salida del bucle si la expresión es falsa
            currentIL.Emit(OpCodes.Brfalse, loopExitLabel);

            // Generar una etiqueta para el punto de salida del bucle
            Label breakLabel = loopExitLabel; // Establecer la etiqueta del break

            // Visitar el cuerpo del bucle
            Visit(context.statement(1));

            // Salto incondicional al inicio del bucle
            currentIL.Emit(OpCodes.Br, loopStartLabel);

            // Marcar el punto de salida del bucle
            currentIL.MarkLabel(loopExitLabel);

            return null;
        }

        public override object VisitWhileStatementAST(MiniCSharpParser.WhileStatementASTContext context)
        {
            // Generar la etiqueta de inicio del bucle
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            Label loopStartLabel = currentIL.DefineLabel();
            loopExitLabel = currentIL.DefineLabel();

            // Marcar el inicio del bucle
            currentIL.MarkLabel(loopStartLabel);

            // Visitar la expresión del bucle
            Visit(context.condition());

            // Salto condicional al punto de salida del bucle si la expresión es falsa
            currentIL.Emit(OpCodes.Brfalse, loopExitLabel);

            // Visitar el cuerpo del bucle
            Visit(context.statement());

            // Salto incondicional al inicio del bucle
            currentIL.Emit(OpCodes.Br, loopStartLabel);

            // Marcar el punto de salida del bucle
            currentIL.MarkLabel(loopExitLabel);

            return null;
        }

        public override object VisitBreakStatementAST(MiniCSharpParser.BreakStatementASTContext context)
        {
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();

            // Generar una instrucción de salto incondicional para salir del bucle
            currentIL.Emit(OpCodes.Br, loopExitLabel); // breakLabel es una Label previamente definida
            return null;
        }

        public override object VisitReturnStatementAST(MiniCSharpParser.ReturnStatementASTContext context)
        {
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            //IL_0001: ldstr        "a"
            //IL_0006: stloc.0      // V_0
            //IL_0007: br.s         IL_0009
            ///PENSAR
            if (context.expr() != null)
            {
                if (tipoMethod == typeof(double))
                {
                    tipoDesignator = typeof(double);
                }else if (tipoMethod == typeof(double?))
                {
                    tipoDesignator = typeof(double?);
                }
                Visit(context.expr());
                tipoDesignator = null;
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
            
            // se debe visitar a los parámetros reales para generar el código que corresponda
            Type tipo = (Type)Visit(context.expr());

            if (tipo == typeof(int))
            {
                //TODO: debe conocerse el tipo de la expresión para saber a cual write llamar
                currentIL.EmitCall(OpCodes.Call, writeInt/*OJO... EL QUE CORRESPONDA SEGUN TIPO*/, null); 
            }else if (tipo == typeof(double))
            {
                currentIL.EmitCall(OpCodes.Call, writeDouble/*OJO... EL QUE CORRESPONDA SEGUN TIPO*/, null); 
            }else if (tipo == typeof(char))
            {
                currentIL.EmitCall(OpCodes.Call, writeChar/*OJO... EL QUE CORRESPONDA SEGUN TIPO*/, null); 
            }else if (tipo == typeof(string))
            {
                currentIL.EmitCall(OpCodes.Call, writeString/*OJO... EL QUE CORRESPONDA SEGUN TIPO*/, null); 
            }else if (tipo == typeof(bool))
            {
                currentIL.EmitCall(OpCodes.Call, writeBool/*OJO... EL QUE CORRESPONDA SEGUN TIPO*/, null); 
            }

            entradaMethod = false;
            entradaMethodNull = false;
            
            //Visit(context.expr());
            
            //if (context.COMMA() != null)
            //{
               ///PENSAR
            //}
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
            //MessageBox.Show(tiposMetodos[0].Count().ToString());
            for (int i = 0; context.expr().Count() > i; i++)
            {
                if (tiposMetodos[indiceMethod][i]== typeof(double))
                {
                    tipoDesignator = typeof(double);
                }else if (tiposMetodos[indiceMethod][i]== typeof(double?))
                {
                    tipoDesignator = typeof(double?);
                }
                Visit(context.expr(i));
                tipoDesignator = null;
            }
            isArgument = false;
            return null;
        }

        public override object VisitConditionAST(MiniCSharpParser.ConditionASTContext context)
        {
            //ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            //List<Label> labels = new List<Label>();
            for (int i = 0; i < context.condTerm().Length; i++)
            {
                
                Visit(context.condTerm(i));
                //Label labelFalse = currentIL.DefineLabel();
                //Label labelEnd = currentIL.DefineLabel();
                //Label label = currentIL.DefineLabel();
                //currentIL.Emit(OpCodes.Br, labelEnd);
                //currentIL.MarkLabel(labelEnd);
                //labelIf.Add(label);
            }
            return null;
        }

        public override object VisitCondTermAST(MiniCSharpParser.CondTermASTContext context)
        {
            //Visit(context.condFact(0));
            
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
                //Visit(context.actPars());
                ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                if(!context.designator().GetText().Equals("Main"))
                {
                    indiceMethod = buscarMetodoIndice(context.designator().GetText());
                    if (context.actPars() != null)
                    {
                        Visit(context.actPars());
                    }
                    //se busca el método en la lista de métodos globales para referenciarlo
                    currentIL.Emit(OpCodes.Call, buscarMetodo(context.designator().GetText()));
                    //currentIL.Emit(OpCodes.Pop);
                    //currentIL.Emit(OpCodes.Stloc, 0); //TODO se debería llevar una lista de argumentos para saber cual es cual cuando se deban llamar
                    if (buscarMetodo(context.designator().GetText()).ReturnType== typeof(double) && entradaMethod.Equals(false))
                    {
                        //tipoDesignator = typeof(double);
                        entradaMethod=true;
                       
                    }
                    
                    if (buscarMetodo(context.designator().GetText()).ReturnType== typeof(double?) && entradaMethodNull.Equals(false))
                    {
                        //tipoDesignator = typeof(double);
                        entradaMethodNull=true;
                       
                    }
                    
                    tipo = buscarMetodo(context.designator().GetText()).ReturnType;
                    
                    if (entradaMethod.Equals(true))
                    {
                        tipoDesignator = typeof(double);
                        currentIL.Emit(OpCodes.Conv_R8);
                        //currentIL.Emit(OpCodes.Ldc_I4 , Int32.Parse("1"));
                        tipo = typeof(double);
                    }
                    
                    if (entradaMethodNull.Equals(true))
                    {
                        tipoDesignator = typeof(double?);
                        currentIL.Emit(OpCodes.Conv_R8);
                        //currentIL.Emit(OpCodes.Ldc_I4 , Int32.Parse("1"));
                        tipo = typeof(double?);
                    }
                    
                }
            }
            return tipo;
        }

        public override object VisitNumberFactorAST(MiniCSharpParser.NumberFactorASTContext context)
        {
            Type tipo = null;
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();

            if (tipoDesignator == typeof(double))
            {
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
            }
            else
            {
                try
                {
                    //currentIL.Emit(OpCodes.Ldc_I4 , Int32.Parse(context.NUMBER().GetText()));
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
                //tipo= typeof(int);
                tipo= typeof(double);
            }
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
            ILGenerator currentIL = currentMethodBldr.GetILGenerator();
            Type classType = buscarClase(context.IDENTIFIER().GetText());
            ConstructorInfo constructor = classType.GetConstructor(Type.EmptyTypes);
            //MessageBox.Show(constructor.ToString());
            // Generar el código IL para crear la instancia utilizando el constructor
            currentIL.Emit(OpCodes.Newobj, constructor);
            //currentIL.Emit(OpCodes.Newobj, buscarClase(context.IDENTIFIER().GetText()).GetType());
            
            
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
                    //MessageBox.Show(buscarVariableGlobal(context.IDENTIFIER(0).GetText()).FieldType.ToString());
                    ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                    if (isArgument)
                    {
                        currentIL.Emit(OpCodes.Ldsfld, buscarVariableGlobal(context.IDENTIFIER(0).GetText())); //no siempre será 0
                    }
                    else
                    {
                        currentIL.Emit(OpCodes.Ldsfld, buscarVariableGlobal(context.IDENTIFIER(0).GetText())); //no siempre será 0
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
                if (buscarVariableGlobal(context.IDENTIFIER(0).GetText()) != null)
                {
                    //MessageBox.Show(buscarVariableGlobal(context.IDENTIFIER(0).GetText()).FieldType.ToString());
                    ILGenerator currentIL = currentMethodBldr.GetILGenerator();
                    if (isArgument)
                    {
                        currentIL.Emit(OpCodes.Ldsfld, buscarVariableGlobal(context.IDENTIFIER(0).GetText())); //no siempre será 0
                    }
                    else
                    {
                        currentIL.Emit(OpCodes.Ldsfld, buscarVariableGlobal(context.IDENTIFIER(0).GetText())); //no siempre será 0
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

                        if (inClassVar.Equals(true))
                        {
                            currentIL.Emit(OpCodes.Dup);
                        }

                        //currentIL.Emit(OpCodes.Ldfld, buscarClase(context.IDENTIFIER(0).GetText()).GetField("pos").FieldType.ToString());
                        currentIL.Emit(OpCodes.Ldfld, buscarClase("miclase").GetField(context.IDENTIFIER(1).GetText())); //no siempre será 0
                    }
                    tipo = buscarClase("miclase").GetField(context.IDENTIFIER(1).GetText()).FieldType;
                }
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