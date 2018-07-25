using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XCentiumWebScraper.Core.Model;

namespace XCentiumWebScraper.ViewModel
{
    public class DashboardViewModel
    {
        public IEnumerable<DocInfo> HistoryItems { get; set; }
        public string Message { get; set; }
    }
}