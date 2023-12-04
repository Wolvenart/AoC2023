using AoCHelper;

namespace Day_03
{
    public static class Main
    {
        private class Node
        {
            public int SetID { get; set; } = 0;
            public string SetValue { get; set; } = "";
            public char Value { get; set; } = ' ';
            public bool IsValid { get; set; } = false;
        }
        public static int Part1(bool test)
        {
            var map = MapInput(test ? Properties.Resources.TestInput : Properties.Resources.RealInput);

            List<int> validValues = new List<int>();

            for (int y = 0; y < map.GetLength(0); y++)
            {
                List<Node> setNodes = new List<Node>();

                for (int x = 0; x < map.GetLength(1); x++)
                {
                    var curNode = map[y, x];
                    if (char.IsDigit(curNode.Value))
                    {
                        foreach (var direction in DirectionHelper.Modifiers_Diagonal)
                        {
                            var checkY = y + direction.Y;
                            var checkX = x + direction.X;

                            if (checkY < 0 || checkY >= map.GetLength(0)) continue;
                            if (checkX < 0 || checkX >= map.GetLength(1)) continue;

                            var checkVal = map[checkY, checkX];

                            if (!char.IsDigit(checkVal.Value) && checkVal.Value != '.')
                            {
                                curNode.IsValid = true;
                                break;
                            }
                        }

                        setNodes.Add(curNode);
                    }
                    else
                    {
                        if (setNodes.Count > 0 && setNodes.Any(x => x.IsValid))
                            validValues.Add(Convert.ToInt32(setNodes.Select(x => x.Value).Aggregate("", (x, y) => $"{x}{y}")));

                        setNodes.Clear();
                    }
                }
                if (setNodes.Count > 0 && setNodes.Any(x => x.IsValid))
                    validValues.Add(Convert.ToInt32(setNodes.Select(x => x.Value).Aggregate("", (x, y) => $"{x}{y}")));

                setNodes.Clear();
            }
            return validValues.Sum();
        }
        public static double Part2(bool test)
        {
            var map = MapInput((test ? Properties.Resources.TestInput : Properties.Resources.RealInput));

            var id = 0;

            for (int y = 0; y < map.GetLength(0); y++)
            {
                var setPoints = new List<System.Drawing.Point>();

                for (int x = 0; x < map.GetLength(1); x++)
                {
                    var curNode = map[y, x];
                    if (char.IsDigit(curNode.Value))
                        setPoints.Add(new System.Drawing.Point() { X = x, Y = y });
                    else
                    {
                        if (setPoints.Count > 0)
                        {
                            id++;

                            foreach (var point in setPoints)
                            {
                                map[point.Y, point.X].SetID = id;
                                map[point.Y, point.X].SetValue = setPoints.Select(x => map[x.Y, x.X].Value).Aggregate("", (x, y) => $"{x}{y}");
                            }
                        }

                        setPoints.Clear();
                    }
                }

                if (setPoints.Count > 0)
                {
                    id++;

                    foreach (var point in setPoints)
                    {
                        map[point.Y, point.X].SetID = id;
                        map[point.Y, point.X].SetValue = setPoints.Select(x => map[x.Y, x.X].Value).Aggregate("", (x, y) => $"{x}{y}");
                    }
                }

                setPoints.Clear();
            }

            var lstGearRatio = new List<double>();

            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    var curNode = map[y, x];
                    if (curNode.Value == '*')
                    {
                        List<Node> setNodes = new List<Node>();

                        foreach (var direction in DirectionHelper.Modifiers_Diagonal)
                        {
                            var checkY = y + direction.Y;
                            var checkX = x + direction.X;
                            if (checkY < 0 || checkY >= map.GetLength(0)) continue;
                            if (checkX < 0 || checkX >= map.GetLength(1)) continue;

                            var checkNode = map[checkY, checkX];

                            if (!String.IsNullOrWhiteSpace(checkNode.SetValue) && setNodes.FirstOrDefault(x => x.SetID == checkNode.SetID) == null)
                                setNodes.Add(checkNode);
                        }
                        if (setNodes.Count == 2)
                            lstGearRatio.Add(setNodes.Select(x => Convert.ToDouble(x.SetValue)).Aggregate(1.0, (x, y) => x * y));
                    }
                }
            }

            return lstGearRatio.Sum();
        }
        private static Node[,] MapInput(string rawInput)
        {
            var inputLines = rawInput.Split("\r\n");

            var map = new Node[inputLines.First().Length, inputLines.Length];

            for (int y = 0; y < inputLines.Length; y++)
            {
                for (int x = 0; x < inputLines[y].Length; x++)
                    map[y, x] = new Node() { Value = inputLines[y][x] };
            }
            return map;
        }
    }
}