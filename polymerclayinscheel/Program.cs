/*
    Void: He (helium)
    Null: Ar (argon)
    Bool: H (hydrogen)
    Byte: Ne (neon)
    Short: O (oxygen)
    Int: C (carbon)
    Long: Pb (lead)
    Int128: S (sulfur)
    String: Si (silicon)
    Float: Ge (solution)
    Double: Hg (mercury)
    Complex: Mu (muonium)
    Char: N (nitrogen)
    SByte: F (fluorine)
    UShort: P (phosphorus)
    UInt: U (uranium)
    ULong: W (tungsten)
    Decimal: Cd (cadmium)
    BigInt: Th (thorium)
    - Li (lithium)
    - Rb (rubidium)
    - Cs (cesium)
    - Be (beryllium)
    - B (boron)
    
    C var <- 0 // initialization and declaration
    C var // initialization
    var <- 0 // declaration
       
    // function
    He func1 ->
        // stuff here   
    ;

    // function with args
    He func2 + C arg1 ->
        
    ;

    C func3 ->
        <- 93489
    ;
    
 */

namespace polymerclayinscheel;

internal class Program {
	static string file = "C:\\Users\\111\\source\\repos\\polymerclayinscheel\\polymerclayinscheel\\test.chem";
	static void Main(string[] args) {
		if (args.Length != 0)
			file = args[0];
		if (!File.Exists(file)) {
			Console.WriteLine("The file does not exist bruvver");
			return;
		}
		string code = File.ReadAllText(file);
		IToken[] tokens = Lexer.Lex(code);
        foreach (var token in tokens)
            Console.WriteLine(token);
    }
}
internal struct Atom {

}
