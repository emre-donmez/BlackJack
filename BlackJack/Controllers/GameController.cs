using BlackJack.Enums;
using BlackJack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Numerics;

namespace BlackJack.Controllers
{
	public class GameController : Controller
	{
		private GameViewModel gameViewModel;

        public GameController()
        {
           gameViewModel = new GameViewModel();
        }
        public IActionResult StartGame()
        {

            gameViewModel.Gamer = new Gamer();
            gameViewModel.Dealer = new Dealer();
            gameViewModel.Deck = GetNewDeck();
            gameViewModel.GameStarted = true;

            return View("Index", gameViewModel);
        }

        public IActionResult Index()
		{
			return View(gameViewModel);
		}

		public List<Card> GetNewDeck()
		{
			List<Card> deck = new List<Card>();

			String[] types = { "Hearts", "Diamonds", "Clubs", "Spades" };
			String[] values = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

			foreach (var type in types)
			{
				foreach (var value in values)
				{
					deck.Add(new Card(type, value));
				}
			}

			return Shuffle(deck);
		}

		public List<Card> Shuffle(List<Card> cards)
		{
			Random random = new Random();

			return cards.OrderBy(x=> random.Next()).ToList();
		}

        public void Hit(PlayerType target)
        {
            Card drawnCard = gameViewModel.Deck[0];

            if (target == PlayerType.Gamer)
            {
                gameViewModel.Gamer.Hand.Add(drawnCard);
            }
            else if (target == PlayerType.Dealer)
            {
                gameViewModel.Dealer.Hand.Add(drawnCard);
            }

            gameViewModel.Deck.RemoveAt(0);
        }

        public IActionResult DealCards()
		{
            for (int i = 0; i < 2; i++)
            {
				Hit(PlayerType.Gamer);
				Hit(PlayerType.Dealer);
            }
            return View("Index", gameViewModel);

        }

        private IActionResult DetermineWinner()
        {
            return View();
        }
    }
}
