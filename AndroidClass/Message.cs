﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using static ClientServer.SendRecieve;

namespace ClientServer
{
    public class ServerMessage
    {
        public string message;
        public BaseEncode messagearray;
        public bool successful;
        public ServerMessage()
        {
            successful = false;
            message = "null";
            messagearray = new BaseEncode("null");
        }

        public ServerMessage(BaseEncode bytes, char separator)
        {
            string data = bytes.String();
            var param = data.Split(separator);
            message = param[0];
            successful = Boolean.Parse(param[1]);
            messagearray = new BaseEncode(param[2]);
        }
        public BaseEncode Bytes(char separator)
        {
            string arguments = $"{message}{separator}{successful}{separator}{messagearray.String()}";
            var u = new BaseEncode(arguments);
            return u;
        }
        public ServerMessage(string message1, bool successful1, BaseEncode messagearray1 = null)
        {
            if (message1 == string.Empty)
            {
                message = "null";
            }
            else message = message1;
            if (messagearray1 == null)
            {
                messagearray = new BaseEncode("null");
            }
            else messagearray = messagearray1;
            successful = successful1;
        }
    }

    public class ClientMessage
    {
        public string operation;
        public string message;
        public BaseEncode messagearray;
        public ClientMessage()
        {
            operation = "null";
            message = "null";
            messagearray = new BaseEncode("null");
        }

        public ClientMessage(BaseEncode bytes, char separator)
        {
            string data = bytes.String();
            var param = data.Split(separator);
            operation = param[0];
            message = param[1];
            messagearray = new BaseEncode(param[2]);
        }
        public ClientMessage(string operation1, string message1, BaseEncode messagearray1 = null)
        {
            operation = operation1;
            if (message1 == string.Empty)
            {
                message = "null";
            }
            else message = message1;
            if (messagearray1 == null)
            {
                messagearray = new BaseEncode("null");
            }
            else messagearray = messagearray1;
        }
        public BaseEncode Bytes(char separator)
        {
            string arguments = $"{operation}{separator}{message}{separator}";
            var u = new BaseEncode(arguments);
            return u;
        }

    }

    public class SendRecieve
    {
        public static void SendBytes(TcpClient c, BaseEncode BaseEncode, byte ender)
        {
            //declare variables
            byte[] bytes = new byte[1024];
            var utf = BaseEncode.GetnetworkEncoding();
            
            
            NetworkStream s = c.GetStream();
            MemoryStream ms = new MemoryStream();
            ms.Write(utf.data, 0, utf.data.Length);
            ms.WriteByte(ender);

            int x = 0;
            foreach (byte b in ms.ToArray())
            {
                s.WriteByte(b);
                Console.WriteLine(x++);
            }

        }
        public static BaseEncode RecieveBytes(TcpClient c, byte ender)
        {
            int x = 0;
            // Retrieve the network stream.
            NetworkStream s = c.GetStream();
            MemoryStream ms = new MemoryStream();
            while (true)
            {
                try
                {
                    if (s.DataAvailable)
                    {
                        byte b = Convert.ToByte(s.ReadByte());
                        Console.WriteLine(x++);
                        if (b.Equals(ender))
                        {
                            break;
                        }

                        ms.WriteByte(b);
                    }
                }
                catch (Exception)
                {

                }
            }

            var data1 = new NetworkEncoding(ms.ToArray());

            return data1.GetBaseEncode();
        }

    }
    public class NetworkEncoding
    {
        public byte[] data;
        public NetworkEncoding(byte[] data1)
        {
            data = data1;
        }
        public NetworkEncoding(byte data1)
        {
            MemoryStream ms = new MemoryStream();
            ms.WriteByte(data1);
            data = ms.ToArray();
        }
        public NetworkEncoding(string data1)
        {
            data = Encoding.UTF8.GetBytes(data1);
        }
        public BaseEncode GetBaseEncode()
        {
            return new BaseEncode(Encoding.Convert(Encoding.UTF8, Encoding.ASCII, data));
        }
        public string String()
        {
            return Encoding.UTF8.GetString(data);
        }
    }
    public class BaseEncode
    {
        public byte[] data;
        public BaseEncode(byte[] data1)
        {
            data = data1;
        }
        public BaseEncode(byte data1)
        {
            MemoryStream ms = new MemoryStream();
            ms.WriteByte(data1);
            data = ms.ToArray();
        }
        public NetworkEncoding GetnetworkEncoding()
        {
            return new NetworkEncoding(Encoding.Convert(Encoding.ASCII, Encoding.UTF8, data));
        }
        public BaseEncode(string data1)
        {
            data = Encoding.ASCII.GetBytes(data1);
        }
        public string String()
        {
            return Encoding.ASCII.GetString(data);
        }
    }
}