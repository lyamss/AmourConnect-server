using Application.Interfaces.Services.Email;
using Domain.Entities;

namespace Application.Services.Email
{
    public class SendMail(IConfigEmail cEmail, IBodyEmail bodyEmail) : ISendMail
    {
        private readonly IConfigEmail _cEmail = cEmail;
        private readonly IBodyEmail _bodyEmail = bodyEmail;

        public async Task MailRegisterAsync(string email, string pseudo)
        => await _cEmail.configMail(email, _bodyEmail.subjectRegister, _bodyEmail._emailBodyRegister(pseudo));

        public async Task RequestFriendMailAsync(User dataUserReceiver, User dataUserIssuer)
        => await _cEmail.configMail(dataUserReceiver.EmailGoogle, _bodyEmail.subjectRequestFriend, _bodyEmail._requestFriendBodyEmail(dataUserReceiver.Pseudo, dataUserIssuer));

        public async Task AcceptRequestFriendMailAsync(User dataUserReceiver, User dataUserIssuer) 
        => await _cEmail.configMail(dataUserReceiver.EmailGoogle, dataUserIssuer.Pseudo + _bodyEmail.subjectAcceptFriend, _bodyEmail._acceptFriendBodyEmail(dataUserReceiver.Pseudo, dataUserIssuer));
    }
}