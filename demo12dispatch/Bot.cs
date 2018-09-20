using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Ai.LUIS;
using Microsoft.Bot.Builder.Ai.QnA;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;

namespace dispatch
{
    public class Bot : IBot
    {
        private Dictionary<string, object> models;

        public Bot(IConfiguration configuration)
        {
            var modelConfigurations = Config.GetDispatchConfiguration(configuration).Models;

            this.models = new Dictionary<string, object>();
            foreach (var modelConfiguration in modelConfigurations)
            {
                if (modelConfiguration.ModelType == DispatchConfiguration.ModelConfiguration.ModelTypes.luis)
                {
                    this.models.Add(modelConfiguration.Name, new LuisModel(modelConfiguration.Id, modelConfiguration.Key, modelConfiguration.Url));
                }
                else if(modelConfiguration.ModelType == DispatchConfiguration.ModelConfiguration.ModelTypes.qna)
                {
                    this.models.Add(modelConfiguration.Name, new QnAMakerEndpoint(){ KnowledgeBaseId = modelConfiguration.Id, EndpointKey = modelConfiguration.Key, Host = modelConfiguration.Url.ToString()});
                }
            }
        }

        public async Task OnTurn(ITurnContext context)
        {
            if (context.Activity.Type is ActivityTypes.Message)
            {
                var dispatchResult = context.Services.Get<RecognizerResult>(LuisRecognizerMiddleware.LuisRecognizerResultKey) as RecognizerResult;
                var topIntent = dispatchResult?.GetTopScoringIntent();

                if (topIntent == null)
                {
                    await context.SendActivity("Unable to get the top intent.");
                }
                else
                {
                    // we can use the model name to index into our array of qna/luis models
                    var modelName = topIntent.Value.intent;

                    var targetModel = this.models[modelName];

                    if (targetModel is QnAMakerEndpoint qnaModel)
                    {
                        await context.SendActivity($"Dispatching to QnAMaker model {modelName}");
                        var qnaMaker = new QnAMaker(qnaModel);
                        var qnaResponse = await qnaMaker.GetAnswers(context.Activity.Text);
                        await context.SendActivity($"Result from QnAMaker: {qnaResponse.FirstOrDefault().Answer}");
                    }
                    else if (targetModel is LuisModel luisModel)
                    {
                        await context.SendActivity($"Dispatching to luis model {modelName}");

                        var luisRecognizer = new LuisRecognizer(luisModel);
                        var recognizerResult = await luisRecognizer.Recognize(context.Activity.Text, System.Threading.CancellationToken.None);
                        await context.SendActivity($"Result from luis: {recognizerResult.GetTopScoringIntent().intent}");
                    }
                }
            }
        }
    }
}
