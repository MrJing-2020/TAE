using System.Web.Http;
using WebActivatorEx;
using TAE.WebServer;
using Swashbuckle.Application;
using TAE.WebServer.Providers;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace TAE.WebServer
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Web");
                    c.IncludeXmlComments(string.Format("{0}/bin/TAE.WebServer.XML", System.AppDomain.CurrentDomain.BaseDirectory));
                    c.IncludeXmlComments(string.Format("{0}/bin/TAE.Data.Model.XML", System.AppDomain.CurrentDomain.BaseDirectory));
                    c.CustomProvider((defaultProvider) => new CachingSwaggerProvider(defaultProvider));

                })
                .EnableSwaggerUi(c =>
                {
                    c.InjectStylesheet(thisAssembly, "TAE.WebServer.Content.bootstrap.min.css");
                    c.InjectStylesheet(thisAssembly, "TAE.WebServer.Content.cumSwagger.css");
                    c.InjectJavaScript(thisAssembly, "TAE.WebServer.Scripts.swagger_lang.js");
                    c.InjectJavaScript(thisAssembly, "TAE.WebServer.Scripts.bootstrap.min.js");
                });
        }
    }
}
