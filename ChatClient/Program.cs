using Core.Messaging;
using Core.Messaging.Constants;
using Core.Messaging.Payloads;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChatClient
{
    class Program
    {
        //TODO: Move to config
        static readonly string DefaultServerAddress = "https://localhost:5001/gamehub"; //"ws://localhost:5000/gamehub";

        static readonly ClientContext Context = new ClientContext();

        static void Main(string[] args)
        {
            Run(args).GetAwaiter().GetResult();
        }

        private static async Task Run(string[] args)
        {
            HubConnection hubConnection = null;

            try
            {
                hubConnection = await ConnectToHub(args);

                Console.WriteLine("*** Welcome to the hub ***");

                var userName = GetUserName();

                Console.WriteLine($"Hi {userName}!");

                await Login(hubConnection, userName);

                while (true)
                {
                    if (!Context.IsInGroup)
                    {
                        Console.WriteLine("Join the party. Type '/help' to get started");
                    }

                    string userInput = Console.ReadLine();

                    if (userInput.StartsWith(ConsoleCommand.CommandPrefix))
                    {
                        await ProcessConsoleCommand(userInput, hubConnection);
                    }
                    else if (Context.IsInGroup)
                    {
                        await SendMessageToGroup(hubConnection, userInput);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                await hubConnection.DisposeAsync();
            }

            Console.WriteLine("Session is closed. Press any key to exit");
            Console.ReadKey();
        }

        private static async Task<HubConnection> ConnectToHub(string[] args)
        {
            var serverAddress = args.Any() ? args[0] : DefaultServerAddress;

            var hubConnection = new HubConnectionBuilder().WithUrl(serverAddress).Build();

            RegisterMessageHandlers(hubConnection);

            await hubConnection.StartAsync();

            return hubConnection;
        }

        private static void RegisterMessageHandlers(HubConnection hubConnection)
        {
            hubConnection.On<string>(ClientCommand.Echo, (message) =>
            {
                Console.WriteLine($"Echo response: {message}");
            });

            hubConnection.On<Message<LoginResultMessage>>(ClientCommand.LoginSuccess, (message) =>
            {
                var msgPayload = message.Payload;
                var groups = msgPayload.Groups;

                Context.UserId = msgPayload.UserId;

                if (groups != null && groups.Any())
                {
                    var firstGroup = groups.First();

                    Context.CurrentGroupId = firstGroup.GroupId;
                    Context.CurrentGroupName = firstGroup.GroupName;
                }
            });

            hubConnection.On<Message<UserJoinedGroupMessage>>(ClientCommand.UserJoinedGroup, (message) =>
            {
                var msgPayload = message.Payload;

                Console.WriteLine($"{msgPayload.UserName} joined {msgPayload.GroupName}");
            });

            hubConnection.On<Message<JoinGroupAcceptedMessage>>(ClientCommand.JoinGroupAccepted, (message) =>
            {
                var msgPayload = message.Payload;

                Context.CurrentGroupName = msgPayload.GroupName;
                Context.CurrentGroupId = msgPayload.GroupId;
            });

            hubConnection.On<Message<JoinGroupDeniedMessage>>(ClientCommand.JoinGroupDenied, (message) =>
            {
                var msgPayload = message.Payload;

                Console.WriteLine($"Unable to join group {msgPayload.GroupName}. {msgPayload.DeniedReason}");
            });

            hubConnection.On<Message<UserLeftGroupMessage>>(ClientCommand.UserLeftGroup, (message) =>
            {
                var msgPayload = message.Payload;

                Console.WriteLine($"{msgPayload.UserName} left group");
            });

            hubConnection.On<Message<UserLeftGroupMessage>>(ClientCommand.RemovedFromGroup, (message) =>
            {
                Context.CurrentGroupName = null;
                var msgPayload = message.Payload;

                Console.Clear();

                Console.WriteLine($"You have left group {msgPayload.GroupName}");
            });

            hubConnection.On<Message<ChatMessage>>(ClientCommand.MessageToGroup, (message) =>
            {
                var msgPayload = message.Payload;
                Console.WriteLine($"{msgPayload.UserName}: {msgPayload.Content}");
            });
        }

        private static string GetUserName()
        {
            Console.WriteLine("What is your name?");

            string userName;

            while (string.IsNullOrWhiteSpace(userName = Console.ReadLine()) || userName.StartsWith(ConsoleCommand.CommandPrefix))
            {
                Console.WriteLine("Please enter a valid user name");
            }

            Context.UserName = userName;

            return userName;
        }

        private static async Task Echo(HubConnection hubConnection)
        {
            await hubConnection.SendAsync(HubCommand.Echo, "Test");

            //HACK: Need to get response from the server
            await Task.Delay(500);
        }

        private static async Task Login(HubConnection hubConnection, string userName)
        {
            //TODO: Proper implementation of authentication

            var loginRequestMsg =
                new MessageBuilder<LoginRequestMessage>()
                    .WithPayload(new LoginRequestMessage { UserName = userName })
                    .Build();
                
            await hubConnection.SendAsync(HubCommand.HubLogin, loginRequestMsg);

            //HACK: Need to get response from the server
            await Task.Delay(500);
        }

        private static async Task SendJoinGroupCommand(HubConnection hubConnection, string groupName)
        {
            var joinRequestMsg =
                new MessageBuilder<JoinGroupRequestMessage>()
                    .WithPayload(
                        new JoinGroupRequestMessage 
                        ( 
                            Context.UserId, 
                            Context.UserName, 
                            groupName 
                        ))
                    .Build();

            await hubConnection.SendAsync(HubCommand.HubJoinGroup, joinRequestMsg);

            //HACK: Need to get response from the server
            await Task.Delay(500);
        }

        private static async Task SendLeaveGroupCommand(HubConnection hubConnection)
        {
            var leaveGroupMsg =
                new MessageBuilder<LeaveGroupMessage>()
                    .WithPayload(new LeaveGroupMessage { UserName = Context.UserName, GroupName = Context.CurrentGroupName })
                    .Build();

            await hubConnection.SendAsync(HubCommand.HubLeaveGroup, leaveGroupMsg);

            Context.CurrentGroupName = null;
        }

        private static async Task SendMessageToGroup(HubConnection hubConnection, string chatContent)
        {
            var chatMessage =
                new MessageBuilder<ChatMessage>()
                    .WithPayload(
                        new ChatMessage(
                            Context.UserId,
                            Context.UserName,
                            Context.CurrentGroupId.Value,
                            Context.CurrentGroupName,
                            chatContent,
                            DateTime.UtcNow))
                    .Build();

            await hubConnection.SendAsync(HubCommand.HubGroupChat, chatMessage);
        }

        private static async Task ProcessConsoleCommand(string command, HubConnection hubConnection)
        {
            if (command.Equals(ConsoleCommand.Exit, StringComparison.InvariantCultureIgnoreCase))
            {
                Environment.Exit(0);
            }
            else if (command.Equals(ConsoleCommand.Help, StringComparison.InvariantCultureIgnoreCase))
            {
                PrintHelp();
            }
            else if (command.Equals(ConsoleCommand.List, StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine($"Command {command} needs to be implemented");
            }
            else if (command.Equals(ConsoleCommand.Echo, StringComparison.InvariantCultureIgnoreCase))
            {
                await Echo(hubConnection);
            }
            else if (command.Equals(ConsoleCommand.Join, StringComparison.InvariantCultureIgnoreCase))
            {
                if (Context.IsInGroup)
                {
                    Console.WriteLine("You must leave current group first");
                    return;
                }

                Console.WriteLine("Which group do you want to join?");

                string groupName;
                while (string.IsNullOrWhiteSpace(groupName = Console.ReadLine()) || groupName.StartsWith(ConsoleCommand.CommandPrefix))
                {
                    Console.WriteLine("Please enter a valid group name");
                }

                await SendJoinGroupCommand(hubConnection, groupName);
            }
            else if (command.Equals(ConsoleCommand.Leave, StringComparison.InvariantCultureIgnoreCase))
            {
                if (!Context.IsInGroup)
                {
                    Console.WriteLine("You are not in any group");
                    return;
                }

                await SendLeaveGroupCommand(hubConnection);
            }
            else
            {
                Console.WriteLine($"Command '{command}' is not recognized");
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("  {0}            Show command line help", ConsoleCommand.Help);
            Console.WriteLine("  {0}            List available groups", ConsoleCommand.List); 
            Console.WriteLine("  {0}            Join group", ConsoleCommand.Join);
            Console.WriteLine("  {0}           Leave current group", ConsoleCommand.Leave);
            Console.WriteLine("  {0}            Echo test", ConsoleCommand.Echo);
            Console.WriteLine("  {0}            Exit application", ConsoleCommand.Exit);
            Console.WriteLine();
        }
    }
}
