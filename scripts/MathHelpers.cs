using System;

namespace SnapTraining.scripts;

/// <summary>
/// A helper tool for picking a random value given an array of probabilities for indicies.
/// </summary>
public class WeightedRandomPicker
{
    private readonly double[] _cdf;
    private readonly Random _random;

    public WeightedRandomPicker(double[] probabilities)
    {
        if (probabilities == null || probabilities.Length == 0)
            throw new ArgumentException("Probabilities array cannot be null or empty.");

        double sum = 0;
        _cdf = new double[probabilities.Length];

        for (var i = 0; i < probabilities.Length; i++)
        {
            sum += probabilities[i];
            _cdf[i] = sum;
        }

        if (Math.Abs(_cdf[^1] - 1.0) > 1e-9)
            throw new ArgumentException("Probabilities must sum to 1.");

        _random = new Random();
    }

    public int PickIndex()
    {
        var rand = _random.NextDouble();
        
        var index = Array.BinarySearch(_cdf, rand);
        if (index < 0)
            index = ~index;
        return index;
    }
}