using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Services.MailService
{
    public interface IMailService
    {
        Task<bool> SendMail(MailContent mailContent);
    }
}
