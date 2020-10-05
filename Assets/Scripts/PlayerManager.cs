using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    Vector3 playerSpawn;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (PV.IsMine)
        {
            Vector3 spawnPlace = Vector3.up;
            if (RoomManager.Instance.Team1 <= RoomManager.Instance.Team2)
            {
                spawnPlace =  MapManager.Instance.spawners[0].transform.position;
                RoomManager.Instance.Team1++;
                RoomManager.Instance.AddPlayerToTeam1();
            }
            else
            {
                spawnPlace = MapManager.Instance.spawners[1].transform.position;
                RoomManager.Instance.Team2++;
                RoomManager.Instance.AddPlayerToTeam2();
            }

            CreateController(spawnPlace);
        }
    }
    public void CreateController(Vector3 spawnePlace)
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnePlace, Quaternion.identity);
    }
}
