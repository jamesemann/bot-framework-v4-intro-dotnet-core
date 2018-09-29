using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace demo2mysimplemiddleware.Middleware
{
    public class SimpleMiddleware1 : IMiddleware
    {
        public async Task OnTurnAsync(ITurnContext context, NextDelegate next, CancellationToken cancellationToken = default(CancellationToken))
        {
            await context.SendActivityAsync($"[SimpleMiddleware1] {context.Activity.Type}/OnTurn/Before");

            await next(cancellationToken);

            await context.SendActivityAsync($"[SimpleMiddleware1] {context.Activity.Type}/OnTurn/After");
        }
    }
}
