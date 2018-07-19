using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demo3qnamakermiddleware.Bots
{
    public class SimpleBot : IBot
    {
        public async Task OnTurn(ITurnContext turnContext)
        {
            if (!turnContext.Responded && turnContext.Activity.Type == ActivityTypes.Message)
            {
                await turnContext.SendActivity($"Sorry, I don't know how to answer that...");
            }
        }
    }
}
