using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Prost.Net
{
    public class HttpsServer : HttpServer
    {
        private X509Certificate2 certificate;

        public new void Listen() { /* TODO: throw */ }
        public new void Listen(IPAddress localaddr, UInt16 port) { /* TODO: throw */ }
        public void Listen(IPAddress localaddr, UInt16 port, X509Certificate2 cert)
        {
            base.Listen(localaddr, port);
            this.certificate = cert;
        }

        private void ProcessListen()
        {
            this.socket = new TcpListener(this.conf_localaddr, this.conf_port);

            try
            {
                while (!this.stop_flag)
                {
                    TcpClient client = this.socket.AcceptTcpClient();
                    using (SslStream sslStream = new SslStream(client.GetStream(), false))
                    {
                        sslStream.AuthenticateAsServer(this.certificate, false, SslProtocols.Tls, false);
                        //while (!ThreadPool.QueueUserWorkItem(new WaitCallback(this.ProcessConnection), client)) ;
                    }
                }
            }
            catch (ThreadAbortException) { }
            catch (SocketException) { }
        }
    }
}