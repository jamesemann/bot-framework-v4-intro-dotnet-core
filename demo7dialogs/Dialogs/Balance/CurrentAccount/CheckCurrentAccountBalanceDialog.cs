using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace demo7dialogs.Bots
{
    public class CheckCurrentAccountBalanceDialog : DialogContainer
    {
        public static string Id { get { return "checkCurrentAccountBalanceDialog"; } }
        public static CheckCurrentAccountBalanceDialog Instance { get; } = new CheckCurrentAccountBalanceDialog();
        public CheckCurrentAccountBalanceDialog() : base(Id)
        {
            this.Dialogs.Add(Id, new WaterfallStep[]
{
                async (dc, args, next) =>
                {
                    await dc.Context.SendActivity($"[CheckCurrentAccountBalanceDialog] Your current account balance is £2000");
                    //await dc.Continue();
                    await dc.End();
                    //..await dc.cpm();
                }
});
        }
    }
}