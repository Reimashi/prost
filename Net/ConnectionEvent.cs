using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Prost.Net
{
    public delegate void ConnectionEventHandler(object sender, ConnectionEventArgs e);

    public class ConnectionEventArgs : EventArgs
    {
        private Stream stream;
        private IPEndPoint address;

        public ConnectionEventArgs(IPEndPoint remote, Stream st) : base()
        {
            this.address = remote;
            this.stream = st;
        }

        public Stream Stream { get { return this.stream; } }

        public IPEndPoint Address { get { return this.address; } }
    }
}
