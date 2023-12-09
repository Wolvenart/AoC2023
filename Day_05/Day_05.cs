using AoCHelper;
using System.Reflection.PortableExecutable;

namespace Day_05
{
    public static class Main
    {
        private class Base
        {
            public long StartVal { get; set; } = 0;
            public long EndVal { get; set; } = 0;
        }
        private class Map : Base
        {
            public long Offset { get; set; } = 0;
        }
        
        public static long Part1(bool test)
        {
            var input = (test ? Properties.Resources.TestInput : Properties.Resources.RealInput).ReadAllLines();

            var seeds = input.First().Split(':')[1].Split(' ')
                .Where(x => Int64.TryParse(x, out _)).Select(x => Convert.ToInt64(x)).ToList();

            var mapHeaders = input.Where(x => x.Contains("map:"));

            var maps = new List<long[]>();
            foreach (var seed in seeds)
            {
                var map = new long[mapHeaders.Count() + 1];
                var idx = 0;
                map[idx++] = seed;

                foreach (var header in mapHeaders)
                {
                    foreach (var valMap in input.SkipWhile(x => x != header).Skip(1))
                    {
                        if (String.IsNullOrWhiteSpace(valMap)) continue;
                        if (mapHeaders.Contains(valMap)) break;

                        var valSplit = valMap.Split(' ');
                        var sourceStart = Convert.ToInt64(valSplit[1]);
                        var range = Convert.ToInt64(valSplit[2]);
                        if (map[idx - 1] >= sourceStart && map[idx - 1] <= (sourceStart + range))
                        {
                            var destStart = Convert.ToInt64(valSplit[0]);
                            var destVal = (map[idx - 1] - sourceStart) + destStart;
                            map[idx] = destVal;
                            break;
                        }
                    }
                    if (map[idx] == 0) map[idx] = map[idx - 1];
                    idx++;
                }

                maps.Add(map);
            }

            return maps.Select(x => x[7]).Min();
        }
        public static long Part2(bool test)
        {
            var input = (test ? Properties.Resources.TestInput : Properties.Resources.RealInput).ReadAllLines();
            var seeds = input.First().Split(':')[1].Split(' ').Where(x => Int64.TryParse(x, out _)).Select(x => Convert.ToInt64(x));
            var mapHeaders = input.Where(x => x.Contains("map:"));

            var seedBag = new Queue<Base[]>();

            for (int i = 0; i < seeds.Count(); i += 2)
            {
                var start = seeds.ElementAt(i);
                var range = seeds.ElementAt(i + 1);
                var curSeed = new Base[mapHeaders.Count() + 1];
                curSeed[0] = new Base() { StartVal = start, EndVal = start + (range - 1) };
                seedBag.Enqueue(curSeed);
            }

            var lstMap = new List<Map>[mapHeaders.Count()];
            for (int i = 0; i < mapHeaders.Count(); i++)
            {
                var header = mapHeaders.ElementAt(i);
                var curMap = new List<Map>();
                foreach (var mapLine in input.SkipWhile(x => !x.Contains(header)).Skip(1))
                {
                    if (String.IsNullOrWhiteSpace(mapLine)) continue;
                    if (mapHeaders.Contains(mapLine)) break;
                    var valSplit = mapLine.Split(" ");
                    var destStart = Convert.ToInt64(valSplit[0]);
                    var sourceStart = Convert.ToInt64(valSplit[1]);
                    var range = Convert.ToInt64(valSplit[2]);
                    var offset = destStart - sourceStart;
                    curMap.Add(new Map() { StartVal = sourceStart, EndVal = sourceStart + (range - 1), Offset = offset });
                }

                var orderMap = curMap.OrderBy(x => x.StartVal);
                if (orderMap.First().StartVal != 0)
                    curMap.Add(new Map() { StartVal = 0, EndVal = orderMap.First().StartVal - 1, Offset = 0 });
                for (int temp = 1; temp < orderMap.Count(); temp++)
                {
                    var curStart = orderMap.ElementAt(temp).StartVal;
                    var prevEnd = orderMap.ElementAt(temp - 1).EndVal;
                    if (Math.Abs(curStart - prevEnd) > 1)
                        curMap.Add(new Map() { StartVal = prevEnd + 1, EndVal = curStart - 1, Offset = 0 });
                }
                orderMap = curMap.OrderBy(x => x.StartVal);
                if (orderMap.Last().EndVal != long.MaxValue)
                    curMap.Add(new Map() { StartVal = orderMap.Last().EndVal + 1, EndVal = long.MaxValue, Offset = 0 });

                lstMap[i] = curMap.OrderBy(x => x.StartVal).ToList();
            }

            var lstResult = new List<Base[]>();
            while (seedBag.Count > 0)
            {
                var curPath = seedBag.Dequeue();
                var idx = 0;

                for (int i = 0; i < lstMap.Count(); i++)
                {
                    var curSeed = curPath[idx];
                    if (curPath[idx + 1] != null) { idx++; continue; }
                    
                    var mapResult = GetMappingResults(curSeed, lstMap[i]);

                    curPath[idx + 1] = mapResult.First().Item2;
                    
                    if(mapResult.Count > 1)
                    {
                        for (int x = 1; x < mapResult.Count; x++)
                        {
                            var branch = new Base[curPath.Length];
                            curPath.CopyTo(branch, 0);
                            branch[idx] = mapResult[x].Item1;
                            branch[idx + 1] = mapResult[x].Item2;
                            seedBag.Enqueue(branch);
                        }
                    }
                    idx++;
                }

                lstResult.Add(curPath);
            }

            return lstResult.Min(x => x.Last().StartVal);
        }

        private static List<Tuple<Base, Base>> GetMappingResults(Base curSeed, List<Map> maps)
        {
            var lstResult = new List<Tuple<Base, Base>>();
            var checkSeed = new Base() { StartVal = curSeed.StartVal, EndVal = curSeed.EndVal };

            foreach (var map in maps)
            {
                if (map.EndVal < checkSeed.StartVal) continue;

                var end = Math.Min(checkSeed.EndVal, map.EndVal);
                lstResult.Add(new Tuple<Base, Base>(new Base() { StartVal = checkSeed.StartVal, EndVal = end },
                    new Base() { StartVal = checkSeed.StartVal + map.Offset, EndVal = end + map.Offset }));
                checkSeed.StartVal = end + 1;

                if (checkSeed.EndVal < checkSeed.StartVal) break;
            }

            return lstResult;
        }
    }
}
