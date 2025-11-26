using System.Collections.Generic;

public class LanguageValidator
{
    private static readonly Dictionary<char, char> LayoutMap = new Dictionary<char, char>
    {
        {'é', 'q'}, {'ö', 'w'}, {'ó', 'e'}, {'ê', 'r'}, {'å', 't'}, {'í', 'y'}, {'ã', 'u'}, {'ø', 'i'}, {'ù', 'o'}, {'ç', 'p'},
        {'õ', '['}, {'ú', ']'},
        {'ô', 'a'}, {'û', 's'}, {'â', 'd'}, {'à', 'f'}, {'ï', 'g'}, {'ð', 'h'}, {'î', 'j'}, {'ë', 'k'}, {'ä', 'l'},
        {'æ', ';'}, {'ý', '\''},
        {'ÿ', 'z'}, {'÷', 'x'}, {'ñ', 'c'}, {'ì', 'v'}, {'è', 'b'}, {'ò', 'n'}, {'ü', 'm'}, {'á', ','}, {'þ', '.'},
        
        {'³', 's'}, {'¿', ']'}, {'º', '\''}, {'´', '`'},
    };

    public static char ValidateChar(char c)
    {
        c = char.ToLower(c);

        if (!char.IsLetter(c)) return c;

        if (c >= 'a' && c <= 'z') return c;

        if (LayoutMap.TryGetValue(c, out char englishChar)) return englishChar;

        return c;
    }
}