using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_User { get; set; }

        [Required]
        public string? userIdGoogle { get; set; }

        [Required]
        [MaxLength(15)]
        public string? Pseudo { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string? EmailGoogle { get; set; }

        public byte[]? Profile_picture { get; set; }

        public DateTime? date_token_session_expiration { get; set; }

        public string? token_session_user { get; set; }

        [Required]
        [MaxLength(50)]
        public string? city { get; set; }

        [Required]
        public string? sex { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime date_of_birth { get; set; }

        [Required]
        public DateTime account_created_at { get; set; }

        public virtual ICollection<Message> MessagesSent { get; set; }
        public virtual ICollection<Message> MessagesReceived { get; set; }
        public virtual ICollection<RequestFriends> RequestsSent { get; set; }
        public virtual ICollection<RequestFriends> RequestsReceived { get; set; }
    }
}