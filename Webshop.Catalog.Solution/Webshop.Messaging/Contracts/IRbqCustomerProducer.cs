using System.Threading.Tasks;

namespace Webshop.Messaging.Contracts
{
    public interface IRbqCustomerProducer
    {
        Task SendMessageAsync(string message);
    }
}
