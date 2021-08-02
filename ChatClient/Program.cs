using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace ChatClient
{
    class Program
    {
        //TODO: Move to config
        static readonly string DefaultServerAddress = "https://localhost:5001/chathub"; //"ws://localhost:5000/chathub";

        static void Main(string[] args)
        {
            var serverAddress = args.Any() ? args[0] : DefaultServerAddress;

            var userDetails = Login();

            StartChat(serverAddress, userDetails).GetAwaiter().GetResult();
        }

        private static UserDetails Login()
        {
            Console.WriteLine("What is your name?");

            string userName;
            string groupName;

            while (string.IsNullOrWhiteSpace(userName = Console.ReadLine()))
            {
                Console.WriteLine("Please enter a valid user name");
            }

            Console.WriteLine("Which chat group do you want to join?");

            while (string.IsNullOrWhiteSpace(groupName = Console.ReadLine()))
            {
                Console.WriteLine("Please enter a valid group name");
            }

            return new UserDetails
            {
                UserName = userName,
                GroupName = groupName
            };
        }

        private static async Task StartChat(string serverAddress, UserDetails user)
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl(serverAddress)
                .Build();

            ConfigureBehavior(hubConnection);

            await hubConnection.StartAsync();

            await hubConnection.SendAsync("AddUserToGroup", user.UserName, user.GroupName);

            string msg;
            while (ContinueConversation(msg = Console.ReadLine()))
            {
                await hubConnection.SendAsync("SendMessageToGroup", user.UserName, user.GroupName, msg);
            }

            await hubConnection.DisposeAsync();
            
            Console.WriteLine("Session is closed. Press any key to exit");
            Console.ReadKey();
        }

        private static void ConfigureBehavior(HubConnection hubConnection)
        {
            hubConnection.On<string>("GroupChanged", (message) =>
            {
                Console.WriteLine(message);
            });

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                Console.WriteLine(encodedMsg);
            });
        }

        private static bool ContinueConversation(string msg)
        {
            var isExitRequest = string.IsNullOrEmpty(msg) || msg.ToLower().Equals("exit");
            return !isExitRequest;
        }

        //private static void SendUserJoinedMessage(WebSocket ws, UserDetails user)
        //{
        //    var envelope =
        //            new MessageEnvelope
        //            {
        //                MessageType = MessageType.UserJoined,
        //                Author = user
        //            };

        //    ws.Send(JsonConvert.SerializeObject(envelope));
        //}

        //private static void SendChatMessage(WebSocket ws, UserDetails user, string message)
        //{
        //    var envelope =
        //        new MessageEnvelope
        //        {
        //            MessageType = MessageType.Chat,
        //            Author = user,
        //            Payload = JsonConvert.SerializeObject(
        //                new ChatMessage
        //                {
        //                    Content = message
        //                })
        //        };

        //    ws.Send(JsonConvert.SerializeObject(envelope));
        //}

        //private static void OnMessageReceived(object sender, MessageEventArgs e)
        //{
        //    try
        //    {
        //        var envelope = JsonConvert.DeserializeObject<MessageEnvelope>(e.Data);

        //        switch (envelope.MessageType) 
        //        {
        //            case MessageType.Chat:
        //                var message = JsonConvert.DeserializeObject<ChatMessage>(envelope.Payload);
        //                Console.WriteLine($"[{envelope.DateCreatedUtc.ToLocalTime()}] {envelope.Author.UserName}: {message.Content}");
        //                break;

        //            case MessageType.UserJoined:
        //                Console.WriteLine($"{envelope.Author.UserName} has joined");
        //                break;

        //            default:
        //                Console.WriteLine($"Unable to process '{e.Data}'");
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //    }
        //}
    }
}
