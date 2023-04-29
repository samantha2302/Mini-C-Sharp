parser grammar MiniCSharpParser;

options {
    tokenVocab = Scanner;
}

// Regla inicial del programa
program: using* CLASS IDENTIFIER LBRACE (varDecl | classDecl | methodDecl)* RBRACE EOF             #programAST;

// Regla para la directiva "using"
using: USING IDENTIFIER SEMICOLON                                                                  #usingAST;

// Regla para declaración de variable
varDecl: type IDENTIFIER (COMMA IDENTIFIER)* SEMICOLON                                             #varDeclAST;

// Regla para declaración de clase
classDecl: CLASS IDENTIFIER LBRACE varDecl* RBRACE                                                 #classDeclAST;

// Regla para declaración de método
methodDecl: (type | VOID) IDENTIFIER LPAREN (formPars)? RPAREN block                               #methodDeclAST;

// Regla para parámetros de método (MODIFICADO)
formPars: type IDENTIFIER (COMMA type IDENTIFIER)*                                                 #formParsAST;

// Regla para tipos de dato
type: IDENTIFIER (LESS_THAN type GREATER_THAN)?  (LBRACK RBRACK)?                                  #typeAST;

// Regla para una instrucción
statement: designator (ASSIGN expr | LPAREN (actPars)? RPAREN | INCREMENT | DECREMENT) SEMICOLON   #designatorStatementAST
         | IF LPAREN condition RPAREN statement (ELSE statement)?                                  #ifStatementAST
         | FOR LPAREN expr SEMICOLON (condition)? SEMICOLON (statement)? RPAREN statement          #forStatementAST
         | WHILE LPAREN condition RPAREN statement                                                 #whileStatementAST
         | BREAK SEMICOLON                                                                         #breakStatementAST
         | RETURN (expr)? SEMICOLON                                                                #returnStatementAST
         | READ LPAREN designator RPAREN SEMICOLON                                                 #readStatementAST
         | WRITE LPAREN expr (COMMA NUMBER)? RPAREN SEMICOLON                                      #writeStatementAST
         | block                                                                                   #blockStatementAST
         | SEMICOLON                                                                               #semicolonStatementAST;

// Regla para un bloque de código
block: LBRACE (varDecl | statement)* RBRACE                                                        #blockAST; 

// Regla para argumentos de método
actPars: expr (COMMA expr)*                                                                        #actParsAST;

// Regla para una condición
condition: condTerm (LOGICAL_OR condTerm)*                                                         #conditionAST;

// Regla para un término de condición
condTerm: condFact (LOGICAL_AND condFact)*                                                         #condTermAST;

// Regla para un factor de condición
condFact: expr relop expr                                                                          #condFactAST;

// Regla para una conversión de tipo
cast: LPAREN type RPAREN                                                                           #castAST;

// Regla para una expresión
expr: (MINUS)? (cast)? term (addop term)*                                                          #exprAST;

// Regla para un término de expresión
term: factor (mulop factor)*                                                                       #termAST;

// Regla para un factor de expresión
factor: designator (LPAREN (actPars)? RPAREN)?                                                     #designatorFactorAST
      | NUMBER                                                                                     #numberFactorAST
      | CHAR_CONSTANT                                                                              #charFactorAST
      | STRING_CONSTANT                                                                            #stringFactorAST
      | DOUBLE_CONST                                                                               #doubleFactorAST //SE AGREGO PARA MANEJAR TIPOS DOUBLE.
      | (TRUE | FALSE)                                                                             #boolFactorAST
      | NEW IDENTIFIER                                                                             #newFactorAST
      | LPAREN expr RPAREN                                                                         #exprFactorAST;

// Regla para un designador
designator: IDENTIFIER (DOT IDENTIFIER | LBRACK expr RBRACK)*                                      #designatorAST;

// Regla para operadores relacionales
relop: EQUALS | NOT_EQUALS | GREATER_THAN | GREATER_EQUALS | LESS_THAN | LESS_EQUALS               #relopAST;

// Regla para operadores adicionales
addop : PLUS | MINUS                                                                               #addopAST;

// Regla para operadores aritmeticos
mulop : MULT | DIV | MOD                                                                           #mulopAST;
