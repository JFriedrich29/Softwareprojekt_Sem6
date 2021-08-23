using System.Collections.Generic;

namespace QuantumCryptoCram.Common.Random
{
    /// <summary>
    /// An interface for describing a random generator.
    /// </summary>
    public interface IRandomGenerator
    {
        /// <summary>
        /// Creates a random bool value.
        /// </summary>
        /// <returns>A random bool value.</returns>
        bool GetRandomBool();

        /// <summary>
        /// Randomly picks items out of a list via the Reservoir Sampling technique.
        /// </summary>
        /// <typeparam name="T">The type of the objects that should be randomly selected.</typeparam>
        /// <param name="values">A list of type T out of which <see cref="numValues"/> should be picked.</param>
        /// <param name="numValues">The number of items that should be picked out of values.</param>
        /// <returns>Return num_items random items out of values.</returns>
        IList<T> PickRandomItems<T>(IEnumerable<T> values, int numValues);
    }
}