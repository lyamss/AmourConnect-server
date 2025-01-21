using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Message { get; set; }


        [Required]
        [ForeignKey("UserIssuer")]
        public int IdUserIssuer { get; set; }
        public User UserIssuer { get; set; }


        [Required]
        [ForeignKey("UserReceiver")]
        public int Id_UserReceiver { get; set; }
        public User UserReceiver { get; set; }


        [Required]
        public string message_content { get; set; }

        [Required]
        public DateTime Date_of_request { get; set; }
    }
}