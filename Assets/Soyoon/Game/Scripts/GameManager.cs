using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace SoYoon
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private TMP_Text infoText;

        private void Start()
        {
            if (PhotonNetwork.InRoom)
            {
                // 동시에 start하기 위해 -> 로비씬을 거쳐서 들어온 경우
                Hashtable props = new Hashtable() { { "Load", true } };
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            }
            else
            {
                // 게임씬 테스트용 빠른 접속
                PhotonNetwork.ConnectUsingSettings();
                //infoText.text = "Test Game";
                infoText.text = "";
            }
        }

        public override void OnConnectedToMaster()
        {
            // 테스트용 방 만들기
            PhotonNetwork.JoinOrCreateRoom("TestRoom", new RoomOptions() { MaxPlayers = 8 },  null);
        }

        public override void OnJoinedRoom()
        {
            // 테스트용 게임 시작
            //infoText.text = "Test Game Start";
            infoText.gameObject.SetActive(false);   
            TestGameStart();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log(string.Format("Disconnected : {0}", cause.ToString()));
            SceneManager.LoadScene("LobbyScene");
        }

        public override void OnLeftRoom()
        {
            Debug.Log(string.Format("LeftRoom"));
            SceneManager.LoadScene("LobbyScene");
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if(changedProps.ContainsKey("Load"))
            {
                if(AllPlayersLoadLevel())
                {
                    // 게임시작
                    PrintInfo("Game Start");
                    infoText.gameObject.SetActive(false);
                    GameStart();
                }
                else
                {
                    // 기다리기
                    PrintInfo("Waiting other players..");
                }
            }
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (newMasterClient.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                StartCoroutine(SpawnStone());
        }

        private void GameStart()
        {
            // 로비를 거쳐 게임 시작
        }

        private void TestGameStart()
        {
            // 테스트용으로 바로 게임 시작
            float angularStart = (360.0f / PhotonNetwork.CurrentRoom.PlayerCount) * PhotonNetwork.LocalPlayer.GetPlayerNumber();
            float x = 20.0f * Mathf.Sin(angularStart * Mathf.Deg2Rad);
            float z = 20.0f * Mathf.Cos(angularStart * Mathf.Deg2Rad);
            Vector3 position = new Vector3(x, 0.0f, z);
            Quaternion rotation = Quaternion.Euler(0.0f, angularStart, 0.0f);

            PhotonNetwork.Instantiate("Player", position, rotation);
            if(PhotonNetwork.IsMasterClient)
                StartCoroutine(SpawnStone());
        }

        private bool AllPlayersLoadLevel()
        {
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                object playerLoaded;
                if (player.CustomProperties.TryGetValue("Load", out playerLoaded))
                {
                    if (!(bool)playerLoaded)
                        return false;
                    else
                        continue;
                }
                else
                    return false;   
            }
            return true;
        }

        private IEnumerator SpawnStone()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(3, 5));

                Vector2 direction = Random.insideUnitCircle;
                Vector3 position = Vector3.zero;

                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    // Make it appear on the left/right side
                    position = new Vector3(Mathf.Sign(direction.x) * Camera.main.orthographicSize * Camera.main.aspect, 0, direction.y * Camera.main.orthographicSize);
                }
                else
                {
                    // Make it appear on the top/bottom
                    position = new Vector3(direction.x * Camera.main.orthographicSize * Camera.main.aspect, 0, Mathf.Sign(direction.y) * Camera.main.orthographicSize);
                }

                // Offset slightly so we are not out of screen at creation time (as it would destroy the asteroid right away)
                position -= position.normalized * 0.1f;


                Vector3 force = -position.normalized * 1000.0f;
                Vector3 torque = Random.insideUnitSphere * Random.Range(100.0f, 300.0f);
                object[] instantiationData = { force, torque };

                if (Random.Range(0, 10) < 5)
                {
                    PhotonNetwork.InstantiateRoomObject("BigStone", position, Quaternion.Euler(Random.value * 360.0f, Random.value * 360.0f, Random.value * 360.0f), 0, instantiationData);
                }
                else
                {
                    PhotonNetwork.InstantiateRoomObject("SmallStone", position, Quaternion.Euler(Random.value * 360.0f, Random.value * 360.0f, Random.value * 360.0f), 0, instantiationData);
                }
            }
        }

        private void PrintInfo(string info)
        {
            Debug.Log(info);
            infoText.text = info;
        }
    }
}
