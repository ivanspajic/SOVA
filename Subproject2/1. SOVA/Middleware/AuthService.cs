using System.Threading.Tasks;
using _3._Data_Layer.Database_Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using _2._Data_Layer_Abstractions;

namespace WebServiceSimple.Middleware
{
    // TODO: This service is not complete.
    public class AuthService
    {
        private readonly RequestDelegate _next;

        public AuthService(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var auth = context.Request.Headers["Authorization"];
            if (auth != StringValues.Empty)
            {
            }
            await _next(context);
        }
    }
}
