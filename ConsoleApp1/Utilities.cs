using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class StringUtilities
    {
        public static bool IsValidEmail(this string input, string pattern)
        {
            return input.Contains(pattern); 
        }

    }
}
