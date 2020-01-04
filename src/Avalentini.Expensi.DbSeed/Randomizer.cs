using System;
using System.Collections.Generic;

namespace Avalentini.Expensi.DbSeed
{
    public static class Randomizer
    {
        private static readonly DateTime Start = new DateTime(2000, 1, 1);
        private static readonly TimeSpan Range = DateTime.Today - Start;    
        //Calculate cumulative number of seconds between two DateTimes
        private static readonly int Days = Range.Days * 60 * 60 *24;
        private static readonly int Hours = Range.Hours * 60 * 60;
        private static readonly int Minutes = Range.Minutes * 60;
        private static readonly int Seconds = Range.Seconds;
        private static readonly int RangeInSeconds = Days + Hours + Minutes + Seconds;

        public static decimal NextDecimal(this Random rng)
        {
            return new decimal(rng.Next(100000),
                0,
                0,
                false,
                2);
        }

        public static string NextPlace()
        {
            var random = new Random();
            var index = random.Next(0, Places.Count);
            return Places[index];
        }

        public static string NextItem()
        {
            var random = new Random();
            var index = random.Next(0, Items.Count);
            return Items[index];
        }

        public static DateTime NextDateTime(Random rnd)
        {
            //Add random number of seconds to Start
            return Start.AddSeconds(rnd.Next(RangeInSeconds));
        }

        private static readonly List<string> Places = new List<string>
        {
            "Store1",
            "Store2",
            "Store3",
            "Store4",
        };

        private static readonly List<string> Items = new List<string>
        {
            "Item1",
            "Item2",
            "Item3",
            "Item4",
            "Item5",
            "Item6",
            "Item7",
            "Item8",
            "Item9",
        };
    }
}
