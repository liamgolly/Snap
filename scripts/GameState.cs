using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SnapTraining.scripts;

public class GameState
{
    public List<int> Center;
    public List<int>[] Players;
    public int CurrentPlayer;
    public int LastRoyalPlayer = -1;
    
    /// <summary>
    /// Creates a new managed instance of the game.
    /// </summary>
    /// <param name="players"> The number of players in this game. </param>
    public GameState(int players)
    {
        Players = new List<int>[players];

        Center = Deck.CreateDeck();
        Center.Shuffle();

        for (var i = 0; i < players; i++)
            Players[i] = new List<int>();

        var playerCount = 0;
        for (var i = 0; i < 52; i++)
            Players[playerCount++ % Players.Length].Add(Center.Pop());
    }

    /// <summary>
    /// Has a player take their turn. If they have a card, they play it. If they don't, the next person does. If they
    /// don't, the next person... If nobody does, nobody plays anything. Only advances the turn if a royal was played,
    /// or no royals have played yet.
    /// </summary>
    public void Play()
    {
        for (var i = CurrentPlayer; i < CurrentPlayer + Players.Length; i++)
        {
            var curPlayer = i % Players.Length;
            if (Players[curPlayer].Count == 0) continue;
            Center.Push(Players[curPlayer].Pop());
            
            if (!Center.Peek().IsRoyal())
            {
                if (Center.ActiveRoyal())
                    return;
                CurrentPlayer = (curPlayer + 1) % Players.Length;
                return;
            }
            CurrentPlayer = (curPlayer + 1) % Players.Length;
            LastRoyalPlayer = curPlayer;
            return;
        }
    }

    /// <summary>
    /// Royal death can be avoided by snap, so this should be called after Play() and all Snap() logic, but before
    /// the next Play() is called.
    /// </summary>
    /// <code>
    /// // An example from Simulator.cs:
    /// State.Play();
    /// if (State.Center.CanSnap())
    ///     State.Snap(_weightedRandomPicker.PickIndex());
    /// State.PostPlay();
    /// </code>
    public void PostPlay()
    {
        if (!Center.RoyalKill()) return;
        Players[LastRoyalPlayer].PushRangeBottom(Center);
        Center.Clear();
        CurrentPlayer = LastRoyalPlayer;
        LastRoyalPlayer = -1;
    }

    /// <summary>
    /// Has a player attempt to snap. If the deck is snappable, they win. If it isn't, they give everyone else a card.
    /// </summary>
    /// <param name="player"> The player ID. </param>
    public void Snap(int player)
    {
        if (player < 0 || player >= Players.Length) return;
        
        if (Center.CanSnap() || (Center.RoyalKill() && player == LastRoyalPlayer))
        {
            Players[player].PushRangeBottom(Center);
            Center.Clear();
            LastRoyalPlayer = -1;
            CurrentPlayer = player;
            return;
        }

        for (var i = 0; i < Players.Length; i++)
        {
            if (i == player) continue;
            try
            {
                Players[i].PushBottom(Players[player].Pop());
            }
            catch
            {
                break; // The player who failed the snap ran out of cards!
            }
        }
    }

    /// <summary>
    /// Checks the state of the game.
    /// </summary>
    /// <returns>
    /// Returns -1 if the game is still ongoing, 0-n if player 0-n has won the game, and -2 if there is a tie.
    /// </returns>
    public int CheckGameState()
    {
        if (Center.Count == 52)
            return -2;

        for (var i = 0; i < Players.Length; i++)
            if (Players[i].Count == 52)
                return i;

        return -1;
    }
}