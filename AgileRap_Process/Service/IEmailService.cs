using AgileRap_Process.Helpter;

namespace AgileRap_Process.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(Mailrequest mailrequest);
    }
}
