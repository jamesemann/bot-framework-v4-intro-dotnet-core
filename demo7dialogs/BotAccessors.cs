using System;
using demo7dialogs.Bots;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace demo7dialogs
{
    public class BotAccessors
    {
        public BotAccessors(ConversationState conversationState)
        {
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
        }

        public static string BankingBotStateAccessorName { get; } = $"{nameof(BotAccessors)}.BankingBotState";
        public IStatePropertyAccessor<BankingBotState> BankingBotStateStateAccessor { get; internal set; }

        public static string DialogStateAccessorName { get; } = $"{nameof(BotAccessors)}.DialogState";
        public IStatePropertyAccessor<DialogState> DialogStateAccessor { get; internal set; }
        public ConversationState ConversationState { get; }
    }
}