using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace Brainshare.Infrastructure.Infrastructure
{
    public class OzParser
    {
        private const string DescriptionTableId = "all-params";

        public static string GetIsbnByOzBookId(string id)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(GetStreamFromString(DownloadHtmlPage(GetOzDescriptionPageUrl(id))));

            HtmlNode table =  doc.GetElementbyId(DescriptionTableId);

            var row = table.Descendants("tr")
                .First(
                                 tr => tr.ChildNodes
                                         .Any(
                                               td => td.InnerText.StartsWith("ISBN",StringComparison.InvariantCultureIgnoreCase)
                                             )
                       );

            return row.ChildNodes.First(n => n.Name == "td").InnerText;
        }

        private static string GetOzDescriptionPageUrl(string bookId)
        {
            return string.Format("http://oz.by/books/more{0}.html", bookId);
        }

        private static string DownloadHtmlPage(string url)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_1) AppleWebKit/537.73.11 (KHTML, like Gecko) Version/7.0.1 Safari/537.73.11");
                return client.DownloadString(url);         
            }
        }

        private static Stream GetStreamFromString(string str)
        {
            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(str);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            return new MemoryStream(byteArray);
        }
    }
}