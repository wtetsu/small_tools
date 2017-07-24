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

                    string[] arr = line.Split('\t');
                    string key = arr[0];
                    string val = arr[1];

                    if (key.StartsWith("cookie:"))
                    {
                        string[] c = key.Split(':');
                        string name = c[1];

                        var newCookie = new Cookie(name, val);
                        result.Cookies.Add(newCookie);
                    }
                    else
                    {
                        switch (key)
                        {
                            case "url":
                                result.Url = val;
                                result.CookieUrl = result.Url.Substring(0, result.Url.IndexOf("/", 7));
                                break;
                            case "postdata":
                                result.PostData = val;
                                break;
                        }
                    }
                }
            }
            return result;
        }
    }
}
