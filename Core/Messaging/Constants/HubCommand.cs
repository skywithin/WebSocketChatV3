namespace Core.Messaging.Constants
{
    public static class HubCommand
    {
        public static string Echo => "Echo";
        public static string HubLogin => "HubLogin";
        public static string HubGroupChat => "HubGroupChat";
        public static string HubJoinGroup => "HubJoinGroup";
        public static string HubLeaveGroup => "HubLeaveGroup";
        public static string HubListGroups => "HubListGroups";
    }
}
