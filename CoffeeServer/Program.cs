using System;
using System.Net.Sockets;
using System.IO;

namespace CoffeeServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(8888);
            server.Start();
            Console.WriteLine("Coffee Shop Server Started...");
            Console.WriteLine("Waiting for customer...");

            Socket socketForClient = server.AcceptSocket();

            if (socketForClient.Connected)
            {
                NetworkStream stream = new NetworkStream(socketForClient);
                StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };
                StreamReader reader = new StreamReader(stream);

                // Send menu
                writer.WriteLine("Welcome to ChatCoffee!");
                writer.WriteLine("Menu:");
                writer.WriteLine("1. Espresso - $3");
                writer.WriteLine("2. Latte - $4");
                writer.WriteLine("3. Cappuccino - $5");
                writer.WriteLine("Please enter your choice:");

                try
                {
                    while (true)
                    {
                        string choice = reader.ReadLine();
                        if (choice == null) break;

                        string response;

                        switch (choice)
                        {
                            case "1":
                                response = "You ordered Espresso ($3). Preparing your drink!";
                                break;
                            case "2":
                                response = "You ordered Latte ($4). Preparing your drink!";
                                break;
                            case "3":
                                response = "You ordered Cappuccino ($5). Preparing your drink!";
                                break;
                            default:
                                response = "Invalid choice. Please choose 1, 2 or 3.";
                                break;
                        }

                        Console.WriteLine($"Client >> {choice}");
                        writer.WriteLine("Server >> " + response);
                    }
                }
                finally
                {
                    writer.Close();
                    reader.Close();
                    stream.Close();
                    socketForClient.Close();
                    server.Stop();
                }
            }
        }
    }
}
