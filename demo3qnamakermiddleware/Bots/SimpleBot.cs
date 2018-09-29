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
        public SimpleBot(QnAMaker qnAMaker)
        {
            QnAMaker = qnAMaker;
        }

        public QnAMaker QnAMaker { get; }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!turnContext.Responded && turnContext.Activity.Type == ActivityTypes.Message)
            {
                var result = await QnAMaker.GetAnswersAsync(turnContext);

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
