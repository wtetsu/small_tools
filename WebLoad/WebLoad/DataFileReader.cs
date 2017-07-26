using System.Collections.Generic;
using System.IO;

namespace WebLoad
{
    public class DataFileReader
    {
        public string DataFileSearchPattern { get; set; }

        public DataFileReader()
        {
            DataFileSearchPattern = "data*.txt";
        }

        public WebData[] LoadData(params string[] dataEntries)
        {
            var dataList = new List<WebData>();

            foreach (string dataEntry in dataEntries)
            {
                if (File.Exists(dataEntry))
                {
                    dataList.Add(WebData.ParseFile(dataEntry));
                }
                else if (Directory.Exists(dataEntry))
                {
                    dataList.AddRange(FindAndParseDataFilesInDirectory(dataEntry));
                }
                else
                {
                    throw new FileNotFoundException("File not found:" + dataEntry);
                }
            }

            return dataList.ToArray() ;
        }

        public WebData[] FindAndParseDataFilesInDirectory(string dirpath)
        {
            var dataList = new List<WebData>();

            foreach (string path in Directory.EnumerateFiles(dirpath, DataFileSearchPattern))
            {
                WebData newData = WebData.ParseFile(path);
                dataList.Add(newData);
            }
            return dataList.ToArray();
        }
    }
}
