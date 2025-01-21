using Domain.Entities;

namespace Application.Interfaces.Services.Email
{
    public interface IBodyEmail
    {
        string _acceptFriendBodyEmail(string pseudoReceiver, User dataUserIssuer);
        string _requestFriendBodyEmail(string pseudoReceiver, User dataUserIssuer);
        string _emailBodyRegister(string pseudo);

        public string subjectAcceptFriend { get; }
        public string subjectRequestFriend { get; }
        public string subjectRegister { get; }
    }
}