using Microsoft.Extensions.Logging;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RS232cOptoma
{
    public sealed class RS232cSerialOptomaClient : IRS232cOptomaClient
    {
        private readonly ILogger<RS232cSerialOptomaClient> logger;
        private readonly SerialPort serialPort;
        private const int timeout = 100;

        public RS232cSerialOptomaClient(ILogger<RS232cSerialOptomaClient> logger)
        {
            this.logger = logger;
            this.serialPort = new SerialPort();
        }

        public Task Start(string address, int port)
        {
            logger.LogInformation($"Starting COM{address} @ {port}");
            serialPort.PortName = $"COM{address}";
            serialPort.BaudRate = port;
            serialPort.Open();
            return Task.CompletedTask;
        }

        public void Stop()
        {
            logger.LogInformation($"Stopping");
            serialPort.Close();
        }

        public bool IsConnected() => serialPort.IsOpen == true;

        public string Get(string command) => SendCommandAndGetResponse(command);

        public string SendCommandAndGetResponse(string command)
        {
            logger.LogInformation("SendCommandAndGetResponse", command);

            var bytes = Encoding.ASCII.GetBytes(command + '\r');
            var chars = Encoding.ASCII.GetChars(bytes);

            serialPort.DiscardInBuffer();
            serialPort.Write(chars, 0, chars.Length);

            return GetResponse();
        }

        private string GetResponse()
        {
            var stringBuilder = new StringBuilder();

            using var cancellationToken = new CancellationTokenSource();
            cancellationToken.CancelAfter(timeout);

            while (serialPort.BytesToRead == 0 && !cancellationToken.IsCancellationRequested) ;

            while (!cancellationToken.IsCancellationRequested)
            {
                if (serialPort.BytesToRead > 0)
                {
                    stringBuilder.Append((char)serialPort.ReadByte());
                }
            }

            var result = stringBuilder.ToString();
            logger.LogInformation("Result " + result);

            return result;
        }
    }
}