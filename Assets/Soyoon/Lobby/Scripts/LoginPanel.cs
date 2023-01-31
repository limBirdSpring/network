using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField idInputField;

    private void Start()
    {
        idInputField.text = "Player " + Random.Range(1000, 10000);
    }

    public void OnLoginButtonClicked()
    {
        if(idInputField.text == "")
        {
            StatePanel.Instance.AddMessage("Invalid Player Name");
        }
        PhotonNetwork.LocalPlayer.NickName = idInputField.text;
        PhotonNetwork.ConnectUsingSettings();
    }
}
