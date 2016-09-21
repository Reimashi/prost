using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prost.Http
{
    public delegate Boolean HttpMethodHandler(HttpRequest req, HttpResponse res);

    public interface HttpHandler
    {
        Boolean Delete(HttpRequest req, HttpResponse res);
        Boolean Get(HttpRequest req, HttpResponse res);
        Boolean Head(HttpRequest req, HttpResponse res);
        Boolean Patch(HttpRequest req, HttpResponse res);
        Boolean Post(HttpRequest req, HttpResponse res);
        Boolean Put(HttpRequest req, HttpResponse res);
    }
}
