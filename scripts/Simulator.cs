using System;
using System.Linq;

namespace SnapTraining.scripts;

public class Simulator
{
    public GameState State;

    private WeightedRandomPicker _weightedRandomPicker;
    public Random Random;
    
    /// <summary>
    /// Creates a new simulation instance that manages playing the game automatically.
    /// </summary>
    /// <param name="playerCount">The number of players in this simulation instance.</param>
    public Simulator(int playerCount)
    {
        State = new GameState(playerCount);
        _weightedRandomPicker = new WeightedRandomPicker(
            Enumerable.Repeat(1.0 / playerCount, playerCount).ToArray());
        
        Random = new Random();
    }

    /// <summary>
    /// Sets the win chances for each player. You must pass in exactly one chance for each player,
    /// and the chances must sum to 1.
    /// </summary>
    /// <param name="chances">The chances for each player, as an array. 0 for player 0, ...</param>
    /// <returns>True if the set succeeds, false otherwise.</returns>
    public bool SetWinChances(double[] chances)
    {
        if (chances.Length != State.Players.Length) return false;
        if (chances.Sum() - 1 > double.Epsilon) return false;
        
        _weightedRandomPicker = new WeightedRandomPicker(chances);
        return true;
    }

    /// <summary>
    /// Performs a single step of the simulation instance. Handles playing, snapping, and royal killing.
    /// </summary>
    /// <returns>The current game state, as defined by GameState.cs</returns>
    public int Step()
    {
        State.Play();
        if (State.Center.CanSnap())
            State.Snap(_weightedRandomPicker.PickIndex());
        State.PostPlay();
        return State.CheckGameState();
    }
}