namespace Core.Messaging.Constants
{
    public static class ClientCommand
    {
        public static string Echo => "Echo";
        public static string LoginSuccess => "LoginSuccess";
        public static string MessageToGroup => "MessageToGroup";
        public static string UserJoinedGroup => "UserJoinedGroup";
        public static string UserLeftGroup => "UserLeftGroup";
        public static string JoinGroupAccepted => "JoinGroupAccepted";
        public static string JoinGroupDenied => "JoinGroupDenied";
        public static string RemovedFromGroup => "RemovedFromGroup";
        public static string GroupList => "GroupList";

    }
}
