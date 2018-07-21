using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public async Task OnTurn(ITurnContext turnContext)
        {
            // turnContext.Responded will be false if qna doesn't know how to answer. An example is 'how do I install office 365'.
            if (!turnContext.Responded && turnContext.Activity.Type == ActivityTypes.Message)
            {
                var conversationReference = TurnContext.GetConversationReference(turnContext.Activity);

                var filepath = Path.Combine(hostingEnvironment.WebRootPath, "resume.json");
                File.WriteAllText(filepath, JsonConvert.SerializeObject(conversationReference));
                
                await turnContext.SendActivity($"Sorry, I don't know how to answer that automatically, we will get an answer to you ASAP and get back to you. Your reference number is {String.Format("{0:X}", conversationReference.ActivityId.GetHashCode())}");
            }
        }
    }
}
