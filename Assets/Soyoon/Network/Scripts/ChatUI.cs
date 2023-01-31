using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SoYoon
{
    public class ChatUI : MonoBehaviour
    {
        [SerializeField]
        private Client client;

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
            if (Input.GetButtonDown("Submit"))
            {
                if (inputField.IsActive() && inputField.text != "")
                    client.SendChat(inputField.text);
                else
                    inputField.ActivateInputField();
            }
        }

        public void AddMessage(string chat) // 정보 표시용
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
            chatScrollView.verticalScrollbar.value = 0; // 채팅을 치면 가장 아래로

            inputField.text = "";
            inputField.ActivateInputField();
        }
    }
}
