using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts.Choices;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demo7dialogs.Bots
{
    public class SimpleBot : IBot
    {
        private DialogSet _dialogs;

        public SimpleBot()
        {
            this._dialogs = new DialogSet();
            _dialogs.Add("mainDialog", new WaterfallStep[] {
            async (dc, args, next) => {
                var options = new ChoicePromptOptions {
                    PromptString = $"[MainDialog] I'm banking 🤖{Environment.NewLine}Would you like to check balance or make payment?",
                    Choices = new List<Choice>(new []{
                        new Choice() { Value = "check balance" },
                        new Choice() { Value = "make payment" }  })   };
                await new ChoicePrompt("en").Begin(dc.Context, dc.ActiveDialog.State, options);
            },

            async (dc, args, next) => {
                 var activity = args["Activity"] as Activity;
                if (activity.Text == "check balance"){
                 await dc.Begin("checkBalanceDialog");
                }
                else if (activity.Text == "make payment")
                {
                    await dc.Begin("makePaymentDialog");
                }
            }
           });
            _dialogs.Add("checkBalanceDialog", new WaterfallStep[] {
            async (dc, args, next) => {
             await dc.Context.SendActivity($"checkBalanceDialog");
            }
           });
            _dialogs.Add("makePaymentDialog", new WaterfallStep[] {
            async (dc, args, next) => {
             await dc.Context.SendActivity($"makePaymentDialog");
            }
           });
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