using System.Runtime.CompilerServices;

namespace UFD.UfdElements;

public static class MiscHelpers
{
    [SkipLocalsInit]
    public static string FormatString(ReadOnlySpan<int> complexNumberParts, char complexIdentifier)
    {
        var re = complexNumberParts[0];
        var com = complexNumberParts[1];

        if (re == 0 && com == 0)
            return "0";

        Span<char> buffer = stackalloc char[11 + 1 + 11 + 1];

        var charsWritten = 0;

        if (re == 0)
        {
            if (com == 1)
            {
                buffer[charsWritten++] = complexIdentifier;
                return buffer[..charsWritten].ToString();
            }

            if (com == -1)
            {
                buffer[charsWritten++] = '-';
                buffer[charsWritten++] = complexIdentifier;
                return buffer[..charsWritten].ToString();
            }

            com.TryFormat(buffer, out charsWritten);
            buffer[charsWritten++] = complexIdentifier;
            return buffer[..charsWritten].ToString();
        }

        if (com == 0)
        {
            re.TryFormat(buffer, out charsWritten);
            return buffer[..charsWritten].ToString();
        }

        if (com == 1)
        {
            re.TryFormat(buffer, out charsWritten);
            buffer[charsWritten++] = '+';
            buffer[charsWritten++] = complexIdentifier;
            return buffer[..charsWritten].ToString();
        }

        if (com == -1)
        {
            re.TryFormat(buffer, out charsWritten);
            buffer[charsWritten++] = '-';
            buffer[charsWritten++] = complexIdentifier;
            return buffer[..charsWritten].ToString();
        }

        if (com < 0)
        {
            re.TryFormat(buffer, out charsWritten);
            com.TryFormat(buffer[charsWritten..], out var c1);
            charsWritten += c1;
            buffer[charsWritten++] = complexIdentifier;
            return buffer[..charsWritten].ToString();
        }

        re.TryFormat(buffer, out charsWritten);
        buffer[charsWritten++] = '+';
        com.TryFormat(buffer[charsWritten..], out var c2);
        charsWritten += c2;
        buffer[charsWritten++] = complexIdentifier;
        return buffer[..charsWritten].ToString();
    }
}
