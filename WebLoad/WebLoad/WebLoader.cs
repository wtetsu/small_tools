using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebLoad
{
    public class WebLoader
    {
        public string ContentType { get; set; }
        public uint? RepetitionNumber { get; set; }
        public int ParallelNumber { get; set; }

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
            this.RepetitionNumber = null;
            this.ParallelNumber = 1;
        }

        public void Start(WebData[] data)
        {
            var concatinatedData = new List<WebData>();
            concatinatedData.AddRange(_data);
            concatinatedData.AddRange(data);
            StartCommon(concatinatedData.ToArray());
        }

        public void Start()
        {
            StartCommon(_data);
        }

        private void StartCommon(WebData[] data)
        {
            if (this.ParallelNumber == 1)
            {
                PostContinuously(data);
            }
            else
            {
                Parallel.For(0, this.ParallelNumber, j =>
                {
                    PostContinuously(data);
                });
            }
        }

        private void PostContinuously(WebData[] datas)
        {
            uint i = 0;
            for (; ; )
            {
                if (this.RepetitionNumber != null && this.RepetitionNumber <= i)
                {
                    break;
                }
                try
                {
                    foreach (var d in datas)
                    {
                        HttpWebResponse response = Post(d);

                        string responseString = null;
                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            responseString = streamReader.ReadToEnd();
                        }

                        int tid  = Thread.CurrentThread.ManagedThreadId;
                        string dt = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
                        string status = response.StatusCode.ToString();
                        string responseByte = responseString.Length.ToString() + " byte";

                        Console.WriteLine("[{0}][{1}]{2}:{3}", dt, tid, status, responseByte);
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

        public HttpWebResponse Post(WebData data)
        {
            var request = WebRequest.Create(data.Url) as HttpWebRequest;
            request.Method = data.Method;
            request.ContentType = this.ContentType;
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Uri(data.CookieUrl), data.Cookies);

            long contentLength = 0;
            if (data.PostData != null)
            {
                byte[] postDataBytes = Encoding.ASCII.GetBytes(data.PostData);
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(postDataBytes, 0, postDataBytes.Length);
                }
                contentLength = postDataBytes.Length;
            }
            request.ContentLength = contentLength;

            var response = request.GetResponse() as HttpWebResponse;
            return response;
        }
    }
}
