lexer grammar Scanner;

@lexer::members {
			    public override void NotifyListeners(LexerNoViableAltException e){
			    this.ErrorListenerDispatch.SyntaxError(this.ErrorOutput, (IRecognizer) this, 0, TokenStartLine, this.TokenStartColumn, "reconocimiento de token : '" + this.GetErrorDisplay(this.EmitEOF().InputStream.GetText(Interval.Of(this.TokenStartCharIndex, this.InputStream.Index)))  + "'", (RecognitionException) e);
			   }
}
// El @lexer::members sobreescribe el metodo NotifyListeners de la clase Lexer de ANTLR4, se vuelve Override 
//y se traduce la descripcion del error del Scanner.

// Palabras reservadas
CLASS: 'class';
VOID: 'void';
IF: 'if';
ELSE: 'else';
FOR: 'for';
WHILE: 'while';
BREAK: 'break';
RETURN: 'return';
READ: 'read';
WRITE: 'write';
NEW: 'new';
TRUE: 'true';
FALSE: 'false';
USING: 'using';

// Tipos
//INT: 'int';
//INTQ: 'int?';
//DOUBLE: 'double';
//DOUBLEQ: 'double?';
//CHAR: 'char';
//CHARQ: 'char?';
//STRING: 'string';
//STRINGQ: 'string?';
//BOOL: 'bool';
//BOOLQ: 'bool?';
//LIST : 'list';


// Operadores
PLUS: '+';
MINUS: '-';
MULT: '*';
DIV: '/';
MOD: '%';
ASSIGN: '=';
LPAREN: '(';
RPAREN: ')';
LBRACE: '{';
RBRACE: '}';
LBRACK: '[';
RBRACK: ']';
SEMICOLON: ';';
COMMA: ',';
INCREMENT: '++';
DECREMENT: '--';
LOGICAL_OR: '||';
LOGICAL_AND: '&&';
EQUALS: '==';
NOT_EQUALS: '!=';
GREATER_THAN: '>';
GREATER_EQUALS: '>=';
LESS_THAN: '<';
LESS_EQUALS: '<=';
DOT:'.';

// Identificador, número, constante de caracteres y de cadena
IDENTIFIER:  LETTER (LETTER | DIGIT)*;
fragment LETTER : [a-zA-Z_];
fragment DIGIT : [0-9]+;

NUMBER : [0-9]+;

DOUBLE_CONST: NUMBER ('.' NUMBER)? EXPONENT? [fF]?;
fragment EXPONENT: [+-]? NUMBER;

CHAR_CONSTANT: '\'' (ESCAPE_SEQUENCE | ~['\\]) '\'';

STRING_CONSTANT : '"' ( ESCAPE_SEQUENCE | ~["\\] )* '"';

fragment ESCAPE_SEQUENCE : '\\' ( 'n' | 'r' | 't' | '\\' | '\'');

// Comentarios
COMMENT: '//' ~[\r\n]* -> skip;
BLOCK_COMMENT: '/*' .*? '*/' -> skip;

// Reglas para ignorar espacios en blanco y caracteres de nueva línea
WHITESPACE: [ \t\r\n]+ -> skip;