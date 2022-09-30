using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task1.Task1;

namespace Task1;

public class Tests
{
    [Test]
    public void RoundWinnerTest()
    {
        That(Equals(RoundWinner(new Card(Suit.Diamonds, Rank.Six), new Card(Suit.Hearts, Rank.Queen)), Player.Second));
        That(Equals(RoundWinner(new Card(Suit.Spades, Rank.Ten), new Card(Suit.Clubs, Rank.Seven)), Player.First));
        That(Equals(RoundWinner(new Card(Suit.Hearts, Rank.Ace), new Card(Suit.Clubs, Rank.Ace)), null));
    }

    [Test]
    public void FullDeckTest()
    {
        var deck = FullDeck();
        That(deck, Has.Count.EqualTo(DeckSize));
        
        foreach (Rank rank in Enum.GetValues(typeof(Rank)))
        {
            That(deck.FindAll(card => card.CardRank == rank), Has.Count.EqualTo(4));
        }
        
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            That(deck.FindAll(card => card.CardSuit == suit), Has.Count.EqualTo(9));
        }
    }

    [Test]
    public void RoundTest()
    {
        Dictionary<Player, List<Card>> hands = new Dictionary<Player, List<Card>>
        {
            { Player.First, new List<Card> { new Card(Suit.Clubs, Rank.Ace), new Card(Suit.Diamonds, Rank.Eight) } },
            { Player.Second, new List<Card> { new Card(Suit.Hearts, Rank.Ace), new Card(Suit.Spades, Rank.Ten) } }
        };
        
        Tuple<Player, List<Card>> result = new Tuple<Player, List<Card>>(Player.Second, new List<Card>
        {
            new Card(Suit.Clubs, Rank.Ace),new Card(Suit.Hearts, Rank.Ace),
            new Card(Suit.Diamonds, Rank.Eight), new Card(Suit.Spades, Rank.Ten),
        });

        var test = Round(hands);
        That(test is { Item1: Player.Second });
        That(test != null && test.Item2.Count() == 4);
        for (int i = 0; i < 4; i++)
        {
            That(test != null && test.Item2[i] == result.Item2[i]);
        }
    }

    [Test]
    public void Game2CardsTest()
    {
        var six = new Card(Suit.Diamonds,Rank.Six);
        var ace = new Card(Suit.Hearts, Rank.Ace);
        Dictionary<Player, List<Card>> hands = new Dictionary<Player, List<Card>>
        {
            { Player.First, new List<Card> {six} },
            { Player.Second, new List<Card> {ace} }
        };
        var gameWinner = Game(hands);
        That(gameWinner, Is.EqualTo(Player.Second));
    }

    [Test]
    public void FastSecondWinTest()
    {
        var firstHand = new List<Card>();
        var secondHand = new List<Card>();

        var i = true;
        foreach (Rank rank in Enum.GetValues(typeof(Rank)))
        {
            if (i)
            {
                i = false;
                firstHand.Add(new Card(Suit.Hearts, rank));
            }
            else
            {
                i = true;
                secondHand.Add(new Card(Suit.Clubs, rank));
            }
        }

        firstHand.Remove(new Card(Suit.Hearts, Rank.Ace));
        
        Dictionary<Player, List<Card>> hands = new Dictionary<Player, List<Card>>
        {
            { Player.First, firstHand },
            { Player.Second, secondHand }
        };
        
        /*
         * first has:   6 8 10 Q
         * second nas:  7 9 J  K
         */
        
        var gameWinner = Game(hands);
        That(gameWinner, Is.EqualTo(Player.Second));
    }
}