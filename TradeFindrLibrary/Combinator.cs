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
        private List<ushort> Indices;
        private T[] Items;
        private ushort ItemCount
        {
            get { return Convert.ToUInt16(Items.Length -1); }
        }
        private ushort MaxDepth;
        private int CurrentDepth
        {
            get { return Indices.Count - 1; }
        }
        
        public Combinator(List<T> items, ushort maxDepth)
        {
            Items = items.ToArray();
            MaxDepth = maxDepth;
            Indices = new List<ushort>() { 0 };
        }
        
        public T[] getNextCombo()
        {
            var newIndices = new List<ushort>(Indices);
            if (Indices.Count > 0)
            {
                if (CurrentDepth < MaxDepth && Indices[CurrentDepth] < ItemCount)
                {
                    // doesn't like (Indices[x] + 1) for some reason
                    ushort nextIndex = Indices[CurrentDepth];
                    nextIndex++;
                    Indices.Add(nextIndex);
                } 
                else if (Indices[CurrentDepth] < ItemCount)
                {
                    Indices[CurrentDepth]++;
                } 
                else
                {
                    while (Indices.Count > 0 && Indices[CurrentDepth] >= ItemCount)
                    {
                        Indices.RemoveAt(CurrentDepth);
                    }
             
                    if (Indices.Count > 0)
                    {
                        Indices[CurrentDepth]++;
                    }
                }
            }

            var result = new List<T>();

            foreach (ushort index in Indices)
            {
                result.Add(Items[index]);
            }

            return result.ToArray();
        }

        public bool atEnd()
        {
            return CurrentDepth == -1;
        }
        public void reset()
        {
            Indices = new List<ushort>() { 0 };
        }
    }
}
