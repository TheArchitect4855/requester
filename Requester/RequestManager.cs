using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Requester
{
    public enum RequestMethod
    {
        GET = 0,
        POST, PUT,
        DELETE,
    }

    public class RequestManager
    { 
        public struct Response
        {
            public int statusCode;
            public string statusCodeName;
            public string content;
        }

        private static RequestManager singleton = null;

        public static RequestManager Get()
        {
            if(singleton == null)
            {
                singleton = new RequestManager();
            }

            return singleton;
        }

        private HttpClient client;

        private RequestManager()
        {
            client = new HttpClient();
        }

        public async Task<Response> SendRequest(string target, RequestMethod method, HttpContent body)
        {
            HttpResponseMessage httpResponse = null;

            try { 
                switch (method)
                {
                    case RequestMethod.DELETE:
                        httpResponse = await client.DeleteAsync(target); break;
                    case RequestMethod.GET:
                        httpResponse = await client.GetAsync(target); break;
                    case RequestMethod.POST:
                        httpResponse = await client.PostAsync(target, body); break;
                    case RequestMethod.PUT:
                        httpResponse = await client.PutAsync(target, body); break;
                }
            } catch(Exception e)
            {
                return new Response {
                    statusCode = 0,
                    statusCodeName = "ERROR",
                    content = e.Message
                };
            }

            Response response = new Response();

            HttpStatusCode responseStatus = httpResponse.StatusCode;
            response.statusCode = (int)responseStatus;
            response.statusCodeName = responseStatus.ToString();

            HttpContent responseContent = httpResponse.Content;
            response.content = await responseContent.ReadAsStringAsync();

            httpResponse.Dispose();
            return response;
        }
    }
}
