using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demo7dialogs.Bots
{
    public class SimpleBotComponents : IBot
    {
        private DialogSet _dialogs;

        public SimpleBotComponents()
        {
            this._dialogs = new DialogSet();
            _dialogs.Add("mainDialog", MainDialog.Instance);
        }

        public async Task OnTurn(ITurnContext context)
        {
            if (context.Activity.Type == ActivityTypes.Message)
            {
                var state = context.GetConversationState<Dictionary<string, object>>();
                var dialogCtx = _dialogs.CreateContext(context, state);

                await dialogCtx.Continue();
                if (!context.Responded)
                {
                    // call next dialog#
                    await dialogCtx.Begin("mainDialog");
                }
            }
        }
    }

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

    public class MakePaymentDialog : DialogContainer
    {
        public static string Id { get { return "makePaymentDialog"; } }
        public static MakePaymentDialog Instance { get; } = new MakePaymentDialog();
        public MakePaymentDialog() : base(Id)
        {
            this.Dialogs.Add(Id, new WaterfallStep[]
             {
                async (dc, args, next) =>
                {
                    await dc.Context.SendActivity(dc.Context.Activity.CreateReply("making payment..."));
                }
             });
        }
    }

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