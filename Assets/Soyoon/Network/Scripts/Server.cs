using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

namespace SoYoon
{
    public class Server : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField ipField;
        [SerializeField]
        private TMP_InputField portField;
        [SerializeField]
        private RectTransform logContent;
        [SerializeField]
        private TMP_Text logText;

        private IPAddress ip;
        private int port;

        private TcpListener listener;

        private List<TcpClient> connectedClients;
        private List<TcpClient> disconnectedClients;

        public bool IsOpened { get; private set; }

        private void Update()
        {
            if (!IsOpened) return;

            foreach (TcpClient client in connectedClients)
            {
                if (!ConnectCheck(client))
                {
                    client?.Close();
                    disconnectedClients.Add(client);
                }
            }

            foreach (TcpClient client in disconnectedClients)
            {
                connectedClients.Remove(client);
            }
            disconnectedClients.Clear();

            foreach (TcpClient client in connectedClients)
            {
                NetworkStream stream = client.GetStream();
                if (stream.DataAvailable)
                {
                    StreamReader reader = new StreamReader(stream);
                    string chat = reader.ReadLine();
                    if (chat != null) SendAll(chat);
                }
            }
        }

        private void OnDisable()
        {
            Close();
        }

        public void Open()
        {
            if (IsOpened) return;

            AddLog("Try To Open");

            port = portField.text == "" ? 5555 : int.Parse(portField.text);
            connectedClients = new List<TcpClient>();
            disconnectedClients = new List<TcpClient>();

            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                AddLog("Server Opened");
                IsOpened = true;
                portField.interactable = false;
            }
            catch (Exception exception)
            {
                AddLog("Server Open Failed : " + exception.Message);
                Close();
            }

            listener.BeginAcceptTcpClient(AcceptCallback, null);
        }

        public void Close()
        {
            listener?.Stop();
            listener = null;
            IsOpened = false;
            portField.interactable = true;

            AddLog("Server Closed");
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            Debug.Log("Client Connected");

            if (listener == null)
                return;

            TcpClient client = listener.EndAcceptTcpClient(ar);
            connectedClients.Add(client);
            listener.BeginAcceptTcpClient(AcceptCallback, null);
        }

        private bool ConnectCheck(TcpClient client)
        {
            try
            {
                if (client != null && client.Client != null && client.Connected)
                {
                    if (client.Client.Poll(0, SelectMode.SelectRead))
                    {
                        return !(client.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                    }
                    return true;
                }
                else
                    return false;
            }
            catch (Exception exception)
            {
                AddLog("Connect Check Error");
                AddLog(exception.Message);
                return false;
            }
        }

        public void SendAll(string chat)
        {
            AddLog("SendAll " + chat);
            foreach (TcpClient client in connectedClients)
            {
                StreamWriter writer = new StreamWriter(client.GetStream());
                writer.WriteLine(chat);
                writer.Flush();
            }
        }

        public void AddLog(string log)
        {
            Debug.Log(string.Format("[Server] {0}", log));

            TMP_Text newLog = Instantiate(logText, logContent);
            newLog.text = log;
            newLog.fontSize = 20;
        }
    }
}
