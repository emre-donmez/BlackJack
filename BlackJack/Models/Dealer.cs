namespace BlackJack.Models
{
	public class Dealer
	{
		public List<Card>? Hand { get; set; }

		public int CalculateHandValue()
		{
			int handValue = 0;
			int aceCount = 0;

			foreach (var card in Hand)
			{
				handValue += card.Value;

				if (card.Name == "A")
				{
					aceCount++;
				}
			}

			if (handValue > 21)
			{
				while (aceCount > 0)
				{
					handValue -= 10;
					aceCount--;

					if (handValue <= 21)
						break;
				}
			}
			return handValue;
		}
	}
}
