using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace demo7dialogs.Bots
{
    public class SimpleBotComponents : IBot
    {
        private DialogSet _dialogs;

        public SimpleBotComponents()
        {
            this._dialogs = new DialogSet();
            _dialogs.Add("mainDialog", MainDialog.Instance);
        }

        public async Task OnTurn(ITurnContext context)
        {
            if (context.Activity.Type == ActivityTypes.Message)
            {
                var state = context.GetConversationState<Dictionary<string, object>>();
                var dialogCtx = _dialogs.CreateContext(context, state);

                await dialogCtx.Continue();
                if (!context.Responded)
                {
                    // call next dialog#
                    await dialogCtx.Begin("mainDialog");
                }
            }
        }
    }
}