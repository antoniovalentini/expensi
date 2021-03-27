using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Avalentini.Expensi.Api.Controllers
{
    public class DefaultController : ControllerBase
    {
        [HttpGet]
        [Route("/")]
        public IActionResult Get()
        {
            return new JsonResult(new DefaultResult
            {
                SwaggerJson = "https://localhost:44358/swagger/v1/swagger.json",
                Swagger = "https://localhost:44358/swagger",
                Test = "https://localhost:44358/api/expenses?userid=1"
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