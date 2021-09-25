using System;
using System.Collections.Generic;

namespace PokerHandKata1st
{
    public class Card
    {
        private readonly Dictionary<string, int> _numberMapping = new Dictionary<string, int>()
        {
            {"2", 2}, {"3", 3}, {"4", 4}, {"5", 5}, {"6", 6}, {"7", 7}, {"8", 8}, {"9", 9}, {"10", 10}, {"J", 11},
            {"Q", 12}, {"K", 13}, {"A", 14}
        };


        public Card(string cardInput)
        {
            if (cardInput.Length > 2)
                Number = _numberMapping[cardInput.Substring(0, 2)];
            else
                Number = _numberMapping[cardInput.Substring(0, 1)];

            Type = (CardTypeEnum) Enum.Parse(typeof(CardTypeEnum), cardInput.Substring(cardInput.Length - 1, 1));
        }

        public int Number { get; set; }
        public CardTypeEnum Type { get; set; }
    }
}