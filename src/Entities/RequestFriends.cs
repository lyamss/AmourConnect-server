using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class RequestFriends
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_RequestFriends { get; set; }

        [Required]
        [ForeignKey("UserIssuer")]
        public int IdUserIssuer { get; set; }
        public User UserIssuer { get; set; }


        [Required]
        [ForeignKey("UserReceiver")]
        public int Id_UserReceiver { get; set; }
        public User UserReceiver { get; set; }


        [Required]
        public RequestStatus Status { get; set; }

        [Required]
        public DateTime Date_of_request { get; set; }
    }

    public enum RequestStatus
    {
        Onhold,
        Accepted,
    }
}