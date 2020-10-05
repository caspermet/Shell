using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Transform itemHolder;
    public Item[] items;
    Item equippedGun;

    int itemIndex;
    int previousItemIndex = -1;

    private void Start()
    {
        EquipGun();
    }

    public void EquipGun()
    {
        equippedGun = items[0];
    }

    public void EquipItem(int _index)
    {
        itemIndex = _index;

        items[itemIndex].itemGameObject.SetActive(true);

        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;
    }

    public void OnTriggerHold(Transform view)
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerHold(view);
        }
    }

    public void OnTriggerRelease()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerRelease();
        }
    }

    public float GunHeight
    {
        get
        {
            return itemHolder.position.y;
        }
    }

    public void Reload()
    {
        if(equippedGun != null && equippedGun is GunItem)
        {
            equippedGun.gameObject.GetComponent<GunItem>().Reload();
        }
    }
}
