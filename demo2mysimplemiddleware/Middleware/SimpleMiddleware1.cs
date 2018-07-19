using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demo2mysimplemiddleware.Middleware
{
    public class SimpleMiddleware1 : IMiddleware
    {
        public async Task OnTurn(ITurnContext context, MiddlewareSet.NextDelegate next)
        {
            await context.SendActivity($"[SimpleMiddleware1] {context.Activity.Type}/OnTurn/Before");

            await next();

            await context.SendActivity($"[SimpleMiddleware1] {context.Activity.Type}/OnTurn/After");
        }
    }
}
