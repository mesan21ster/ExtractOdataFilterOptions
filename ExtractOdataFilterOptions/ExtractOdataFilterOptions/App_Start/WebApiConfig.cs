namespace WebAPITest
{
    using System.Web.Http;
    using Microsoft.OData.Edm;
    using WebAPITest.Models;
    using Microsoft.AspNet.OData.Builder;
    using Microsoft.AspNet.OData.Extensions;

    public static class WebApiConfig
    {
        private static IEdmModel GenerateEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Company>("companyProcessing");
            return builder.GetEdmModel();
        }

        public static void Register(HttpConfiguration config)
        {

            ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<Company>("companyProcessing"); // We are exposing only Movies via oData
            config.MapODataServiceRoute("company", "odata", modelBuilder.GetEdmModel()); // Specify the 

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
