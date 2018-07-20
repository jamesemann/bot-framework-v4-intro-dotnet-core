using Microsoft.Bot.Builder.Dialogs;

namespace demo7dialogs.Dialogs.Balance.SavingsAccount
{
    public class CheckSavingsAccountBalanceDialog : DialogContainer
    {
        public CheckSavingsAccountBalanceDialog() : base(Id)
        {
            Dialogs.Add(Id, new WaterfallStep[]
            {
                async (dc, args, next) =>
                {
                    await dc.Context.SendActivity($"[CheckCurrentAccountBalanceDialog] Your savings account balance is £5000");
                    await dc.End();
                }
            });
        }

        public static string Id => "checkSavingsAccountBalanceDialog";
        public static CheckSavingsAccountBalanceDialog Instance { get; } = new CheckSavingsAccountBalanceDialog();
    }
}