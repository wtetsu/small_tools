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

        public WebData[] LoadData(params string[] dataFiles)
        {
            var dataList = new List<WebData>();

            foreach (string dataFile in dataFiles)
            {
                if (File.Exists(dataFile))
                {
                    dataList.Add(WebData.ParseFile(dataFile));
                }
                else if (Directory.Exists(dataFile))
                {
                    dataList.AddRange(FindAndParseDataFilesInDirectory("."));
                }
                else
                {
                    throw new FileNotFoundException("File not found:" + dataFile);
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
