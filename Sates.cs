namespace States {
        public record Message(string Role, string Content);
        public class ConversationState {
                public List<Message> Messages { get; } = [];
        }
}