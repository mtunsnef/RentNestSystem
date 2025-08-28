using RentNest.Core.Domains;
using RentNest.Infrastructure.Repositories.MessageRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Services.ChatService
{
    public class ChatService : IChatService
    {
        private readonly IMessageRepository _messageRepository;
        public ChatService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task AddMessage(Message message)
        {
            await _messageRepository.AddAsync(message);
        }

        public async Task<Message?> GetMessageByConversationId(int id)
        {
            return await _messageRepository.GetByIdAsync(id);
        }
    }
}
