namespace PokerBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PokerBot pokerBot = new PokerBot();

            // Example hand and community cards
            List<Card> hand = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Clubs, Rank.King)
            };

            List<Card> communityCards = new List<Card>
            {
                new Card(Suit.Hearts, Rank.Queen),
                new Card(Suit.Clubs, Rank.Ten),
                new Card(Suit.Spades, Rank.Ace)
            };

            // Example actions from other players
            pokerBot.TrackPlayerAction(new PlayerAction { PlayerId = "Player1", Action = "Check", BetAmount = 0 });
            pokerBot.TrackPlayerAction(new PlayerAction { PlayerId = "Player2", Action = "Fold", BetAmount = 0 });
            pokerBot.TrackPlayerAction(new PlayerAction { PlayerId = "Player3", Action = "Check", BetAmount = 0 });
            pokerBot.TrackPlayerAction(new PlayerAction { PlayerId = "Player4", Action = "Check", BetAmount = 0 });
            pokerBot.TrackPlayerAction(new PlayerAction { PlayerId = "Player5", Action = "Fold", BetAmount = 0 });
            pokerBot.TrackPlayerAction(new PlayerAction { PlayerId = "Player6", Action = "Check", BetAmount = 0 });

            // Get the bot's decision and print it
            string decision = pokerBot.MakeDecision(hand, communityCards);
            Console.WriteLine("Poker Bot Decision: " + decision);
        }
    }

    public class HandEvaluator
    {
        public static string EvaluateHand(List<Card> hand, List<Card> communityCards)
        {
            // Combine hand and community cards
            List<Card> allCards = new List<Card>(hand);
            allCards.AddRange(communityCards);

            // Sort cards by rank to facilitate hand evaluation
            allCards = allCards.OrderByDescending(card => card.Rank).ToList();

            // Check for hand ranks in descending order of value
            if (IsRoyalFlush(allCards)) return "Royal Flush";
            if (IsStraightFlush(allCards)) return "Straight Flush";
            if (IsFourOfAKind(allCards)) return "Four of a Kind";
            if (IsFullHouse(allCards)) return "Full House";
            if (IsFlush(allCards)) return "Flush";
            if (IsStraight(allCards)) return "Straight";
            if (IsThreeOfAKind(allCards)) return "Three of a Kind";
            if (IsTwoPair(allCards)) return "Two Pair";
            if (IsPair(allCards)) return "Pair";

            // If no other hand is made, return High Card
            return "High Card: " + allCards.First().Rank;
        }

        private static bool IsRoyalFlush(List<Card> cards)
        {
            return IsStraightFlush(cards) && cards.Any(card => card.Rank == Rank.Ace);
        }

        private static bool IsStraightFlush(List<Card> cards)
        {
            return IsFlush(cards) && IsStraight(cards);
        }

        private static bool IsFourOfAKind(List<Card> cards)
        {
            return cards.GroupBy(card => card.Rank).Any(group => group.Count() == 4);
        }

        private static bool IsFullHouse(List<Card> cards)
        {
            var groups = cards.GroupBy(card => card.Rank).ToList();
            return groups.Count(g => g.Count() == 3) == 1 && groups.Count(g => g.Count() == 2) == 1;
        }

        private static bool IsFlush(List<Card> cards)
        {
            return cards.GroupBy(card => card.Suit).Any(group => group.Count() >= 5);
        }

        private static bool IsStraight(List<Card> cards)
        {
            // Remove duplicate ranks to handle cases like A, A, K, Q, J
            var distinctRanks = cards.Select(card => card.Rank).Distinct().OrderByDescending(rank => rank).ToList();

            for (int i = 0; i <= distinctRanks.Count - 5; i++)
            {
                if (IsConsecutive(distinctRanks.Skip(i).Take(5))) return true;
            }

            // Special case: Ace can be low in A, 2, 3, 4, 5
            return distinctRanks.Take(4).SequenceEqual(new List<Rank> { Rank.Five, Rank.Four, Rank.Three, Rank.Two })
                && distinctRanks.Contains(Rank.Ace);
        }

        private static bool IsThreeOfAKind(List<Card> cards)
        {
            return cards.GroupBy(card => card.Rank).Any(group => group.Count() == 3);
        }

        private static bool IsTwoPair(List<Card> cards)
        {
            return cards.GroupBy(card => card.Rank).Count(group => group.Count() == 2) >= 2;
        }

        private static bool IsPair(List<Card> cards)
        {
            return cards.GroupBy(card => card.Rank).Any(group => group.Count() == 2);
        }

        private static bool IsConsecutive(IEnumerable<Rank> ranks)
        {
            Rank[] rankArray = ranks.ToArray();
            for (int i = 0; i < rankArray.Length - 1; i++)
            {
                if ((int)rankArray[i] - (int)rankArray[i + 1] != 1) return false;
            }
            return true;
        }
    }
}