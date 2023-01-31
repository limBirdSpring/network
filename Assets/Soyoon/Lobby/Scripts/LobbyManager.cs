using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public enum Panel { Login, InConnect, Lobby, Room}

    [SerializeField]
    private LoginPanel loginPanel;
    [SerializeField]
    private InConnectPanel inConnectPanel;
    [SerializeField]
    private RoomPanel roomPanel;
    [SerializeField]
    private LobbyPanel lobbyPanel;

    public override void OnConnectedToMaster()
    {
        // 접속했을 경우
        SetActivePanel(Panel.InConnect);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // 접속이 끊겼을 경우
        SetActivePanel(Panel.Login);
    }

    public override void OnJoinedRoom()
    {
        // 방 들어갔을 경우
        SetActivePanel(Panel.Room);

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable()
        {
            { "Ready", false },
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        roomPanel.UpdateRoomState();
    }

    public override void OnLeftRoom()
    {
        // 방을 나갔을 경우
        SetActivePanel(Panel.InConnect);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 플레이어 들어온 경우
        roomPanel.UpdateRoomState();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // 플레이어 나간 경우
        roomPanel.UpdateRoomState();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // 방장이 바뀐 경우
        roomPanel.UpdateRoomState();
        roomPanel.UpdateLocalPlayerPropertiesUpdate();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        // 플레이어 상황이 바뀐 경우(준비상태)
        roomPanel.UpdateRoomState();
        roomPanel.UpdateLocalPlayerPropertiesUpdate();
    }

    public override void OnJoinedLobby()
    {
        SetActivePanel(Panel.Lobby);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        lobbyPanel.UpdateRoomList(roomList);
    }

    public override void OnLeftLobby()
    {
        SetActivePanel(Panel.InConnect);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // 방을 만들지 못했을 경우
        SetActivePanel(Panel.InConnect);
        StatePanel.Instance.AddMessage(string.Format("Create room failed with error({0}) : {1}", returnCode, message));
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // 방을 들어가지 못했을 경우
        SetActivePanel(Panel.InConnect);
        StatePanel.Instance.AddMessage(string.Format("Join room failed with error({0}) : {1}", returnCode, message));
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 방에 들어가지 못했을 경우
        StatePanel.Instance.AddMessage(string.Format("Join random room failed with error({0}) : {1}", returnCode, message));
        StatePanel.Instance.AddMessage("Create Room Instead");

        string roomName = string.Format("Room {0}", Random.Range(1000, 10000));
        RoomOptions options = new RoomOptions() { MaxPlayers = (byte)8 };
        PhotonNetwork.CreateRoom(roomName, options);
    }

    private void SetActivePanel(Panel panel)
    {
        loginPanel?.gameObject.SetActive(panel == Panel.Login);
        inConnectPanel?.gameObject.SetActive(panel == Panel.InConnect);
        roomPanel?.gameObject.SetActive(panel == Panel.Room);
        lobbyPanel?.gameObject.SetActive(panel == Panel.Lobby);
    }
}
