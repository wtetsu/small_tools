using System;
using System.IO;
using System.Net;
using System.Text;

namespace WebLoad
{
    public class WebLoader
    {
        public string ContentType { get; set; }

        public WebLoader()
        {
            ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
        }

        public void Start(WebData[] datas)
        {
            for (; ; )
            {
                try
                {
                    foreach (var d in datas)
                    {
                        string response = Post(d);
                        Console.WriteLine(response.Length.ToString() + " byte");
                        System.Threading.Thread.Sleep(d.Sleep);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public string Post(WebData data)
        {
            byte[] postDataBytes = Encoding.ASCII.GetBytes(data.PostData);

            var request = WebRequest.Create(data.Url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = ContentType;
            request.ContentLength = postDataBytes.Length;
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Uri(data.CookieUrl), data.Cookies);

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(postDataBytes, 0, postDataBytes.Length);
            }

            var response = request.GetResponse() as HttpWebResponse;

            string responseString;
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                responseString = streamReader.ReadToEnd();
                Console.WriteLine(response.StatusCode);
            }
            return responseString;
        }
    }
}
