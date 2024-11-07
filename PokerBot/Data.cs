namespace PokerBot
{
    public enum Suit { Hearts, Diamonds, Clubs, Spades }
    public enum Rank { Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }

    public class Card
    {
        public Suit Suit { get; private set; }
        public Rank Rank { get; private set; }

        public Card(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;
        }
    }

    public class PlayerAction
    {
        public string? PlayerId { get; set; }
        public string? Action { get; set; } // "Fold", "Call", "Raise", "Check"
        public int BetAmount { get; set; }
    }
}