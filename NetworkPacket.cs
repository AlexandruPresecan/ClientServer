using System;
using System.Net.Sockets;
using System.Text;

namespace Network
{
    class NetworkPacket
    {
        byte[] buffer = new byte[4096];
        int bufferIndex = 0;
        NetworkStream stream;

        public string name;

        public NetworkPacket(string name)
        {
            this.name = name;
            WriteString(name);
        }

        public void WriteInt(int intNumber)
        {
            byte[] byteValue = BitConverter.GetBytes(intNumber);
            Array.Copy(byteValue, 0, buffer, bufferIndex, sizeof(int));
            bufferIndex += sizeof(int);
        }

        public void WriteFloat(float floatNumber)
        {
            byte[] byteValue = BitConverter.GetBytes(floatNumber);
            Array.Copy(byteValue, 0, buffer, bufferIndex, sizeof(float));
            bufferIndex += sizeof(float);
        }

        public void WriteString(string str)
        {
            WriteInt(str.Length);
            byte[] byteValue = Encoding.ASCII.GetBytes(str);
            Array.Copy(byteValue, 0, buffer, bufferIndex, byteValue.Length);
            bufferIndex += byteValue.Length;
        }

        public NetworkPacket(byte[] buffer)
        {
            this.buffer = buffer;
            name = ReadString();
        }

        public int ReadInt()
        {
            byte[] byteValue = new byte[sizeof(int)];
            Array.Copy(buffer, bufferIndex, byteValue, 0, sizeof(int));
            bufferIndex += sizeof(int);
            return BitConverter.ToInt32(byteValue, 0);
        }

        public float ReadFloat()
        {
            byte[] byteValue = new byte[sizeof(float)];
            Array.Copy(buffer, bufferIndex, byteValue, 0, sizeof(float));
            bufferIndex += sizeof(float);
            return BitConverter.ToSingle(byteValue, 0);
        }

        public string ReadString()
        {
            int length = ReadInt();
            byte[] byteValue = new byte[length];
            Array.Copy(buffer, bufferIndex, byteValue, 0, byteValue.Length);
            bufferIndex += byteValue.Length;
            return Encoding.ASCII.GetString(byteValue);
        }

        public void SendToServer()
        {
            if (!LocalClient.IsConnected())
                return;
            
            LocalClient.networkStream.BeginWrite(buffer, 0, bufferIndex, null, null);
        }

        public void SendToClient(Client client)
        {
            if (!client.IsConnected())
                return;

            try
            {
                client.networkStream.BeginWrite(buffer, 0, bufferIndex, null, null);
            }
            catch
            {

            }
        }

        public void SendToClients()
        {
            foreach (Client client in Server.clients)
                if (client.IsConnected())
                    SendToClient(client);
        }
    }
}
