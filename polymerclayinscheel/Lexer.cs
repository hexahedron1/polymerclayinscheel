using System.Text.RegularExpressions;
using System.Text;
namespace polymerclayinscheel;

internal static partial class Lexer {
    private const RegexOptions FLAGS = RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled;

    [GeneratedRegex("-?\\d+")]
    private static partial Regex INT();

    [GeneratedRegex("([-+]?)((\\d*(?:\\.\\d+|[eE][-+]?\\d+))|inf|nan)", FLAGS)]
    private static partial Regex FLOAT();

    [GeneratedRegex("([-+]?)((\\d+(?:\\.\\d+|[eE][-+]?\\d+))|inf|nan)([+-])((\\d+(?:\\.\\d+|[eE][-+]?\\d+))|inf|nan)i", FLAGS)]
    private static partial Regex COMPLEX();

    private static readonly Dictionary<string, OperatorType> operators = new() {
		{ "<-", OperatorType.LeftArrow },
		{ "->", OperatorType.RightArrow },
		{ "+", OperatorType.Plus }
	};
    public static int End(this Match match) => match.Index + match.Length;
    public static string SubstringSafe(this string s, int startIndex, int length) => startIndex + length <= s.Length ? s.Substring(startIndex, length) : s[startIndex..];
    public static char? CharAt(this string s, int index) => index < s.Length ? s[index] : null;
    public static bool IsAlpha(this char c) => ('a' <= c && c <= 'z') ||
                                               ('A' <= c && c <= 'Z') ||
                                               ('а' <= c && c <= 'я') ||
                                               ('А' <= c && c <= 'Я') ||
                                               c == 'ё' || c == 'Ё';

    public class LexerException(string msg, string doc, int pos) : Exception(
    $"{msg}: " +
        $"line {doc[..pos].Count(c => c == '\n') + 1} " +
        $"column {(doc.LastIndexOf('\n', pos) == -1 ? pos + 1 : pos - doc.LastIndexOf('\n', pos))} " +
        $"(char {pos})"
    ) { }

    public static IToken[] Lex(string s) {
		int idx = 0;
        List<IToken> tokens = [];

        void ReadString() {
            StringBuilder builder = new();
            int begin = idx - 1;
            while (true) {
                if (idx >= s.Length || s[idx] < ' ') {
                    throw new LexerException("Unterminated string starting at", s, begin);
                }
                char nextchar = s[idx++];
                if (nextchar == '"') break;
                else if (nextchar == '\\') {
                    if (idx >= s.Length) throw new LexerException("Unterminated string starting at", s, idx);
                    throw new LexerException($"There Is No Escape Sequences Yet Because Phoque You!", s, idx);
                }
                else {
                    builder.Append(nextchar);
                    if (char.IsSurrogate(nextchar)) {
                        idx++;
                        if (idx >= s.Length) throw new LexerException("Unterminated string starting at", s, idx);
                        builder.Append(s[idx]);
                    }
                }
            }
            tokens.Add(new Token<string>(TokenType.Primitive, builder.ToString()));
        }

        while (true) {
            while (idx < s.Length && " \t\n\r".Contains(s[idx])) {
                unchecked { idx++; }
            }
            if (idx >= s.Length) return [..tokens];
            foreach (var kvp in operators) {
                if (kvp.Key == s.SubstringSafe(idx, kvp.Key.Length)) {
                    tokens.Add(new Token<OperatorType>(TokenType.Type, kvp.Value));
                    idx += kvp.Key.Length;
                    continue;
                }
            }
            if (s.CharAt(idx) == '"') {
                idx++;
                ReadString();
                continue;
            }
            // read variable
            int offset = 0;
            while (idx < s.Length && s[idx + offset].IsAlpha()) {
                unchecked { offset++; }
            }
            if (offset > 0) {
                string k = s.Substring(idx, offset);
                idx += offset;
                tokens.Add(new Token<string>(TokenType.Other, k));
                continue;
            }

        }
	}
}
