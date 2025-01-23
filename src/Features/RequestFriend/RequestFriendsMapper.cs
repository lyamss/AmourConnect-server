using API.Entities;

namespace API.Features.RequestFriend
{
    public static class RequestFriendsMapper
    {
        public static QueryRequestFriend ToGetRequestFriendsMapper(this RequestFriends requestFriends)
        {
            if (requestFriends == null)
            {
                return null;
            }

            return new QueryRequestFriend
            {
                Id_RequestFriends = requestFriends.Id_RequestFriends,
                IdUserIssuer = requestFriends.IdUserIssuer,
                UserIssuerPseudo = requestFriends.UserIssuer.Pseudo,
                Id_UserReceiver = requestFriends.Id_UserReceiver,
                UserReceiverPseudo = requestFriends.UserReceiver.Pseudo,
                Status = requestFriends.Status,
                Date_of_request = requestFriends.Date_of_request,
                UserIssuerPictureProfile = requestFriends.UserIssuer.Profile_picture,
                UserReceiverPictureProfile = requestFriends.UserReceiver.Profile_picture,
                UserIssuerSex = requestFriends.UserIssuer.sex,
                UserReceiverSex = requestFriends.UserReceiver.sex,
            };
        }

        public static RequestFriendForGetMessageDto ToGetRequestFriendsForGetMessageMapper(this RequestFriends requestFriends)
        {
            if (requestFriends == null)
            {
                return null;
            }

            return new RequestFriendForGetMessageDto
            (
                requestFriends.Status
            );
        }
    }
}