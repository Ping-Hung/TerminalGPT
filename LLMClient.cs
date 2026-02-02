using States;
using OpenAI.Responses;
using System.Text.Json;
using dotenv.net;

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
        // we are talking to the streaming endpoint
        // https://platform.openai.com/docs/guides/streaming-responses?lang=csharp

        // Use of experimental APIs which are subject to change: have to acknowledge their 
        // experimental status by suppressing the corresponding warning.
        #pragma warning disable OPENAI001
        private readonly ResponsesClient client; 

        public OpenAILLMClient() {
            DotEnv.Load();
            client = new(
                model: "gpt-5",
                apiKey: DotEnv.Read()["OPENAI_API_KEY"]
            );
        }

        public async Task<string> makeRequest(List<Message> state) {
            // Switch to other context selection approaches by implementing IContextSelector
            IContextSelector selector = new SlidingWindowContextSelector(windowSize: 2); // send latest response as context
            var context = selector.SelectContext(state: state);

            // Call the end point with context, call with serialized context
            var responses = client.CreateResponseStreamingAsync(userInputText: Serializer.Serialize(context));
            if (responses == null) {
                throw new Exception($"request to {client.Model} failed");
            }

            // now print as the model generates response and accumulate all updates into a single string
            await foreach (StreamingResponseUpdate update in responses) {
                if (update.Content != null) {
                    Console.WriteLine(update.Content);
                }
            }
            Console.WriteLine();

            return 
        }
    }
    #pragma warning restore OPENAI001

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
