using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Recognizers.Text;

namespace demo7dialogs.Dialogs.Payment
{
    public class MakePaymentDialog : DialogContainer
    {
        public MakePaymentDialog() : base(Id)
        {
            Dialogs.Add(Id, new WaterfallStep[]
            {
                async (dc, args, next) => { await dc.Prompt("textPrompt", "Who would you like to pay?"); },
                async (dc, args, next) =>
                {
                    var state = dc.Context.GetConversationState<Dictionary<string, object>>();
                    state["Recipient"] = (string) args["Value"];
                    await dc.Prompt("numberPrompt",
                        $"{state["Recipient"]}, got it{Environment.NewLine}How much should I pay?", new PromptOptions
                        {
                            RetryPromptString = "Sorry, please give me a number."
                        });
                },
                async (dc, args, next) =>
                {
                    var state = dc.Context.GetConversationState<Dictionary<string, object>>();
                    state["Amount"] = (int) args["Value"];
                    await dc.Context.SendActivity(
                        dc.Context.Activity.CreateReply(
                            $"Thank you, I've paid {state["Amount"]} to {state["Recipient"]} 💸"));
                    await dc.End();
                }
            });

            // add the prompts
            Dialogs.Add("textPrompt", new TextPrompt());
            Dialogs.Add("numberPrompt", new NumberPrompt<int>(Culture.English));
        }

        public static string Id => "makePaymentDialog";
        public static MakePaymentDialog Instance { get; } = new MakePaymentDialog();
    }
}