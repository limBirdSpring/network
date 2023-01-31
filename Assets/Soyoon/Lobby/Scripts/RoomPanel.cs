using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class RoomPanel : MonoBehaviour
{
    [SerializeField]
    private PlayerEntry playerEntryPrefab;
    [SerializeField]
    private RectTransform playerContent;
    [SerializeField]
    private Button startButton;

    private List<PlayerEntry> playerEntries;

    private void Awake()
    {
        playerEntries = new List<PlayerEntry>();
    }

    public void UpdateRoomState()
    {
        foreach (PlayerEntry player in playerEntries)
        {
            Destroy(player.gameObject);
        }
        playerEntries.Clear();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // 플레이어들 다시 추가
            PlayerEntry entry = Instantiate(playerEntryPrefab, playerContent);
            entry.Initialized(player.ActorNumber, player.NickName);
            object isPlayerReady;
            if(player.CustomProperties.TryGetValue("Ready", out isPlayerReady))
            {
                entry.SetPlayerReady((bool)isPlayerReady);
            }

            playerEntries.Add(entry);
        }
    }

    public void UpdateLocalPlayerPropertiesUpdate()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (CheckPlayerReady())
            startButton.gameObject.SetActive(true);
        else
            startButton.gameObject.SetActive(false);
    }

    public bool CheckPlayerReady()
    {
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;
            if (player.CustomProperties.TryGetValue("Ready", out isPlayerReady))
            {
                if (!(bool)isPlayerReady)
                    return false;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public void OnStartButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel("GameScene");
    }

    public void OnLeaveRoomButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }
}
