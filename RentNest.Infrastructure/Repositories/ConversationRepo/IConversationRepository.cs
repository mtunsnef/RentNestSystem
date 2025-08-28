using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.ConversationRepo
{
    public interface IConversationRepository : IRepositoryBase<Conversation>
    {
        Task<List<Conversation>> GetByUserIdAsync(int userId);
        Task<Conversation?> GetConversationWithMessagesAsync(int conversationId);
        Task<Conversation?> GetExistingConversationAsync(int userAId, int userBId, int? postId);
    }
}
