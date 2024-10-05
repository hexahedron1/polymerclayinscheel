namespace polymerclayinscheel;

/*

C ->
	(C + C) -> 
		шкила
	;
;
 
*/

internal enum OperatorType {
	LeftArrow,
	RightArrow,
	Plus,
	Custom
}

internal enum TokenType {
	Type,
	Name,
	Declare,
	Primitive, // 90309213 , "hehehee"
	BlockStart,
	BlockEnd,
	Other
}

internal interface IToken {
	TokenType Type { get; }
}

internal class Token<T>(TokenType type, T value) : IToken {
	public TokenType Type { get; } = type;
	public T Value { get; } = value;
	public override string ToString() => $"{Type}: {Value}";
}