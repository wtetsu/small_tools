using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace WebLoad
{
    class Data
    {
        public string Url { get; set; }
        public string CookieUrl { get; set; }
        public string PostData { get; set; }
        public CookieCollection Cookies { get; set; }
    }



    class Program
    {
        static void Main(string[] args)
        {
            string dataFilePath;

            Data[] datas;

            if (args.Length >= 1)
            {
                dataFilePath = args[0];
                datas = new Data[] { ParseData(dataFilePath) };
            }
            else
            {
                //dataFilePath = Directory.GetCurrentDirectory() + "\\" + Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".txt";
                datas = FindAndParseDataFiles();
            }
            

            for (; ; )
            {
                try
                {
                    foreach (var d in datas)
                    {
                        string response = Post(d);
                        Console.WriteLine(response);
                        Console.WriteLine(response.Length.ToString() + " byte");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                //System.Threading.Thread.Sleep(1);
            }
        }

        static Data[] FindAndParseDataFiles()
        {
            var dataList = new List<Data>();

            foreach (string path in Directory.EnumerateFiles(".", "data*.txt"))
            {
                Console.WriteLine(path);
                Data newData = ParseData(path);
                dataList.Add(newData);
            }

            return dataList.ToArray();
        }

        static Data ParseData(string dataFile)
        {
            var result = new Data();
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

        static string Post(Data data)
        {
            Encoding enc = Encoding.GetEncoding("utf-8");

            byte[] postDataBytes = Encoding.ASCII.GetBytes(data.PostData);

            var request = WebRequest.Create(data.Url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.ContentLength = postDataBytes.Length;
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Uri(data.CookieUrl), data.Cookies);

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(postDataBytes, 0, postDataBytes.Length);
            }

            var response = request.GetResponse() as HttpWebResponse;

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            Console.WriteLine(response.StatusCode);

            return responseString;

        }


        //static string Post()
        //{
        //    Encoding enc = Encoding.GetEncoding("utf-8");

        //    byte[] postDataBytes = Encoding.ASCII.GetBytes(postData);

        //    var request = WebRequest.Create(url) as HttpWebRequest;
        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
        //    request.ContentLength = postDataBytes.Length;

        //    request.CookieContainer = new CookieContainer();
        //    request.CookieContainer.Add(new Uri("http://localhost/HangupTest/"), new Cookie("HangupTestApp", "9225976B689F722EEDA1D2D428AD503DD50564DCFADAB5B84BAEDDAA5668102B86D733776F9C66A1E7D5892E4054DB3134AC69E1792FE895BDAB77241D22264AEE88526F99858EE6EC3EFD446F03ADCF7830F93DC0C9C0B22E2B8A86381C70C55CBDE0CD4C2543294A079248372E60B50272F255488794B028C3849A359DD62F9D32C2DBD075D76249A667DDF4E6ABAB7A4F3A52294B74129B1C9E711DE40864"));

        //    using (Stream requestStream = request.GetRequestStream())
        //    {
        //        requestStream.Write(postDataBytes, 0, postDataBytes.Length);
        //    }

        //    var response = request.GetResponse() as HttpWebResponse;

        //    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

        //    Console.WriteLine(response.StatusCode);

        //    return responseString;
            
        //}
    }
}
