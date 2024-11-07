namespace PokerBot
{
    public class PokerBot
    {
        private int potSize = 0;
        private int currentBet = 0;
        private bool isEarlyPosition = true;
        private List<PlayerAction> playerActions = new List<PlayerAction>();

        public void TrackPlayerAction(PlayerAction action)
        {
            playerActions.Add(action);

            if (action.Action == "Bet" || action.Action == "Raise")
            {
                potSize += action.BetAmount;
                currentBet = action.BetAmount;
            }
            else if (action.Action == "Call")
            {
                potSize += action.BetAmount;
            }
        }

        public string MakeDecision(List<Card> hand, List<Card> communityCards)
        {
            // Evaluate hand strength
            string handRank = HandEvaluator.EvaluateHand(hand, communityCards);
            double handStrength = CalculateHandStrength(handRank);
            double potOdds = CalculatePotOdds(currentBet, potSize);

            // Decision based on hand strength, position, and pot odds
            if (handStrength < potOdds) return "Fold"; // Fold if pot odds don't justify continuing

            if (isEarlyPosition)
            {
                return handStrength >= 0.7 ? "Raise" : "Fold"; // Raise only with strong hands early
            }

            if (handStrength >= 0.8)
            {
                return playerActions.Any(a => a.Action == "Raise") ? "Re-Raise" : "Raise";
            }

            return handStrength >= 0.5 ? "Call" : "Check";
        }

        private double CalculateHandStrength(string handRank)
        {
            return handRank switch
            {
                "Royal Flush" => 1.0,
                "Straight Flush" => 0.95,
                "Four of a Kind" => 0.9,
                "Full House" => 0.85,
                "Flush" => 0.8,
                "Straight" => 0.75,
                "Three of a Kind" => 0.6,
                "Two Pair" => 0.5,
                "Pair" => 0.4,
                _ => 0.2,
            };
        }

        private double CalculatePotOdds(int callAmount, int potSize)
        {
            return (double)callAmount / (potSize + callAmount);
        }
    }
}