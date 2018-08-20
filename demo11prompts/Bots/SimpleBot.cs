using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Core.Extensions;
using static Microsoft.Bot.Builder.Prompts.DateTimeResult;
using Microsoft.Recognizers.Text;
using Microsoft.Bot.Builder.Prompts.Choices;

namespace demo10prompts.Bots
{
    public class SimpleBot : IBot
    {
        private DialogSet dialogs;

        public SimpleBot()
        {
            dialogs = new DialogSet();
            dialogs.Add("textPrompt", new TextPrompt());
            dialogs.Add("choicePrompt", new ChoicePrompt(Culture.English));
            dialogs.Add("confirmPrompt", new ConfirmPrompt(Culture.English));
            dialogs.Add("dateTimePrompt", new DateTimePrompt(Culture.English));
            dialogs.Add("attachmentPrompt", new AttachmentPrompt());
            dialogs.Add("numberPrompt", new NumberPrompt<int>(Culture.English));
            dialogs.Add("greetings", new WaterfallStep[]
            {async (dc, args, next) =>
                {
                    await dc.Prompt("choicePrompt","Which prompt would you like to test :-)",
                        new ChoicePromptOptions
                        {
                            Choices = new[] {
                                new Choice {Value = "dateTimePrompt"},
                                new Choice {Value = "textPrompt"},
                                new Choice {Value = "confirmPrompt"},
                                new Choice {Value = "attachmentPrompt"},
                                new Choice {Value = "numberPrompt"}
                            }.ToList()
                        });
                },
                async (dc, args, next) =>
                {
                    var choice = args["Value"] as FoundChoice;

                    await dc.Prompt(choice.Value, $"Please provide a response");
                },
                async(dc, args, next) =>
                {
                    //if (args["Resolution"] is List<DateTimeResolution> resolutions && resolutions.Count > 0 && resolutions.FirstOrDefault().Value is string resolution) {
                    //    var parsedResolution = DateTime.Parse(resolution);
                    //    await dc.Context.SendActivity($"You said {parsedResolution.ToShortDateString()}");
                    //}
                    await dc.End();
                }
            });
        }
        public async Task OnTurn(ITurnContext turnContext)
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var state = turnContext.GetConversationState<Dictionary<string,object>>();

                var dialogCtx = dialogs.CreateContext(turnContext, state);

                await dialogCtx.Continue();
                if (!turnContext.Responded)
                {
                    await dialogCtx.Begin("greetings");
                }
            }
        }
    }
}
