using System.Linq;
using demo7dialogs.Dialogs.Balance.CurrentAccount;
using demo7dialogs.Dialogs.Balance.SavingsAccount;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts.Choices;
using Microsoft.Recognizers.Text;

namespace demo7dialogs.Dialogs.Balance
{
    public class CheckBalanceDialog : DialogContainer
    {
        public CheckBalanceDialog() : base(Id)
        {
            Dialogs.Add(Id, new WaterfallStep[]
            {
                async (dc, args, next) =>
                {
                    await dc.Prompt("choicePrompt", $"[CheckBalanceDialog] Which account?",
                        new ChoicePromptOptions
                        {
                            Choices = new[] {new Choice {Value = "Current"}, new Choice {Value = "Savings"}}.ToList()
                        });
                },
                async (dc, args, next) =>
                {
                    var response = (args["Value"] as FoundChoice)?.Value;

                    if (response == "Current")
                    {
                        await dc.Begin(CheckCurrentAccountBalanceDialog.Id);
                    }
                    else if (response == "Savings")
                    {
                        await dc.Begin(CheckSavingsAccountBalanceDialog.Id);
                    }
                }
            });
            
            // add the child dialogs and prompts
            Dialogs.Add(CheckSavingsAccountBalanceDialog.Id, CheckSavingsAccountBalanceDialog.Instance);
            Dialogs.Add(CheckCurrentAccountBalanceDialog.Id, CheckCurrentAccountBalanceDialog.Instance);
            Dialogs.Add("choicePrompt", new ChoicePrompt(Culture.English));
        }

        public static string Id => "checkBalanceDialog";
        public static CheckBalanceDialog Instance { get; } = new CheckBalanceDialog();
    }
}