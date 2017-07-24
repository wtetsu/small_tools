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
    class Program
    {
        static void Main(string[] args)
        {
            string dataFilePath;

            WebData[] datas;

            if (args.Length >= 1)
            {
                dataFilePath = args[0];
                datas = new WebData[] { WebData.ParseFile(dataFilePath) };
            }
            else
            {
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
            }
        }

        static WebData[] FindAndParseDataFiles()
        {
            var dataList = new List<WebData>();

            foreach (string path in Directory.EnumerateFiles(".", "data*.txt"))
            {
                Console.WriteLine(path);
                WebData newData = WebData.ParseFile(path);
                dataList.Add(newData);
            }

            return dataList.ToArray();
        }

        static string Post(WebData data)
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
    }
}
