using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
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

    public void OnTriggerHoldFire2()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggetHoldFire2();
        }
    }

    public void OnTriggerReleaseFire2()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggetReleaseFire2();
        }
    }

    public void Reload()
    {
        if(equippedGun != null && equippedGun is GunItem)
        {
           // equippedGun.gameObject.GetComponent<GunItem>().Reload();
        }
    }

    public ItemType GetItemType()
    {
        return equippedGun.itemType;
    }
}
