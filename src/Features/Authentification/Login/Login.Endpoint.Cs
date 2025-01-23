using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;


namespace API.Features.Authentification;


public partial class Authentification
{
        [HttpGet("login")]
        public IActionResult Login() => this.Challenge(new AuthenticationProperties { RedirectUri = this.secretEnv.IP_Backend + "/api/Auth/signin-google" }, GoogleDefaults.AuthenticationScheme);
}