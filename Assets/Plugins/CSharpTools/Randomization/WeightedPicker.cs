using System;
using System.Collections.Generic;

namespace CSharpTools.Randomization
{
    /// <summary>
    ///     A thread safe implementation of a Weighted Picker for a generic type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WeightedPicker<T>
    {
        /// <summary>
        ///     The Number of Elements that are currently stored inside the Weighted Picker
        /// </summary>
        public int Count
        {
            get
            {
                lock (_elements)
                    return _elements.Count;
            }
        }

        /// <summary>
        ///     The cumulative value Range of all the Elements' probabilities inside the Weighted Picker, from 0 to n.
        /// </summary>
        public float ProbabilityRange
        {
            get
            {
                lock (_cumulativeProbabilities)
                    return _cumulativeProbabilities.Count > 0 ? _cumulativeProbabilities[^1] : 0;
            }
        }

        private readonly List<T> _elements = new();
        private readonly List<float> _cumulativeProbabilities = new();

        private readonly Random _random;
        
        public WeightedPicker(Random random)
        {
            _random = random;
        }
        
        public WeightedPicker() : this(new Random(Guid.NewGuid().GetHashCode()))
        {
        }
        
       /// <summary>
       ///      Picks a random Element based on its probability (weight).
       /// </summary>
       /// <returns>The randomly picked Element of type T.</returns>
       /// <exception cref="EmptyPickerException">The function is called when there are no Elements in the Picker</exception>
        public T Pick()
        {
            lock (_elements)
            {
                if (_elements.Count == 0)
                    throw new EmptyPickerException();

                float randomNumber = (float)_random.NextDouble() * _cumulativeProbabilities[^1];
                int index = _cumulativeProbabilities.BinarySearch(randomNumber);

                if (index < 0)
                    index = ~index;

                return _elements[index];
            }
        }
       
        /// <summary>
        ///     Adds an Element with a specified probability (weight).
        /// </summary>
        /// <param name="element">The Element to add.</param>
        /// <param name="probability">The probability (weight) of the Element.</param>
        /// <exception cref="InvalidProbabilityException">The specified probability is smaller or equal to zero</exception>
        /// <exception cref="DuplicatePickerElementException">The specified Element already exists in the Picker</exception>
        public void Add(T element, float probability)
        {
            if (probability <= 0)
                throw new InvalidProbabilityException($"Invalid Probability {probability}. Probability must be a positive number.");

            lock (_elements)
            {
                if (_elements.Contains(element))
                    throw new DuplicatePickerElementException($"The element '{element}' already exists in the Picker. Consider using the UpdateProbability function to override the probability of an existing Element.");
                
                _elements.Add(element);

                if (_cumulativeProbabilities.Count == 0)
                    _cumulativeProbabilities.Add(probability);
                else
                    _cumulativeProbabilities.Add(_cumulativeProbabilities[^1] + probability);
            }
        }
        
        /// <summary>
        ///     Remove an Element from the Weighted Picker. Should be used conservatively.
        /// </summary>
        /// <param name="element">The Element to remove from the Weighted Picker.</param>
        /// <exception cref="EmptyPickerException">The Picker does not contain any Elements</exception>
        /// <exception cref="MissingPickerElementException">The specified Element does not exist in the Weighted Picker</exception>
        public void Remove(T element)
        {
            lock (_elements)
            {
                if (_elements.Count == 0)
                    throw new EmptyPickerException();
                
                if (!_elements.Contains(element))
                    throw new MissingPickerElementException($"The specified element '{element}' does not exist in the Picker.");
                
                int index = _elements.IndexOf(element);
                float diff;
                
                if (index == 0)
                    diff = _cumulativeProbabilities[0];
                else
                    diff = _cumulativeProbabilities[index] - _cumulativeProbabilities[index - 1];

                _elements.Remove(element);
                _cumulativeProbabilities.RemoveAt(index);
                
                for (int i = index; i < _cumulativeProbabilities.Count; i++)
                {
                    _cumulativeProbabilities[i] -= diff;
                }
            }
        }
        
        /// <summary>
        ///     Update the probability (weight) of an Element that already exists in the Weighted Picker.
        /// </summary>
        /// <param name="element">The Element with the probability (weight) that should be updated.</param>
        /// <param name="probability">The new probability (weight) to assign to the specified Element.</param>
        /// <exception cref="InvalidProbabilityException">The specified probability (weight) is smaller or equal to zero</exception>
        /// <exception cref="EmptyPickerException">The Weighted Picker does not contain any Elements</exception>
        /// <exception cref="MissingPickerElementException">The specified Element does not exist in the Weighted Picker</exception>
        public void UpdateProbability(T element, float probability)
        {
            if (probability <= 0)
                throw new InvalidProbabilityException($"Invalid Probability {probability}. Probability must be a positive number.");
            
            lock (_elements)
            {
                if (_elements.Count == 0)
                    throw new EmptyPickerException();
                
                if (!_elements.Contains(element))
                    throw new MissingPickerElementException($"The specified element '{element}' does not exist in the Picker.");

                float diff = probability - GetProbability(element);
                int index = _elements.IndexOf(element);

                for (int i = index; i < _elements.Count; i++)
                {
                    _cumulativeProbabilities[i] += diff;
                }
            }
        }
        
        /// <summary>
        ///     Get the original probability (weight) of an Element that has previously been added to the Weighted Picker.
        /// </summary>
        /// <param name="element">The Element to get the probability (weight) for.</param>
        /// <returns>The probability (weight) the Element has been added or updated with.</returns>
        /// <exception cref="EmptyPickerException">The Picker does not contain any Elements</exception>
        /// <exception cref="MissingPickerElementException">The specified Element does not exist in the Weighted Picker</exception>
        public float GetProbability(T element)
        {
            lock (_elements)
            {
                if (_elements.Count == 0)
                    throw new EmptyPickerException();
                
                if (!_elements.Contains(element))
                    throw new MissingPickerElementException($"The specified element '{element}' does not exist in the Picker.");
                
                int index = _elements.IndexOf(element);

                if (index == 0)
                    return _cumulativeProbabilities[0];

                return _cumulativeProbabilities[index] - _cumulativeProbabilities[index - 1];
            }
        }

        /// <summary>
        ///     Clears all elements and their probabilities (weights) from the Weighted Picker.
        /// </summary>
        public void Clear()
        {
            lock (_elements)
            {
                _elements.Clear();
                _cumulativeProbabilities.Clear();
            }
        }
    }
}
