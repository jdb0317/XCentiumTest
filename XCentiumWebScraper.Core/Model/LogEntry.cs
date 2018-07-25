using System;
using System.IO;
using System.Text;
using XCentiumWebScraper.Core.Enum;
using XCentiumWebScraper.Core.Helper;

namespace XCentiumWebScraper.Core.Model
{
    /// <summary>
    /// Log entry item
    /// </summary>
    public class LogEntry
    {
        public DateTime Date { get; set; }
        public string Website { get; set; }
        public LogType LogType { get; set; }
        public string ErrorMessage { get; set; }

        public LogEntry()
        {
        }

        public LogEntry(Exception exc)
        {
            LogType = LogType.Error;
            ErrorMessage = exc.ToString();
        }

        /// <summary>
        /// Write LogEntry to file
        /// </summary>
        public void SaveLogEntryFile()
        {
            using (StreamWriter sw = new StreamWriter($"{AppDomain.CurrentDomain.BaseDirectory}/{Constants.LogFileName}", true))
            {
                StringBuilder sb = new StringBuilder()
                    .AppendLine(DateTime.Now.ToString(Constants.DateFormatString))
                    .AppendLine(!string.IsNullOrEmpty(Website) ? Website : Constants.UnknownExternalSite)
                    .AppendLine($"{EnumHelper.GetEnumDescription(LogType)} - {ErrorMessage}");
                sw.Write(sb.ToString());
            }
        }
    }
}
