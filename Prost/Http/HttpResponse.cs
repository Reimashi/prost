using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Prost.Http
{
    public delegate void HttpConnectionEvent(Object src, EventArgs asr);

    public class HttpResponse
    {
        public event HttpConnectionEvent OnClose;
        public event HttpConnectionEvent OnTimeout;
        public event HttpConnectionEvent OnFinnish;

        // Internal vars
        private StreamWriter stream = null;

        // Data
        private HttpStatus status = HttpStatus.InternalServerError;
        private Dictionary<string, string> headers = new Dictionary<string, string>();

        // Options
        private bool sendDate = false;

        // Flags
        private bool finished = false;
        private bool headersSent = false;

        public HttpResponse(StreamWriter sw)
        {
            this.stream = sw;
        }

        /// <summary>
        /// Close the connection internally
        /// </summary>
        internal void Close() { }

        /// <summary>
        /// Set the HTTP Status
        /// </summary>
        /// <param name="status">HTTP standard status</param>
        public void Status(HttpStatus status) {

        }

        /// <summary>
        /// Write a text to the output
        /// </summary>
        /// <param name="str">Text to write</param>
        public void Write(String str) { }
        public void Write(Byte[] str) { }

        /// <summary>
        /// Flush all data and close the connection
        /// </summary>
        public void End() { }

        public Boolean Finnished { get { return this.finished; } }
        public Boolean SendDate { get { return this.sendDate; } set { this.sendDate = value; } }
        public Boolean HeadersSent { get { return this.headersSent; } }
        public Dictionary<string, string> Headers { get { return this.headers; } }

        // public long MaxExecutionTime
    }
}
