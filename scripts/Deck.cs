using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace SnapTraining.scripts;

public static class Deck
{
    /// <summary>
    /// A random instance, used for shuffling the deck.
    /// </summary>
    private static readonly Random Rng = new Random();
    
    /// <summary>
    /// Checks if a deck has been killed by a royal.
    /// </summary>
    /// <param name="deck">A vector of integer cards.</param>
    /// <returns>True if the most recent cards played constitute a royal kill.</returns>
    public static bool RoyalKill(this List<int> deck)
    {
        try
        {
            for (var i = 0; i < 5; i++)
            {
                if (i >= deck.Count) return false;
                var kc = (int)deck[i].GetNumber() - 10;
                if (kc > 0)
                    return kc <= i;
            }
        }
        catch (Exception)
        {
            return false;
        }

        return false;
    }

    /// <summary>
    /// Checks if any royal has been played so far. Useful for knowing if to hand off turn to the next player.
    /// </summary>
    /// <param name="deck">A vector of integer cards.</param>
    /// <returns>True if any royal has played so far.</returns>
    public static bool ActiveRoyal(this List<int> deck)
    {
        for (var i = 0; i < deck.Count; i++)
        {
            if (deck[i].GetNumber() - 10 > 0)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if the current top cards of the deck allow for snapping.
    /// </summary>
    /// <param name="deck">A vector of integer cards.</param>
    /// <returns>True if the deck is snappable.</returns>
    public static bool CanSnap(this List<int> deck)
    {
        try
        {
            var len = deck.Count % 13;
            var top = deck[0].GetNumber();

            if (len == 1 && top == Number.Ace) return true;
            if (len == (int)top) return true;

            if (deck.Count < 2) return false;
            var second = deck[1].GetNumber();
            if (top == second) return true;
            
            if (deck.Count < 3) return false;
            var third = deck[2].GetNumber();
            if (top == third) return true;
        }
        catch (Exception)
        {
            return false;
        }

        return false;
    }

    /// <summary>
    /// Shuffles the deck.
    /// </summary>
    /// <param name="deck">A vector of integer cards.</param>
    public static void Shuffle(this List<int> deck)
    {
        var n = deck.Count;
        while (n > 1)
        {
            n--;
            var k = Rng.Next(n + 1);
            (deck[k], deck[n]) = (deck[n], deck[k]); // Fancy notation for swapping the values of deck[k] and deck[n]
        }
    }

    /// <summary>
    /// Creates a new deck, populated with cards.
    /// </summary>
    /// <returns>A vector of integer cards, unshuffled.</returns>
    public static List<int> CreateDeck()
    {
        return (
            from Suit s in Enum.GetValues(typeof(Suit)) 
            from Number n in Enum.GetValues(typeof(Number)) 
            select Card.CreateCard(s, n)).ToList();
    }

    /// <summary>
    /// Checks the top card of the deck without removing it.
    /// </summary>
    /// <param name="deck">A vector of integer cards.</param>
    /// <returns></returns>
    public static int Peek(this List<int> deck)
    {
        return deck[0];
    }
    
    /// <summary>
    /// Removes and returns the top card of the deck.
    /// </summary>
    /// <param name="deck">A vector of integer cards.</param>
    /// <returns>The top card of the deck.</returns>
    public static int Pop(this List<int> deck)
    {
        var ret = deck[0];
        deck.RemoveAt(0);
        return ret;
    }

    /// <summary>
    /// Puts a card on top of the deck.
    /// </summary>
    /// <param name="deck">A vector of integer cards.</param>
    /// <param name="number">The card to put on top of the deck.</param>
    public static void Push(this List<int> deck, int number)
    {
        deck.Insert(0, number);
    }

    /// <summary>
    /// Puts a card on the bottom of the deck.
    /// </summary>
    /// <param name="deck">A vector of integer cards.</param>
    /// <param name="number">The card to put on the bottom of the deck.</param>
    public static void PushBottom(this List<int> deck, int number)
    {
        deck.Add(number);
    }
    
    /// <summary>
    /// Puts a stack of cards on the bottom of the deck.
    /// </summary>
    /// <param name="deck">A vector of integer cards.</param>
    /// <param name="numbers">A vector of integer cards.</param>
    public static void PushRangeBottom(this List<int> deck, IEnumerable<int> numbers)
    {
        deck.AddRange(numbers);
    }
}