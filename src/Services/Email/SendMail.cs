using API.Entities;

namespace API.Services.Email
{
    internal sealed class SendMail(IConfigEmail cEmail, IBodyEmail bodyEmail) : ISendMail
    {
        private readonly IConfigEmail _cEmail = cEmail;
        private readonly IBodyEmail _bodyEmail = bodyEmail;

        public async Task MailRegisterAsync(string email, string pseudo)
        => await this._cEmail.configMail(email, this. _bodyEmail.subjectRegister, this._bodyEmail._emailBodyRegister(pseudo));

        public async Task RequestFriendMailAsync(User dataUserReceiver, User dataUserIssuer)
        => await this._cEmail.configMail(dataUserReceiver.EmailGoogle, this._bodyEmail.subjectRequestFriend, this._bodyEmail._requestFriendBodyEmail(dataUserReceiver.Pseudo, dataUserIssuer));

        public async Task AcceptRequestFriendMailAsync(User dataUserReceiver, User dataUserIssuer) 
        => await this._cEmail.configMail(dataUserReceiver.EmailGoogle, dataUserIssuer.Pseudo + this._bodyEmail.subjectAcceptFriend, this._bodyEmail._acceptFriendBodyEmail(dataUserReceiver.Pseudo, dataUserIssuer));
    }

    public interface ISendMail
    {
        Task MailRegisterAsync(string email, string pseudo);
        Task RequestFriendMailAsync(User dataUserReceiver, User dataUserIssuer);
        Task AcceptRequestFriendMailAsync(User dataUserReceiver, User dataUserIssuer);
    }
}