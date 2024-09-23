
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Net;
using SementicKernalTest;

var OPEN_AI_MODEL = Environment.GetEnvironmentVariable("OPEN_AI_MODEL");
var OPEN_AI_KEY = Environment.GetEnvironmentVariable("OPEN_AI_KEY");
var OPEN_AI_ORG_ID = Environment.GetEnvironmentVariable("OPEN_AI_ORG_ID");

// Create a kernel with  OpenAI chat completion
var builder = Kernel.CreateBuilder().AddOpenAIChatCompletion(OPEN_AI_MODEL, OPEN_AI_KEY, OPEN_AI_ORG_ID);
// Build the kernel
    // builder.Plugins.AddFromType<GeneralPlugin>();//This line is only to run DEMO 5 with custom Class and Methods and for rest comment this line
Kernel kernel = builder.Build();


/* DEMO 0
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
// Add a plugin (the LightsPlugin class is defined below)
kernel.Plugins.AddFromType<LightsPlugin>("Lights");
// Enable planning
OpenAIPromptExecutionSettings objOpenAIPromptExecutionSettings = new OpenAIPromptExecutionSettings();
// Create a history store the conversation
var history = new ChatHistory();
// Initiate a back-and-forth chat
string? userInput;
do
{
   // Collect user input
   //Console.Write("User > ");
   Console.Write("I'm a friendly bot! Ask me anything you like: \"");
   userInput = Console.ReadLine();
   // Add user input
   history.AddUserMessage(userInput);
   // Get the response from the AI
   var result = await chatCompletionService.GetChatMessageContentAsync(
       history,
       executionSettings: objOpenAIPromptExecutionSettings,// openAIPromptExecutionSettings,
       kernel: kernel);
   // Print the results
   Console.WriteLine("Assistant > " + result);
   // Add the message from the agent to the chat history
   history.AddMessage(result.Role, result.Content ?? string.Empty);
} while (userInput is not null);

*/

/* DEMO 1
// Running your first prompt with Semantic Kernel
string request = "I want to know how much power my solar panels are providing.";
string prompt = $"What is the intent of this request? {request}";
Console.WriteLine(await kernel.InvokePromptAsync(prompt));
Console.WriteLine("---------------------");


// Improving the prompt with prompt engineering
prompt = @$"What is the intent of this request? {request}
You can choose between GetSolarEnergyToday, GetSolarPower, GetSolarBatteryPercentage, StartChargingCar.";
Console.WriteLine(await kernel.InvokePromptAsync(prompt));
Console.WriteLine("---------------------");


// Add structure to the output with formatting
prompt = @$"Instructions: What is the intent of this request?
Choices: GetSolarEnergyToday, GetSolarPower, GetSolarBatteryPercentage, StartChargingCar.
User Input: {request}
Intent: ";
Console.WriteLine(await kernel.InvokePromptAsync(prompt));
Console.WriteLine("---------------------");

prompt = $$"""
         ## Instructions
         Provide the intent of the request using the following format:
         
         ```json
         {
             "intent": {intent}
         }
         ```
         
         ## Choices
         You can choose between the following intents:
         
         ```json
         ["GetSolarEnergyToday", "GetSolarPower", "GetSolarBatteryPercentage", "StartChargingCar"]
         ```
         
         ## User Input
         The user input is:
         
         ```json
         {
             "request": "{{request}}"
         }
         ```
         
         ## Intent
         """;
Console.WriteLine(await kernel.InvokePromptAsync(prompt));
Console.WriteLine("---------------------");


// Provide examples with few-shot prompting
prompt = @$"Instructions: What is the intent of this request?
Choices: GetSolarEnergyToday, GetSolarPower, GetSolarBatteryPercentage, StartChargingCar.

User Input: How much energy did my solar panels provide today?
Intent: GetSolarEnergyToday

User Input: Can you start charging my car?
Intent: StartChargingCar

User Input: {request}
Intent: ";
Console.WriteLine(await kernel.InvokePromptAsync(prompt));
Console.WriteLine("---------------------");


// Tell the AI what to do to avoid doing something wrong
prompt = $"""
         Instructions: What is the intent of this request?
         If you don't know the intent, don't guess; instead respond with "Unknown".
         Choices: GetSolarEnergyToday, GetSolarPower, GetSolarBatteryPercentage, StartChargingCar.
         
         User Input: How much energy did my solar panels provide today?
         Intent: GetSolarEnergyToday
         
         User Input: Can you start charging my car?
         Intent: StartChargingCar

         User Input: {request}
         Intent: 
         """;
Console.WriteLine(await kernel.InvokePromptAsync(prompt));
Console.WriteLine("---------------------");


Console.ReadKey();
*/

/* DEMO 2
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
while (true)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("User > ");
    Console.ForegroundColor = ConsoleColor.White;
    var request = Console.ReadLine();

    var result = chatCompletionService.GetStreamingChatMessageContentsAsync(request!, kernel: kernel);

    string fullMessage = "";
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("Assistant > ");

    await foreach (var content in result)
    {
        Console.Write(content.Content);
        fullMessage += content.Content;
    }

    Console.WriteLine();
}
*/

/* DEMO 3 with CHAT History

ChatHistory history = [];
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

while (true)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("User > ");
    Console.ForegroundColor = ConsoleColor.White;
    var request = Console.ReadLine();
    history.AddUserMessage(request!);

    var result = chatCompletionService.GetStreamingChatMessageContentsAsync(history, kernel: kernel);

    string fullMessage = "";
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("Assistant > ");
    await foreach (var content in result)
    {
        Console.Write(content.Content);
        fullMessage += content.Content;
    }
    Console.WriteLine();

    history.AddAssistantMessage(fullMessage);
}
*/

//DEMO 4 answer as a 10-year old child
ChatHistory history = [];

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

history.AddSystemMessage("You should answer as a 10-year old child.");

while (true)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("User > ");
    Console.ForegroundColor = ConsoleColor.White;
    var request = Console.ReadLine();
    history.AddUserMessage(request!);

    var result = chatCompletionService.GetStreamingChatMessageContentsAsync(history, kernel: kernel);

    string fullMessage = "";
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("Assistant > ");

    await foreach (var content in result)
    {
        Console.Write(content.Content);
        fullMessage += content.Content;
    }

    Console.WriteLine();

    history.AddAssistantMessage(fullMessage);
}
//*/

/* DEMO 5 with custom Class and Methods
var executionSettings = new OpenAIPromptExecutionSettings
{
    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
};

ChatHistory history = [];

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

while (true)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("User > ");
    Console.ForegroundColor = ConsoleColor.White;
    var request = Console.ReadLine();
    history.AddUserMessage(request!);

    var result = chatCompletionService.GetStreamingChatMessageContentsAsync(history, executionSettings, kernel);

    string fullMessage = "";
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("Assistant > ");

    await foreach (var content in result)
    {
        Console.Write(content.Content);
        fullMessage += content.Content;
    }

    Console.WriteLine();

    history.AddAssistantMessage(fullMessage);
}
*/



