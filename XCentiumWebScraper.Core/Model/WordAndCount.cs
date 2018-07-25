using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCentiumWebScraper.Core.Model
{
    /// <summary>
    /// A word and the amount of times that word occurs
    /// </summary>
    public class WordAndCount
    {
        public string Word { get; set; }
        public double Count { get; set; }

        public WordAndCount(string w, double c)
        {
            Word = w;
            Count = c;
        }
    }
}
