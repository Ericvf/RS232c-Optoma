using System.Threading.Tasks;

namespace RS232cOptoma
{
    public interface IRS232cOptomaClient
    {
        string Get(string command);

        bool IsConnected();

        Task Start(string ipAddress, int port);

        void Stop();
    }
}