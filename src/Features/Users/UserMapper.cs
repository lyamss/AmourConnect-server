namespace API.Features.Users
{
    public static class UserMapper
    {
        public static QueryUser ToGetUserMapper(this Entities.User user)
        {
            if (user == null)
            {
                return null;
            }

            return new QueryUser
            (
             user.Id_User,
             user.Pseudo,
             user.Description,
             user.Profile_picture,
             user.city,
             user.sex,
             user.date_of_birth
            );
        }
    }
}