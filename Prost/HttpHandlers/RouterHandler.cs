using Prost.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prost.HttpHandlers
{
    public class RouterHandler : HttpHandler
    {
        public void AddHandler(String path, HttpHandler hand)
        {

        }

        public void AddHandler(String path, HttpMethod method, HttpMethodHandler hand)
        {

        }

        public bool Delete(HttpRequest req, HttpResponse res)
        {
            throw new NotImplementedException();
        }

        public bool Get(HttpRequest req, HttpResponse res)
        {
            throw new NotImplementedException();
        }

        public bool Head(HttpRequest req, HttpResponse res)
        {
            throw new NotImplementedException();
        }

        public bool Patch(HttpRequest req, HttpResponse res)
        {
            throw new NotImplementedException();
        }

        public bool Post(HttpRequest req, HttpResponse res)
        {
            throw new NotImplementedException();
        }

        public bool Put(HttpRequest req, HttpResponse res)
        {
            throw new NotImplementedException();
        }
    }
}
