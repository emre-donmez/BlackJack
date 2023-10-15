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

        public IActionResult StartGame()
        {
            gameViewModel = new GameViewModel
            {
                Gamer = new Gamer { Hand = new List<Card>() },
                Dealer = new Dealer { Hand = new List<Card>() },
                Deck = GetNewDeck(),
                GameStarted = true
            };

            SetGameViewModel(gameViewModel);

            ViewBag.gameViewModel = gameViewModel;

            return View("Index");
        }

        public IActionResult Index()
		{
			return View();
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

        public IActionResult Hit(PlayerType target)
        {
            var gameViewModelFromSession = GetGameViewModel();

            Card drawnCard = gameViewModelFromSession.Deck[0];

            if (target == PlayerType.Gamer)
            {
                gameViewModelFromSession.Gamer.Hand.Add(drawnCard);
            }
            else if (target == PlayerType.Dealer)
            {
                gameViewModelFromSession.Dealer.Hand.Add(drawnCard);
            }

            gameViewModelFromSession.Deck.RemoveAt(0);

            SetGameViewModel(gameViewModelFromSession);

            ViewBag.gameViewModel = gameViewModelFromSession;

            return View("Index");

        }

        public IActionResult DealCards()
        {        
            for (int i = 0; i < 2; i++)
            {
				Hit(PlayerType.Gamer);
				Hit(PlayerType.Dealer);
            }

            var gameViewModelFromSession = GetGameViewModel();
            
            ViewBag.gameViewModel = gameViewModelFromSession;

            return View("Index");
        }
        private GameViewModel GetGameViewModel()
        {
            var gameViewModelJson = HttpContext.Session.GetString("GameViewModel");
            var gameViewModelFromSession = JsonConvert.DeserializeObject<GameViewModel>(gameViewModelJson);

            return gameViewModelFromSession;
        }
        private void SetGameViewModel(GameViewModel gameViewModelFromSession)
        {
            var gameViewModelNewJson = JsonConvert.SerializeObject(gameViewModelFromSession);
            HttpContext.Session.SetString("GameViewModel", gameViewModelNewJson);
        }
        public IActionResult DetermineWinner()
        {
            var gameViewModelFromSession = GetGameViewModel();

            while (gameViewModelFromSession.Dealer.CalculateHandValue()<17)
            {
				Hit(PlayerType.Dealer);
                gameViewModelFromSession = GetGameViewModel();
            }

            if (gameViewModelFromSession.Gamer.CalculateHandValue() == 21)
            {
                ViewBag.message = "You won with blackjack";
            }
            else if (gameViewModelFromSession.Dealer.CalculateHandValue() == 21)
            {
                ViewBag.message = "Dealer won with blackjack";
            }
            else if (gameViewModelFromSession.Gamer.CalculateHandValue() > 21)
            {
                ViewBag.message = "You busted";
            }
            else if (gameViewModelFromSession.Dealer.CalculateHandValue() > 21)
            {
                ViewBag.message = "Dealer busted";
            }
            else if (gameViewModelFromSession.Dealer.CalculateHandValue() == gameViewModelFromSession.Gamer.CalculateHandValue())
            {
                ViewBag.message = "Draw";
            }
            else if (gameViewModelFromSession.Dealer.CalculateHandValue() > gameViewModelFromSession.Gamer.CalculateHandValue())
            {
                ViewBag.message = "Dealer won";
            }
            else if (gameViewModelFromSession.Dealer.CalculateHandValue() < gameViewModelFromSession.Gamer.CalculateHandValue())
            {
                ViewBag.message = "You won";
            }


            gameViewModelFromSession.Gamer.Hand.Clear();
            gameViewModelFromSession.Dealer.Hand.Clear();

            SetGameViewModel(gameViewModelFromSession);

            ViewBag.gameViewModel = gameViewModelFromSession;

            return View("Index");
        }
    }
}
