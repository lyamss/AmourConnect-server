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
            (
                requestFriends.Id_RequestFriends,
                requestFriends.Status,
                requestFriends.Date_of_request,
                requestFriends.Id_UserReceiver,
                requestFriends.IdUserIssuer,
                requestFriends.UserReceiver.Pseudo,
                requestFriends.UserIssuer.Pseudo,
                requestFriends.UserIssuer.Profile_picture,
                requestFriends.UserReceiver.Profile_picture,
                requestFriends.UserReceiver.sex,
                requestFriends.UserIssuer.sex
            );
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