// main entry point of the application, responsible for
// 1. handling user input and send them to agents, where planning was done
// 2. present the plan to user and wait for more actions from user
class Program {
    static void Main(string[] args) {
        while (true) {
            Console.WriteLine("Enter your command (or type 'exit' to quit):");
            string? userInput = Console.ReadLine();

            if (userInput == null || userInput?.ToLower() == "exit") {
                // EOF or "exit" will exit app
                break;
            }
            // Send input to agents for planning
            string plan = ProcessInput(userInput ?? ""); // if userInput is null, send empty string

            // Present the plan to the user
            Console.WriteLine($"Plan: {plan}");
        }
    }

    static string ProcessInput(string input) {
        // Placeholder for processing input and generating a plan
        return input != "" ? $"Processed input: {input}" : "No input provided.";
    }
}