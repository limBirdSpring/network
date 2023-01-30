using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

namespace SoYoon
{
    public class Client : MonoBehaviour
    {
        private TcpClient client;
        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;

        public void Connect()
        {
            
        }

        public void Disconnect()
        {

        }

        public void SendChat(string chat)
        {

        }

        public void ReceiveChat(string chat)
        {

        }
    }
}
