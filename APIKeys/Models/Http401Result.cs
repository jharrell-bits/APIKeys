using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace APIKeys.Models
{
    public class Http401Result : ActionResult
    {
        private string _customMessage { get; set; } = string.Empty;

        public Http401Result() { }

        public Http401Result(string customMessage)
        {
            _customMessage = customMessage;
        }

        public override void ExecuteResult(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            if (!string.IsNullOrEmpty(_customMessage))
            {
                context.HttpContext.Response.ContentType = "text/plain";
                context.HttpContext.Response.WriteAsync(_customMessage).Wait();
            }
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            if (!string.IsNullOrEmpty(_customMessage))
            {
                context.HttpContext.Response.ContentType = "text/plain";
                await context.HttpContext.Response.WriteAsync(_customMessage);
            }
        }
    }
}
