// SocketClient.cs

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                SendMessageFromSocket(22222);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void SendMessageFromSocket(int port)
        {
            var bytes = new byte[1024];

            var ipHost = Dns.GetHostEntry("localhost");
            var ipAddr = ipHost.AddressList[0];
            var ipEndPoint = new IPEndPoint(ipAddr, port);

            var sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sender.Connect(ipEndPoint);

            Console.Write("Введiть повiдомлення: ");
            var message = Console.ReadLine();

            Console.WriteLine("Сокет з’єднується з {0} ", sender.RemoteEndPoint);
            var msg = Encoding.UTF8.GetBytes(message);

            var bytesSent = sender.Send(msg);

            var bytesRec = sender.Receive(bytes);

            Console.WriteLine("\nВiдповiдь вiд сервера: {0}\n\n", Encoding.UTF8.GetString(bytes, 0, bytesRec));

            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
    }
}