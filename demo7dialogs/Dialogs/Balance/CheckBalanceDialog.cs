using System.Collections.Generic;
using System.Linq;
using demo7dialogs.Dialogs.Balance.CurrentAccount;
using demo7dialogs.Dialogs.Balance.SavingsAccount;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace demo7dialogs.Dialogs.Balance
{
    public class CheckBalanceDialog : WaterfallDialog
    {
        public CheckBalanceDialog(string dialogId, IEnumerable<WaterfallStep> steps = null) : base(dialogId, steps)
        {
            AddStep(async (stepContext, cancellationToken) =>
            {
                return await stepContext.PromptAsync("choicePrompt",
                    new PromptOptions
                    {
                        Prompt = stepContext.Context.Activity.CreateReply($"[CheckBalanceDialog] Which account?"),
                        Choices = new[] {new Choice {Value = "Current"}, new Choice {Value = "Savings"}}.ToList()
                    });
            });

            AddStep(async (stepContext, cancellationToken) =>
            {
                var response = stepContext.Result as FoundChoice;

                if (response.Value == "Current")
                {
                    return await stepContext.BeginDialogAsync(CheckCurrentAccountBalanceDialog.Id);
                }

                if (response.Value == "Savings")
                {
                    return await stepContext.BeginDialogAsync(CheckSavingsAccountBalanceDialog.Id);
                }

                return await stepContext.NextAsync();
            });
        }

        public static string Id => "checkBalanceDialog";
        public static CheckBalanceDialog Instance { get; } = new CheckBalanceDialog(Id);
    }
}