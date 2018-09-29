using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace demo8statemiddleware.Bots
{
    public class StateBot : IBot
    {
        public StateBot(BotAccessors botAccessors)
        {
            BotAccessors = botAccessors;
        }

        public BotAccessors BotAccessors { get; }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var state = await BotAccessors.DemoStateAccessor.GetAsync(turnContext, () => new DemoState(), cancellationToken:cancellationToken);
                await turnContext.SendActivityAsync($"You said {turnContext.Activity.Text}, and you have made {++state.Counter} requests.");
            }
        }
    }
}