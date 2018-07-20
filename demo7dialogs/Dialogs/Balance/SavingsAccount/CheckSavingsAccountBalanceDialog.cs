using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace demo7dialogs.Bots
{
    public class CheckSavingsAccountBalanceDialog : DialogContainer
    {
        public static string Id { get { return "checkSavingsAccountBalanceDialog"; } }
        public static CheckSavingsAccountBalanceDialog Instance { get; } = new CheckSavingsAccountBalanceDialog();
        public CheckSavingsAccountBalanceDialog() : base(Id)
        {
            this.Dialogs.Add(Id, new WaterfallStep[]
{
                async (dc, args, next) =>
                {
                    await dc.Context.SendActivity($"[CheckCurrentAccountBalanceDialog] Your savings account balance is £5000");
            //        await dc.Replace(MainDialog.Id);
                    await dc.End();
                }
});


        }
    }
}