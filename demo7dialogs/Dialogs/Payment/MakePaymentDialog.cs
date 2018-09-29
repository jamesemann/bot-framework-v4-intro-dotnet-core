using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;

namespace demo7dialogs.Dialogs.Payment
{
    public class MakePaymentDialog : WaterfallDialog
    {
        public MakePaymentDialog(string dialogId, IEnumerable<WaterfallStep> steps = null) : base(dialogId, steps)
        {
            AddStep(async (stepContext, cancellationToken) =>
            {
                return await stepContext.PromptAsync("textPrompt",
                    new PromptOptions
                    {
                        Prompt = stepContext.Context.Activity.CreateReply("Who would you like to pay?")
                    });
            });

            AddStep(async (stepContext, cancellationToken) =>
            {
                var state = await (stepContext.Context.TurnState["BotAccessors"] as BotAccessors).BankingBotStateStateAccessor.GetAsync(stepContext.Context);
                state.Recipient = stepContext.Result.ToString();

                return await stepContext.PromptAsync("numberPrompt",
                    new PromptOptions
                    {
                        Prompt = stepContext.Context.Activity.CreateReply($"{state.Recipient}, got it{Environment.NewLine}How much should I pay?"),
                        RetryPrompt = stepContext.Context.Activity.CreateReply("Sorry, please give me a number.")
                    });
            });

            AddStep(async (stepContext, cancellationToken) =>
            {
                var state = await (stepContext.Context.TurnState["BotAccessors"] as BotAccessors).BankingBotStateStateAccessor.GetAsync(stepContext.Context);
                state.Amount = int.Parse(stepContext.Result.ToString());

                await stepContext.Context.SendActivityAsync($"Thank you, I've paid {state.Amount} to {state.Recipient} 💸");
                return await stepContext.EndDialogAsync();
            });
        }

        public static string Id => "makePaymentDialog";
        public static MakePaymentDialog Instance { get; } = new MakePaymentDialog(Id);
    }
}