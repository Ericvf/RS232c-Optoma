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
            Console.WriteLine();

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
                Console.WriteLine($"IP Address: {ipAddress}");

                response = rs232cClient.Get("~00150 1");
                Console.WriteLine($"Info: {response}");

                response = rs232cClient.Get("~00124 1");
                Console.WriteLine($"PWR: {(response == "OK1" ? "On" : "Off")}");
                Console.WriteLine();

                // REPL
                while (rs232cClient.IsConnected())
                {
                    Console.Write("> ");

                    // Read
                    var command = Console.ReadLine();

                    if (command == "?")
                    {
                        Console.WriteLine("OFF:        ~0000 0");
                        Console.WriteLine("ON:         ~0000 1");
                        Console.WriteLine("BRIGHTNESS: ~0021 [-50/50]");
                        Console.WriteLine("HDMI1:      ~0012 1");
                        Console.WriteLine("HDMI2:      ~0012 15");
                        Console.WriteLine("INFO:       ~00150 1");
                        Console.WriteLine("MODEL:      ~00353 1");
                        Console.WriteLine("TEMP:       ~00150 18");
                        Console.WriteLine("PWR?:       ~00124 1");
                    }
                    else if (command == "!")
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