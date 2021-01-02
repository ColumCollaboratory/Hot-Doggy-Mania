using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO finish this class, will help automate other aspects of the game.

/// <summary>
/// Holds a distribution outcome value and weight.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class DistributionPair<T>
{
    public T outcome;
    private float weight;

    public float Weight
    {
        get { return weight; }
        set
        {
            // Ensure invalid weights are caught.
            if (value <= 0f)
                throw new ArgumentException("Weight must be greater than zero.");
            weight = value;
        }
    }

    public DistributionPair(T outcome, float weight)
    {
        this.outcome = outcome;
        Weight = weight;
    }
}

/// <summary>
/// Implements an immutable distribution of type T.
/// </summary>
/// <typeparam name="T">The type of outcomes associated with the distribution.</typeparam>
public sealed class FixedDistribution<T>
{


    public FixedDistribution(IList<DistributionPair<T>> outcomes)
    {
        if (outcomes == null)
            throw new ArgumentNullException("outcomes", "Outcomes cannot be a null collection.");
        if (outcomes.Count == 0)
            throw new ArgumentException("Outcomes must have at least one weight pair.", "outcomes");
    }



    public T Next()
    {
        throw new NotImplementedException();
    }

    public T[] GeneratePool()
    {
        throw new NotImplementedException();
    }
}
