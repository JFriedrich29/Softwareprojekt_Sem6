using System;
using System.Collections.Generic;
using System.Linq;

namespace QuantumCryptoCram.Common.Random
{
    /// <summary>
    /// A class describing a random generator.
    /// After initializing a random object (System), the class uses this object to creaste a random bool.
    /// </summary>
    public class RandomGenerator : IRandomGenerator
    {
        private readonly System.Random _random;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomGenerator"/> class.
        /// </summary>
        public RandomGenerator()
        {
            int seed = Guid.NewGuid().GetHashCode();
            _random = new System.Random(seed);
        }

        /// <inheritdoc/>
        public bool GetRandomBool()
        {
            return _random.Next(0, 2) == 0;
        }

        /// <inheritdoc/>
        //public IList<T> PickRandomItems<T>(IEnumerable<T> arr, int num_values)
        //{
        //    List<T> values = arr.ToList();
        //    // Don't exceed the array's length.
        //    if (num_values >= values.Count)
        //        num_values = values.Count - 1;

        //    // Make an array of indexes 0 through values.Count - 1.
        //    int[] indexes =
        //        Enumerable.Range(0, values.Count).ToArray();

        //    // Build the return list.
        //    var results = new List<T>();

        //    // Randomize the first num_values indexes.
        //    for (int i = 0; i < num_values; i++)
        //    {
        //        // Pick a random entry between i and values.Count - 1.
        //        int j = _random.Next(i, values.Count);

        //        // Swap the values.
        //        int temp = indexes[i];
        //        indexes[i] = indexes[j];
        //        indexes[j] = temp;

        //        // Save the ith value.
        //        results.Add(values[indexes[i]]);
        //    }

        //    // Return the selected items.
        //    return results;
        //}

        /// <inheritdoc/>
        /// <seealso cref="https://gist.github.com/riyadparvez/5911802"/>
        public IList<T> PickRandomItems<T>(IEnumerable<T> arr, int numValues)
        {
            if (arr == null)
            {
                throw new ArgumentNullException();
            }

            if (numValues < 1 || numValues > arr.Count())
            {
                throw new ArgumentOutOfRangeException();
            }

            var list = arr.ToList();
            var samples = new List<T>();
            int n = 0;

            foreach (T item in list)
            {
                n++;
                if (samples.Count < numValues)
                {
                    samples.Add(item);
                }
                else
                {
                    int s = _random.Next(n);
                    if (s < numValues)
                    {
                        samples[s] = item;
                    }
                }
            }

            return samples;
        }
    }
}