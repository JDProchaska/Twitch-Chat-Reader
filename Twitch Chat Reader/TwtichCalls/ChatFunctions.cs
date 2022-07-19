using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace Twitch_Chat_Reader.TwtichCalls
{
    internal class ChatFunctions
    {
        TwitchClient client;
        //TextToSpeech.TextToSpeech speaker;
        public Dictionary<string, int> wordsAndCounts;
        public ChatFunctions(string ChannelName)
        {
            ConnectionCredentials credentials = new ConnectionCredentials("WizardInTheCabinet", "eylsvh47gcup9ydgqkpe4vepzvi22k");
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);
            client.Initialize(credentials, ChannelName);

            client.OnLog += Client_OnLog;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnWhisperReceived += Client_OnWhisperReceived;
            client.OnNewSubscriber += Client_OnNewSubscriber;
            client.OnConnected += Client_OnConnected;

            client.Connect();

            wordsAndCounts = new Dictionary<string, int>();
            //speaker = new TextToSpeech.TextToSpeech();
        }

        public void Disconnect()
        {
            Console.WriteLine("Shutting down client");
            client.Disconnect();
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            //Console.WriteLine($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            //Console.WriteLine("Hey guys! I am a bot connected via TwitchLib!");
            //client.SendMessage(e.Channel, "Hey guys! I am a bot connected via TwitchLib!");
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            Console.WriteLine($"{e.ChatMessage.Username} : {e.ChatMessage.Message}");

            AddTexts(e.ChatMessage.Message.ToLower());
           /* if (e.ChatMessage.Message.Length < 80)
                speaker.ReadText(e.ChatMessage.Message);*/
           /* if (e.ChatMessage.Message.Contains("badword"))
                client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromMinutes(30), "Bad word! 30 minute timeout!");*/
        }

        private void AddTexts(string message)
        {
            foreach (string messageWord in message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (wordsAndCounts.ContainsKey(messageWord))
                {
                    wordsAndCounts[messageWord]++;
                }
                else
                    wordsAndCounts.Add(messageWord, 1);
            }
        }

        private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
           /* if (e.WhisperMessage.Username == "my_friend")
                client.SendWhisper(e.WhisperMessage.Username, "Hey! Whispers are so cool!!");*/
        }

        private void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
           /* if (e.Subscriber.SubscriptionPlan == SubscriptionPlan.Prime)
                client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points! So kind of you to use your Twitch Prime on this channel!");
            else
                client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points!");*/
        }
    }
}
