using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Db.Entities
{
    [Table("Groups")]
    public class GroupEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreatedUtc { get; set; }

        public IEnumerable<ChatMessageEntity> ChatMessages { get; set; }
        public List<UserEntity> Members { get; set; }
    }
}
