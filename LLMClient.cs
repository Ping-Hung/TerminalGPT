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
            /** 
             * Context in the most general case is a range (subarray).
             */
            IContextSelector selector = new SlidingWindowContextSelector(windowSize: 2);
            var context = selector.SelectContext(state: state);    // second last to the last

            // Call the end point with context, call with serialized context
            ResponseResult response = await client
                .CreateResponseAsync(userInputText: Serializer.Serialize(value: context))
                .ConfigureAwait(false);     // null check + avoid context capture --ChatGPT
            return response.GetOutputText();    // only generated text is returned
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

    // helper classes and interfaces 
    class Serializer {
        static JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        public static string Serialize<T>(T value) {
            return JsonSerializer.Serialize(value, options: options);
        }
    }
    public interface IContextSelector {
        // implement new strategies if want to use a different strategy
        IReadOnlyList<Message> SelectContext(IReadOnlyList<Message> state);
    }

    public class SlidingWindowContextSelector : IContextSelector {
        /// <summary>
        ///     Returns the last <paramref name="windowSize"/> messages
        ///     from the conversation state.
        /// </summary>
        private readonly int windowSize;

        public SlidingWindowContextSelector(int windowSize) {
            if (windowSize <= 0) {
                throw new ArgumentException("windowSize must be positive");
            }
            this.windowSize = windowSize;
        }

        public IReadOnlyList<Message> SelectContext(IReadOnlyList<Message> state) {
            if (state.Count <= windowSize) {
                return state;
            }

            return state.TakeLast(windowSize).ToList();
        }
    }
}
