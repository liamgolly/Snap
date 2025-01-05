using System;
using System.Collections.Generic;
using System.Linq;

namespace SnapTraining.scripts;
public static class Card
{
    /// <summary>
    /// Creates a unique integer that represents a card given a suit and number.
    /// </summary>
    /// <param name="suit">The suit of the card.</param>
    /// <param name="number">The number of the card.</param>
    /// <returns>A unique integer, with the 100s place representing the suit and the rest representing the number.</returns>
    public static int CreateCard(Suit suit, Number number)
    {
        return (int)suit * 100 + (int)number;
    }
    
    /// <summary>
    /// Gest the suit of an integer that represents a card.
    /// </summary>
    /// <param name="card">An integer card.</param>
    /// <returns>A suit enum.</returns>
    public static Suit GetSuit(this int card) => (Suit)(card / 100);
    
    /// <summary>
    /// Gets the number of an integer that represents a card.
    /// </summary>
    /// <param name="card">An integer card.</param>
    /// <returns>A number enum.</returns>
    public static Number GetNumber(this int card) => (Number)(card % 100);

    /// <summary>
    /// Checks if a given card is royal, as in a Jack, Queen, King, or Ace.
    /// </summary>
    /// <param name="card">An integer card.</param>
    /// <returns>True if the card is a royal.</returns>
    public static bool IsRoyal(this int card) => card.GetNumber() - 10 > 0;
}
public enum Suit
{
    Clubs = 0,
    Diamonds = 1,
    Hearts = 2,
    Spades = 3
}

public enum Number
{
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13,
    Ace = 14,
}