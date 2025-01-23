using System.ComponentModel.DataAnnotations;

namespace API.Services
{
    public sealed record SecretEnv
    {
        [Required]
        public string SecretKeyJWT { get; init; }
        [Required]
        public string IpFrontend { get; init; }
        [Required]
        public string IP_Backend { get; init; }
        [Required]
        public string ConnexionDb { get; init; }
        [Required]
        public string SERVICE_SMTP { get; init; }
        [Required]
        public string EMAIL_MDP_SMTP { get; init; }
        [Required]
        public string EMAIL_USER_SMTP { get; init; }
        [Required]
        public string OAuthGoogleClientId { get; init; }
        [Required]
        public string OAuthGoogleClientSecret { get; init; }
        [Required]
        public string ConnexionRedis { get; init; }
        [Required]
        public string PORT_SMTP { get; init; }
    }
}