using System.Collections.Generic;
using System.Linq;
using demo7dialogs.Dialogs.Balance;
using demo7dialogs.Dialogs.Payment;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace demo7dialogs.Dialogs
{
    public class MainDialog : WaterfallDialog
    {
        public MainDialog(string dialogId, IEnumerable<WaterfallStep> steps = null) : base(dialogId, steps)
        {
            AddStep(async (stepContext, cancellationToken) =>
            {
                return await stepContext.PromptAsync("choicePrompt",
                    new PromptOptions
                    {
                        Prompt = stepContext.Context.Activity.CreateReply("[MainDialog] I'm banking 🤖{Environment.NewLine}Would you like to check balance or make payment?"),
                        Choices = new[] {new Choice {Value = "Check balance"}, new Choice {Value = "Make payment"}}.ToList()
                    });
            });
            AddStep(async (stepContext, cancellationToken) =>
            {
                var response = (stepContext.Result as FoundChoice)?.Value;

                if (response == "Check balance")
                {
                    return await stepContext.BeginDialogAsync(CheckBalanceDialog.Id);
                }

                if (response == "Make payment")
                {
                    return await stepContext.BeginDialogAsync(MakePaymentDialog.Id);
                }

                return await stepContext.NextAsync();
            });

            AddStep(async (stepContext, cancellationToken) => { return await stepContext.ReplaceDialogAsync(Id); });
        }


        public static string Id => "mainDialog";

        public static MainDialog Instance { get; } = new MainDialog(Id);
    }
}