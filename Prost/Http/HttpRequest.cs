using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;

namespace Prost.Http
{
    public enum DoNotTrack : int { DntEnable = 1, DntUnknow = 0, DntDisable = -1 }

    public class HttpRequest
    {
        private readonly long maxExecutionTime;

        private ushort httpMajorVersion;
        private ushort httpMinorVersion;
        private HttpMethod httpMethod = HttpMethod.Head;
        private String httpPath;

        private Dictionary<string, string> httpHeaders = new Dictionary<string, string>();
        private DoNotTrack httpDnt = DoNotTrack.DntUnknow;
        private string httpUserAgent = "";

<<<<<<< HEAD
        internal HttpRequest(IPEndPoint client, StreamReader reader, long limit = 20000)
=======
		private bool finishRequest = false;
		public bool Finished { get { return this.finishRequest; } }

        internal HttpRequest(IPEndPoint client, StreamReader reader)
>>>>>>> origin/master
        {
            this.maxExecutionTime = limit;

            // Parse top header
            IEnumerable<char> header = reader.ReadLine();

            char[] method = header.TakeWhile((ichar) => !HttpLinqParser.IsSpace(ichar)).ToArray();
            switch ((new String(method)).ToUpper())
            {
                case "DELETE":
                    httpMethod = HttpMethod.Delete;
                    break;
                case "GET":
                    this.httpMethod = HttpMethod.Get;
                    break;
                case "HEAD":
                    this.httpMethod = HttpMethod.Head;
                    break;
                case "PATCH":
                    this.httpMethod = HttpMethod.Patch;
                    break;
                case "POST":
                    this.httpMethod = HttpMethod.Post;
                    break;
                case "PUT":
                    this.httpMethod = HttpMethod.Put;
                    break;
                default:
                    throw new Exception("Method error");
            }

            header = header.SkipWhile((ichar) => !HttpLinqParser.IsSpace(ichar)).Skip(1);
            char[] path = header.TakeWhile((ichar) => !HttpLinqParser.IsSpace(ichar)).ToArray();
            this.httpPath = new string(path);

            header = header.SkipWhile((ichar) => !HttpLinqParser.IsSpace(ichar)).Skip(1);
            if (Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(header.Take(4).ToArray())) == "HTTP")
            {
                header = header.Skip(5);
                char[] major = header.TakeWhile((ichar) => HttpLinqParser.IsDigit(ichar)).ToArray();
                this.httpMajorVersion = Convert.ToUInt16(Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(major)));

                header = header.SkipWhile((ichar) => HttpLinqParser.IsDigit(ichar)).Skip(1);
                char[] minor = header.TakeWhile((ichar) => HttpLinqParser.IsDigit(ichar)).ToArray();
                this.httpMinorVersion = Convert.ToUInt16(Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(minor)));
            }
            else throw new Exception();

            // Parse the rest of the headers
            String line = String.Empty;

            do
            {
                line = reader.ReadLine();

                if (line.Length > 0)
                {
                    if (line.Contains(":"))
                    {
                        String[] hpair = line.Split(':');
                        this.httpHeaders.Add(hpair[0].ToLowerInvariant().Trim(), hpair[1].Trim());
                    }
                }
            }
            while (line.Length > 0);

            foreach (KeyValuePair<string, string> headerline in this.httpHeaders)
            {
                switch (headerline.Key)
                {
                    case "dnt":
                        if (headerline.Value == "1") this.httpDnt = DoNotTrack.DntEnable;
                        else if (headerline.Value == "0") this.httpDnt = DoNotTrack.DntDisable;
                        break;
                    case "user-agent":
                        this.httpUserAgent = headerline.Value;
                        break;
                }
            }
        }

        public ushort ProtocolMajorVersion { get { return this.httpMajorVersion; } }
        public ushort ProtocolMinorVersion { get { return this.httpMinorVersion; } }
        public HttpMethod Method { get { return this.httpMethod; } }
        public DoNotTrack DoNotTrack {  get { return this.httpDnt; } }
        public long MaxExecutionTime { get { return this.maxExecutionTime; } }

<<<<<<< HEAD
        public bool KeepAlive {  get { return false; } } // Not supported yet
=======
        // public long MaxExecutionTime
        // public bool KeepAlive

		/// <summary>
		/// End the request and inhibit the next handlers
		/// </summary>
		public void End () {
			this.finishRequest = true;
		}
>>>>>>> origin/master
    }
}
