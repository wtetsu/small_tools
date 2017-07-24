using System;
using System.Collections.Generic;
using System.IO;

namespace WebLoad
{
    class Program
    {
        static void Main(string[] args)
        {
            WebData[] data = LoadData(args);

            WebLoader loader = new WebLoader();
            loader.Start(data);
        }

        static WebData[] LoadData(string[] args)
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
            return datas;
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
    }
}
