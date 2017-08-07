using System.IO;
using System.Net;

namespace WebLoad
{
    public class WebData
    {
        public string Url { get; set; }
        public string CookieUrl { get; set; }
        public string PostData { get; set; }
        public CookieCollection Cookies { get; set; }
        public int Sleep { get; set; }
        public string Method { get; set; }

        public WebData()
        {
            this.Method = "POST";
        }

        public static WebData ParseFile(string dataFile)
        {
            var result = new WebData();
            result.Cookies = new CookieCollection();
            using (var reader = new StreamReader(dataFile))
            {
                for (; ; )
                {
                    string line = reader.ReadLine();

                    if (line == null)
                    {
                        break;
                    }

                    ParseLine(line, ref result);
                }
            }
            return result;
        }

        private static void ParseLine(string line, ref WebData rWebData)
        {
            string[] arr = line.Split('\t');

            if (arr.Length < 2)
            {
                return;
            }

            string key = arr[0];
            string val = arr[1];

            if (key.StartsWith("cookie:"))
            {
                string[] c = key.Split(':');
                string name = c[1];

                var newCookie = new Cookie(name, val);
                rWebData.Cookies.Add(newCookie);
            }
            else
            {
                switch (key)
                {
                    case "url":
                        rWebData.Url = val;
                        rWebData.CookieUrl = val.Substring(0, val.IndexOf("/", 7) + 1);
                        break;
                    case "postdata":
                        rWebData.PostData = val;
                        break;
                    case "sleep":
                        rWebData.Sleep = int.Parse(val);
                        break;
                    case "method":
                        rWebData.Method = val;
                        break;
                }
            }

        }
    }
}
