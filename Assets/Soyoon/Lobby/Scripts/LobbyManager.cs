using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public enum Panel { Login, InConnect, Lobby, Room}

    [SerializeField]
    private LoginPanel loginPanel;
    [SerializeField]
    private InConnectPanel inConnectPanel;

    public override void OnConnectedToMaster()
    {
        // �������� ���
        SetActivePanel(Panel.InConnect);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // ������ ������ ���
        SetActivePanel(Panel.Login);
    }

    private void SetActivePanel(Panel panel)
    {
        loginPanel.gameObject.SetActive(panel == Panel.Login);
        inConnectPanel.gameObject.SetActive(panel == Panel.InConnect);
    }
}
