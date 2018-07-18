using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demo1.Bots
{
    public class SimpleBot : IBot
    {
        public async Task OnTurn(ITurnContext turnContext)
        {
            if (turnContext.Activity.Type is ActivityTypes.Message)
            {
                string input = turnContext.Activity.Text;
                await turnContext.SendActivity($"SimpleBot: {input}");
            }
        }
    }
}
