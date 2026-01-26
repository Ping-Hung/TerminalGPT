using States;
using OpenAI;
using OpenAI.Responses;

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
        // we are talking to the response endpoint: https://platform.openai.com/docs/api-reference/responses/create
#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        private readonly ResponsesClient client = new(
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.


            model: "gpt-5-nano-2025-08-07`",

            apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY")
        );
        Task<string> makeRequest(List<Message> state) {
            // call the endpoint
            // 1. Serialize `state`, and determine how much should be passed as parameter
            // 2. Deserialize the response, and only return content[0].text
        }
    }
    public class ClaudeLLMClient : ILLMClient {
    }
    public class AzureLLMClient : ILLMClient {
    }

}