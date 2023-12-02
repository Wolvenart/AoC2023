namespace Day_02
{
    public static class Main
    {
        private class Hand
        {
            public int R { get; set; } = 0;
            public int G { get; set; } = 0;
            public int B { get; set; } = 0;
        }
        public static int Part1(bool test)
        {
            var input = ParseInput(test ? Properties.Resources.TestInput : Properties.Resources.RealInput);
            var testVal = new Hand()
            {
                R = 12,
                G = 13,
                B = 14,
            };
            var lstValidGamesTemp = input.ToList();
            lstValidGamesTemp.RemoveAll(x => x.Value.Any(c => c.R > testVal.R || c.G > testVal.G || c.B > testVal.B));
            return lstValidGamesTemp.Sum(x => x.Key);
        }
        public static int Part2(bool test)
        {
            var input = ParseInput(test ? Properties.Resources.TestInput : Properties.Resources.RealInput);
            var lstPowers = input.Values.Select(x =>
            {
                var maxR = x.Select(c => c.R).Max();
                var maxG = x.Select(c => c.G).Max();
                var maxB = x.Select(c => c.B).Max();
                return maxR * maxG * maxB;
            }).ToList();
            return lstPowers.Sum();
        }
        private static Dictionary<int, List<Hand>> ParseInput(string input)
        {
            var formatInput = new Dictionary<int, List<Hand>>();
            foreach (var line in input.Split("\r\n"))
            {
                if (String.IsNullOrWhiteSpace(line)) continue;

                var gameSplit = line.Split(":");
                var gameId = Convert.ToInt16(gameSplit[0].Replace("Game", ""));
                var lstVals = gameSplit[1].Split(";")
                    .Select(x =>
                    {
                        var hand = new Hand();
                        foreach (var step in x.Split(","))
                        {
                            var valSplit = step.Split(" ").ToList();
                            valSplit.RemoveAll(c => String.IsNullOrWhiteSpace(c));
                            var numVal = Convert.ToInt16(valSplit[0].Trim(' '));
                            var prop = hand.GetType().GetProperty(valSplit[1].ToUpper()[0].ToString());
                            prop?.SetValue(hand, numVal);
                        }
                        return hand;
                    }).ToList();
                formatInput.Add(gameId, lstVals);
            }
            return formatInput;
        }
    }
}