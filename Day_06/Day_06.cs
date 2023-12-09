using AoCHelper;

namespace Day_06
{
    public static class Main
    {
        private class Chart
        {
            public int TimeLimit { get; set; } = 0;
            public int DistanceRecord { get; set; } = 0;
        }
        private class RaceResult
        {
            public int DownTime { get; set; } = 0;
            public int RunTime { get; set; } = 0;
            public int DistanceTravelled { get => this.DownTime * this.RunTime; }
        }
        public static int Part1(bool test)
        {
            var inputLines = (test ? Properties.Resources.TestInput : Properties.Resources.RealInput).ReadAllLines();
            var raceChart = ParseInput(inputLines);

            var lstResult = new List<int>();

            foreach (var chart in raceChart)
            {
                var results = Enumerable.Range(0, chart.TimeLimit)
                    .Select(x => new RaceResult()
                    {
                        DownTime = x,
                        RunTime = chart.TimeLimit - x
                    }).ToList();
                lstResult.Add(results.Where(x => x.DistanceTravelled > chart.DistanceRecord).Count());
            }

            return lstResult.Aggregate(1, (x, y) => x * y);
        }
        public static double Part2(bool test)
        {
            var inputLines = (test ? Properties.Resources.TestInput : Properties.Resources.RealInput).ReadAllLines();
            var timeLimit = Convert.ToInt64(inputLines[0].Split(":")[1].Where(x => Char.IsNumber(x)).Aggregate("", (x, y) => x + y));
            var distanceRecord = Convert.ToInt64(inputLines[1].Split(":")[1].Where(x => Char.IsNumber(x)).Aggregate("", (x, y) => x + y));

            var d = timeLimit * timeLimit - 4 * (distanceRecord + 1);
            var x1 = (timeLimit - Math.Sqrt(d)) / 2;
            var x2 = (timeLimit + Math.Sqrt(d) / 2);
            var time = (long)Math.Ceiling(Math.Min(x1, x2));

            return (timeLimit - time) - time + 1;
        }
        private static List<Chart> ParseInput(List<string> inputLines)
        {
            var times = inputLines[0].Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(x => int.Parse(x));
            var distances = inputLines[1].Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(x => int.Parse(x));
            var raceChart = new List<Chart>();
            for (int i = 0; i < times.Count(); i++)
            {
                raceChart.Add(new Chart()
                {
                    TimeLimit = times.ElementAt(i),
                    DistanceRecord = distances.ElementAt(i)
                });
            }
            return raceChart;
        }
    }
}
