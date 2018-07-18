using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demo3mysimplemiddleware.Middleware
{
    public class SimpleMiddleware : IMiddleware
    {
        public async Task OnTurn(ITurnContext context, MiddlewareSet.NextDelegate next)
        {
            await context.SendActivity($"Middleware running before, ActivityType:{context.Activity.Type} ");

            if (context.Activity.Type == ActivityTypes.Message && context.Activity.Text == "secret password") {
                // calling next() is totally optional. if the middleware does not call next then the
                // next middleware in the pipeline will not be called, AND the bot will not receive the message.
                //
                // in this instance, we are only handing the message to downstream bots if the user says "secret password"
                await next();
            }

            await context.SendActivity($"Middleware running after, ActivityType:{context.Activity.Type} ");
        }
    }
}
