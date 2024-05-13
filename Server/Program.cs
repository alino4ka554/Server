using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPAddress address = IPAddress.Parse("127.0.0.1");
            int port = 8888;

            TcpListener server = new TcpListener(address, port);

            server.Start();
            Console.WriteLine("Сервер работает");

            try
            {
                while (true)
                {

                    TcpClient client = server.AcceptTcpClient();

                    Console.WriteLine("Клиент подключился");

                    while (true)
                    {

                        NetworkStream stream = client.GetStream();

                        byte[] buffer = new byte[1024];

                        int readByte = stream.Read(buffer, 0, buffer.Length);

                        string question = Encoding.UTF8.GetString(buffer, 0, readByte);

                        Console.WriteLine("Запрос клиента: " + question);

                        byte[] bytes = Encoding.UTF8.GetBytes(ServerAnswer(question));

                        stream.Write(bytes, 0, bytes.Length);

                    }
                }
            } 
            catch(Exception ex)
            {
                server.Stop();
                Console.WriteLine("Сервер отключен");
            }

        }

        static string ServerAnswer(string question)
        {
            string answer = "";
            DirectoryInfo dir = new DirectoryInfo($"C:\\{question}");
            if (dir.Exists)
            {
                DirectoryInfo[] dirs = dir.GetDirectories();
                foreach (DirectoryInfo dird in dirs)
                {
                    answer += $"#{dird.Name.ToString()}";
                }
            }
            else
            {
                StreamWriter streamWriter = new StreamWriter($"C:\\{question}.txt");
                answer = streamWriter.ToString();
            }
            
            return answer;
        }
    }
}
