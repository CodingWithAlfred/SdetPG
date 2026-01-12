using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIPrometheusJobSearchTest.Models
{
    public class TestSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string PrometheusSearchTerm { get; set; } = string.Empty;
        public string ExpectedJobTitle { get; set; } = string.Empty;
        public string BrowserType { get; set; } = "chromium";
        public bool Headless { get; set; }
        public int DefaultTimeout { get; set; } = 30000;
    }
}
