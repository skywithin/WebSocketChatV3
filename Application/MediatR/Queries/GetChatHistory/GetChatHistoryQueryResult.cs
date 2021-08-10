using System;
using System.Collections.Generic;

namespace Application.MediatR.Queries.GetChatHistory
{
    public class GetChatHistoryQueryResult
    {
        public IEnumerable<ChatRecord> ChatHistory { get; set; }
    }

    public class ChatRecord
    {
        public Guid UserId { get; set; }
        public string AuthorName { get; set; }
        public Guid GroupId { get; set; }
        public string Content { get; set; }
        public DateTime DateCreatedUtc { get; set; }

        public ChatRecord(
            Guid groupId,
            Guid userId,
            string authorName,
            string content,
            DateTime dateCreatedUtc)
        {
            GroupId = groupId;
            UserId = userId;
            AuthorName = authorName;
            Content = content;
            DateCreatedUtc = dateCreatedUtc;
        }
    }
}
