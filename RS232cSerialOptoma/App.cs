using System;
using System.Threading.Tasks;

namespace RS232cSerialOptoma
{
    public class App
    {
        private readonly IRS232cSerialOptomaClient rs232cClient;

        public App(IRS232cSerialOptomaClient rs232CTcpClient)
        {
            this.rs232cClient = rs232CTcpClient;
        }

        public Task RunAsync(string[] args)
        {
            Console.WriteLine($"{nameof(RS232cSerialOptoma)} started");

            if (args.Length != 1)
            {
                Console.WriteLine("Please provide a COM port.");
                Console.WriteLine($"e.g. `{nameof(RS232cSerialOptoma)}.exe 15");
                Environment.Exit(1);
            }

            var response = default(string);
            var port = Convert.ToInt32(args[0]);
            var baudRate = 9600;

            while (true)
            {
                // Start the connection and login
                rs232cClient.Start(port, baudRate);

                Console.WriteLine($"Connected to COM{port} @ {baudRate}");

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
                       
                        response = rs232cClient.Get(command!);

                        // Print
                        Console.WriteLine(response);
                    }
                }

                rs232cClient.Stop();
            }
        }
    }
}