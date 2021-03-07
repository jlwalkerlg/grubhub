using System.Threading;
using System.Threading.Tasks;

namespace Web.Services.Mail
{
    public interface IMailer
    {
        Task Send(Mail mail, CancellationToken cancellationToken = default);
    }
}
