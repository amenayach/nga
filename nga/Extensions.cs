using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nga
{
    public static class Extensions
    {
        public static string GetArgument(this string[] args, int index)
        {
            if (args != null && args.Length > index)
            {
                return args[index];
            }

            return string.Empty;
        }

        public static string ToDash(this string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var result = string.Empty;

                return string.Join(string.Empty, name.Select((m, index) => ((index != 0 && Char.IsUpper(m)) ? $"-{m.ToString().ToLower()}" : m.ToString())));                
            }

            return string.Empty;
        }
    }
}
