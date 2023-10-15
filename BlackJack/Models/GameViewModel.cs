namespace BlackJack.Models
{
    public class GameViewModel
    {
        public Gamer Gamer { get; set; } 
        public Dealer Dealer { get; set; } 
        public List<Card> Deck { get; set; }
        public bool GameStarted { get; set; } 
    }
}
