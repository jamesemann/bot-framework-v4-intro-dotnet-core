using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.AspNetCore.Hosting;

namespace demo9proactive.Controllers
{
    [Route("api/callback")]
    public class CallbackController : Controller
    {
        private IHostingEnvironment hostingEnvironment;

        public CallbackController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        // This HTTP Endpoint simulates a long running Proactive callback.  Use a HTTP client to simulate:
        // 
        // POST http://localhost:3979/api/callback
        // Content-Type: application/json
        // 
        // { "Text" :"this is a proactive message!" }
        //
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] ProactiveMessage message)
        {
            // For demonstration - read the cookie from disk.  For a real application
            // read from your persistent store - e.g. blob storage, table storage, document db, etc
            
            var filepath = Path.Combine(hostingEnvironment.WebRootPath,"resume.json");

            if (System.IO.File.Exists(filepath))
            {
                var resumeJson = System.IO.File.ReadAllText(filepath);
                var resumeData = JsonConvert.DeserializeObject<ConversationReference>(resumeJson);
                var client = new ConnectorClient(new Uri(resumeData.ServiceUrl));
                var message1 =resumeData.GetPostToBotMessage().CreateReply($"This is a response to your enquiry reference {String.Format("{0:X}", resumeData.ActivityId.GetHashCode())}...");
                var message2 = resumeData.GetPostToBotMessage().CreateReply($"{message.Text}");
                await client.Conversations.ReplyToActivityAsync((Activity)message1);
                await client.Conversations.ReplyToActivityAsync((Activity)message2);
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }

    public class ProactiveMessage
    {
        public string Text { get; set; }
    }
}
