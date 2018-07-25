using HtmlAgilityPack;
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
        public void GetDocumentImages()
        {
            var imageNodes = Document.DocumentNode.SelectNodes("//body//img")
                    ?.Select(n => n.Attributes["src"]?.Value)
                    ?.Where(n => !string.IsNullOrEmpty(n))
                    .Select(n => n.ToLower().StartsWith("http")
                        ? n : string.Format("{0}://{1}", SiteUri.Scheme, (SiteUri.Host + "/" + n).Replace("//", "/")));

            DocInfo.Images = imageNodes;
        }

        /// <summary>
        /// Scrape document for text data within nodes
        /// </summary>
        public void GetDocumentTextData()
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
                    var words = trimmedText.Split(' ').Select(x => x.Trim(punctuation).RemoveSpecialCharacters());
                    foreach (var word in words)
                    {
                        if (word != string.Empty)
                        {
                            textValues.Add(word);
                        }
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
    }
}
