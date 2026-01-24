namespace chatSession {
        public record Message(string Role, string Content);
        public class ConversationState {
                public List<Message> Messages { get; } = new();
        }

        public interface IChatSession {
                Task<string> HandleUserInputAsync(string userInput);
        }
        public class ChatSession : IChatSession {
                private readonly ConversationState _state;
                private readonly ILLMClient _llm;

                public ChatSession(ConversationState state,  ILLMClient llm) {
                        _state = state;
                        _llm = llm;
                }

                public async Task<string> HandleUserInputAsync(string userInput) {
                        _state.Messages.Add(new Message("user", userInput));

                        var reply = await _llm.GetCompletionAsync(_state.Messages);

                        _state.Messages.Add(new Message("assistant", reply));
                        return reply;
                }
        }
}
