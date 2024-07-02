using Grpc.Net.Client;
using GrpcClient.UI;

namespace GrpcClient
{
    internal class Program
    {
        const string ServerAddress = "https://localhost:7221";
        static void Main(string[] args)
        {
            Console.WriteLine($"Connecting to service at '{ServerAddress}'...");

            using var channel = GrpcChannel.ForAddress(ServerAddress);
            var client = new NumberGuess.NumberGuessClient(channel);

            Console.WriteLine("Client created. Starting Game...");
            Thread.Sleep(500);
            Console.Clear();

            try
            {
                new NumberGuessUI(client).Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("x_x He's dead Jim...\r\n");
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }
    }
}
