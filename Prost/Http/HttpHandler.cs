using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prost.Http
{
    /// <summary>
    /// Handle a HTTP connection.
    /// </summary>
    /// <param name="req">HTTP Request data</param>
    /// <param name="res">HTTP Response</param>
    /// <returns>FALSE to inhibit the propagation thought the handlers and send the response inmediatly</returns>
    public delegate bool HttpHandler(HttpRequest req, HttpResponse res);
}
