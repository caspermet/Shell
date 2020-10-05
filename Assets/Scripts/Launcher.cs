using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using Photon.Pun.UtilityScripts;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject roomListPrefab;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("Choose team");
    }

    public void JoineRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Photon.Realtime.Player[] player = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < player.Count(); i++)
        {
            OnPlayerEnteredRoom(player[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Faild";
        MenuManager.Instance.OpenMenu("error");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
        MenuManager.Instance.OpenMenu("Choose team");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("Loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("Loading");      
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }

    public void ChooseTeam(int team)
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            //we already have a team- so switch teams
            PhotonNetwork.LocalPlayer.CustomProperties["Team"] = team;
        }
        else
        {
            //we dont have a team yet- create the custom property and set it
            //0 for blue, 1 for red
            //set the player properties of this client to the team they clicked
            ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable
        {
            { "Team", team }
        };
            //set the property of Team to the value the user wants
            PhotonNetwork.SetPlayerCustomProperties(playerProps);
        }

        JoineRoom();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue;
            }

            Instantiate(roomListPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }


}
