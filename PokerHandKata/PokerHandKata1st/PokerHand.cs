using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PokerHandKata1st
{
    public class PokerHand
    {
        public PokerHand(string input)
        {
            if (input != null)
            {
                var splitIdx = input.IndexOf("  ", StringComparison.CurrentCultureIgnoreCase);
                var lastSplitIdx = input.LastIndexOf("  ", StringComparison.CurrentCultureIgnoreCase);
                var player1Input = input.Substring(0, splitIdx);
                var player2Input = input.Substring(lastSplitIdx + 2, input.Length - (lastSplitIdx + 2));
                firstPlayerName = player1Input.Split(':').First();
                secondPlayerName = player2Input.Split(':').First();
                var firstPlayerDeckString = player1Input.Split(':').Last();
                foreach (var cardInput in firstPlayerDeckString.Split(' '))
                {
                    if (cardInput.Trim() == string.Empty) continue;
                    firstPlayerDeck.Add(new Card(cardInput));
                }

                secondPlayerName = player2Input.Split(':').First();
                var secondPlayerDeckString = player2Input.Split(':').Last();
                foreach (var cardInput in secondPlayerDeckString.Split(' '))
                {
                    if (cardInput.Trim() == string.Empty) continue;
                    secondPlayerDeck.Add(new Card(cardInput));
                }
            }


            firstPlayerDeckCategory = CheckDeckCategory(firstPlayerDeck);
            secondPlayerDeckCategory = CheckDeckCategory(secondPlayerDeck);
        }

        private DeckCategory CheckDeckCategory(List<Card> cards)
        {
            var firstCard = cards.First();
            if (cards.All(a => a.Type == firstCard.Type))
            {
                //若照順序，就是同花順
                var straghtCheckSum = GetStraightCheckSum(cards);

                if (cards.Sum(a => a.Number) == straghtCheckSum)
                    return DeckCategory.straight_flush;
                else
                    return DeckCategory.flush; //若不照順序，就是同花
            }

            var groupNumber = cards.GroupBy(a => a.Number).ToList();
            if (groupNumber.Count() == 5)
            {
                var straghtCheckSum = GetStraightCheckSum(cards);

                if (cards.Sum(a => a.Number) == straghtCheckSum)
                    return DeckCategory.straight;
                return DeckCategory.high_card;
            }
            else
            {
                if (groupNumber.Count() == 4)
                {
                    return DeckCategory.one_pair;
                }
                else if (groupNumber.Count() == 3)
                {
                    //two pair or three of a kind

                    var groupLargestCardGroupCount = groupNumber.OrderByDescending(a => a.Count()).First().Count();
                    if (groupLargestCardGroupCount == 3)
                    {
                        // three of a kind
                        return DeckCategory.three_of_a_kind;
                    }
                    else
                    {
                        return DeckCategory.two_pairs;
                    }
                }
                else if (groupNumber.Count() == 2)
                {
                    var groupCardCount = groupNumber.OrderByDescending(a => a.Count()).First().Count();
                    if (groupCardCount == 3)
                    {
                        return DeckCategory.full_house;
                    }
                    else
                    {
                        //應該不可能
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    //應該不可能
                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// todo 爛招，確認是否為順子
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        private static int GetStraightCheckSum(List<Card> cards)
        {
            var sortCards = cards.OrderBy(a => a.Number);
            var firstCardNumber = sortCards.First().Number;
            var straghtSum = 0;
            for (int i = 0; i < sortCards.Count(); i++)
            {
                if ((firstCardNumber + i) > 13)
                    straghtSum += (firstCardNumber + i) % 13;
                else
                    straghtSum += firstCardNumber + i;
            }

            return straghtSum;
        }

        private string firstPlayerName = string.Empty;
        private string secondPlayerName = string.Empty;

        private DeckCategory firstPlayerDeckCategory = DeckCategory.high_card;
        private DeckCategory secondPlayerDeckCategory = DeckCategory.high_card;

        private List<Card> firstPlayerDeck = new List<Card>();
        private List<Card> secondPlayerDeck = new List<Card>();

        public string Compare()
        {
            if (firstPlayerDeckCategory == secondPlayerDeckCategory)
            {
                var (winner, result) = CompareHighCardWithSameCategory(firstPlayerDeck, secondPlayerDeck);

                if (winner > 0)
                    return
                        $"{(winner == 1 ? firstPlayerName : secondPlayerName)} wins. - with {(winner == 1 ? GetCategoryName(firstPlayerDeckCategory) : GetCategoryName(secondPlayerDeckCategory))}: {result}";
                return $"{result}.";
            }
            else
            {
                var (winner, result) = CompareByCategory(firstPlayerDeckCategory, secondPlayerDeckCategory,
                    firstPlayerDeck, secondPlayerDeck);
                return
                    $"{(winner == 1 ? firstPlayerName : secondPlayerName)} wins. - with {(winner == 1 ? GetCategoryName(firstPlayerDeckCategory) : GetCategoryName(secondPlayerDeckCategory))}: {result}";
            }


            // if same category 
            //compare with high card

            //else if different category
            //compare with category order
        }

        private (int winner, string result) CompareByCategory(DeckCategory first, DeckCategory second,
            List<Card> firstDeck, List<Card> secondDeck)
        {
            return (first > second ? 1 : 2,
                GetCategoryContent(first > second ? first : second, first > second ? firstDeck : secondDeck));
        }

        private string GetCategoryContent(DeckCategory deckType, List<Card> deck)
        {
            if (deckType == DeckCategory.full_house)
            {
                var groupNumber = deck.GroupBy(a => a.Number);
                var sortGroup = groupNumber.OrderByDescending(a => a.Count());
                return $"{sortGroup.First().Key} over {sortGroup.Last().Key}";
            }
            else if (new[] {DeckCategory.straight, DeckCategory.straight_flush}.Contains(deckType))
            {
                var sortDeck = deck.OrderBy(a => a.Number);
                return $"from {GetCardName(sortDeck.First().Number)} to {GetCardName(sortDeck.Last().Number)}";
            }
            else if (deckType == DeckCategory.flush)
            {
                return $"{deck.First().Type.ToString()} type flush";
            }
            else if (deckType == DeckCategory.four_of_a_kind)
            {
                var groupNumber = deck.GroupBy(a => a.Number);
                var sortGroup = groupNumber.OrderByDescending(a => a.Count());
                return $"four of {GetCardName(sortGroup.First().Key)}";
            }
            else if (deckType == DeckCategory.three_of_a_kind)
            {
                var groupNumber = deck.GroupBy(a => a.Number);
                var sortGroup = groupNumber.OrderByDescending(a => a.Count());
                return $"three of {GetCardName(sortGroup.First().Key)}";
            }
            else if (deckType == DeckCategory.two_pairs)
            {
                var groupNumber = deck.GroupBy(a => a.Number);
                var sortGroup = groupNumber.OrderByDescending(a => a.Count()).ThenByDescending(b=>b.Key).ToArray();
                return $"{GetCardName(sortGroup[0].Key)}, {sortGroup[1].Key} pairs";
            }
            else if (deckType == DeckCategory.one_pair)
            {
                var groupNumber = deck.GroupBy(a => a.Number);
                var sortGroup = groupNumber.OrderByDescending(a => a.Count());
                return $"one pair of {GetCardName(sortGroup.First().Key)}";
            }
            else
            {
                var sortDeck = deck.OrderByDescending(a => a.Number);

                return $"{GetCardName(sortDeck.First().Number)}";
            }
        }

        private (int winner, string result) CompareHighCardWithSameCategory(List<Card> cardset1, List<Card> cardset2)
        {
            var set1Group = cardset1.GroupBy(a => a.Number).OrderByDescending(b => b.Count()).OrderByDescending(c=>c.Key).ToArray();
            var set2Group = cardset2.GroupBy(a => a.Number).OrderByDescending(b => b.Count()).OrderByDescending(c=>c.Key).ToArray();

            if (firstPlayerDeckCategory == DeckCategory.full_house)
            {
                return set1Group.First().Key > set2Group.First().Key
                    ? (1, GetCategoryContent(firstPlayerDeckCategory, firstPlayerDeck))
                    : (2, GetCategoryContent(secondPlayerDeckCategory, secondPlayerDeck));
            }
            else if (firstPlayerDeckCategory == DeckCategory.four_of_a_kind)
            {
                return set1Group.First().Key > set2Group.First().Key
                    ? (1, GetCategoryContent(firstPlayerDeckCategory, firstPlayerDeck))
                    : (2, GetCategoryContent(secondPlayerDeckCategory, secondPlayerDeck));
            }
            else if (firstPlayerDeckCategory == DeckCategory.three_of_a_kind)
            {
                return set1Group.First().Key > set2Group.First().Key
                    ? (1, GetCategoryContent(firstPlayerDeckCategory, firstPlayerDeck))
                    : (2, GetCategoryContent(secondPlayerDeckCategory, secondPlayerDeck));
            }
            else if (firstPlayerDeckCategory == DeckCategory.two_pairs ||
                     firstPlayerDeckCategory == DeckCategory.one_pair)
            {
                for (int i = 0; i < set1Group.Count(); i++)
                {
                    if (set1Group[i].Key == set2Group[i].Key && set1Group[i].Count() > 1 && set2Group[i].Count() > 1)
                    {
                        continue;
                    }

                    return set1Group[i].Key > set2Group[i].Key
                        ? (1, GetCategoryContent(firstPlayerDeckCategory, firstPlayerDeck))
                        : (2, GetCategoryContent(secondPlayerDeckCategory, secondPlayerDeck));
                }
            }
            else if (new[] {DeckCategory.straight, DeckCategory.straight_flush}.Contains(firstPlayerDeckCategory))
            {
                Card[] set1 = cardset1.OrderByDescending(a => a.Number).ToArray();
                Card[] set2 = cardset2.OrderByDescending(a => a.Number).ToArray();

                for (var i = 0; i < set1.Count(); i++)
                {
                    var card1 = set1[i];
                    var card2 = set2[i];

                    if (card1.Number != card2.Number)
                        return (card1.Number > card2.Number ? 1 : 2,
                            GetCategoryContent(
                                card1.Number > card2.Number ? firstPlayerDeckCategory : secondPlayerDeckCategory,
                                card1.Number > card2.Number ? firstPlayerDeck : secondPlayerDeck));
                }

                return (0, "Tied");
            }
            else
            {
                Card[] set1 = cardset1.OrderByDescending(a => a.Number).ToArray();
                Card[] set2 = cardset2.OrderByDescending(a => a.Number).ToArray();

                for (var i = 0; i < set1.Count(); i++)
                {
                    var card1 = set1[i];
                    var card2 = set2[i];

                    if (card1.Number != card2.Number)
                        return (card1.Number > card2.Number ? 1 : 2,
                            GetCardName(card1.Number > card2.Number ? card1.Number : card2.Number));
                }
            }
            return (0, "Tied");
        }

        public string GetCardName(int cardNumber)
        {
            switch (cardNumber)
            {
                case 14:
                    return "Ace";
                case 13:
                    return "K";
                case 12:
                    return "Q";
                case 11:
                    return "J";
                default:
                    return cardNumber.ToString();
            }
        }


        private string GetCategoryName(DeckCategory playerDeckCategory)
        {
            switch (playerDeckCategory)
            {
                case DeckCategory.flush:
                    return "flush";
                case DeckCategory.straight_flush:
                    return "straight flush";
                case DeckCategory.four_of_a_kind:
                    return "four of a kind";
                case DeckCategory.full_house:
                    return "full house";
                case DeckCategory.straight:
                    return "straight";
                case DeckCategory.three_of_a_kind:
                    return "three of a kind";
                case DeckCategory.two_pairs:
                    return "two pairs";
                case DeckCategory.one_pair:
                    return "one pair";
                case DeckCategory.high_card:
                    return "high card";
                default:
                    return playerDeckCategory.ToString();
            }
        }
    }
}