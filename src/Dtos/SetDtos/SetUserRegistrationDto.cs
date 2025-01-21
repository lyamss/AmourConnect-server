using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.SetDtos
{
    public class SetUserRegistrationDto
    {
        [Required]
        public string? Pseudo { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public DateTime? date_of_birth { get; set; }

        [Required]
        public string? sex { get; set; }

        [Required]
        public string? city { get; set; }
    }
}