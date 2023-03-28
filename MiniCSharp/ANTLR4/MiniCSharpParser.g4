parser grammar MiniCSharpParser;

options {
    tokenVocab=Scanner;
}

// Regla inicial del programa
program: using* CLASS IDENTIFIER LBRACE (varDecl | classDecl | methodDecl)* RBRACE EOF;

// Regla para la directiva "using"
using: USING IDENTIFIER SEMICOLON;

// Regla para declaración de variable
varDecl: type IDENTIFIER (COMMA IDENTIFIER)* SEMICOLON;

// Regla para declaración de clase
classDecl: CLASS IDENTIFIER LBRACE varDecl* RBRACE;

// Regla para declaración de método
methodDecl: (type | VOID) IDENTIFIER LPAREN (formPars)? RPAREN block;

// Regla para parámetros de método
formPars: type IDENTIFIER (COMMA type IDENTIFIER)*;

// Regla para tipos de dato
type: IDENTIFIER (LBRACK RBRACK)?;

// Regla para una instrucción
statement: designator (ASSIGN expr | LPAREN (actPars)? RPAREN | INCREMENT | DECREMENT) SEMICOLON
         | IF LPAREN condition RPAREN statement (ELSE statement)?
         | FOR LPAREN expr SEMICOLON (condition)? SEMICOLON (statement)? RPAREN statement
         | WHILE LPAREN condition RPAREN statement
         | BREAK SEMICOLON
         | RETURN (expr)? SEMICOLON
         | READ LPAREN designator RPAREN SEMICOLON
         | WRITE LPAREN expr (COMMA NUMBER)? RPAREN SEMICOLON
         | block
         | SEMICOLON
         | LIST LESS_THAN IDENTIFIER GREATER_THAN IDENTIFIER (ASSIGN listFactor)?; ////////////////////MODIFICACION EXTRA

// Regla para un bloque de código
block: LBRACE (varDecl | statement)* RBRACE;

// Regla para argumentos de método
actPars: expr (COMMA expr)*;

// Regla para una condición
condition: condTerm (LOGICAL_OR condTerm)*;

// Regla para un término de condición
condTerm: condFact (LOGICAL_AND condFact)*;

// Regla para un factor de condición
condFact: expr relop expr;

// Regla para una conversión de tipo
cast: LPAREN type RPAREN;

// Regla para una expresión
expr: (MINUS)? (cast)? term (addop term)*;

// Regla para un término de expresión
term: factor (mulop factor)*;

// Regla para una lista
listFactor: NEW LIST LESS_THAN IDENTIFIER GREATER_THAN LPAREN RPAREN; ////////////////////MODIFICACION EXTRA

// Regla para un factor de expresión
factor: designator (LPAREN (actPars)? RPAREN)?
      | NUMBER
      | CHAR_CONSTANT
      | STRING_CONSTANT
      | DOUBLE_CONST ////////////////////MODIFICACION EXTRA
      | (TRUE | FALSE)
      | NEW IDENTIFIER (LBRACK RBRACK| LBRACK expr RBRACK)? ////////////////////MODIFICACION EXTRA
      | LPAREN expr RPAREN;

// Regla para un designador
designator: IDENTIFIER (DOT IDENTIFIER | LBRACK expr RBRACK)*;

// Regla para operadores relacionales
relop: EQUALS | NOT_EQUALS | GREATER_THAN | GREATER_EQUALS | LESS_THAN | LESS_EQUALS;

// Regla para operadores adicionales
addop : PLUS | MINUS;

// Regla para operadores aritmeticos
mulop : MULT | DIV | MOD;
