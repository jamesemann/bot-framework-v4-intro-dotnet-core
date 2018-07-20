using System;
using System.Linq;
using demo7dialogs.Dialogs.Balance;
using demo7dialogs.Dialogs.Payment;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts.Choices;

namespace demo7dialogs.Dialogs
{
    public class MainDialog : DialogContainer
    {
        private MainDialog() : base(Id)
        {
            Dialogs.Add(DialogId, new WaterfallStep[]
                {
                    async (dc, args, next) =>
                    {
                        await dc.Prompt("choicePrompt", $"[MainDialog] I'm banking 🤖{Environment.NewLine}Would you like to check balance or make payment?",
                            new ChoicePromptOptions
                            {
                                Choices = new[] {new Choice {Value = "Check balance"}, new Choice {Value = "Make payment"}}.ToList()
                            });
                    },

                    async (dc, args, next) =>
                    {
                        var response = (args["Value"] as FoundChoice)?.Value;
                        if (response == "Check balance")
                        {
                            await dc.Begin(CheckBalanceDialog.Id);
                        }
                        else if (response == "Make payment")
                        {
                            await dc.Begin(MakePaymentDialog.Id);
                        }
                    },
                    async (dc, args, next) =>
                    {
                        await dc.Replace(Id);
                    }
                }
            );

            // add the child dialogs and prompts
            Dialogs.Add(MakePaymentDialog.Id, MakePaymentDialog.Instance);
            Dialogs.Add(CheckBalanceDialog.Id, CheckBalanceDialog.Instance);
            Dialogs.Add("choicePrompt", new ChoicePrompt("en"));
        }

        public static string Id => "mainDialog";

        public static MainDialog Instance { get; } = new MainDialog();
    }
}