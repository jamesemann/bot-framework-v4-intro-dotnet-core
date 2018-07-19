using demo4mysentimentmiddleware.Middleware;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;
using System.Threading.Tasks;

namespace demo4mysentimentmiddleware.Bots
{
    public class SimpleBot : IBot
    {
        public async Task OnTurn(ITurnContext turnContext)
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var sentimentAnalysisResult = turnContext.Services.Get<SentimentAnalyisResult>();

                await turnContext.SendActivity($"You said {turnContext.Activity.Text}, the sentiment according to the middleware is {sentimentAnalysisResult.Sentiment}");
            }
        }
    }
}
