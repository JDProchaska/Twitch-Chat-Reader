// See https://aka.ms/new-console-template for more information
using TwitchLib.Client;
using Twitch_Chat_Reader.TwtichCalls;
using Newtonsoft.Json;
using System.IO;


Console.Write("Which chat would you like to monitor?");
string channelName = Console.ReadLine();

string chatMessagesPath = Directory.GetCurrentDirectory() + @"\ChatWords";
string chatMessages = Path.Combine(chatMessagesPath, $"{DateTime.Today.ToString("MM-dd-yyyy")}_{channelName}_ChatWords.txt");
string chatMessagesText = String.Empty;

ChatFunctions chat = new ChatFunctions(channelName);
if (File.Exists(chatMessages))
{
    chatMessagesText = File.ReadAllText(chatMessages);
    if (chatMessagesText.Length > 0)
        chat.wordsAndCounts = JsonConvert.DeserializeObject<Dictionary<string, int>>(chatMessagesText);
}
Console.ReadLine();
chat.Disconnect();

var jsonString = JsonConvert.SerializeObject(chat.wordsAndCounts);
Console.WriteLine(jsonString);
using (StreamWriter sw = new StreamWriter(chatMessages))
{
    sw.WriteLine(jsonString);
}
Console.WriteLine("Top 5 words used in chat: ");
List<KeyValuePair<string, int>> inOrder = chat.wordsAndCounts.OrderByDescending(x => x.Value).ToList();
for (int i = 0; i < 4; i++)
{
    Console.WriteLine($"{inOrder[i].Key} : {inOrder[i].Value}");
}
Console.ReadLine();
