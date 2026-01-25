// main entry point of the application
while (true) {
    Console.WriteLine("Enter your command (or type 'exit' to quit):");
    Console.Write("> ");
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

static string ProcessInput(string input) {
    // Placeholder for processing input and generating a plan
    return input != "" ? $"Processed input: {input}" : "No input provided.";
}