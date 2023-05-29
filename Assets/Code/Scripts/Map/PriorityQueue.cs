using System;
using System.Collections.Generic;

namespace Code.Scripts.Map
{
    /// <summary>
    ///     A Queue class in which each item is associated with a Double value
    ///     representing the item's priority.
    ///     Dequeue and Peek functions return item with the best priority value.
    /// </summary>
    public class PriorityQueue<T>
    {
        private readonly List<Tuple<T, double>> elements = new();

        /// <summary>
        ///     Return the total number of elements currently in the Queue.
        /// </summary>
        /// <returns>Total number of elements currently in Queue</returns>
        public int Count => elements.Count;

        /// <summary>
        ///     Add given item to Queue and assign item the given priority value.
        /// </summary>
        /// <param name="item">Item to be added.</param>
        /// <param name="priorityValue">Item priority value as Double.</param>
        public void Enqueue(T item, double priorityValue)
        {
            var newVal = Tuple.Create(item, priorityValue);
            for (var i = 0; i < elements.Count; i++)
                if (EqualityComparer<T>.Default.Equals(item, elements[i].Item1))
                {
                    elements[i] = newVal;
                    return;
                }

            elements.Add(newVal);
        }

        /// <summary>
        ///     Return lowest priority value item and remove item from Queue.
        /// </summary>
        /// <returns>Queue item with lowest priority value.</returns>
        public T Dequeue()
        {
            var bestPriorityIndex = 0;

            for (var i = 0; i < elements.Count; i++)
                if (elements[i].Item2 < elements[bestPriorityIndex].Item2)
                    bestPriorityIndex = i;

            var bestItem = elements[bestPriorityIndex].Item1;
            elements.RemoveAt(bestPriorityIndex);
            return bestItem;
        }


        /// <summary>
        ///     Return lowest priority value item without removing item from Queue.
        /// </summary>
        /// <returns>Queue item with lowest priority value.</returns>
        public T Peek()
        {
            var bestPriorityIndex = 0;

            for (var i = 0; i < elements.Count; i++)
                if (elements[i].Item2 < elements[bestPriorityIndex].Item2)
                    bestPriorityIndex = i;

            var bestItem = elements[bestPriorityIndex].Item1;
            return bestItem;
        }

        public bool Contains(T item)
        {
            for (var i = 0; i < elements.Count; i++)
                if (EqualityComparer<T>.Default.Equals(item, elements[i].Item1))
                    return true;

            return false;
        }
    }
}