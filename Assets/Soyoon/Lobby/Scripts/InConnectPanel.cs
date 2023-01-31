using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InConnectPanel : MonoBehaviour
{
    public void OnLogoutButtonClicked()
    {
        PhotonNetwork.Disconnect();
    }
}
