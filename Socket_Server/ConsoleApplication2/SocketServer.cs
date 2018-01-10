using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // встановлюємо для сокета кінцеву точку
            var ipHost = Dns.GetHostEntry("localhost");
            var ipAddr = ipHost.AddressList[0];
            var ipEndPoint = new IPEndPoint(ipAddr, 11111);
            var ipEndPoint2 = new IPEndPoint(ipAddr, 22222);
            // створюємо сокет Tcp/Ip
            var sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            var sListener2 = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // назначаємо сокет локальній кінцевій точці та слухаємо порти
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);
                sListener2.Bind(ipEndPoint2);
                sListener2.Listen(10);
                // слухаємо порти
                while (true)
                {
                    Console.WriteLine("Очiкуємо з’єднання через порт {0} та {1}", ipEndPoint, ipEndPoint2);
                    // призупиняємо роботу програми, очікуючи вхідне з’єднання
                    var handler = sListener.Accept();
                    string data = null;
                    // після отримання даних від клієнта кодуємо їх в строку
                    var bytes = new byte[1024];
                    var bytesRec = handler.Receive(bytes);
                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    // виводимо дані в консоль
                    Console.Write("Отриманий текст: " + data + "\n\n");
                    // надсилаємо відповідь клієнту
                    var reply = "Катя";
                    var msg = Encoding.UTF8.GetBytes(reply);
                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                    // розриваємо зв’язок для завершення рекурсії у клієнта
                    if (data.IndexOf("<TheEnd>") > -1)
                    {
                        Console.WriteLine("Завершено роботу з сервером");
                        break;
                    }
                }
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
    }
}