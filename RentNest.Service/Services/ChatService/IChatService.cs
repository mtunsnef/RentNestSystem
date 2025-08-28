using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Services.ChatService
{
    public interface IChatService
    {
        Task AddMessage(Message message);
        Task<Message?> GetMessageByConversationId(int id);
    }
}
