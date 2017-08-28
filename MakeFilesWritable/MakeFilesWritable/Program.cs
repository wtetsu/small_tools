using System;
using System.Collections.Generic;
using System.IO;

namespace MakeFilesWritable
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Body(args);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static void Body(string[] args)
        {
            string target = args[0];

            var exts = new HashSet<string> { ".cs", ".csproj", ".sln", ".resx", ".txt", };

            foreach (string path in Directory.EnumerateFiles(target, "*.*", SearchOption.AllDirectories))
            {
                string ext = Path.GetExtension(path);

                if (exts.Contains(ext))
                {
                    var finfo = new FileInfo(path);
                    if (finfo.IsReadOnly)
                    {
                        Console.WriteLine(path);
                        finfo.IsReadOnly = false;
                    }
                }
            }
        }
    }
}
