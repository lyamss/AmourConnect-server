using Domain.Entities;

namespace Domain.Dtos.AppLayerDtos
{
    public record RequestFriendForGetMessageDto
    {
        public RequestStatus Status { get; set; }
    }
}