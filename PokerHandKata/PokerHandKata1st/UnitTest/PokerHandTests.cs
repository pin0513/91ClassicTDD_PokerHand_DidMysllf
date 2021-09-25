using NUnit.Framework;

namespace PokerHandKata1st.UnitTest
{
    public class PokerHandTests
    {
        [SetUp]
        public void Setup()
        {
        }

        //SuggestedTestCases
        //Black: 2H 3D 5S 9C KD  White: 2C 3H 4S 8C AH
        //Black: 2H 4S 4C 2D 4H  White: 2S 8S AS QS 3S
        //Black: 2H 3D 5S 9C KD  White: 2C 3H 4S 8C KH
        //Black: 2H 3D 5S 9C KD  White: 2D 3H 5C 9S KH

        //Sample output
        //White wins. - with high card: Ace
        //Black wins. - with full house: 4 over 2
        //Black wins. - with high card: 9
        //Tie.

        //Straight Flush	rank by highest cards 
        //Four of a Kind	rank by value of the 4 cards
        //Full house rank by value of the 3 cards
        //Flush rank by high card
        //Straight rank by highest card
        //Three of a kind  rank by highest value
        //Two Pairs rank by highest pairs value , but if all same rank by remainging
        //One Pair rank by pair value , if same then rank by remaining card with high card rule
        //High card highest cards have the same value , then rank by next highest card and so on 

        //同花順> 四條> 葫蘆> 同花> 順子> 三條> 兩對> 一對> 散牌

        //A> K> Q> J> 10> 9> 8> 7> 6> 5> 4> 3> 2

        //♠> ♥> ♣> ♦


        [Test]
        public void test01_compare_two_deck_win_with_high_cards()
        {
            var cardKata = new PokerHand("Black: 2H 3D 5S 9C KD  White: 2C 3H 4S 8C AH");

            var result = cardKata.Compare();

            Assert.AreEqual("White wins. - with high card: Ace", result);
        }

        [Test]
        public void test02_compare_two_deck_tied()
        {
            var cardKata = new PokerHand("Black: 2H 3D 5S 9C KD  White: 2D 3H 5C 9S KH");

            var result = cardKata.Compare();

            Assert.AreEqual("Tied.", result);
        }
        
        
        [Test]
        public void test03_compare_two_deck_fullhouse_with_high_group_value()
        {
            var cardKata = new PokerHand("Black: 2H 4S 4C 2D 4H  White: 2S 8S AS QS 3S");

            var result = cardKata.Compare();

            Assert.AreEqual("Black wins. - with full house: 4 over 2", result);
        }     
        
        [Test]
        public void test04_compare_two_deck_with_second_high_card_compare()
        {
            var cardKata = new PokerHand("Black: 2H 3D 5S 9C KD  White: 2C 3H 4S 8C KH");

            var result = cardKata.Compare();

            Assert.AreEqual("Black wins. - with high card: 9", result);
        }
        
        [Test]
        public void test05_compare_two_deck_with_two_straight_flush()
        {
            var cardKata = new PokerHand("Black: 2S 3S 4S 5S 6S  White: 3D 4D 5D 6D 7D");

            var result = cardKata.Compare();

            Assert.AreEqual("White wins. - with straight flush: from 3 to 7", result);
        }
                
        [Test]
        public void test06_compare_two_deck_with_two_full_house()
        {
            var cardKata = new PokerHand("Black: 2H 2D 2S 3C 3D  White: 4C 4H 4S 5C 5H");

            var result = cardKata.Compare();

            Assert.AreEqual("White wins. - with full house: 4 over 5", result);
        }

        [Test]
        public void test07_compare_two_deck_with_two_pair_case1()
        {
            var cardKata = new PokerHand("Black: 7H 7D 3S 3C 4D  White: 2C 2S 4S 4C 5H");

            var result = cardKata.Compare();

            Assert.AreEqual("Black wins. - with two pairs: 7, 3 pairs", result);
        }
        
        [Test]
        public void test08_compare_two_deck_with_two_pair_case2()
        {
            var cardKata = new PokerHand("Black: 2H 2D 3S 3C 4D  White: 2C 2S 4S 4C 5H");

            var result = cardKata.Compare();

            Assert.AreEqual("White wins. - with two pairs: 4, 2 pairs", result);
        }
        
        [Test]
        public void test09_compare_two_deck_with_one_pair_case1()
        {
            var cardKata = new PokerHand("Black: AH AD 8S 3C 4D  White: 2C 2S 3S 4C KH");

            var result = cardKata.Compare();

            Assert.AreEqual("Black wins. - with one pair: one pair of Ace", result);
        }
        
        [Test]
        public void test10_compare_two_deck_with_one_pair_case2()
        {
            var cardKata = new PokerHand("Black: 2H 2D 8S 3C 4D  White: 2C 2S 3S 4C KH");

            var result = cardKata.Compare();

            Assert.AreEqual("White wins. - with one pair: one pair of 2", result);
        }
    }

    //Straight Flush	
    //Four of a Kind	
    //Full house	
    //Flush
    //Straight
    //Three of a kind
    //Two Pairs
    //One Pair
    //High card
}