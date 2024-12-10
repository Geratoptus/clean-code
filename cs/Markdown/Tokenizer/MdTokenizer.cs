using System.Diagnostics;
using Markdown.Tokenizer.Scanners;
using Markdown.Tokens;

namespace Markdown.Tokenizer;

public class MdTokenizer
{
    private readonly ITokenScanner[] scanners = [
        new SpecScanner(), new TextScanner(), new NumberScanner()
    ];

    public List<Token> Tokenize(string text)
    {
        var begin = 0;
        var tokenList = new List<Token>();

        while (begin < text.Length)
        {
            var token = scanners
                .Select(scanner => scanner.Scan(text, begin))
                .First(token => token != null);

            Debug.Assert(token != null, nameof(token) + " != null");
            begin += token.Length;
            tokenList.Add(token);
        }
        return tokenList;
    }
}