
namespace WebLoad
{
    class Program
    {
        static void Main(string[] args)
        {
            WebLoader loader;

            if (args.Length >= 1)
            {
                loader = new WebLoader(args);
            }
            else
            {
                loader = new WebLoader(".");
            }

            loader.Start();
        }
    }
}
