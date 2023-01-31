using System;
using System.IO;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

namespace SoYoon
{
    public class Client : MonoBehaviour
    {
        [SerializeField]
        private ChatUI chatUI;
        [SerializeField]
        private TMP_InputField nameField;
        [SerializeField]
        private TMP_InputField ipField;
        [SerializeField]
        private TMP_InputField portField; // 3389, 22, 21, 1289

        private string clientName;
        private string ip;
        private int port;

        public bool IsConnected { get; private set; } = false;

        private TcpClient client;
        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;

        private void Update()
        {
            if(IsConnected && stream.DataAvailable)
            {
                string chat = reader.ReadLine();
                if (chat != null) ReceiveChat(chat);
            }
        }

        private void OnDisable()
        {
            Disconnect();
        }

        public void Connect()
        {
            if (IsConnected) return; // 捞固 立加等 版快

            AddMessage("Try To Connect");

            clientName = nameField.text == "" ? "NickName" : nameField.text;
            ip = ipField.text == "" ? "127.0.0.1" : ipField.text;
            port = portField.text == "" ? 5555 : int.Parse(portField.text);

            try
            { 
                client = new TcpClient(ip, port);
                stream = client.GetStream();
                writer = new StreamWriter(stream);
                reader = new StreamReader(stream);

                AddMessage("Connect Success");
                IsConnected = true;
                nameField.interactable = false;
                ipField.interactable = false;
                portField.interactable = false;
            }
            catch (Exception exception)
            {
                AddMessage("Connect Failed : " + exception.Message);
                Disconnect();
            }
        }

        public void Disconnect()
        {
            writer?.Close();
            writer = null;
            reader?.Close();
            reader = null;
            stream?.Close();
            stream = null;
            client?.Close();
            client = null;

            IsConnected = false;
            nameField.interactable = true;
            ipField.interactable = true;
            portField.interactable = true;
            AddMessage("Disconnected");
        }

        public void SendChat(string chat)
        {
            if(!IsConnected)
            {
                AddMessage("Client is not connected");
                return;
            }

            try
            {
                writer.WriteLine(string.Format("{0} : {1}", clientName, chat));
                writer.Flush();
            }
            catch (Exception exception)
            {
                AddMessage("Send chat Failed " + exception.Message);
                Disconnect();
            }
        }

        public void ReceiveChat(string chat)
        {
            UnityEngine.Debug.Log(chat);
            chatUI.AddChat(chat);
        }

        private void AddMessage(string message)
        {
            UnityEngine.Debug.Log(string.Format("[Client] {0}", message));
            chatUI.AddMessage(string.Format("[Client] {0}", message));
        }
    }
}
