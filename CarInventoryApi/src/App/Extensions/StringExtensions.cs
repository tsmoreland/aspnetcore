using System.Runtime.CompilerServices;
using System.Text;

namespace CarInventory.App.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Convert <paramref name="source"/> to snake_case
    /// </summary>
    /// <param name="source">string to convert to snake case</param>
    /// <returns><paramref name="source"/> in snake_case</returns>
    public static string ToSnakeCase(this string source)
    {
        if (source is not { Length: > 0 })
        {
            return source;
        }

        // worst case size
        StringBuilder builder = new(source.Length * 2);

        ReadOnlySpan<char> asSpan = source.AsSpan();
        builder.Append(char.ToLower(asSpan[0]));

        for (int i = 1; i < asSpan.Length; i++)
        {
            char ch = asSpan[i];

            if (IsLowerAlphaOrNumericOrUnderscore(ch))
            {
                builder.Append(ch);
                continue;
            }
            if (ch == '-') // kebab-case
            {
                builder.Append('_');
            }

            bool previousCheck = asSpan[i] != '_';
            bool nextCheck = i + 1 < asSpan.Length && !char.IsUpper(asSpan[i + 1]) && asSpan[i + 1] != '_' && !char.IsNumber(asSpan[i + 1]);

            if (previousCheck && nextCheck)
            {
                builder.Append('_');
            }

            builder.Append(char.ToLower(ch));
        }
        return builder.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsLowerAlphaOrNumericOrUnderscore(char ch)
    {
        return ch is >= 'a' and <= 'z' or >= '0' and <= '9' or '_';
    }
}
