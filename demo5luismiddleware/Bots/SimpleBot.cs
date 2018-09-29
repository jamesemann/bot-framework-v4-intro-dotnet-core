using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.AI.Luis;

namespace demo5luismiddleware.Bots
{
    public class SimpleBot : IBot
    {
        public SimpleBot(LuisRecognizer luisRecognizer)
        {
            LuisRecognizer = luisRecognizer;
        }

        public LuisRecognizer LuisRecognizer { get; }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var result = await LuisRecognizer.RecognizeAsync(turnContext, cancellationToken);
                var topIntent = result?.GetTopScoringIntent();
                switch ((topIntent != null) ? topIntent.Value.intent : null)
                {
                    case null:
                        await turnContext.SendActivityAsync("Failed to get results from LUIS.");
                        break;
                    case "none":
                        await turnContext.SendActivityAsync("Sorry, I don't understand.");
                        break;
                    case "adjustlights":
                        await turnContext.SendActivityAsync("Adjusting the lights");
                        break;
                    case "adjusttemperature":
                        // Cancel the process.
                        await turnContext.SendActivityAsync("Adjusting the temperature.");
                        break;
                    default:
                        // Received an intent we didn't expect, so send its name and score.
                        await turnContext.SendActivityAsync($"Intent: {topIntent.Value.intent} ({topIntent.Value.score}).");
                        break;
                }
            }
        }
    }
}
