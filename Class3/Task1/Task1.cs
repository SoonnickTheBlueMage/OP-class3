// Колода

using Console = System.Console;
using Deck = System.Collections.Generic.List<Card>;
// Набор карт у игрока
using Hand = System.Collections.Generic.List<Card>;
// Набор карт, выложенных на стол
using Table = System.Collections.Generic.List<Card>;

// Масть
internal enum Suit : ushort
{
    Hearts = 4,
    Diamonds = 3,
    Spades = 2,
    Clubs = 1
}

// Значение
internal enum Rank : ushort
{
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13,
    Ace = 14
}

// Карта
record Card(Suit CardSuit, Rank CardRank);

// Тип для обозначения игрока (первый, второй)
internal enum Player
{
    First,
    Second
}

namespace Task1
{
    public class Task1
    {

 /*
 * Реализуйте игру "Пьяница" (в простейшем варианте, на колоде в 36 карт)
 * https://ru.wikipedia.org/wiki/%D0%9F%D1%8C%D1%8F%D0%BD%D0%B8%D1%86%D0%B0_(%D0%BA%D0%B0%D1%80%D1%82%D0%BE%D1%87%D0%BD%D0%B0%D1%8F_%D0%B8%D0%B3%D1%80%D0%B0)
 * Рука — это набор карт игрока. Карты выкладываются на стол из начала "рук" и сравниваются
 * только по значениям (масть игнорируется). При равных значениях сравниваются следующие карты.
 * Набор карт со стола перекладывается в конец руки победителя. Шестерка туза не бьёт.
 *
 * Реализация должна сопровождаться тестами.
 */

        // Размер колоды
        internal const int DeckSize = 36;

        // Возвращается null, если значения карт совпадают
        internal static Player? RoundWinner(Card card1, Card card2)
        {
            if (card1.CardRank > card2.CardRank)
            {
                return Player.First;
            }

            if (card2.CardRank > card1.CardRank)
            {
                return Player.Second;
            }

            return null;
        }
        
// Возвращает полную колоду (36 карт) в фиксированном порядке
        internal static Deck FullDeck()
        {
            Deck deck = new Deck();

            foreach (Suit iSuit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank jRank in Enum.GetValues(typeof(Rank)))
                {
                    deck.Add(new Card(iSuit, jRank));
                }
            }

            return deck;
        }

// Раздача карт: случайное перемешивание (shuffle) и деление колоды пополам
        internal static Dictionary<Player, Hand> Deal(Deck deck)
        {
            Deck shuffledDeck = deck;
            Random random = new Random();

            for (int i = 0; i < DeckSize; i++)
            {
                int swapWithInd = random.Next(DeckSize - 1);
                (shuffledDeck[i], shuffledDeck[swapWithInd]) = (shuffledDeck[swapWithInd], shuffledDeck[i]);
            }

            Dictionary<Player, Hand> gameSet = new Dictionary<Player, Deck>
            {
                { Player.First, shuffledDeck.GetRange(0, 18) },
                { Player.Second, shuffledDeck.GetRange(18, 18) }
            };

            return gameSet;
        }

// Один раунд игры (в том числе спор при равных картах).
// Возвращается победитель раунда и набор карт, выложенных на стол.
        internal static Tuple<Player, Table> Round(Dictionary<Player, Hand> hands)
        {
            Hand firstPlayersHand = hands[Player.First];
            Hand secondPlayersHand = hands[Player.Second];
            Player? roundWinner = null;
            Table table = new Table();

            do
            {
                roundWinner = RoundWinner(firstPlayersHand.First(), secondPlayersHand.First());
                table.Add(firstPlayersHand.First());
                firstPlayersHand.RemoveAt(0);
                table.Add(secondPlayersHand.First());
                secondPlayersHand.RemoveAt(0);

            } while (firstPlayersHand.Count > 0 && secondPlayersHand.Count > 0 && roundWinner == null);

            var winner = roundWinner.GetValueOrDefault(table[^2].CardSuit > table[^1].CardSuit ? Player.First : Player.Second);

            Tuple<Player, Table> result = new Tuple<Player, Table>(winner, table);

            return result;
        }

// Полный цикл игры (возвращается победивший игрок)
// в процессе игры печатаются ходы
        internal static Player Game(Dictionary<Player, Hand> hands)
        {
            Hand firstPayersHand = hands[Player.First];
            Hand secondPayersHand = hands[Player.Second];
            Tuple<Player, Table> round = new Tuple<Player, Deck>(Player.First, new Table());
            int roundsCounter = 1;

            while (firstPayersHand.Any() && secondPayersHand.Any() && roundsCounter <= 100)
            {
                Console.WriteLine($"Round {roundsCounter++}");

                var current = new Dictionary<Player, Hand>
                {
                    { Player.First, firstPayersHand }, { Player.Second, secondPayersHand }
                };
                round = Round(current);

                int pairChecker = 1;
                foreach (var card in round.Item2)
                {
                    Console.Write($"{card.CardRank} of {card.CardSuit} ");
                    
                    if (firstPayersHand.Contains(card)) firstPayersHand.Remove(card);
                    if (secondPayersHand.Contains(card)) secondPayersHand.Remove(card);

                    if (pairChecker == 1)
                    {
                        Console.Write("VS ");
                        pairChecker++;
                    }
                    else
                    {
                        Console.WriteLine();
                        pairChecker--;
                    }
                }
                
                Console.WriteLine($"{round.Item1} wins the round\n");

                if (round.Item1 == Player.First)
                    foreach (var card in round.Item2)
                        firstPayersHand.Add(card);

                else
                    foreach (var card in round.Item2)
                        secondPayersHand.Add(card);
            }

            return round.Item1;
        }

        public static void Main(string[] args)
        {
            var deck = FullDeck();
            var hands = Deal(deck);
            var winner = Game(hands);
            Console.WriteLine($"Победитель: {winner}");
        }
    }
}