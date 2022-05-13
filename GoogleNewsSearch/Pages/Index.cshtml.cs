using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace GoogleNewsSearch.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                XmlDocument xmlDocument = new();
                xmlDocument.Load($"https://news.google.com/rss/search?q={WebUtility.UrlEncode(search)}&hl=en-US&gl=US&ceid=US:en");

                List<dynamic> newsList = new();
                var news = xmlDocument.SelectNodes($"//item");

                foreach (XmlNode xmlNode in news)
                {
                    var title = xmlNode.SelectSingleNode("title").InnerText;
                    var link = xmlNode.SelectSingleNode("link").InnerText;
                    var dates = xmlNode.SelectSingleNode("pubDate").InnerText;
                    var description = xmlNode.SelectSingleNode("description").InnerText;
                    string trimdescription = Regex.Replace(description, @"<[^>]+>", "").Trim();
                    trimdescription = WebUtility.HtmlDecode(trimdescription);

                    newsList.Add(new { Title = title, Link = link, Date = dates, Description = trimdescription });
                }
                ViewData["List"] = newsList;
                ViewData["Search"] = search;
            }





        }
    }

}