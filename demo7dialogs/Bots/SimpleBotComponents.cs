using System.Collections.Generic;
using System.Threading.Tasks;
using demo7dialogs.Dialogs;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace demo7dialogs.Bots
{
    public class SimpleBotComponents : IBot
    {
        private readonly DialogSet dialogs;

        public SimpleBotComponents()
        {
            // compose dialogs
            dialogs = new DialogSet();
            dialogs.Add("mainDialog", MainDialog.Instance);
        }

        public async Task OnTurn(ITurnContext turnContext)
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var state = turnContext.GetConversationState<Dictionary<string, object>>();
                var dialogCtx = dialogs.CreateContext(turnContext, state);

                await dialogCtx.Continue();
                if (!turnContext.Responded)
                {
                    await dialogCtx.Begin("mainDialog");
                }
            }
        }
    }
}