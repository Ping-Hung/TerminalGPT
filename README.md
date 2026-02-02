# TerminalGPT
* Console app built with C# for the sake of learning. 
* Current objective is to build a Mininal "LLM-Shell" desktop application.

a minimal data-flow diagram ![Data Flow Diagram](App_Data_Flow.png)

# Learnt Concepts
1. **Vibe Coding**:
  + How to leverage the power of chatGPT to learn a new programming language and build a usable app.
2. **OO Programming**: 
  + How to model the entire app as interactions between entities with distinct roles.
3. **C# Features**:
  + **Pattern matching**:
    - `switch` expression
    - `=>` (arrow function) for lambda functions.
  + **Indeces and Range**
    - https://learn.microsoft.com/en-us/dotnet/csharp/tutorials/ranges-indexes#type-support-for-indices-and-ranges
4. **Adaptor Pattern**:
  + Essentially "I have an existing class with desired functionalities, let me do something around it so this class fits into the entire system".
  + Typcially use `interface` (or **abstract base class** in C++) as wrapper around a service(dependency), so the service could easily interact with the rest of the program through the client.
  + The client is implemented based on the contract defined in the `interface`/**abstract base class**.
  + Adaptors implement the client and wraps around the service.

# Project Status/Demo
![demo picture](demo.png)

# Resources
## Csharp Tutorial
1.  https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/tutorials/
## OpenAI API
1.  https://platform.openai.com/docs/api-reference/authentication?utm_source=chatgpt.com
2.  https://platform.openai.com/docs/guides/streaming-responses
3.  https://github.com/openai/openai-dotnet/tree/main
4.  https://platform.openai.com/docs/guides/text

# Design Notes

- ``context``, the user-assistant chat history list, is passed into the LLM as a serialized JSON list.
