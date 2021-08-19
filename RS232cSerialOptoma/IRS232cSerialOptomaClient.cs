using System.Threading.Tasks;

namespace RS232cSerialOptoma
{
    public interface IRS232cSerialOptomaClient
    {
        string Get(string command);

        bool IsConnected();

        void Start(int port,int baudRate);

        void Stop();
    }
}