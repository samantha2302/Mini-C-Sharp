//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.11.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:/Users/playa/Desktop/Proyecto 2/Mini-C-Sharp/MiniCSharp/ANTLR4\Scanner.g4 by ANTLR 4.11.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace generated {
using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.11.1")]
[System.CLSCompliant(false)]
public partial class Scanner : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		CLASS=1, VOID=2, IF=3, ELSE=4, FOR=5, WHILE=6, BREAK=7, RETURN=8, READ=9, 
		WRITE=10, NEW=11, TRUE=12, FALSE=13, USING=14, PLUS=15, MINUS=16, MULT=17, 
		DIV=18, MOD=19, ASSIGN=20, LPAREN=21, RPAREN=22, LBRACE=23, RBRACE=24, 
		LBRACK=25, RBRACK=26, SEMICOLON=27, COMMA=28, INCREMENT=29, DECREMENT=30, 
		LOGICAL_OR=31, LOGICAL_AND=32, EQUALS=33, NOT_EQUALS=34, GREATER_THAN=35, 
		GREATER_EQUALS=36, LESS_THAN=37, LESS_EQUALS=38, DOT=39, IDENTIFIER=40, 
		NUMBER=41, DOUBLE_CONST=42, CHAR_CONSTANT=43, STRING_CONSTANT=44, COMMENT=45, 
		BLOCK_COMMENT=46, WHITESPACE=47;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"CLASS", "VOID", "IF", "ELSE", "FOR", "WHILE", "BREAK", "RETURN", "READ", 
		"WRITE", "NEW", "TRUE", "FALSE", "USING", "PLUS", "MINUS", "MULT", "DIV", 
		"MOD", "ASSIGN", "LPAREN", "RPAREN", "LBRACE", "RBRACE", "LBRACK", "RBRACK", 
		"SEMICOLON", "COMMA", "INCREMENT", "DECREMENT", "LOGICAL_OR", "LOGICAL_AND", 
		"EQUALS", "NOT_EQUALS", "GREATER_THAN", "GREATER_EQUALS", "LESS_THAN", 
		"LESS_EQUALS", "DOT", "IDENTIFIER", "LETTER", "DIGIT", "NUMBER", "DOUBLE_CONST", 
		"EXPONENT", "CHAR_CONSTANT", "STRING_CONSTANT", "ESCAPE_SEQUENCE", "COMMENT", 
		"BLOCK_COMMENT", "WHITESPACE"
	};


				    public override void NotifyListeners(LexerNoViableAltException e){
				    this.ErrorListenerDispatch.SyntaxError(this.ErrorOutput, (IRecognizer) this, 0, TokenStartLine, this.TokenStartColumn, "reconocimiento de token : '" + this.GetErrorDisplay(this.EmitEOF().InputStream.GetText(Interval.Of(this.TokenStartCharIndex, this.InputStream.Index)))  + "'", (RecognitionException) e);
				   }


	public Scanner(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public Scanner(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'class'", "'void'", "'if'", "'else'", "'for'", "'while'", "'break'", 
		"'return'", "'read'", "'write'", "'new'", "'true'", "'false'", "'using'", 
		"'+'", "'-'", "'*'", "'/'", "'%'", "'='", "'('", "')'", "'{'", "'}'", 
		"'['", "']'", "';'", "','", "'++'", "'--'", "'||'", "'&&'", "'=='", "'!='", 
		"'>'", "'>='", "'<'", "'<='", "'.'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "CLASS", "VOID", "IF", "ELSE", "FOR", "WHILE", "BREAK", "RETURN", 
		"READ", "WRITE", "NEW", "TRUE", "FALSE", "USING", "PLUS", "MINUS", "MULT", 
		"DIV", "MOD", "ASSIGN", "LPAREN", "RPAREN", "LBRACE", "RBRACE", "LBRACK", 
		"RBRACK", "SEMICOLON", "COMMA", "INCREMENT", "DECREMENT", "LOGICAL_OR", 
		"LOGICAL_AND", "EQUALS", "NOT_EQUALS", "GREATER_THAN", "GREATER_EQUALS", 
		"LESS_THAN", "LESS_EQUALS", "DOT", "IDENTIFIER", "NUMBER", "DOUBLE_CONST", 
		"CHAR_CONSTANT", "STRING_CONSTANT", "COMMENT", "BLOCK_COMMENT", "WHITESPACE"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "Scanner.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static Scanner() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,47,323,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,2,29,7,29,2,30,7,30,2,31,7,31,2,32,7,32,2,33,7,33,2,34,7,34,2,35,
		7,35,2,36,7,36,2,37,7,37,2,38,7,38,2,39,7,39,2,40,7,40,2,41,7,41,2,42,
		7,42,2,43,7,43,2,44,7,44,2,45,7,45,2,46,7,46,2,47,7,47,2,48,7,48,2,49,
		7,49,2,50,7,50,1,0,1,0,1,0,1,0,1,0,1,0,1,1,1,1,1,1,1,1,1,1,1,2,1,2,1,2,
		1,3,1,3,1,3,1,3,1,3,1,4,1,4,1,4,1,4,1,5,1,5,1,5,1,5,1,5,1,5,1,6,1,6,1,
		6,1,6,1,6,1,6,1,7,1,7,1,7,1,7,1,7,1,7,1,7,1,8,1,8,1,8,1,8,1,8,1,9,1,9,
		1,9,1,9,1,9,1,9,1,10,1,10,1,10,1,10,1,11,1,11,1,11,1,11,1,11,1,12,1,12,
		1,12,1,12,1,12,1,12,1,13,1,13,1,13,1,13,1,13,1,13,1,14,1,14,1,15,1,15,
		1,16,1,16,1,17,1,17,1,18,1,18,1,19,1,19,1,20,1,20,1,21,1,21,1,22,1,22,
		1,23,1,23,1,24,1,24,1,25,1,25,1,26,1,26,1,27,1,27,1,28,1,28,1,28,1,29,
		1,29,1,29,1,30,1,30,1,30,1,31,1,31,1,31,1,32,1,32,1,32,1,33,1,33,1,33,
		1,34,1,34,1,35,1,35,1,35,1,36,1,36,1,37,1,37,1,37,1,38,1,38,1,39,1,39,
		1,39,5,39,239,8,39,10,39,12,39,242,9,39,1,40,1,40,1,41,4,41,247,8,41,11,
		41,12,41,248,1,42,4,42,252,8,42,11,42,12,42,253,1,43,1,43,1,43,3,43,259,
		8,43,1,43,3,43,262,8,43,1,43,3,43,265,8,43,1,44,3,44,268,8,44,1,44,1,44,
		1,45,1,45,1,45,3,45,275,8,45,1,45,1,45,1,46,1,46,1,46,5,46,282,8,46,10,
		46,12,46,285,9,46,1,46,1,46,1,47,1,47,1,47,1,48,1,48,1,48,1,48,5,48,296,
		8,48,10,48,12,48,299,9,48,1,48,1,48,1,49,1,49,1,49,1,49,5,49,307,8,49,
		10,49,12,49,310,9,49,1,49,1,49,1,49,1,49,1,49,1,50,4,50,318,8,50,11,50,
		12,50,319,1,50,1,50,1,308,0,51,1,1,3,2,5,3,7,4,9,5,11,6,13,7,15,8,17,9,
		19,10,21,11,23,12,25,13,27,14,29,15,31,16,33,17,35,18,37,19,39,20,41,21,
		43,22,45,23,47,24,49,25,51,26,53,27,55,28,57,29,59,30,61,31,63,32,65,33,
		67,34,69,35,71,36,73,37,75,38,77,39,79,40,81,0,83,0,85,41,87,42,89,0,91,
		43,93,44,95,0,97,45,99,46,101,47,1,0,9,3,0,65,90,95,95,97,122,1,0,48,57,
		2,0,70,70,102,102,2,0,43,43,45,45,2,0,39,39,92,92,2,0,34,34,92,92,5,0,
		39,39,92,92,110,110,114,114,116,116,2,0,10,10,13,13,3,0,9,10,13,13,32,
		32,332,0,1,1,0,0,0,0,3,1,0,0,0,0,5,1,0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,0,11,
		1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,0,17,1,0,0,0,0,19,1,0,0,0,0,21,1,0,0,
		0,0,23,1,0,0,0,0,25,1,0,0,0,0,27,1,0,0,0,0,29,1,0,0,0,0,31,1,0,0,0,0,33,
		1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,0,39,1,0,0,0,0,41,1,0,0,0,0,43,1,0,0,
		0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,1,0,0,0,0,51,1,0,0,0,0,53,1,0,0,0,0,55,
		1,0,0,0,0,57,1,0,0,0,0,59,1,0,0,0,0,61,1,0,0,0,0,63,1,0,0,0,0,65,1,0,0,
		0,0,67,1,0,0,0,0,69,1,0,0,0,0,71,1,0,0,0,0,73,1,0,0,0,0,75,1,0,0,0,0,77,
		1,0,0,0,0,79,1,0,0,0,0,85,1,0,0,0,0,87,1,0,0,0,0,91,1,0,0,0,0,93,1,0,0,
		0,0,97,1,0,0,0,0,99,1,0,0,0,0,101,1,0,0,0,1,103,1,0,0,0,3,109,1,0,0,0,
		5,114,1,0,0,0,7,117,1,0,0,0,9,122,1,0,0,0,11,126,1,0,0,0,13,132,1,0,0,
		0,15,138,1,0,0,0,17,145,1,0,0,0,19,150,1,0,0,0,21,156,1,0,0,0,23,160,1,
		0,0,0,25,165,1,0,0,0,27,171,1,0,0,0,29,177,1,0,0,0,31,179,1,0,0,0,33,181,
		1,0,0,0,35,183,1,0,0,0,37,185,1,0,0,0,39,187,1,0,0,0,41,189,1,0,0,0,43,
		191,1,0,0,0,45,193,1,0,0,0,47,195,1,0,0,0,49,197,1,0,0,0,51,199,1,0,0,
		0,53,201,1,0,0,0,55,203,1,0,0,0,57,205,1,0,0,0,59,208,1,0,0,0,61,211,1,
		0,0,0,63,214,1,0,0,0,65,217,1,0,0,0,67,220,1,0,0,0,69,223,1,0,0,0,71,225,
		1,0,0,0,73,228,1,0,0,0,75,230,1,0,0,0,77,233,1,0,0,0,79,235,1,0,0,0,81,
		243,1,0,0,0,83,246,1,0,0,0,85,251,1,0,0,0,87,255,1,0,0,0,89,267,1,0,0,
		0,91,271,1,0,0,0,93,278,1,0,0,0,95,288,1,0,0,0,97,291,1,0,0,0,99,302,1,
		0,0,0,101,317,1,0,0,0,103,104,5,99,0,0,104,105,5,108,0,0,105,106,5,97,
		0,0,106,107,5,115,0,0,107,108,5,115,0,0,108,2,1,0,0,0,109,110,5,118,0,
		0,110,111,5,111,0,0,111,112,5,105,0,0,112,113,5,100,0,0,113,4,1,0,0,0,
		114,115,5,105,0,0,115,116,5,102,0,0,116,6,1,0,0,0,117,118,5,101,0,0,118,
		119,5,108,0,0,119,120,5,115,0,0,120,121,5,101,0,0,121,8,1,0,0,0,122,123,
		5,102,0,0,123,124,5,111,0,0,124,125,5,114,0,0,125,10,1,0,0,0,126,127,5,
		119,0,0,127,128,5,104,0,0,128,129,5,105,0,0,129,130,5,108,0,0,130,131,
		5,101,0,0,131,12,1,0,0,0,132,133,5,98,0,0,133,134,5,114,0,0,134,135,5,
		101,0,0,135,136,5,97,0,0,136,137,5,107,0,0,137,14,1,0,0,0,138,139,5,114,
		0,0,139,140,5,101,0,0,140,141,5,116,0,0,141,142,5,117,0,0,142,143,5,114,
		0,0,143,144,5,110,0,0,144,16,1,0,0,0,145,146,5,114,0,0,146,147,5,101,0,
		0,147,148,5,97,0,0,148,149,5,100,0,0,149,18,1,0,0,0,150,151,5,119,0,0,
		151,152,5,114,0,0,152,153,5,105,0,0,153,154,5,116,0,0,154,155,5,101,0,
		0,155,20,1,0,0,0,156,157,5,110,0,0,157,158,5,101,0,0,158,159,5,119,0,0,
		159,22,1,0,0,0,160,161,5,116,0,0,161,162,5,114,0,0,162,163,5,117,0,0,163,
		164,5,101,0,0,164,24,1,0,0,0,165,166,5,102,0,0,166,167,5,97,0,0,167,168,
		5,108,0,0,168,169,5,115,0,0,169,170,5,101,0,0,170,26,1,0,0,0,171,172,5,
		117,0,0,172,173,5,115,0,0,173,174,5,105,0,0,174,175,5,110,0,0,175,176,
		5,103,0,0,176,28,1,0,0,0,177,178,5,43,0,0,178,30,1,0,0,0,179,180,5,45,
		0,0,180,32,1,0,0,0,181,182,5,42,0,0,182,34,1,0,0,0,183,184,5,47,0,0,184,
		36,1,0,0,0,185,186,5,37,0,0,186,38,1,0,0,0,187,188,5,61,0,0,188,40,1,0,
		0,0,189,190,5,40,0,0,190,42,1,0,0,0,191,192,5,41,0,0,192,44,1,0,0,0,193,
		194,5,123,0,0,194,46,1,0,0,0,195,196,5,125,0,0,196,48,1,0,0,0,197,198,
		5,91,0,0,198,50,1,0,0,0,199,200,5,93,0,0,200,52,1,0,0,0,201,202,5,59,0,
		0,202,54,1,0,0,0,203,204,5,44,0,0,204,56,1,0,0,0,205,206,5,43,0,0,206,
		207,5,43,0,0,207,58,1,0,0,0,208,209,5,45,0,0,209,210,5,45,0,0,210,60,1,
		0,0,0,211,212,5,124,0,0,212,213,5,124,0,0,213,62,1,0,0,0,214,215,5,38,
		0,0,215,216,5,38,0,0,216,64,1,0,0,0,217,218,5,61,0,0,218,219,5,61,0,0,
		219,66,1,0,0,0,220,221,5,33,0,0,221,222,5,61,0,0,222,68,1,0,0,0,223,224,
		5,62,0,0,224,70,1,0,0,0,225,226,5,62,0,0,226,227,5,61,0,0,227,72,1,0,0,
		0,228,229,5,60,0,0,229,74,1,0,0,0,230,231,5,60,0,0,231,232,5,61,0,0,232,
		76,1,0,0,0,233,234,5,46,0,0,234,78,1,0,0,0,235,240,3,81,40,0,236,239,3,
		81,40,0,237,239,3,83,41,0,238,236,1,0,0,0,238,237,1,0,0,0,239,242,1,0,
		0,0,240,238,1,0,0,0,240,241,1,0,0,0,241,80,1,0,0,0,242,240,1,0,0,0,243,
		244,7,0,0,0,244,82,1,0,0,0,245,247,7,1,0,0,246,245,1,0,0,0,247,248,1,0,
		0,0,248,246,1,0,0,0,248,249,1,0,0,0,249,84,1,0,0,0,250,252,7,1,0,0,251,
		250,1,0,0,0,252,253,1,0,0,0,253,251,1,0,0,0,253,254,1,0,0,0,254,86,1,0,
		0,0,255,258,3,85,42,0,256,257,5,46,0,0,257,259,3,85,42,0,258,256,1,0,0,
		0,258,259,1,0,0,0,259,261,1,0,0,0,260,262,3,89,44,0,261,260,1,0,0,0,261,
		262,1,0,0,0,262,264,1,0,0,0,263,265,7,2,0,0,264,263,1,0,0,0,264,265,1,
		0,0,0,265,88,1,0,0,0,266,268,7,3,0,0,267,266,1,0,0,0,267,268,1,0,0,0,268,
		269,1,0,0,0,269,270,3,85,42,0,270,90,1,0,0,0,271,274,5,39,0,0,272,275,
		3,95,47,0,273,275,8,4,0,0,274,272,1,0,0,0,274,273,1,0,0,0,275,276,1,0,
		0,0,276,277,5,39,0,0,277,92,1,0,0,0,278,283,5,34,0,0,279,282,3,95,47,0,
		280,282,8,5,0,0,281,279,1,0,0,0,281,280,1,0,0,0,282,285,1,0,0,0,283,281,
		1,0,0,0,283,284,1,0,0,0,284,286,1,0,0,0,285,283,1,0,0,0,286,287,5,34,0,
		0,287,94,1,0,0,0,288,289,5,92,0,0,289,290,7,6,0,0,290,96,1,0,0,0,291,292,
		5,47,0,0,292,293,5,47,0,0,293,297,1,0,0,0,294,296,8,7,0,0,295,294,1,0,
		0,0,296,299,1,0,0,0,297,295,1,0,0,0,297,298,1,0,0,0,298,300,1,0,0,0,299,
		297,1,0,0,0,300,301,6,48,0,0,301,98,1,0,0,0,302,303,5,47,0,0,303,304,5,
		42,0,0,304,308,1,0,0,0,305,307,9,0,0,0,306,305,1,0,0,0,307,310,1,0,0,0,
		308,309,1,0,0,0,308,306,1,0,0,0,309,311,1,0,0,0,310,308,1,0,0,0,311,312,
		5,42,0,0,312,313,5,47,0,0,313,314,1,0,0,0,314,315,6,49,0,0,315,100,1,0,
		0,0,316,318,7,8,0,0,317,316,1,0,0,0,318,319,1,0,0,0,319,317,1,0,0,0,319,
		320,1,0,0,0,320,321,1,0,0,0,321,322,6,50,0,0,322,102,1,0,0,0,15,0,238,
		240,248,253,258,261,264,267,274,281,283,297,308,319,1,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace generated
