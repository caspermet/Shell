using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemInfo itemInfo;
    public GameObject itemGameObject;
    public ItemType itemType;

    public abstract void OnTriggerHold(Transform view);

    public abstract void OnTriggerRelease();
}
