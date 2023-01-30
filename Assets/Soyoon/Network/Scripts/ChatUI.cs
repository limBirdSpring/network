using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private RectTransform chatContent;
    [SerializeField]
    private ScrollRect chatScrollView;
    [SerializeField]
    private TMP_Text chatText;


    private void Update()
    {
        if(Input.GetButtonDown("Submit"))
        {
            if (inputField.IsActive() && inputField.text != "")
                AddChat(inputField.text);
            else
                inputField.ActivateInputField();
        }
    }

    public void AddMessage(string chat) // ���� ǥ�ÿ�
    {
        TMP_Text newMessage = Instantiate(chatText, chatContent);
        newMessage.text = chat;
        newMessage.fontSize = 32;
        newMessage.color = Color.red;
        chatScrollView.verticalScrollbar.value = 0;

        inputField.text = "";
        inputField.ActivateInputField();
    }

    public void AddChat(string chat)
    {
        TMP_Text newMessage = Instantiate(chatText, chatContent);
        newMessage.text = chat;
        newMessage.fontSize = 32;
        chatScrollView.verticalScrollbar.value = 0; // ä���� ġ�� ���� �Ʒ���

        inputField.text = "";
        inputField.ActivateInputField();
    }
}
