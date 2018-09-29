using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;

namespace dispatch
{
    public class Bot : IBot
    {
        private readonly Dictionary<string, object> models;

        public Bot(IConfiguration configuration, LuisRecognizer dispatchRecognizer)
        {
            DispatchRecognizer = dispatchRecognizer;

            var modelConfigurations = Config.GetDispatchConfiguration(configuration).Models;

            models = new Dictionary<string, object>();
            foreach (var modelConfiguration in modelConfigurations)
            {
                if (modelConfiguration.ModelType == DispatchConfiguration.ModelConfiguration.ModelTypes.luis)
                {
                    models.Add(modelConfiguration.Name, new LuisApplication(modelConfiguration.Id, modelConfiguration.Key, modelConfiguration.Url));
                }
                else if (modelConfiguration.ModelType == DispatchConfiguration.ModelConfiguration.ModelTypes.qna)
                {
                    models.Add(modelConfiguration.Name, new QnAMakerEndpoint {KnowledgeBaseId = modelConfiguration.Id, EndpointKey = modelConfiguration.Key, Host = modelConfiguration.Url});
                }
            }
        }

        public LuisRecognizer DispatchRecognizer { get; }

        public async Task OnTurnAsync(ITurnContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (context.Activity.Type is ActivityTypes.Message)
            {
                var dispatchResult = await DispatchRecognizer.RecognizeAsync(context, cancellationToken);

                var topIntent = dispatchResult?.GetTopScoringIntent();

                if (topIntent == null)
                {
                    await context.SendActivityAsync("Unable to get the top intent.");
                }
                else
                {
                    // we can use the model name to index into our array of qna/luis models
                    var modelName = topIntent.Value.intent;

                    var targetModel = models[modelName];

                    if (targetModel is QnAMakerEndpoint qnaModel)
                    {
                        await context.SendActivityAsync($"Dispatching to QnAMaker model {modelName}");
                        var qnaMaker = new QnAMaker(qnaModel);
                        var qnaResponse = await qnaMaker.GetAnswersAsync(context);
                        await context.SendActivityAsync($"Result from QnAMaker: {qnaResponse.FirstOrDefault().Answer}");
                    }
                    else if (targetModel is LuisApplication luisModel)
                    {
                        await context.SendActivityAsync($"Dispatching to luis model {modelName}");

                        var luisRecognizer = new LuisRecognizer(luisModel);
                        var recognizerResult = await luisRecognizer.RecognizeAsync(context, cancellationToken);
                        await context.SendActivityAsync($"Result from luis: {recognizerResult.GetTopScoringIntent().intent}");
                    }
                }
            }
        }
    }
}