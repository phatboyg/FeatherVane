namespace Rachis
{
    using Topshelf;


    class Program
    {
        static int Main()
        {
            return (int)HostFactory.Run(x => x.Service<HttpService>());
        }
    }
}