using System.ComponentModel;

namespace XCentiumWebScraper.Core.Enum
{
    /// <summary>
    /// Urgency level of log
    /// </summary>
    public enum LogType
    {
        [Description("Info")]
        Info,
        [Description("Warning")]
        Warning,
        [Description("Error")]
        Error
    }
}
