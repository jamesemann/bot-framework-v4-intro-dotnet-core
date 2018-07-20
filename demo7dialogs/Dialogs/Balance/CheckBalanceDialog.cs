using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts.Choices;
using Microsoft.Recognizers.Text;
using System.Linq;

namespace demo7dialogs.Bots
{
    public class CheckBalanceDialog : DialogContainer
    {
        public static string Id { get { return "checkBalanceDialog"; } }
        public static CheckBalanceDialog Instance { get; } = new CheckBalanceDialog();
        public CheckBalanceDialog() : base(Id)
        {
            this.Dialogs.Add(Id, new WaterfallStep[]
            {
                async (dc, args, next) =>
                {
                    await dc.Prompt("choicePrompt", $"[CheckBalanceDialog] Which account?",
                        new ChoicePromptOptions
                        {
                            Choices = new [] { new Choice() { Value = "Current" }, new Choice() { Value = "Savings" } }.ToList()
                        });
                },
                async (dc, args, next) =>
                {
                    var response = (args["Value"] as FoundChoice).Value;

                    if (response == "Current"){
                         await dc.Begin(CheckCurrentAccountBalanceDialog.Id);
                        }
                        else if (response == "Savings")
                        {
                            await dc.Begin(CheckSavingsAccountBalanceDialog.Id);
                        }
                }
            });
            // Add our child dialogs.
            this.Dialogs.Add(CheckSavingsAccountBalanceDialog.Id, CheckSavingsAccountBalanceDialog.Instance);
            this.Dialogs.Add(CheckCurrentAccountBalanceDialog.Id, CheckCurrentAccountBalanceDialog.Instance);

            // Define the prompts used in this conversation flow.
            this.Dialogs.Add("choicePrompt", new ChoicePrompt(Culture.English));
        }
    }
}