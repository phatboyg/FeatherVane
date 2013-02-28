namespace Rachis
{
    using System;
    using System.Text;
    using FeatherVane;
    using FeatherVane.Vanes;
    using FeatherVane.Web.Http;
    using FeatherVane.Web.Http.Feathers;
    using Topshelf;
    using Util;


    public class HttpService :
        ServiceControl
    {
        HttpServer _server;
        protected Uri ServerUri { get; private set; }

        public bool Start(HostControl hostControl)
        {
            Console.WriteLine("Configuring handlers...");

            Vane<ConnectionContext> vane = CreateMainVane();

            ServerUri = new Uri("http://localhost:8008/HttpService");

            Console.WriteLine("Creating HTTP server at: {0}", ServerUri);

            _server = new HttpServer(ServerUri, vane);

            Console.WriteLine("Starting HTTP server...");
            _server.Start();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            Console.WriteLine("Stopping HTTP server...");
            _server.Stop();

            Console.WriteLine("Maximum Concurrent Connections: {0}", _server.MaxConnections);

            return true;
        }


        Vane<ConnectionContext> CreateMainVane()
        {
            return VaneFactory.Connect(new UnhandledVane<ConnectionContext>(),
                new HelloFeather(),
                new NotFoundFeather());
        }


        class HelloFeather :
            Feather<ConnectionContext>
        {
            public void Compose(Composer composer,
                Payload<ConnectionContext> payload, Vane<ConnectionContext> next)
            {
                if (payload.Data.Request.Url.ToString().EndsWith("hello"))
                {
                    composer.Execute(() =>
                        {
                            payload.Data.Response.StatusCode = 200;

                            byte[] data = Encoding.UTF8.GetBytes("Hello!");
                            payload.Data.Response.BodyStream.Write(data, 0, data.Length);
                        });
                }
                else
                    next.Compose(composer, payload);
            }
        }
    }
}