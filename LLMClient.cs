using States;
using OpenAI;
using OpenAI.Responses;
using System.Text.Json;
using Microsoft.VisualBasic;

namespace LLM {
    /// <summary>
    ///     interface (or abstract base class in C++) of LLM implementations from
    ///     OpenAI, Claude, or Azure OpenAI.
    /// </summary>
    public interface ILLMClient {
        // note: Adaptor: the implementations should adapt to the spec of this
        // interface, not other way around
        Task<string> makeRequest(List<Message> state);
    }

    // "wrapper classes" of the sdk provided classes (if they are provided)
    public class OpenAILLMClient : ILLMClient {
        // we are talking to the response endpoint:
        // https://platform.openai.com/docs/api-reference/responses/create
        private readonly ResponsesClient client = new(
            model: "gpt-5-nano-2025-08-07`",
            apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY")
        );

        public async Task<string> makeRequest(List<Message> state) {
            Message prompt = state[^1]; // last element in list
            // context in the most general case is a range (subarray), might implement some context selection logic here,
            // but  will only use the latest response from LLM for now
            var context = state[^2];

            // Call the end point with context
            // determine how much should be passed as parameter
            ResponseResult response = await client.CreateResponseAsync(
                userInputText: prompt.Content,
                previousResponseId: context?.Content    // a string id for previous responses, can be null
            );

            // Handle possible error on failed calls
            if (response == null) {
                throw new Exception($"openAI request with prompt {prompt} failed");
            }

            // Return the generated text in the response objecct
            return response.GetOutputText();
        }
    }
    public class ClaudeLLMClient : ILLMClient {
        public ClaudeLLMClient() {
            throw new NotImplementedException();
        }
        public async Task<string> makeRequest(List<Message> state) {
            throw new NotImplementedException(); 
        }
    }
    public class AzureLLMClient : ILLMClient {
        public AzureLLMClient() {
            throw new NotImplementedException();
        }
        public async Task<string> makeRequest(List<Message> state) {
            throw new NotImplementedException(); 
        }
    }

    // helper class dedicated to serialize data using C# generics
    public class Serializer {
        static JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        public static string Serialize<T>(T value) {
            return JsonSerializer.Serialize(value, options: options);
        }
    }

}
