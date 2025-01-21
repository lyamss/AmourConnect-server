using Domain.Dtos.GetDtos;
using Infrastructure.Interfaces;

namespace Infrastructure.DistributedCaching
{
    public class MessageCaching(IMessageRepository messageRepository, ICacheService cacheService) : IMessageCaching
    {
        private readonly IMessageRepository _messageRepository = messageRepository;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<ICollection<GetMessageDto>> GetMessagesAsync(int idUserIssuer, int idUserReceiver)
        {
            string KeyM = idUserIssuer.ToString() + "GetMessagesAsync" + idUserReceiver.ToString();

            ICollection<GetMessageDto> getMessageInCache = await _cacheService.GetAsync<ICollection<GetMessageDto>>(KeyM);
            if(getMessageInCache is null)
            {
                ICollection<GetMessageDto> getMessageInDb = await _messageRepository.GetMessagesAsync(idUserIssuer, idUserReceiver);

                await _cacheService.SetAsync(KeyM, getMessageInDb, TimeSpan.FromSeconds(5));

                return getMessageInDb;
            }
            
            return getMessageInCache;
        }
    }
}