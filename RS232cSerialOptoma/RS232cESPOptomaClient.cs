using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RS232cOptoma
{
    public sealed class RS232cESPOptomaClient : IRS232cOptomaClient
    {
        private readonly ILogger<RS232cESPOptomaClient> logger;
        private HttpClient httpClient = new HttpClient();

        public RS232cESPOptomaClient(ILogger<RS232cESPOptomaClient> logger)
        {
            this.logger = logger;
        }

        public async Task Start(string address, int port = 80)
        {
            logger.LogInformation($"Starting {address} @ {port}");

            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"http://{address}");
            
            var output = await httpClient.GetStringAsync("/");
            logger.LogInformation(output);
        }

        public void Stop()
        {
            logger.LogInformation($"Stopping");
        }

        public bool IsConnected() => true;

        public string Get(string command) => SendCommandAndGetResponse(command);
        
        public string SendCommandAndGetResponse(string command)
        {
            if (httpClient == null)
                return string.Empty;

            logger.LogInformation("SendCommandAndGetResponse", command);

            var response = httpClient.Send(new HttpRequestMessage(HttpMethod.Get, $"/cmd/{command}"));

            using var responseStream = response.Content.ReadAsStream();
            using var streamReader = new StreamReader(responseStream);

            return streamReader.ReadToEnd();
        }
    }

}