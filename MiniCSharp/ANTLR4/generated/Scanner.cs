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
		WRITE=10, NEW=11, TRUE=12, FALSE=13, USING=14, NULL=15, QMARK=16, PLUS=17, 
		MINUS=18, MULT=19, DIV=20, MOD=21, ASSIGN=22, LPAREN=23, RPAREN=24, LBRACE=25, 
		RBRACE=26, LBRACK=27, RBRACK=28, SEMICOLON=29, COMMA=30, INCREMENT=31, 
		DECREMENT=32, LOGICAL_OR=33, LOGICAL_AND=34, EQUALS=35, NOT_EQUALS=36, 
		GREATER_THAN=37, GREATER_EQUALS=38, LESS_THAN=39, LESS_EQUALS=40, DOT=41, 
		IDENTIFIER=42, NUMBER=43, DOUBLE_CONST=44, CHAR_CONSTANT=45, STRING_CONSTANT=46, 
		COMMENT=47, BLOCK_COMMENT=48, WHITESPACE=49;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"CLASS", "VOID", "IF", "ELSE", "FOR", "WHILE", "BREAK", "RETURN", "READ", 
		"WRITE", "NEW", "TRUE", "FALSE", "USING", "NULL", "QMARK", "PLUS", "MINUS", 
		"MULT", "DIV", "MOD", "ASSIGN", "LPAREN", "RPAREN", "LBRACE", "RBRACE", 
		"LBRACK", "RBRACK", "SEMICOLON", "COMMA", "INCREMENT", "DECREMENT", "LOGICAL_OR", 
		"LOGICAL_AND", "EQUALS", "NOT_EQUALS", "GREATER_THAN", "GREATER_EQUALS", 
		"LESS_THAN", "LESS_EQUALS", "DOT", "IDENTIFIER", "LETTER", "DIGIT", "NUMBER", 
		"DOUBLE_CONST", "CHAR_CONSTANT", "STRING_CONSTANT", "ESCAPE_SEQUENCE", 
		"COMMENT", "BLOCK_COMMENT", "WHITESPACE"
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
		"'null'", "'?'", "'+'", "'-'", "'*'", "'/'", "'%'", "'='", "'('", "')'", 
		"'{'", "'}'", "'['", "']'", "';'", "','", "'++'", "'--'", "'||'", "'&&'", 
		"'=='", "'!='", "'>'", "'>='", "'<'", "'<='", "'.'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "CLASS", "VOID", "IF", "ELSE", "FOR", "WHILE", "BREAK", "RETURN", 
		"READ", "WRITE", "NEW", "TRUE", "FALSE", "USING", "NULL", "QMARK", "PLUS", 
		"MINUS", "MULT", "DIV", "MOD", "ASSIGN", "LPAREN", "RPAREN", "LBRACE", 
		"RBRACE", "LBRACK", "RBRACK", "SEMICOLON", "COMMA", "INCREMENT", "DECREMENT", 
		"LOGICAL_OR", "LOGICAL_AND", "EQUALS", "NOT_EQUALS", "GREATER_THAN", "GREATER_EQUALS", 
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
		4,0,49,320,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,2,29,7,29,2,30,7,30,2,31,7,31,2,32,7,32,2,33,7,33,2,34,7,34,2,35,
		7,35,2,36,7,36,2,37,7,37,2,38,7,38,2,39,7,39,2,40,7,40,2,41,7,41,2,42,
		7,42,2,43,7,43,2,44,7,44,2,45,7,45,2,46,7,46,2,47,7,47,2,48,7,48,2,49,
		7,49,2,50,7,50,2,51,7,51,1,0,1,0,1,0,1,0,1,0,1,0,1,1,1,1,1,1,1,1,1,1,1,
		2,1,2,1,2,1,3,1,3,1,3,1,3,1,3,1,4,1,4,1,4,1,4,1,5,1,5,1,5,1,5,1,5,1,5,
		1,6,1,6,1,6,1,6,1,6,1,6,1,7,1,7,1,7,1,7,1,7,1,7,1,7,1,8,1,8,1,8,1,8,1,
		8,1,9,1,9,1,9,1,9,1,9,1,9,1,10,1,10,1,10,1,10,1,11,1,11,1,11,1,11,1,11,
		1,12,1,12,1,12,1,12,1,12,1,12,1,13,1,13,1,13,1,13,1,13,1,13,1,14,1,14,
		1,14,1,14,1,14,1,15,1,15,1,16,1,16,1,17,1,17,1,18,1,18,1,19,1,19,1,20,
		1,20,1,21,1,21,1,22,1,22,1,23,1,23,1,24,1,24,1,25,1,25,1,26,1,26,1,27,
		1,27,1,28,1,28,1,29,1,29,1,30,1,30,1,30,1,31,1,31,1,31,1,32,1,32,1,32,
		1,33,1,33,1,33,1,34,1,34,1,34,1,35,1,35,1,35,1,36,1,36,1,37,1,37,1,37,
		1,38,1,38,1,39,1,39,1,39,1,40,1,40,1,41,1,41,1,41,5,41,248,8,41,10,41,
		12,41,251,9,41,1,42,1,42,1,43,4,43,256,8,43,11,43,12,43,257,1,44,4,44,
		261,8,44,11,44,12,44,262,1,45,1,45,1,45,1,45,1,46,1,46,1,46,3,46,272,8,
		46,1,46,1,46,1,47,1,47,1,47,5,47,279,8,47,10,47,12,47,282,9,47,1,47,1,
		47,1,48,1,48,1,48,1,49,1,49,1,49,1,49,5,49,293,8,49,10,49,12,49,296,9,
		49,1,49,1,49,1,50,1,50,1,50,1,50,5,50,304,8,50,10,50,12,50,307,9,50,1,
		50,1,50,1,50,1,50,1,50,1,51,4,51,315,8,51,11,51,12,51,316,1,51,1,51,1,
		305,0,52,1,1,3,2,5,3,7,4,9,5,11,6,13,7,15,8,17,9,19,10,21,11,23,12,25,
		13,27,14,29,15,31,16,33,17,35,18,37,19,39,20,41,21,43,22,45,23,47,24,49,
		25,51,26,53,27,55,28,57,29,59,30,61,31,63,32,65,33,67,34,69,35,71,36,73,
		37,75,38,77,39,79,40,81,41,83,42,85,0,87,0,89,43,91,44,93,45,95,46,97,
		0,99,47,101,48,103,49,1,0,7,3,0,65,90,95,95,97,122,1,0,48,57,2,0,39,39,
		92,92,2,0,34,34,92,92,5,0,39,39,92,92,110,110,114,114,116,116,2,0,10,10,
		13,13,3,0,9,10,13,13,32,32,326,0,1,1,0,0,0,0,3,1,0,0,0,0,5,1,0,0,0,0,7,
		1,0,0,0,0,9,1,0,0,0,0,11,1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,0,17,1,0,0,
		0,0,19,1,0,0,0,0,21,1,0,0,0,0,23,1,0,0,0,0,25,1,0,0,0,0,27,1,0,0,0,0,29,
		1,0,0,0,0,31,1,0,0,0,0,33,1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,0,39,1,0,0,
		0,0,41,1,0,0,0,0,43,1,0,0,0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,1,0,0,0,0,51,
		1,0,0,0,0,53,1,0,0,0,0,55,1,0,0,0,0,57,1,0,0,0,0,59,1,0,0,0,0,61,1,0,0,
		0,0,63,1,0,0,0,0,65,1,0,0,0,0,67,1,0,0,0,0,69,1,0,0,0,0,71,1,0,0,0,0,73,
		1,0,0,0,0,75,1,0,0,0,0,77,1,0,0,0,0,79,1,0,0,0,0,81,1,0,0,0,0,83,1,0,0,
		0,0,89,1,0,0,0,0,91,1,0,0,0,0,93,1,0,0,0,0,95,1,0,0,0,0,99,1,0,0,0,0,101,
		1,0,0,0,0,103,1,0,0,0,1,105,1,0,0,0,3,111,1,0,0,0,5,116,1,0,0,0,7,119,
		1,0,0,0,9,124,1,0,0,0,11,128,1,0,0,0,13,134,1,0,0,0,15,140,1,0,0,0,17,
		147,1,0,0,0,19,152,1,0,0,0,21,158,1,0,0,0,23,162,1,0,0,0,25,167,1,0,0,
		0,27,173,1,0,0,0,29,179,1,0,0,0,31,184,1,0,0,0,33,186,1,0,0,0,35,188,1,
		0,0,0,37,190,1,0,0,0,39,192,1,0,0,0,41,194,1,0,0,0,43,196,1,0,0,0,45,198,
		1,0,0,0,47,200,1,0,0,0,49,202,1,0,0,0,51,204,1,0,0,0,53,206,1,0,0,0,55,
		208,1,0,0,0,57,210,1,0,0,0,59,212,1,0,0,0,61,214,1,0,0,0,63,217,1,0,0,
		0,65,220,1,0,0,0,67,223,1,0,0,0,69,226,1,0,0,0,71,229,1,0,0,0,73,232,1,
		0,0,0,75,234,1,0,0,0,77,237,1,0,0,0,79,239,1,0,0,0,81,242,1,0,0,0,83,244,
		1,0,0,0,85,252,1,0,0,0,87,255,1,0,0,0,89,260,1,0,0,0,91,264,1,0,0,0,93,
		268,1,0,0,0,95,275,1,0,0,0,97,285,1,0,0,0,99,288,1,0,0,0,101,299,1,0,0,
		0,103,314,1,0,0,0,105,106,5,99,0,0,106,107,5,108,0,0,107,108,5,97,0,0,
		108,109,5,115,0,0,109,110,5,115,0,0,110,2,1,0,0,0,111,112,5,118,0,0,112,
		113,5,111,0,0,113,114,5,105,0,0,114,115,5,100,0,0,115,4,1,0,0,0,116,117,
		5,105,0,0,117,118,5,102,0,0,118,6,1,0,0,0,119,120,5,101,0,0,120,121,5,
		108,0,0,121,122,5,115,0,0,122,123,5,101,0,0,123,8,1,0,0,0,124,125,5,102,
		0,0,125,126,5,111,0,0,126,127,5,114,0,0,127,10,1,0,0,0,128,129,5,119,0,
		0,129,130,5,104,0,0,130,131,5,105,0,0,131,132,5,108,0,0,132,133,5,101,
		0,0,133,12,1,0,0,0,134,135,5,98,0,0,135,136,5,114,0,0,136,137,5,101,0,
		0,137,138,5,97,0,0,138,139,5,107,0,0,139,14,1,0,0,0,140,141,5,114,0,0,
		141,142,5,101,0,0,142,143,5,116,0,0,143,144,5,117,0,0,144,145,5,114,0,
		0,145,146,5,110,0,0,146,16,1,0,0,0,147,148,5,114,0,0,148,149,5,101,0,0,
		149,150,5,97,0,0,150,151,5,100,0,0,151,18,1,0,0,0,152,153,5,119,0,0,153,
		154,5,114,0,0,154,155,5,105,0,0,155,156,5,116,0,0,156,157,5,101,0,0,157,
		20,1,0,0,0,158,159,5,110,0,0,159,160,5,101,0,0,160,161,5,119,0,0,161,22,
		1,0,0,0,162,163,5,116,0,0,163,164,5,114,0,0,164,165,5,117,0,0,165,166,
		5,101,0,0,166,24,1,0,0,0,167,168,5,102,0,0,168,169,5,97,0,0,169,170,5,
		108,0,0,170,171,5,115,0,0,171,172,5,101,0,0,172,26,1,0,0,0,173,174,5,117,
		0,0,174,175,5,115,0,0,175,176,5,105,0,0,176,177,5,110,0,0,177,178,5,103,
		0,0,178,28,1,0,0,0,179,180,5,110,0,0,180,181,5,117,0,0,181,182,5,108,0,
		0,182,183,5,108,0,0,183,30,1,0,0,0,184,185,5,63,0,0,185,32,1,0,0,0,186,
		187,5,43,0,0,187,34,1,0,0,0,188,189,5,45,0,0,189,36,1,0,0,0,190,191,5,
		42,0,0,191,38,1,0,0,0,192,193,5,47,0,0,193,40,1,0,0,0,194,195,5,37,0,0,
		195,42,1,0,0,0,196,197,5,61,0,0,197,44,1,0,0,0,198,199,5,40,0,0,199,46,
		1,0,0,0,200,201,5,41,0,0,201,48,1,0,0,0,202,203,5,123,0,0,203,50,1,0,0,
		0,204,205,5,125,0,0,205,52,1,0,0,0,206,207,5,91,0,0,207,54,1,0,0,0,208,
		209,5,93,0,0,209,56,1,0,0,0,210,211,5,59,0,0,211,58,1,0,0,0,212,213,5,
		44,0,0,213,60,1,0,0,0,214,215,5,43,0,0,215,216,5,43,0,0,216,62,1,0,0,0,
		217,218,5,45,0,0,218,219,5,45,0,0,219,64,1,0,0,0,220,221,5,124,0,0,221,
		222,5,124,0,0,222,66,1,0,0,0,223,224,5,38,0,0,224,225,5,38,0,0,225,68,
		1,0,0,0,226,227,5,61,0,0,227,228,5,61,0,0,228,70,1,0,0,0,229,230,5,33,
		0,0,230,231,5,61,0,0,231,72,1,0,0,0,232,233,5,62,0,0,233,74,1,0,0,0,234,
		235,5,62,0,0,235,236,5,61,0,0,236,76,1,0,0,0,237,238,5,60,0,0,238,78,1,
		0,0,0,239,240,5,60,0,0,240,241,5,61,0,0,241,80,1,0,0,0,242,243,5,46,0,
		0,243,82,1,0,0,0,244,249,3,85,42,0,245,248,3,85,42,0,246,248,3,87,43,0,
		247,245,1,0,0,0,247,246,1,0,0,0,248,251,1,0,0,0,249,247,1,0,0,0,249,250,
		1,0,0,0,250,84,1,0,0,0,251,249,1,0,0,0,252,253,7,0,0,0,253,86,1,0,0,0,
		254,256,7,1,0,0,255,254,1,0,0,0,256,257,1,0,0,0,257,255,1,0,0,0,257,258,
		1,0,0,0,258,88,1,0,0,0,259,261,7,1,0,0,260,259,1,0,0,0,261,262,1,0,0,0,
		262,260,1,0,0,0,262,263,1,0,0,0,263,90,1,0,0,0,264,265,3,89,44,0,265,266,
		3,81,40,0,266,267,3,89,44,0,267,92,1,0,0,0,268,271,5,39,0,0,269,272,3,
		97,48,0,270,272,8,2,0,0,271,269,1,0,0,0,271,270,1,0,0,0,272,273,1,0,0,
		0,273,274,5,39,0,0,274,94,1,0,0,0,275,280,5,34,0,0,276,279,3,97,48,0,277,
		279,8,3,0,0,278,276,1,0,0,0,278,277,1,0,0,0,279,282,1,0,0,0,280,278,1,
		0,0,0,280,281,1,0,0,0,281,283,1,0,0,0,282,280,1,0,0,0,283,284,5,34,0,0,
		284,96,1,0,0,0,285,286,5,92,0,0,286,287,7,4,0,0,287,98,1,0,0,0,288,289,
		5,47,0,0,289,290,5,47,0,0,290,294,1,0,0,0,291,293,8,5,0,0,292,291,1,0,
		0,0,293,296,1,0,0,0,294,292,1,0,0,0,294,295,1,0,0,0,295,297,1,0,0,0,296,
		294,1,0,0,0,297,298,6,49,0,0,298,100,1,0,0,0,299,300,5,47,0,0,300,301,
		5,42,0,0,301,305,1,0,0,0,302,304,9,0,0,0,303,302,1,0,0,0,304,307,1,0,0,
		0,305,306,1,0,0,0,305,303,1,0,0,0,306,308,1,0,0,0,307,305,1,0,0,0,308,
		309,5,42,0,0,309,310,5,47,0,0,310,311,1,0,0,0,311,312,6,50,0,0,312,102,
		1,0,0,0,313,315,7,6,0,0,314,313,1,0,0,0,315,316,1,0,0,0,316,314,1,0,0,
		0,316,317,1,0,0,0,317,318,1,0,0,0,318,319,6,51,0,0,319,104,1,0,0,0,11,
		0,247,249,257,262,271,278,280,294,305,316,1,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace generated
