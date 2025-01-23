using API.Entities;

namespace API.Features.RequestFriend
{
    public record QueryRequestFriend
    {
        public int Id_RequestFriends { get; init; }
        public RequestStatus Status { get; init; }
        public DateTime Date_of_request { get; init; }
        public int Id_UserReceiver { get; init; }
        public int IdUserIssuer { get; init; }
        public string UserReceiverPseudo { get; init; }
        public string UserIssuerPseudo { get; init; }
        public byte[] UserIssuerPictureProfile { get; set; }
        public byte[] UserReceiverPictureProfile { get; set; }
        public string UserReceiverSex { get; init; }
        public string UserIssuerSex { get; init; }
    }

    public record RequestFriendForGetMessageDto(RequestStatus Status);
}