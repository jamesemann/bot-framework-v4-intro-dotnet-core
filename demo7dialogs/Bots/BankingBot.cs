using System.Threading;
using System.Threading.Tasks;
using demo7dialogs.Dialogs;
using demo7dialogs.Dialogs.Balance;
using demo7dialogs.Dialogs.Balance.CurrentAccount;
using demo7dialogs.Dialogs.Balance.SavingsAccount;
using demo7dialogs.Dialogs.Payment;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace demo7dialogs.Bots
{
    public class BankingBot : IBot
    {
        private readonly DialogSet dialogs;

        public BankingBot(BotAccessors botAccessors)
        {
            var dialogState = botAccessors.DialogStateAccessor;
            // compose dialogs
            dialogs = new DialogSet(dialogState);
            dialogs.Add(MainDialog.Instance);
            dialogs.Add(MakePaymentDialog.Instance);
            dialogs.Add(CheckBalanceDialog.Instance);
            dialogs.Add(CheckCurrentAccountBalanceDialog.Instance);
            dialogs.Add(CheckSavingsAccountBalanceDialog.Instance);
            dialogs.Add(new ChoicePrompt("choicePrompt"));
            dialogs.Add(new TextPrompt("textPrompt"));
            dialogs.Add(new NumberPrompt<int>("numberPrompt"));
            BotAccessors = botAccessors;
        }

        public BotAccessors BotAccessors { get; }


        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                // initialize state if necessary
                var state = await BotAccessors.BankingBotStateStateAccessor.GetAsync(turnContext, () => new BankingBotState(), cancellationToken);

                turnContext.TurnState.Add("BotAccessors", BotAccessors);

                var dialogCtx = await dialogs.CreateContextAsync(turnContext, cancellationToken);

                if (dialogCtx.ActiveDialog == null)
                {
                    await dialogCtx.BeginDialogAsync(MainDialog.Id, cancellationToken);
                }
                else
                {
                    await dialogCtx.ContinueDialogAsync(cancellationToken);
                }

                await BotAccessors.ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            }
        }
    }
}