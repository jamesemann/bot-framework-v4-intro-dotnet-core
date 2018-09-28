using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Threading.Tasks;

namespace demo8statemiddleware.Bots
{
    public class StateBot : IBot
    {
        public async Task OnTurn(ITurnContext turnContext)
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var state = turnContext.GetConversationState<DemoState>();
                await turnContext.SendActivity($"You said {turnContext.Activity.Text}, and you have made {++state.Counter} requests.");
            }
        }
    }
}
