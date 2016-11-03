using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prost.Http
{
    public class HttpStatus
    {
        private ushort code;
        private string message;

        public HttpStatus(ushort code) : this(code, "Unknow") { }

        public HttpStatus(ushort code, string message)
        {
            this.code = code;
            this.message = message;
        }

        public String Message { get { return this.message; } }
        public UInt16 Code { get { return this.code; } }

        public static HttpStatus Continue { get { return new HttpStatus(100, "Continue"); } }
        public static HttpStatus SwitchingProtocols { get { return new HttpStatus(101, "Switching Protocols"); } }
        public static HttpStatus Processing { get { return new HttpStatus(102, "Processing"); } }
        public static HttpStatus OK { get { return new HttpStatus(200, "OK"); } }
        public static HttpStatus Created { get { return new HttpStatus(201, "Created"); } }
        public static HttpStatus Accepted { get { return new HttpStatus(202, "Accepted"); } }
        public static HttpStatus NonAuthoritativeInformation { get { return new HttpStatus(203, "Non-Authoritative Information"); } }
        public static HttpStatus NoContent { get { return new HttpStatus(204, "No Content"); } }
        public static HttpStatus ResetContent { get { return new HttpStatus(205, "Reset Content"); } }
        public static HttpStatus PartialContent { get { return new HttpStatus(206, "Partial Content"); } }
        public static HttpStatus MultipleChoices { get { return new HttpStatus(300, "Multiple Choices"); } }
        public static HttpStatus MovedPermanently { get { return new HttpStatus(301, "Moved Permanently"); } }
        public static HttpStatus Found { get { return new HttpStatus(302, "Found"); } }
        public static HttpStatus SeeOther { get { return new HttpStatus(303, "See Other"); } }
        public static HttpStatus NotModified { get { return new HttpStatus(304, "Not Modified"); } }
        public static HttpStatus UseProxy { get { return new HttpStatus(305, "Use Proxy"); } }
        public static HttpStatus TemporaryRedirect { get { return new HttpStatus(307, "Temporary Redirect"); } }
        public static HttpStatus BadRequest { get { return new HttpStatus(400, "Bad Request"); } }
        public static HttpStatus Unauthorized { get { return new HttpStatus(401, "Unauthorized"); } }
        public static HttpStatus PaymentRequired { get { return new HttpStatus(402, "Payment Required"); } }
        public static HttpStatus Forbidden { get { return new HttpStatus(403, "Forbidden"); } }
        public static HttpStatus NotFound { get { return new HttpStatus(404, "Not Found"); } }
        public static HttpStatus MethodNotAllowed { get { return new HttpStatus(405, "Method Not Allowed"); } }
        public static HttpStatus NotAcceptable { get { return new HttpStatus(406, "Not Acceptable"); } }
        public static HttpStatus ProxyAuthenticationRequired { get { return new HttpStatus(407, "Proxy Authentication Required"); } }
        public static HttpStatus RequestTimeout { get { return new HttpStatus(408, "Request Time-out"); } }
        public static HttpStatus Conflict { get { return new HttpStatus(409, "Conflict"); } }
        public static HttpStatus Gone { get { return new HttpStatus(410, "Gone"); } }
        public static HttpStatus LengthRequired { get { return new HttpStatus(411, "Length Required"); } }
        public static HttpStatus PreconditionFailed { get { return new HttpStatus(412, "Precondition Failed"); } }
        public static HttpStatus PayloadTooLarge { get { return new HttpStatus(413, "Payload Too Large"); } }
        public static HttpStatus URITooLong { get { return new HttpStatus(414, "URI Too Long"); } }
        public static HttpStatus UnsupportedMediaType { get { return new HttpStatus(415, "Unsupported Media Type"); } }
        public static HttpStatus RangeNotSatisfiable { get { return new HttpStatus(416, "Range Not Satisfiable"); } }
        public static HttpStatus ExpectationFailed { get { return new HttpStatus(417, "Expectation Failed"); } }
        public static HttpStatus ImATeapot { get { return new HttpStatus(418, "I'm a teapot"); } }
        public static HttpStatus MisdirectedRequest { get { return new HttpStatus(421, "Misdirected Request"); } }
        public static HttpStatus UpgradeRequired { get { return new HttpStatus(426, "Upgrade Required"); } }
        public static HttpStatus PreconditionRequired { get { return new HttpStatus(428, "Precondition Required"); } }
        public static HttpStatus TooManyRequests { get { return new HttpStatus(429, "Too Many Requests"); } }
        public static HttpStatus RequestHeaderFieldsTooLarge { get { return new HttpStatus(431, "Request Header Fields Too Large"); } }
        public static HttpStatus InternalServerError { get { return new HttpStatus(500, "Internal Server Error"); } }
        public static HttpStatus NotImplemented { get { return new HttpStatus(501, "Not Implemented"); } }
        public static HttpStatus BadGateway { get { return new HttpStatus(502, "Bad Gateway"); } }
        public static HttpStatus ServiceUnavailable { get { return new HttpStatus(503, "Service Unavailable"); } }
        public static HttpStatus GatewayTimeout { get { return new HttpStatus(504, "Gateway Time-out"); } }
        public static HttpStatus HTTPVersionNotSupported { get { return new HttpStatus(505, "HTTP Version Not Supported"); } }
        public static HttpStatus NotExtended { get { return new HttpStatus(510, "Not Extended"); } }
    }
}
