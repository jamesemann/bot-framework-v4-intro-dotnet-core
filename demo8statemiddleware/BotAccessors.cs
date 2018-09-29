using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using demo8statemiddleware.Bots;
using Microsoft.Bot.Builder;

namespace demo8statemiddleware
{
    public class BotAccessors
    {
        public BotAccessors(ConversationState conversationState)
        {
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
        }

        public static string DemoStateName { get; } = $"{nameof(BotAccessors)}.DemoStateAccessor";

        public IStatePropertyAccessor<DemoState> DemoStateAccessor { get; set; }
        public ConversationState ConversationState { get; }
    }
}
