using States;
namespace LLM {
        public interface ILLMClient {
                Task<bool> connectToLLM(string modelName);
                Task<string> makeRequest(List<Message> state);
        }
}