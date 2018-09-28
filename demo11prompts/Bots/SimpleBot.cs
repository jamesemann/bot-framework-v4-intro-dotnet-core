using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;

namespace demo10prompts.Bots
{
    public class SimpleBot : IBot
    {
        private readonly DialogSet dialogs;

        public SimpleBot(BotAccessors botAccessors)
        {
            var dialogState = botAccessors.DialogStateAccessor; 

            dialogs = new DialogSet(dialogState);
            dialogs.Add(new TextPrompt("textPrompt"));
            dialogs.Add(new ChoicePrompt("choicePrompt"));
            dialogs.Add(new ConfirmPrompt("confirmPrompt"));
            dialogs.Add(new DateTimePrompt("dateTimePrompt"));
            dialogs.Add(new AttachmentPrompt("attachmentPrompt"));
            dialogs.Add(new NumberPrompt<int>("numberPrompt"));
            dialogs.Add(new WaterfallDialog("greetings", new WaterfallStep[]
            {
                async (stepContext, cancellationToken) =>
                {
                    return await stepContext.PromptAsync("choicePrompt",
                        new PromptOptions
                        {
                            Prompt = stepContext.Context.Activity.CreateReply("Which prompt would you like to test :-)"),
                            Choices = new[]
                            {
                                new Choice {Value = "dateTimePrompt"},
                                new Choice {Value = "textPrompt"},
                                new Choice {Value = "confirmPrompt"},
                                new Choice {Value = "attachmentPrompt"},
                                new Choice {Value = "numberPrompt"}
                            }.ToList()
                        });
                },
                async (stepContext, cancellationToken) =>
                {
                    var choice = stepContext.Result as FoundChoice;

                    return await stepContext.PromptAsync(choice.Value,
                        new PromptOptions
                        {
                            Prompt = stepContext.Context.Activity.CreateReply($"Please provide a response")
                        });
                },
                async (stepContext, cancellationToken) =>
                {
                    await stepContext.Context.SendActivityAsync($"Handle response data here = {stepContext.Result.ToString()}");
                    return await stepContext.EndDialogAsync();
                }
            }));
            BotAccessors = botAccessors;
        }

        public BotAccessors BotAccessors { get; }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var dialogCtx = await dialogs.CreateContextAsync(turnContext, cancellationToken);

                if (dialogCtx.ActiveDialog == null)
                {
                    await dialogCtx.BeginDialogAsync("greetings", cancellationToken);
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