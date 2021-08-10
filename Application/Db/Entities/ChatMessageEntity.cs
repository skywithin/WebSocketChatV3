using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Db.Entities
{
    [Table("ChatMessages")]
    public class ChatMessageEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreatedUtc { get; set; }

        public UserEntity Author { get; set; }

        public GroupEntity Group { get; set; }
    }
}
