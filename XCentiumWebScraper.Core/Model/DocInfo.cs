using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace XCentiumWebScraper.Core.Model
{
    /// <summary>
    /// Holds document information relevant to our needs
    /// </summary>
    public class DocInfo
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public bool IsValidUrl { get; set; }
        public double TotalWordCount { get; set; }
        public double UniqueWordCount { get; set; }
        public List<WordAndCount> TopWords { get; set; }
        public IEnumerable<string> Images { get; set; }
        public DateTime RunDate { get; set; }

        public DocInfo()
        {
            Id = Guid.NewGuid();
            RunDate = DateTime.Now;
        }

        /// <summary>
        /// Save for record keeping
        /// </summary>
        public void SaveToHistory()
        {
            using (StreamWriter sw = new StreamWriter($"{AppDomain.CurrentDomain.BaseDirectory}/{Constants.HistoryFileName}", true))
            {
                StringBuilder sb = new StringBuilder().AppendLine(new JavaScriptSerializer().Serialize(this));
                sw.Write(sb.ToString());
            }
        }
    }
}
