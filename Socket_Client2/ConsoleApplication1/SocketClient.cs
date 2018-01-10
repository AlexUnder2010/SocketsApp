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
                SendMessageFromSocket(11111);
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
            // створюємо буфер для вхідних повідомлень
            var bytes = new byte[1024];
            // створюємо віддалену точку для сокета
            var ipHost = Dns.GetHostEntry("localhost");
            var ipAddr = ipHost.AddressList[0];
            var ipEndPoint = new IPEndPoint(ipAddr, port);
            var sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // з’єднуємо сокет з віддаленою точкою
            sender.Connect(ipEndPoint);
            Console.Write("Введiть повiдомлення: ");
            var message = Console.ReadLine();
            Console.WriteLine("Сокет з’єднується з {0} ", sender.RemoteEndPoint);
            var msg = Encoding.UTF8.GetBytes(message);
            // Отправляем данные через сокет
            var bytesSent = sender.Send(msg);
            // отримуємо відповідь від серверу
            var bytesRec = sender.Receive(bytes);
            Console.WriteLine("\nВiдповiдь вiд сервера: {0}\n\n", Encoding.UTF8.GetString(bytes, 0, bytesRec));
            // рекурсія для виклику SendMessageFromSocket()
            if (message.IndexOf("<TheEnd>") == -1)
                SendMessageFromSocket(port);
            // вивільнюємо сокет
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
    }
}