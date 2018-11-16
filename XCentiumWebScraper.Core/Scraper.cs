using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using XCentiumWebScraper.Core.Extension;
using XCentiumWebScraper.Core.Model;

namespace XCentiumWebScraper.Core
{
    /// <summary>
    /// Handles the scraping of an Html document for a given URL
    /// </summary>
    public class Scraper : BaseClass
    {
        public DocInfo DocInfo { get; set; }
        public HtmlDocument Document { get; set; }
        public bool Loaded { get; set; }
        private Uri SiteUri { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="url"></param>
        public Scraper(string url)
        {
            try
            {
                if (IsValidUrl(url))
                {
                    SiteUri = new Uri(url);
                    var webGet = new HtmlWeb();
                    Document = webGet.Load(url);

                    DocInfo = new DocInfo
                    {
                        Url = url,
                        IsValidUrl = true
                    };
                    Loaded = true;
                }
                else
                {
                    throw new Exception($"Invalid url! {url}");
                }
            }
            catch (Exception exc)
            {
                LogEntry = new LogEntry(exc);
                LogEntry.SaveLogEntryFile();
            }
        }

        public bool Execute()
        {
            try
            {
                GetDocumentImages();
                GetDocumentTextData();
                DocInfo.SaveToHistory();
                return true;
            }
            catch (Exception exc)
            {
                LogEntry = new LogEntry(exc);
                LogEntry.SaveLogEntryFile();
                return false;
            }
        }

        /// <summary>
        /// Tests given URL for validity
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        /// Scrape document for img tags and return src attribute values
        /// </summary>
        /// <returns></returns>
        private void GetDocumentImages()
        {
            var imageNodes = Document.DocumentNode.SelectNodes("//body//img")
                    ?.Select(n => n.Attributes["src"]?.Value)
                    ?.Where(n => !string.IsNullOrEmpty(n))
                    .Select(n => ParseImagePath(n));

            DocInfo.Images = imageNodes ?? new List<string>();
        }

        private string ParseImagePath(string imgPath)
        {
            //string.Format("{0}://{1}", SiteUri.Scheme, (SiteUri.Host + "/" + n).Replace("//", "/")))

            if (imgPath.ToLower().StartsWith("http"))
                return imgPath;
            if (imgPath.StartsWith("//"))
                return string.Format("{0}:{1}", SiteUri.Scheme, imgPath);

            return string.Format("{0}://{1}", SiteUri.Scheme, (SiteUri.Host + "/" + imgPath).Replace("//", "/"));
        }

        /// <summary>
        /// Scrape document for text data within nodes
        /// </summary>
        private void GetDocumentTextData()
        {
            var textValues = new List<string>();
            var illegalNodeNames = new string[] { "script", "style" };
            var strangeExclusionList = new string[] { "</form>" };

            foreach (var node in Document.DocumentNode.DescendantsAndSelf()
                .Where(n => n.NodeType == HtmlNodeType.Text
                    && !n.HasChildNodes
                    && !illegalNodeNames.Contains(n.ParentNode.Name)))
            {
                string text = node.InnerText;
                if (!string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text)
                    && !strangeExclusionList.Contains(text.ToLower()))
                {
                    var trimmedText = text.Trim();
                    var punctuation = text.Where(Char.IsPunctuation).Distinct().ToArray();
                    var words = trimmedText.Split(' ')
                        .Where(x => !x.StartsWith("&"))
                        .Select(x => x.Trim(punctuation).RemoveSpecialCharacters())
                        .Where(x => x != string.Empty);

                    foreach (var word in words)
                    {
                        textValues.Add(word);
                    }
                }
            }

            DocInfo.TotalWordCount = textValues.Count();
            DocInfo.UniqueWordCount = textValues.Distinct().Count();
            DocInfo.TopWords = textValues
                .GroupBy(tv => tv)
                .OrderByDescending(g => g.Count())
                .Take(10)
                .Select(g => new WordAndCount(g.Key, g.Count()))
                .ToList();
        }

        /// <summary>
        /// Return all records currently in the history file
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DocInfo> ReadHistory()
        {
            try
            {
                var filePath = $"{AppDomain.CurrentDomain.BaseDirectory}/{Constants.HistoryFileName}";
                var historyItems = System.IO.File.Exists(filePath)
                    ? System.IO.File.ReadAllLines(filePath)
                        .Select(x => JsonConvert.DeserializeObject<DocInfo>(x))
                        .Reverse()
                    : new List<DocInfo>();
                return historyItems ?? new List<DocInfo>();
            }
            catch (Exception exc)
            {
                new LogEntry(exc).SaveLogEntryFile();
                return null;
            }
        }

        /// <summary>
        /// Return a single record that exists in the history file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DocInfo ReadHistoryItem(Guid id)
        {
            try
            {
                var filePath = $"{AppDomain.CurrentDomain.BaseDirectory}/{Constants.HistoryFileName}";
                var item = System.IO.File.ReadAllLines(filePath)
                    ?.Select(x => JsonConvert.DeserializeObject<DocInfo>(x))
                    ?.FirstOrDefault(x => x.Id == id);
                return item;
            }
            catch (Exception exc)
            {
                new LogEntry(exc).SaveLogEntryFile();
                return null;
            }
        }

        /// <summary>
        /// Clear all recorded execution history
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static void ClearHistory()
        {
            try
            {
                var filePath = $"{AppDomain.CurrentDomain.BaseDirectory}/{Constants.HistoryFileName}";
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
