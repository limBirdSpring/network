using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntry : MonoBehaviour
{
    [SerializeField]
    private TMP_Text playerName;
    [SerializeField]
    private TMP_Text playerReady;
    [SerializeField]
    private Button playerReadyButton;

    private int ownerId;

    public void Initialized(int id, string name)
    {
        ownerId = id;
        playerName.text = name;
        playerReady.text = "";
        if(PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
        {
            playerReadyButton.gameObject.SetActive(false);
        }
    }

    public void OnReadyButtonClicked()
    {
        object isPlayerReady;
        if(!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Ready", out isPlayerReady))
            isPlayerReady = false;

        bool ready = (bool)isPlayerReady;
        SetPlayerReady(!ready);

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable()
        {
            {"Ready", !ready },
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

    }

    public void SetPlayerReady(bool ready)
    {
        playerReady.text = ready ? "Ready" : "";
        PhotonNetwork.AutomaticallySyncScene = ready; // 같이 씬이 넘어가도록
    }
}
