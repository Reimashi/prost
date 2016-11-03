using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;

namespace Prost.Http
{
    public class HttpRequest
    {
        public enum DoNotTrack : int { DntEnable = 1, DntUnknow = 0, DntDisable = -1 }

        private ushort httpMajorVersion;
        private ushort httpMinorVersion;
        private HttpMethod httpMethod = HttpMethod.Head;
        private String httpPath;

        private Dictionary<string, string> httpHeaders = new Dictionary<string, string>();
        private DoNotTrack httpDnt = DoNotTrack.DntUnknow;
        private string httpUserAgent = String.Empty;

        internal HttpRequest(IPEndPoint client, StreamReader reader)
        {
            this.ReadHttpHeader(reader.ReadLine());
            this.ReadHeaders(reader);
            this.FormatHeaders();
            
        }

        private void ReadHttpHeader(IEnumerable<char> header)
        {
            char[] method = header.TakeWhile((ichar) => !HttpLinqParser.IsSpace(ichar)).ToArray();
            switch((new String(method)).ToUpper())
            {
                case "DELETE":
                    this.httpMethod = HttpMethod.Delete;
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
        }

        private void ReadHeaders(StreamReader reader)
        {
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
        }

        private void FormatHeaders()
        {
            foreach (KeyValuePair<string, string> header in this.httpHeaders) {
                switch(header.Key)
                {
                    case "dnt":
                        if (header.Value == "1") this.httpDnt = DoNotTrack.DntEnable;
                        else if (header.Value == "0") this.httpDnt = DoNotTrack.DntDisable;
                        break;
                    case "user-agent":
                        this.httpUserAgent = header.Value;
                        break;
                }
            }
        }

        public ushort ProtocolMajorVersion { get { return this.httpMajorVersion; } }
        public ushort ProtocolMinorVersion { get { return this.httpMinorVersion; } }
        public HttpMethod Method { get { return this.httpMethod; } }

        // public long MaxExecutionTime
        // public bool KeepAlive
    }
}
