using API.Entities;

namespace API.Features.RequestFriend
{
    public record QueryRequestFriend(int Id_RequestFriends, RequestStatus Status, DateTime Date_of_request, int Id_UserReceiver,
    int IdUserIssuer, string UserReceiverPseudo, string UserIssuerPseudo, byte[] UserIssuerPictureProfile, byte[] UserReceiverPictureProfile,
    string UserReceiverSex, string UserIssuerSex);

    public record RequestFriendForGetMessageDto(RequestStatus Status);
}