using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InConnectPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private GameObject createRoomPanel;

    [SerializeField]
    private TMP_InputField roomNameInputField;
    [SerializeField]
    private TMP_InputField maxPlayerInputField;

    private void OnEnable()
    {
        menuPanel.SetActive(true);
        createRoomPanel.SetActive(false);
    }

    public void OnCreateRoomButtonClicked()
    {
        menuPanel.SetActive(false);
        createRoomPanel.SetActive(true);
    }

    public void OnCreateRoomCancelButtonClicked()
    {
        menuPanel.SetActive(true);
        createRoomPanel.SetActive(false);
    }

    public void OnCreateRoomConfirmButtonClicked()
    {
        string roomName = roomNameInputField.text;
        if (roomName == "")
            roomName = string.Format("Room {0}", Random.Range(1000, 10000));

        int maxPlayer = maxPlayerInputField.text == "" ? 8 : int.Parse(maxPlayerInputField.text);
        maxPlayer = Mathf.Clamp(maxPlayer, 1, 8);

        RoomOptions options = new RoomOptions() { MaxPlayers = (byte)maxPlayer };
        PhotonNetwork.CreateRoom(roomName, options);
    }

    public void OnLobbyButtonClicked()
    {
        PhotonNetwork.JoinLobby();
    }

    public void OnRandomMatchingButtonClicked()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnLogoutButtonClicked()
    {
        PhotonNetwork.Disconnect();
    }
}
