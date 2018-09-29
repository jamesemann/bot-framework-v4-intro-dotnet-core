using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace demo9proactive.Bots
{
    public class SimpleBot : IBot
    {
        private IHostingEnvironment hostingEnvironment;

        public SimpleBot(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            // turnContext.Responded will be false if qna doesn't know how to answer. An example is 'how do I install office 365'.
            if (!turnContext.Responded && turnContext.Activity.Type == ActivityTypes.Message)
            {
                var conversationReference = turnContext.Activity.GetConversationReference();

                var filepath = Path.Combine(hostingEnvironment.WebRootPath, "resume.json");
                File.WriteAllText(filepath, JsonConvert.SerializeObject(conversationReference));
                
                await turnContext.SendActivityAsync($"Sorry, I don't know how to answer that automatically, we will get an answer to you ASAP and get back to you. Your reference number is {String.Format("{0:X}", conversationReference.ActivityId.GetHashCode())}");
            }
        }
    }
}
