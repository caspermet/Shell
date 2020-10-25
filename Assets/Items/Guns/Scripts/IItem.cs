using Photon.Pun;
using System;
using UnityEngine;

public abstract class Item : MonoBehaviourPunCallbacks
{
    public string itemName;
    public ItemInfo itemInfo;
    public GameObject itemGameObject;
    public ItemType itemType;
    public PlayerController playerController;

    public abstract void OnTriggerHold(Transform view);

    public abstract void OnTriggerRelease();
    public abstract void OnTriggetReleaseFire2();
    public abstract void OnTriggetHoldFire2();
    internal abstract void Run(bool isRunning);

    internal void SetPlayer(PlayerController _playerController)
    {
        playerController = _playerController;
    }


}
