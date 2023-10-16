using BlackJack.Enums;
using BlackJack.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlackJack.Controllers
{
	public class GameController : Controller
	{
        private GameViewModel GetGameViewModel()
        {
            var gameViewModelJson = HttpContext.Session.GetString("GameViewModel");         
            return JsonConvert.DeserializeObject<GameViewModel>(gameViewModelJson);
        }
        private void SetGameViewModel(GameViewModel gameViewModelFromSession)
        {
            var gameViewModelNewJson = JsonConvert.SerializeObject(gameViewModelFromSession);
            HttpContext.Session.SetString("GameViewModel", gameViewModelNewJson);
        }

        [HttpPost]
        public IActionResult StartGame(int balance)
        {
            var gameViewModel = new GameViewModel
            {
                Gamer = new Gamer { Hand = new List<Card>() , Balance = balance},
                Dealer = new Dealer { Hand = new List<Card>() },
                Deck = GetNewDeck(),
                GameStarted = true,
                EndGame = true
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
			String[] names = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

			foreach (var type in types)
			{
				foreach (var name in names)
				{
					deck.Add(new Card(type, name));
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

            if (gameViewModelFromSession.Deck.Count == 0)
                gameViewModelFromSession.Deck = GetNewDeck();

            Card drawnCard = gameViewModelFromSession.Deck[0];

            if (target == PlayerType.Gamer)
                gameViewModelFromSession.Gamer.Hand.Add(drawnCard);
            else if (target == PlayerType.Dealer)
                gameViewModelFromSession.Dealer.Hand.Add(drawnCard);

            gameViewModelFromSession.Deck.RemoveAt(0);

            SetGameViewModel(gameViewModelFromSession);

            ViewBag.gameViewModel = gameViewModelFromSession;

            if (gameViewModelFromSession.Gamer.CalculateHandValue()>21)
               return IsBusted();

            return View("Index");
        }
     
        public IActionResult IsBusted()
        {
            var gameViewModelFromSession = GetGameViewModel();

            ViewBag.message = "You busted";

            gameViewModelFromSession.EndGame = true;

            SetGameViewModel(gameViewModelFromSession);

            ViewBag.gameViewModel = gameViewModelFromSession;

            return View("Index");
        }

        public IActionResult DealCards()
        {
            var gameViewModelFromSession = GetGameViewModel();

            gameViewModelFromSession.Gamer.Hand.Clear();
            gameViewModelFromSession.Dealer.Hand.Clear();
            gameViewModelFromSession.EndGame = false;

            SetGameViewModel(gameViewModelFromSession);

            for (int i = 0; i < 2; i++)
            {
				Hit(PlayerType.Gamer);
				Hit(PlayerType.Dealer);
            }

            gameViewModelFromSession = GetGameViewModel();
            
            ViewBag.gameViewModel = gameViewModelFromSession;

            return View("Index");
        }

        public IActionResult Bet(int bet)
        {
            var gameViewModelFromSession = GetGameViewModel();

            if (bet > gameViewModelFromSession.Gamer.Balance)
            {
                ViewBag.gameViewModel = gameViewModelFromSession;
                ViewBag.message = "You have not enough money";
                return View("Index");
            }
            else if(bet < 0)
            {
                ViewBag.gameViewModel = gameViewModelFromSession;
                ViewBag.message = "Bet cannot negative numbers";
                return View("Index");
            }
            gameViewModelFromSession.Bet = bet;
            gameViewModelFromSession.Gamer.Balance -= bet;

            SetGameViewModel(gameViewModelFromSession);

            return DealCards();
        }
        public IActionResult Double()
        {
            var gameViewModelFromSession = GetGameViewModel();
            if (gameViewModelFromSession.Bet > gameViewModelFromSession.Gamer.Balance)
            {
                ViewBag.gameViewModel = gameViewModelFromSession;
                ViewBag.message = "You have not enough money";
                return View("Index");
            }
            gameViewModelFromSession.Gamer.Balance -= gameViewModelFromSession.Bet;
            SetGameViewModel(gameViewModelFromSession);
            Hit(PlayerType.Gamer);
            return DetermineWinner();
        }
        
        public IActionResult DetermineWinner()
        {
            var gameViewModelFromSession = GetGameViewModel();

            while (gameViewModelFromSession.Dealer.CalculateHandValue()<17)
            {
				Hit(PlayerType.Dealer);
                gameViewModelFromSession = GetGameViewModel();
            }

            var dealerHandValue = gameViewModelFromSession.Dealer.CalculateHandValue();
            var gamerHandValue = gameViewModelFromSession.Gamer.CalculateHandValue();

            if (dealerHandValue == gamerHandValue && dealerHandValue<=21) 
            {
                gameViewModelFromSession.Gamer.Balance += gameViewModelFromSession.Bet;
                ViewBag.message = "Draw";
            }                
            else if (gamerHandValue ==21 && gameViewModelFromSession.Gamer.Hand.Count ==2) 
            {
                gameViewModelFromSession.Gamer.Balance += gameViewModelFromSession.Bet * 5/2;
                ViewBag.message = "You won with black jack";
            }              
            else if (dealerHandValue > 21 || gamerHandValue > dealerHandValue)
            {
                ViewBag.message = "You won";
                gameViewModelFromSession.Gamer.Balance += gameViewModelFromSession.Bet * 2;
            }
            else
                ViewBag.message = "You lose";
           
            gameViewModelFromSession.EndGame = true;
            
            SetGameViewModel(gameViewModelFromSession);

            ViewBag.gameViewModel = gameViewModelFromSession;

            return View("Index");
        }
    }
}
