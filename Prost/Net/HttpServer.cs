using Prost.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Prost.Net
{
    public class HttpServer : IDisposable
    {
        public static readonly char[] HTTP = { (char)0x48, (char)0x54, (char)0x54, (char)0x50 };
        
        // Variables del servidor
        protected TcpListener socket;
        private Thread listenerThread;
        protected IPAddress conf_localaddr;
        protected UInt16 conf_port;

        // Variables de configuración de las peticiones
        protected ulong conf_maxconnections = 20;
        protected ulong conf_maxexecutiontime = 20000;

        // Manejadores de las conexiones HTTP entrantes
        private List<HttpHandler> handlers = new List<HttpHandler>();

        // Flag interno para evitar intentos de recreación del socket
        private bool listen_flag = false;

        /// <summary>
        /// Inicia la escucha del servidor
        /// </summary>
        public void Listen() { this.Listen(IPAddress.Any, 80); }
        public void Listen(UInt16 port) { this.Listen(IPAddress.Any, port); }
        public void Listen(IPAddress localaddr, UInt16 port)
        {
            if (this.listen_flag) return;

            this.listen_flag = true;

            this.conf_localaddr = localaddr;
            this.conf_port = port;

            this.listenerThread = new Thread(new ThreadStart(this.ProcessListen));
            this.listenerThread.Start();
        }

        public Boolean Listening { get { return listen_flag; } }

        protected bool stop_flag = false;
        private void ProcessListen()
        {
            this.socket = new TcpListener(this.conf_localaddr, this.conf_port);
            this.socket.Start();

            try
            {
                while (!this.stop_flag)
                {
                    TcpClient client = this.socket.AcceptTcpClient();
                    //ProcessRequest(client.GetStream(), (IPEndPoint)client.Client.RemoteEndPoint);
                }
            }
            catch (ThreadAbortException) { }
            catch (SocketException) { }
        }

        public void ProcessRequest(Stream connStream, IPEndPoint origin)
        {
            using (StreamReader sr = new StreamReader(connStream, Encoding.ASCII))
            using (StreamWriter sw = new StreamWriter(connStream, Encoding.ASCII))
            {
                bool keepalive = true;
                while (keepalive)
                {
                    HttpRequest request = null;
                    HttpResponse response = null;

                    try
                    {
                        keepalive = false; // TODO: Implement keep-alive
                        request = new HttpRequest(origin, sr);
                        response = new HttpResponse(sw);

                        foreach (HttpHandler handler in this.handlers)
                        {
                            if (!handler(request, response)) break;
                        }
                    }
                    catch (ArgumentNullException r)
                    {
                        // Se ha perdido la conexion
                        Console.WriteLine("conexion interrumpida");
                        if (response != null) { response.Close(); }
                    }
                }
            }
        }

        private void ProcessStop()
        {
            this.stop_flag = true;
            this.socket.Stop();
            this.listenerThread.Abort();
            this.listenerThread.Join();
        }

        public void Close()
        {
            this.ProcessStop();
        }

        public void Dispose()
        {
            this.ProcessStop();
        }

        public void AddHandler(HttpHandler handler)
        {
            this.handlers.Add(handler);
        }
    }
}
