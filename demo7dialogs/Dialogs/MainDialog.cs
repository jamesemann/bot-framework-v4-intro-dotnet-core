using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts.Choices;
using System;
using System.Linq;

namespace demo7dialogs.Bots
{
    public class MainDialog : DialogContainer
    {
        public static string Id { get { return "mainDialog"; } }

        public static MainDialog Instance { get; } = new MainDialog();

        private MainDialog() : base(Id)
        {
            this.Dialogs.Add(this.DialogId, new WaterfallStep[]
            {
                async (dc, args, next) => {

                                        await dc.Prompt("choicePrompt", $"[MainDialog] I'm banking 🤖{Environment.NewLine}Would you like to check balance or make payment?",
                        new ChoicePromptOptions
                        {
                            Choices = new [] { new Choice() { Value = "Check balance" }, new Choice() { Value = "Make payment" } }.ToList()
                        });
            },

            async (dc, args, next) => {
                        var response = (args["Value"] as FoundChoice).Value;
                if (response == "Check balance"){
                 await dc.Begin(CheckBalanceDialog.Id);
                }
                else if (response == "Make payment")
                {
                    await dc.Begin(MakePaymentDialog.Id);
                }
            }
            ,
                async (dc, args, next) =>
                {
                    // Show the main menu again.
                    await dc.Replace(Id);
                }
            }
            );

            // Add our child dialogs.
            this.Dialogs.Add(MakePaymentDialog.Id, MakePaymentDialog.Instance);
            this.Dialogs.Add(CheckBalanceDialog.Id, CheckBalanceDialog.Instance);
            // Define the prompts used in this conversation flow.
            this.Dialogs.Add("choicePrompt", new ChoicePrompt("en"));
        }
    }
}