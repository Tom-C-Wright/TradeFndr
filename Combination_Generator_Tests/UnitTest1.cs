using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit.Sdk;
using System.Collections.Generic;
using System.Linq;
using TradeFindr;
using System;
using System.Runtime.InteropServices;

namespace Combination_Generator_Tests
{
    [TestClass]
    public class CombinatorTests
    {
        [TestMethod]
        public void AllCombosUnique()
        {
            List<ushort> items = new List<ushort>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
            Combinator<ushort> combinator = new Combinator<ushort>(maxSize: 10, items: items);
            List<ushort[]> combinations = new List<ushort[]>();
            //List<int> combo = new List<int>();
            while (!combinator.AtEnd())
            {
                var combo = combinator.GetNextCombo();
                combinations.Add(combo);
            }

            for (int i = 0; i < combinations.Count; i++)
            {
                for (int j = i + 1; j < combinations.Count; j++)
                {
                    List<ushort> result1 = combinations[i].Except(combinations[j]).ToList();
                    List<ushort> result2 = combinations[j].Except(combinations[i]).ToList();
                    // If there are no elements present in list I from list J, and vice versa then the lists are identical
                    Assert.IsFalse(result1.Count() == 0 && result2.Count() == 0);
                }
            }
        }
    }
}
