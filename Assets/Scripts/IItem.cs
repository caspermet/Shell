﻿using Photon.Pun;
using UnityEngine;

public abstract class Item : MonoBehaviourPunCallbacks
{
    public ItemInfo itemInfo;
    public GameObject itemGameObject;
    public ItemType itemType;

    public abstract void OnTriggerHold(Transform view);

    public abstract void OnTriggerRelease();
}
