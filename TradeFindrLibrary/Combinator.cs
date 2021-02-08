using System;
using System.Collections.Generic;
using System.Text;

namespace TradeFindr
{
    /*
     *  This Class takes in a list and sequentially returns a unique combination of 
     *  the elements with each call of getNextCombo().
     *  
     *  A new instance of this class should be instantiated for each new List of Items.
     */
    public class Combinator<T>
    {
        private List<int> Indices;

        private readonly List<T> Items;
        private uint MaxSize;
        private int ItemCount
        {
            get { return Items.Count - 1; }
        }

        public Combinator(List<T> items, uint maxSize)
        {
            Items = new List<T>(items);
            Indices = new List<int>() { 0 };
            MaxSize = maxSize;
        }

        public T[] GetNextCombo()
        {
            var result = new List<T>();
            var indexCount = Indices.Count - 1;

            if (Items.Count == 0)
            {
                return result.ToArray();
            }

            foreach (ushort index in Indices)
            {
                result.Add(this.Items[index]);
            }

            // Increment the last index 
            if (Indices[indexCount] < ItemCount)
            {
                Indices[indexCount]++;
            }
            else
            {
                if (Indices[0] < Items.Count - Indices.Count)
                {
                   
                    var i = indexCount;
                    var diff = ItemCount;
                    // Iterate backwards until we find an index that can be incremented
                    do
                    {
                        i--;
                        diff--;
                    }
                    while (Indices[i] >= diff && i > 0);
                    Indices[i]++;

                    // From the incremented index, reset all indices onwards
                    i++;
                    for (; i < Indices.Count; i++)
                    {
                        Indices[i] = Indices[i - 1] + 1;
                    }
                }
                else if (Indices.Count < Items.Count)
                {
                    // We can carry a 1 back a place
                    Indices.Add(0);
                    for (int j = Indices.Count - 1; j >= 0; --j)
                    {
                        Indices[j] = j;
                    }
                }
                else
                {
                    // We're at the end, add an extra index to flag AtEnd()
                    Indices.Add(0);
                }
            }

            return result.ToArray();
        }

        // Removes an item from the collection and restarts combination generation at current size
        public bool Remove(T obj)
        {
            if (Items.Contains(obj))
            {
                var index = Items.IndexOf(obj);
                Items.Remove(obj);

                while (Indices.Count > Items.Count)
                {
                    Indices.RemoveAt(0);
                }

                ResetFrom(0);
                return true;
            }
            return false;
        }

        // Sets the generator to start at a combination of a specific size
        public void SetCount(uint count)
        {
            Reset();
            if (count <= Items.Count && count > 0)
            {
                // Index starts from 1 because Reset() adds 0 to the collection
                for (int i = 1; i < count; i++)
                {
                    Indices.Add(i);
                }
            }
        }

        
        private void ResetFrom(int i)
        {
            // Index safety
            if (i > 0) 
            {
                for (; i < Indices.Count; i++)
                {
                    Indices[i] = i;
                }
            }
        }

        public bool AtEnd()
        {
            // Either stop when combo goes beyond set size, 
            // or stop when our combo tries to become larger than the whole collection
            return Indices.Count > Items.Count || Indices.Count > MaxSize;
        }
        public void Reset()
        {
            Indices.Clear();
            Indices.Add(0);
        }
    }
}
