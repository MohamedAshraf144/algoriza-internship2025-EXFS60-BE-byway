using System.Threading.Tasks;

namespace Byway.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendWelcomeEmailAsync(string email, string firstName);
        Task SendPaymentConfirmationEmailAsync(string email, string firstName, decimal totalAmount);
    }
}




