using demo4sentiment.bot;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace demo4mysentimentmiddleware.Bots
{
    public class SimpleBot : IBot
    {
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var sentimentAnalysisResult = (SentimentPrediction)turnContext.TurnState["SentimentPrediction"];

                var result = sentimentAnalysisResult.Sentiment ? "Positive" : "Negative";
                await turnContext.SendActivityAsync($"You said {turnContext.Activity.Text}, the sentiment according to the middleware is {result}");
            }
        }
    }
}
