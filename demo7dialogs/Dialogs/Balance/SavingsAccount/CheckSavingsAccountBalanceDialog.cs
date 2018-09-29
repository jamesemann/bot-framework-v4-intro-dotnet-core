using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;

namespace demo7dialogs.Dialogs.Balance.SavingsAccount
{
    public class CheckSavingsAccountBalanceDialog : WaterfallDialog
    {
        public CheckSavingsAccountBalanceDialog(string dialogId, IEnumerable<WaterfallStep> steps = null) : base(dialogId, steps)
        {
            this.AddStep(async (stepContext, cancellationToken) =>
                {
                    await stepContext.Context.SendActivityAsync($"Your savings balance is...");
                    return await stepContext.EndDialogAsync();
                });
        }

        public static string Id => "checkSavingsAccountBalanceDialog";
        public static CheckSavingsAccountBalanceDialog Instance { get; } = new CheckSavingsAccountBalanceDialog(Id);
    }
}