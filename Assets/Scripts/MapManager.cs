using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapManager : MonoBehaviourPunCallbacks
{
    public static MapManager Instance;
    public GameObject[] spawners;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void ChooseRedTeam()
    {
        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable
        {
            { "Team", 0 }
        };
        PhotonNetwork.SetPlayerCustomProperties(playerProps);
    }

    public void ChooseBlueTeam()
    {
        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable
        {
            { "Team", 1 }
        };
        PhotonNetwork.SetPlayerCustomProperties(playerProps);
    }
}
