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

        protected TcpListener socket;
        private Thread listenerThread;

        protected IPAddress conf_localaddr;
        protected UInt16 conf_port;

        private List<HttpHandler> handlers = new List<HttpHandler>();

        public void Listen() { this.Listen(IPAddress.Any, 80); }
        public void Listen(UInt16 port) { this.Listen(IPAddress.Any, port); }

        private bool listen_flag = false;
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

                    while (!ThreadPool.QueueUserWorkItem(new WaitCallback(this.ProcessConnection), client)) ;
                }
            }
            catch (ThreadAbortException) { }
            catch (SocketException) { }
        }

        protected void ProcessConnection(Object o)
        {
            TcpClient client = o as TcpClient;

            using (StreamReader sr = new StreamReader(client.GetStream(), Encoding.ASCII))
            using (StreamWriter sw = new StreamWriter(client.GetStream(), Encoding.ASCII))
            {
                bool keepalive = true;
                while (keepalive)
                {
                    HttpRequest request = null;
                    HttpResponse response = null;

                    try
                    {
                        keepalive = false; // TODO: Implement keep-alive
                        request = new HttpRequest((IPEndPoint)client.Client.RemoteEndPoint, sr);
                        response = new HttpResponse(sw);

                        foreach (HttpHandler handler in this.handlers)
                        {
							handler(request, response);
							if (request.Finished) break;
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

            client.Close();
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
