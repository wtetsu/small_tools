using Microsoft.Web.Administration;
using System;

namespace RecyclePool
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var serverManager = new ServerManager())
            {
                foreach (Site site in serverManager.Sites)
                {
                    string poolName = site.Applications["/"].ApplicationPoolName;
                    ApplicationPool pool = serverManager.ApplicationPools[poolName];
                    pool.Recycle();
                    Console.WriteLine(site.Name);
                }
            }
        }
    }
}
