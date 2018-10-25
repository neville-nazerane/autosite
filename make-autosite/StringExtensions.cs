using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace make_autosite
{
    static class StringExtensions
    {

        internal static string CamelCase(this string str)
            => string.Join("", str.Trim().TrimEnd(':')
                                        .Replace(',', '_')
                                        .Replace('.', '_')
                                        .Split(" ")
                    .Select(s =>
                        s[0].ToString().ToUpper() + s.Substring(1)));
    }
}
