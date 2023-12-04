using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoCHelper
{
    public static class InputHelper
    {
        public static List<string> ReadAllLines(this string rawInput)
        {
            return rawInput.Split("\r\n").ToList();
        }
    }
}
