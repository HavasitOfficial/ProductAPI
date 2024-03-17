using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Product.Application.Helpers;
using System.Net;

namespace Product.Application.Middlewares
{
    public class ErrorHandlerMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception error)
            {
                HttpResponse response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {

                    case ValidationException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await HandleExceptions(response, error);
                        break;
                    case NotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        await HandleExceptions(response, error);
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await HandleExceptions(response, error);
                        break;
                }
            }
        }


        private async Task HandleExceptions(HttpResponse response, Exception exception)
        {
            var errorResponse = new ErrorResponse
            {
                ErrorType = env.IsDevelopment() ? exception.Message : null,
                StackTrace = env.IsDevelopment() ? exception.StackTrace : null
            };

            await CreateMessage(response, errorResponse);
        }

        private async Task CreateMessage(HttpResponse response, ErrorResponse errorResponse)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };

            jsonSerializerSettings.Converters.Add(new StringEnumConverter(typeof(CamelCaseNamingStrategy)) { AllowIntegerValues = false });

            var result = JsonConvert.SerializeObject(errorResponse, jsonSerializerSettings);
            await response.WriteAsync(result);
        }
    }
}
