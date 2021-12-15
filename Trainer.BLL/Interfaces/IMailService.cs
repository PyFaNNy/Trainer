using System.Threading.Tasks;
using Trainer.BLL.DTO;

namespace Trainer.BLL.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);

    }
}
