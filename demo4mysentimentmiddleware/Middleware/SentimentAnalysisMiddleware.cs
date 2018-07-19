using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace demo4mysentimentmiddleware.Middleware
{
    public class SentimentAnalysisMiddleware : IMiddleware
    {
        private TextAnalyticsClient client;

        public SentimentAnalysisMiddleware(string subscriptionKey, string textAnalyticsEndpointUrl)
        {
            this.client = new TextAnalyticsClient(new ApiKeyServiceClientCredentials(subscriptionKey))
            {
                BaseUri = new Uri(textAnalyticsEndpointUrl)
            };
        }

        public async Task OnTurn(ITurnContext context, MiddlewareSet.NextDelegate next)
        {
            if (context.Activity.Type == ActivityTypes.Message)
            {
                var documentBatchId = $"{ Guid.NewGuid() }";
                var input = new MultiLanguageBatchInput
                {
                    Documents = new List<MultiLanguageInput>(new[] { new MultiLanguageInput("en", documentBatchId, context.Activity.Text) })
                };
                var sentiment = (await client.SentimentAsync(input)).Documents.SingleOrDefault(x => x.Id == documentBatchId).Score.Value;

                context.Services.Add<SentimentAnalyisResult>(new SentimentAnalyisResult() { Sentiment = sentiment });                
            }

            await next();
        }
    }

    public class ApiKeyServiceClientCredentials : ServiceClientCredentials
    {
        private string subscriptionKey;

        public ApiKeyServiceClientCredentials(string subscriptionKey)
        {
            this.subscriptionKey = subscriptionKey;
        }

        public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Ocp-Apim-Subscription-Key", this.subscriptionKey);
            return base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }

    public class SentimentAnalyisResult
    {
        public double Sentiment { get; set; }
    }
}
