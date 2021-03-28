using Avalentini.Expensi.Core.Misc;
using Microsoft.AspNetCore.Mvc;

namespace Avalentini.Expensi.Api.Controllers
{
    public class DefaultController : ControllerBase
    {
        [HttpGet]
        [Route("/")]
        public IActionResult Get()
        {
            var endpoint = Endpoints.GetEndpoint();
            
            return new JsonResult(new DefaultResult
            {
                SwaggerJson = Endpoints.UrlCombine(endpoint, "/swagger/v1/swagger.json"),
                Swagger = Endpoints.UrlCombine(endpoint, "/swagger"),
                Test = Endpoints.UrlCombine(endpoint, "/api/expenses?userid=1"),
            });
        }
    }

    public class DefaultResult
    {
        public string Swagger { get; set; }
        public string SwaggerJson { get; set; }
        public string Test { get; set; }
    }
}
