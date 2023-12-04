using AoCHelper;

namespace Day_04
{
    public static class Main
    {
        private class ScratchCard
        {
            public int CardNr { get; set; } = 0;

            public int CopyCount { get; set; } = 1;

            public List<int> Hand { get; set; } = new List<int>();
            public List<int> WinningNumbers { get; set; } = new List<int>();

            public IEnumerable<int> Result { get => this.Hand.Intersect(this.WinningNumbers); }
            public double CardScore { get => WinCount == 0 ? 0 : Math.Pow(2, WinCount - 1); }
            public int WinCount { get => Result.Count(); }
        }
        
        public static double Part1(bool test)
        {
            var cards = ParseInput(test ? Properties.Resources.TestInput : Properties.Resources.RealInput);

            return cards.Sum(x => x.CardScore);
        }
        public static int Part2(bool test)
        {
            var cards = ParseInput(test ? Properties.Resources.TestInput : Properties.Resources.RealInput);

            for (int i = 0; i < cards.Count; i++)
            {
                var curCard = cards[i];
                for (int x = 1; x <= curCard.WinCount; x++)
                {
                    var nextIdx = i + x;
                    if (nextIdx >= cards.Count) break;

                    cards[nextIdx].CopyCount += curCard.CopyCount;
                }
            }

            return cards.Sum(x => x.CopyCount);
        }
        private static List<ScratchCard> ParseInput(string rawInput)
        {
            var lstOut = new List<ScratchCard>();

            foreach (var line in rawInput.ReadAllLines())
            {
                var card = new ScratchCard();
                var cardSplit = line.Split(':');
                card.CardNr = Convert.ToInt32(cardSplit[0].Split(' ').Last());

                var numberSplit = cardSplit[1].Split('|');

                var winningSplit = numberSplit[0].Split(' ').ToList();
                winningSplit.RemoveAll(x => String.IsNullOrWhiteSpace(x));
                card.WinningNumbers = winningSplit.Select(x => Convert.ToInt32(x)).ToList();

                var handSplit = numberSplit[1].Split(' ').ToList();
                handSplit.RemoveAll(x => String.IsNullOrWhiteSpace(x));
                card.Hand = handSplit.Select(x => Convert.ToInt32(x)).ToList();

                lstOut.Add(card);
            }

            return lstOut;
        }
    }
}
