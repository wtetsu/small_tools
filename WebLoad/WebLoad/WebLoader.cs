using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace WebLoad
{
    public class WebLoader
    {
        public string ContentType { get; set; }
        public uint? RepetitionNumber { get; set; }

        private WebData[] _data;
        

        public WebLoader()
        {
            Initialize();
        }

        public WebLoader(params string[] dataFiles)
        {
            Initialize();
            var dataFileReader = new DataFileReader();
            _data = dataFileReader.LoadData(dataFiles);
        }

        private void Initialize()
        {
            this.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
        }

        public void Start(WebData[] data)
        {
            var concatinatedData = new List<WebData>();
            concatinatedData.AddRange(_data);
            concatinatedData.AddRange(data);
            PostContinuously(concatinatedData.ToArray());
        }

        public void Start()
        {
            PostContinuously(_data);
        }

        private void PostContinuously(WebData[] datas)
        {
            uint i = 0;
            for (; ; )
            {
                if (i >= this.RepetitionNumber)
                {
                    break;
                }
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

                i += 1;
            }
        }

        public string Post(WebData data)
        {
            byte[] postDataBytes = Encoding.ASCII.GetBytes(data.PostData);

            var request = WebRequest.Create(data.Url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = this.ContentType;
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
