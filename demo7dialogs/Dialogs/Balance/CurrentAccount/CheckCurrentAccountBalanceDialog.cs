using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;

namespace demo7dialogs.Dialogs.Balance.CurrentAccount
{
    public class CheckCurrentAccountBalanceDialog : WaterfallDialog
    {
        public CheckCurrentAccountBalanceDialog(string dialogId, IEnumerable<WaterfallStep> steps = null) : base(dialogId, steps)
        {
            AddStep(async (stepContext, cancellationToken) =>
            {
                await stepContext.Context.SendActivityAsync($"Your current balance is...");
                return await stepContext.EndDialogAsync();
            });
        }

        public static string Id => "checkCurrentAccountBalanceDialog";
        public static CheckCurrentAccountBalanceDialog Instance { get; } = new CheckCurrentAccountBalanceDialog(Id);
    }
}