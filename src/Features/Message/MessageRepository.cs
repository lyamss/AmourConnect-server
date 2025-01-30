using Microsoft.EntityFrameworkCore;
using API.Services;
using API.Persistence;


namespace API.Features.Message
{
    internal sealed class MessageRepository : Repository<Entities.Message>,IMessageRepository
    {

        public MessageRepository(BackendDbContext _context) : base(_context) {}


        public async Task<ICollection<QueryMessage>> GetMessagesAsync(int idUserIssuer, int idUserReceiver, CancellationToken cancellationToken) =>
            await this.context.Message
                        .Include(m => m.UserIssuer)
                        .Include(m => m.UserReceiver)
                        .Where(m => (m.IdUserIssuer == idUserIssuer && m.Id_UserReceiver == idUserReceiver) ||
                                    (m.IdUserIssuer == idUserReceiver && m.Id_UserReceiver == idUserIssuer))
                        .AsSplitQuery()
                        .Select(m => m.ToGetMessageMapper())
                        .ToListAsync(cancellationToken);


        public async Task<bool> DeleteMessagesAsync(List<int> messageIds, CancellationToken cancellationToken)
        {
            var messages = await this.context.Message.Where(m => messageIds.Contains(m.Id_Message))
                .ToListAsync();

            if (messages.Any())
            {
                try
                {
                    this.context.Message.RemoveRange(messages);
                    await this.context.SaveChangesAsync(cancellationToken);
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {

                    foreach (var message in messages) 
                    {
                        this.context.Entry(message).Reload();
                    }
                    return await this.DeleteMessagesAsync(messageIds, cancellationToken);
                }
            }

            return false;
        }
    }

    public interface IMessageRepository
    {
        Task<ICollection<QueryMessage>> GetMessagesAsync(int idUserIssuer, int idUserReceiver, CancellationToken cancellationToken);
        Task<bool> DeleteMessagesAsync(List<int> Id_Message, CancellationToken cancellationToken);
    }
}