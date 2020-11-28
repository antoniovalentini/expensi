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
                SwaggerJson = "http://localhost:5000/swagger/v1/swagger.json",
                Swagger = "http://localhost:5000/swagger",
                Test = "http://localhost:5000/api/expenses?userid=1"
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