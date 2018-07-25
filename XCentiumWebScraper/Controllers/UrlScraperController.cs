using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using XCentiumWebScraper.Core;
using XCentiumWebScraper.Core.Model;

namespace XCentiumWebScraper.Controllers
{
    public class UrlScraperController : Controller
    {
        /// <summary>
        /// Homepage
        /// Displays url input field and history of previous executions
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult Dashboard()
        {
            var filePath = $"{AppDomain.CurrentDomain.BaseDirectory}/{Constants.HistoryFileName}";
            var serializer = new JavaScriptSerializer();
            var historyItems = System.IO.File.Exists(filePath)
                ? System.IO.File.ReadAllLines(filePath)
                    .Select(x => serializer.Deserialize<DocInfo>(x))
                    .Reverse()
                : new List<DocInfo>();

            return View(historyItems);
        }

        /// <summary>
        /// Scrapes html page for image and text data
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult Scrape(string url)
        {
            Scraper scraper = new Scraper(url);
            if(scraper.Loaded) scraper.Execute();

            return scraper.DocInfo != null 
                ? View("~/Views/UrlScraper/Results.cshtml", scraper.DocInfo) 
                : null;
        }

        /// <summary>
        /// Displays prerecorded data on a previously run url
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult History(Guid id)
        {
            var filePath = $"{AppDomain.CurrentDomain.BaseDirectory}/{Constants.HistoryFileName}";
            var serializer = new JavaScriptSerializer();
            var item = System.IO.File.ReadAllLines(filePath)
                ?.Select(x => serializer.Deserialize<DocInfo>(x))
                ?.FirstOrDefault(x => x.Id == id);

            return item != null 
                ? View("~/Views/UrlScraper/Results.cshtml", item)
                : null;
        }
    }
}