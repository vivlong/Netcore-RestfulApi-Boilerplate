using System;
using ServiceStack;
using Api.ServiceModel;

namespace Api.ServiceInterface
{
    public class HelloServices : Service
    {
        public object Any(Hello request)
        {
            return new HelloResponse { Result = $"Hello, {request.Name}!" };
        }
    }
}
