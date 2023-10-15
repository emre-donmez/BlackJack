using System.ComponentModel;
using System.Numerics;

namespace BlackJack.Models
{
	public class Card
	{
		public Card(string type, string name)
		{
			Type = type;
			Name = name;
		}

		public string Type { get; set; }
        public string Name { get; set; }

		public int Value
		{
			get
			{
				if (Name.Equals("K") || Name.Equals("Q") || Name.Equals("J"))
                {
					return 10;
                }
				else if (Name.Equals("A"))
				{
					return 11;
				}
                return Int32.Parse(Name);
			}
		}

        public override string? ToString()
		{
			return Name + " of " + Type;
		}
	}
}
