using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.AI.QnA;

namespace demo3qnamakermiddleware.Bots
{
    public class SimpleBot : IBot
    {
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!turnContext.Responded && turnContext.Activity.Type == ActivityTypes.Message)
            {
                var qnaService = new QnAMaker(new QnAMakerEndpoint()
                {
                    // get these details from qnamaker.ai
                    // https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-howto-qna?view=azure-bot-service-4.0&tabs=cs
                    KnowledgeBaseId = "",
                    Host = "",
                    EndpointKey = ""
                });

                var result = await qnaService.GetAnswersAsync(turnContext);

                if (result != null && result.Length > 0)
                {
                    await turnContext.SendActivityAsync(result[0].Answer, cancellationToken: cancellationToken);
                }
                else
                {
                    var msg = @"Sorry, I don't know how to answer that...";

                    await turnContext.SendActivityAsync(msg, cancellationToken: cancellationToken);
                }
            }
        }
    }
}
