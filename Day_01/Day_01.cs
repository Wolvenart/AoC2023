namespace Day_01
{
    public static class Main
    {
        private static readonly Dictionary<string, int> NumText = new Dictionary<string, int>()
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 },
        };
        public static int Part1(bool test)
        {
            var input = test ? Properties.Resources.TestInput : Properties.Resources.RealInput;

            List<int> lstVals = new List<int>();
            foreach (var line in input.Split("\r\n"))
            {
                if (String.IsNullOrWhiteSpace(line)) continue;
                var valTemp = String.Empty;

                valTemp += line.FirstOrDefault(x => Char.IsDigit(x));
                valTemp += line.LastOrDefault(x => Char.IsDigit(x));
                lstVals.Add(Convert.ToInt32(valTemp));
            }
            return lstVals.Sum();
        }
        public static int Part2(bool test)
        {
            var input = test ? Properties.Resources.TestInput2 : Properties.Resources.RealInput;

            var lstVals = new List<int>();
            foreach (var line in input.Split("\r\n"))
            {
                var valTemp = 0;

                var strTempVal = String.Empty;
                for (int i = 0; i < line.Length; i++)
                {
                    if (NumText.Any(x => strTempVal.Contains(x.Key)))
                    {
                        var keyCheck = NumText.FirstOrDefault(x => strTempVal.Contains(x.Key)).Value;
                        valTemp = keyCheck * 10;
                        break;
                    }
                    if (Int32.TryParse(Convert.ToString(line[i]), out var val))
                    {
                        valTemp = val * 10; 
                        break;
                    }
                    strTempVal += line[i];
                }

                strTempVal = String.Empty;
                for (int i = line.Length - 1; i >= 0; i--)
                {
                    if (NumText.Any(x => new string(strTempVal.Reverse().ToArray()).Contains(x.Key)))
                    {
                        var keyCheck = NumText.FirstOrDefault(x => new string(strTempVal.Reverse().ToArray()).Contains(x.Key)).Value;
                        valTemp += keyCheck;
                        break;
                    }
                    if (Int32.TryParse(Convert.ToString(line[i]), out var val))
                    {
                        valTemp += val;
                        break;
                    }
                    strTempVal += line[i];
                }
                lstVals.Add(valTemp);
            }

            return lstVals.Sum();
        }
    }
}