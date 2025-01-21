namespace Domain.Dtos.GetDtos
{
    public class GetUserDto
    {
        public int Id_User { get; set; }
        public string Pseudo { get; set; }
        public string Description { get; set; }
        public byte[] Profile_picture { get; set; }
        public string city { get; set; }
        public string sex { get; set; }
        public DateTime date_of_birth { get; set; }
    }
}