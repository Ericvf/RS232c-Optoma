using System;
using System.Threading.Tasks;

namespace RS232cOptoma
{
    public class App
    {
        private readonly IRS232cOptomaClient rs232cClient;

        public App(IRS232cOptomaClient rs232cClient)
        {
            this.rs232cClient = rs232cClient;
        }

        public async Task RunAsync(string[] args)
        {
            Console.WriteLine($"{nameof(RS232cOptoma)} started");

            if (args.Length != 2)
            {
                Console.WriteLine("Please provide a hostname/ipaddress and port.");
                Console.WriteLine($"e.g. `{nameof(RS232cOptoma)}.exe 192.168.1.114 80`");
                Environment.Exit(1);
            }


            var response = default(string);
            var ipAddress = args[0];
            var port = Convert.ToInt32(args[1]);

            while (true)
            {
                // Start the connection and login
                await rs232cClient.Start(ipAddress, port);

                Console.WriteLine($"Connected: {ipAddress}");

                // REPL
                while (rs232cClient.IsConnected())
                {
                    Console.Write("> ");

                    // Read
                    var command = Console.ReadLine();
                    if (command == "!")
                    {
                        break;
                    }
                    else if (!string.IsNullOrEmpty(command))
                    {

                        response = rs232cClient.Get(command);

                        // Print
                        Console.WriteLine(response);
                    }
                }

                rs232cClient.Stop();
            }
        }
    }
}