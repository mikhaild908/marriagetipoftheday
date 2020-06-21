using System;
using System.IO;
using System.Net;

namespace MarriageTipOfTheDay
{
    public class MarriageTipWebScraper
    {
        public MarriageTip GetTodaysMarriageTip()
        {
            var marriageTip = new MarriageTip
            {
                Date = DateTime.Now,
                Tip = GetTodaysMarriageTip(GetResponseFromMarriageSite())
            };

            return marriageTip; 
        }

        private static string GetResponseFromMarriageSite()
        {
            var url = "http://www.foryourmarriage.org/daily-marriage-tips/";
            // var url = "http://www.foryourmarriage.org/marriage-resources/tips-and-advice/daily-marriage-tip/";
            return GetResponse(url);
        }

        private static string GetResponse(string url)
        {
            var request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            
            var response = (HttpWebResponse)request.GetResponse();
            var dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);

            string responseFromServer = reader.ReadToEnd();

            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }

        private string GetTodaysMarriageTip(string content)
        {
            var tipMarker = "<div class='dmt'><p>";
            var dmt = content.IndexOf(tipMarker);

            if (dmt == -1)
            {
                return string.Empty;
            }

            var closingParagraphTagIndex = content.IndexOf("</p>", dmt);
            return content.Substring(dmt + tipMarker.Length, closingParagraphTagIndex - dmt - tipMarker.Length);
        }
    }
}