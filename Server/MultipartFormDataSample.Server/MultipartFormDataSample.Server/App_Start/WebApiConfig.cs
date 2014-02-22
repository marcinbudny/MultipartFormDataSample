using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MultipartFormDataSample.Server.Utis;
using Newtonsoft.Json.Serialization;

namespace MultipartFormDataSample.Server
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Formatters.Add(new ImageSetMediaTypeFormatter());
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
