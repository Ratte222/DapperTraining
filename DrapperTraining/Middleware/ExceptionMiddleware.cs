using AuxiliaryLib.Exceprions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTraining.Middleware
{
    public class ExeptionMeddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExeptionMeddleware> _logger;

        public ExeptionMeddleware(RequestDelegate next, ILogger<ExeptionMeddleware> logger)
        {
            this._next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (AuxiliaryLib.Exceprions.ValidationException ex)
            {
                _logger.LogDebug(context.Request.RouteValues.ToString(), ex?.Message, ex?.StackTrace);
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(ex.Message);
            }
            //catch (SelectException ex)
            //{
            //    _logger.LogDebug(context.Request.RouteValues.ToString(), ex?.Message, ex?.StackTrace);
            //    context.Response.StatusCode = 404;
            //    await context.Response.WriteAsync(ex.Message);
            //}
            catch (Exception ex)
            {
                _logger.LogWarning(context.Request.RouteValues.ToString(), ex?.Message, ex?.StackTrace);
                context.Response.StatusCode = 500;
                //await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}
