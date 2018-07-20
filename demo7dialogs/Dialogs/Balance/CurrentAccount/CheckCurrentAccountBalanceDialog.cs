using Microsoft.Bot.Builder.Dialogs;

namespace demo7dialogs.Dialogs.Balance.CurrentAccount
{
    public class CheckCurrentAccountBalanceDialog : DialogContainer
    {
        public CheckCurrentAccountBalanceDialog() : base(Id)
        {
            Dialogs.Add(Id, new WaterfallStep[]
            {
                async (dc, args, next) =>
                {
                    await dc.Context.SendActivity($"[CheckCurrentAccountBalanceDialog] Your current account balance is £2000");
                    await dc.End();
                }
            });
        }

        public static string Id => "checkCurrentAccountBalanceDialog";
        public static CheckCurrentAccountBalanceDialog Instance { get; } = new CheckCurrentAccountBalanceDialog();
    }
}